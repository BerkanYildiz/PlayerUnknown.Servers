namespace PlayerUnknown.Lobby.Services
{
    using System;
    using System.Linq;
    using System.Reflection;

    using PlayerUnknown.Lobby.Collections;
    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Lobby.Services.Api;
    using PlayerUnknown.Logic;
    using PlayerUnknown.Network;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public sealed class UserProxy : WebSocketBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProxy"/> class.
        /// </summary>
        public UserProxy()
        {
            // UserProxy.
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

            if (Collections.Sessions.TryAdd(PubgSession))
            {
                var Authenticated = await PubgSession.Authenticate(Provider, Ticket, Username, Password, PlayerId, Country, Version);

                if (Authenticated)
                {
                    ClientApi.ConnectionAccepted(PubgSession);
                }
                else
                {
                    Logging.Warning(this.GetType(), "Authenticated != true at OnOpen().");

                    PubgSession.Player = await Players.Create(
                        new Player
                        {
                            Username = Username,
                            Password = Password
                        }
                    );

                    ClientApi.ConnectionAccepted(PubgSession);
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "At OnOpen(), TryAdd(PubgSession) == false, aborting.");
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

            Console.WriteLine("--------------------------");

            if (string.IsNullOrEmpty(ClassName) == false && string.IsNullOrEmpty(MethodName) == false)
            {
                Type Class = Type.GetType("PlayerUnknown.Lobby.Services.Api." + ClassName);

                if (Class != null)
                {
                    MethodInfo Method = Class.GetMethod(MethodName, BindingFlags.Static | BindingFlags.Public);

                    if (Method != null)
                    {
                        PubgSession Session = Collections.Sessions.Get(this.ID);

                        if (Session != null)
                        {
                            Method.Invoke(null, new object[] { Message, Session }); 
                        }
                        else
                        {
                            Logging.Warning(this.GetType(), "PubgSession == null at OnMessage(Args).");
                        }

                        Message.Log();
                    }
                    else
                    {
                        Logging.Warning(this.GetType(), "Method(" + MethodName + ") == null at OnMessage(Args).");
                    }
                }
                else
                {
                    Logging.Warning(this.GetType(), "Class(" + ClassName + ") == null at OnMessage(Args).");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Message.IsValid != true at OnMessage(Args).");
            }
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
                Logging.Warning(this.GetType(), "Args.WasClean != true at OnClose(Args).");
            }

            bool Cleaned = Collections.Sessions.TryRemove(this.ID);

            if (Cleaned == false)
            {
                Logging.Fatal(this.GetType(), "Something is wrong with the server, please put the maintenance mode and check the logs.");
            }
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            this.SendAsync(Message.ToJson(), Completed =>
            {
                Message.Log();

                if (Completed == false)
                {
                    Logging.Warning(this.GetType(), "Completed != true at SendMessage(" + Message.Identifier + ").");
                }
            });
        }
    }
}
