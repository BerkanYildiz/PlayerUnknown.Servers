namespace PlayerUnknown.Lobby.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Lobby.Services.Api;
    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Network;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public sealed class UserProxy : WebSocketBehavior
    {
        /// <summary>
        /// Gets the server.
        /// </summary>
        private PubgLobbyServer Server
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProxy"/> class.
        /// </summary>
        /// <param name="Server">The server.</param>
        public UserProxy(PubgLobbyServer Server)
        {
            this.Server     = Server;

            Lobby.Log.Info(this.GetType(), "UserProxy has been initalized.");
        }

        /// <summary>
        /// Authenticates to the provider using the specified parameters.
        /// </summary>
        /// <param name="Provider">The provider.</param>
        /// <param name="Ticket">The ticket.</param>
        /// <param name="Username">The username.</param>
        /// <param name="Password">The password.</param>
        /// <param name="PlayerNetId">The player net identifier.</param>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Version">The version.</param>
        public async Task<bool> Authenticate(PubgSession Session, string Provider, string Ticket, string Username, string Password, string PlayerNetId, string CountryCode, string Version)
        {
            if (Provider == "bro")
            {
                Console.WriteLine("[*] Login with Bro !");

                Session.Player = await this.Server.Players.Get(Username, Password);

                if (Session.Player != null)
                {
                    return true;
                }
            }
            else if (Provider == "steam")
            {
                // Not implemented yet.
            }
            else if (Provider == "outer")
            {
                // Not implemented yet.
            }
            else if (Provider == "xbox")
            {
                // How ?
            }

            return false;
        }

        /// <summary>
        /// Called when the WebSocket connection used in a session has been established.
        /// </summary>
        protected override async void OnOpen()
        {
            string Provider = this.Context.QueryString.Get("provider");
            string Ticket   = this.Context.QueryString.Get("ticket");
            string Username = this.Context.QueryString.Get("id");
            string Password = this.Context.QueryString.Get("password");
            string PlayerId = this.Context.QueryString.Get("playerNetId");
            string Country  = this.Context.QueryString.Get("cc");
            string Version  = this.Context.QueryString.Get("clientGameVersion");

            var PubgSession = new PubgSession(this);

            if (this.Server.Sessions.TryAdd(PubgSession))
            {
                var Authenticated = await this.Authenticate(PubgSession, Provider, Ticket, Username, Password, PlayerId, Country, Version);

                if (Authenticated)
                {
                    ClientApi.ConnectionAccepted(PubgSession);
                }
                else
                {
                    Lobby.Log.Warning(this.GetType(), "Authenticated != true at OnOpen().");

                    PubgSession.Player = new Player
                    {
                        Username = Username,
                        Password = Password
                    };

                    await this.Server.Database.Create(PubgSession.Player);

                    ClientApi.ConnectionAccepted(PubgSession);
                }
            }
            else
            {
                Lobby.Log.Warning(this.GetType(), "At OnOpen(), TryAdd(PubgSession) == false, aborting.");
            }
        }

        /// <summary>
        /// Called when the <see cref="T:WebSocketSharp.WebSocket" /> used in a session receives a message.
        /// </summary>
        /// <param name="Args">A <see cref="T:WebSocketSharp.MessageEventArgs" /> that represents the event data passed to
        /// a <see cref="E:WebSocketSharp.WebSocket.OnMessage" /> event.</param>
        protected override void OnMessage(MessageEventArgs Args)
        {
            if (Args.IsPing)
            {
                return;
            }

            Message Message   = new Message(Args.Data);

            string ClassName  = (string) Message.Parameters[1];
            string MethodName = (string) Message.Parameters[2];

            foreach (object Parameter in Message.Parameters.Skip(1))
            {
                Console.WriteLine(" - " + Parameter + " #" + Message.Parameters.IndexOf(Parameter) + ".");
            }

            if (string.IsNullOrEmpty(ClassName) == false && string.IsNullOrEmpty(MethodName) == false)
            {
                Type Class = Type.GetType("PlayerUnknown.Lobby.Services.Api." + ClassName);

                if (Class != null)
                {
                    MethodInfo Method = Class.GetMethod(MethodName, BindingFlags.Static | BindingFlags.Public);

                    if (Method != null)
                    {
                        PubgSession Session = this.Server.Sessions.Get(this.ID);

                        if (Session != null)
                        {
                            Method.Invoke(null, new object[] { Message, Session }); 
                        }
                        else
                        {
                            Lobby.Log.Warning(this.GetType(), "PubgSession == null at OnMessage(Args).");
                        }
                    }
                    else
                    {
                        Lobby.Log.Warning(this.GetType(), "Method(" + MethodName + ") == null at OnMessage(Args).");
                    }
                }
                else
                {
                    Lobby.Log.Warning(this.GetType(), "Class(" + ClassName + ") == null at OnMessage(Args).");
                }
            }
            else
            {
                Lobby.Log.Warning(this.GetType(), "Message.IsValid != true at OnMessage(Args).");
            }

            Console.WriteLine("--------------------------");
        }

        /// <summary>
        /// Called when the WebSocket connection used in a session has been closed.
        /// </summary>
        /// <param name="Args">A <see cref="T:WebSocketSharp.CloseEventArgs" /> that represents the event data passed to
        /// a <see cref="E:WebSocketSharp.WebSocket.OnClose" /> event.</param>
        protected override void OnClose(CloseEventArgs Args)
        {
            base.OnClose(Args);

            if (Args.WasClean == false)
            {
                // Log.Warning(this.GetType(), "Args.WasClean != true at OnClose(Args).");
            }

            bool Cleaned = this.Server.Sessions.TryRemove(this.ID);

            if (Cleaned == false)
            {
                Lobby.Log.Fatal(this.GetType(), "Something is wrong with the server, please put the maintenance mode and check the logs.");
            }
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            this.SendAsync(Message.ToNetwork().ToString(Formatting.None), Completed =>
            {
                Message.Log();

                if (Completed == false)
                {
                    Lobby.Log.Warning(this.GetType(), "Completed != true at SendMessage(" + Message.Identifier + ").");
                }
            });
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(string Message)
        {
            this.SendAsync(Message, Completed =>
            {
                if (Completed == false)
                {
                    Lobby.Log.Warning(this.GetType(), "Completed != true at SendMessage(string).");
                }
            });
        }
    }
}
