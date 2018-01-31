namespace PlayerUnknown.Lobby.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Network;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    using JsonConvert = Newtonsoft.Json.JsonConvert;

    public sealed class UserProxy : WebSocketBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProxy"/> class.
        /// </summary>
        public UserProxy() : base()
        {
            // UserProxy.
        }

        /// <summary>
        /// Called when the WebSocket connection used in a session has been established.
        /// </summary>
        protected override async void OnOpen()
        {
            Logging.Warning(this.GetType(), "Query : " + this.Context.QueryString.ToString().Replace("&", "  -  "));

            string Provider = this.Context.QueryString.Get("provider");
            string Ticket   = this.Context.QueryString.Get("ticket");
            string Username = this.Context.QueryString.Get("id");
            string Password = this.Context.QueryString.Get("password");
            string PlayerId = this.Context.QueryString.Get("playerNetId");
            string Country  = this.Context.QueryString.Get("cc");
            string Version  = this.Context.QueryString.Get("clientGameVersion");

            if (this.Sessions.TryGetSession(this.ID, out IWebSocketSession Session))
            {
                var PubgSession = new PubgSession(Session);

                if (Collections.Sessions.TryAdd(PubgSession))
                {
                    var Authenticated = await PubgSession.Authenticate(Provider, Ticket, Username, Password, PlayerId, Country, Version);

                    if (Authenticated)
                    {
                        var Packet = new Message
                        {
                            Service = "ClientApi",
                            Method  = "ConnectionAccepted"
                        };

                        Packet.Parameters.Add(PubgSession.Account.AccountId);
                        Packet.Parameters.Add(JObject.Parse(JsonConvert.SerializeObject(PubgSession.Player)));

                        this.SendAsync(Packet.ToJson(), Completed =>
                        {
                            Packet.Log();
                        });
                    }
                    else
                    {
                        Logging.Warning(this.GetType(), "Player is not authenticated.");
                    }
                }
                else
                {
                    Logging.Warning(this.GetType(), "At OnOpen(), TryAdd(PubgSession) == false, aborting.");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "TryGetSession(ID, out IWebSocketSession) != true at UserProxy.OnOpen().");
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

            Message Message = new Message(Args.Data);

            if (Message.IsValid)
            {
                Type Class = Type.GetType("PlayerUnknown.Lobby.Services.Api." + Message.Service);

                if (Class != null)
                {
                    MethodInfo Method = Class.GetMethod(Message.Method, BindingFlags.Static | BindingFlags.Public);

                    if (Method != null)
                    {
                        Method.Invoke(null, new object[] { Args, Message });
                    }
                    else
                    {
                        Logging.Warning(this.GetType(), "Method == null at OnMessage(Args).");
                    }
                }
                else
                {
                    Logging.Warning(this.GetType(), "Class(" + Message.Service + ") == null at OnMessage(Args).");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Message.IsValid != true at OnMessage(Args).");
            }
        }
    }
}
