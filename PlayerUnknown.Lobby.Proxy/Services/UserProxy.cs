namespace PlayerUnknown.Lobby.Proxy.Services
{
    using System;
    using System.Reflection;

    using PlayerUnknown.Lobby.Proxy.Models.Sessions;
    using PlayerUnknown.Network;

    using WebSocketSharp;
    using WebSocketSharp.Server;

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
        protected override void OnOpen()
        {
            Logging.Info(this.GetType(), "OnOpen();");

            if (this.Sessions.TryGetSession(this.ID, out IWebSocketSession Session))
            {
                var PubgSession = new PubgSession(Session, this);

                if (Collections.Sessions.TryAdd(PubgSession))
                {
                    PubgSession.ConnectToServer(this.Context.QueryString.ToString());
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
            Logging.Info(this.GetType(), "OnMessage();");

            if (Args.IsPing)
            {
                return;
            }

            Message Message = new Message(Args.Data);

            if (Message.IsValid)
            {
                Type Class = Type.GetType("PlayerUnknown.Lobby.Proxy.Services.Api." + Message.Service);

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
            
            var PubgSession = Collections.Sessions.Get(this.ID);

            if (PubgSession != null)
            {
                PubgSession.SendMessageToServer(Message);
            }
            else
            {
                Logging.Warning(this.GetType(), "At OnOpen(), TryAdd(PubgSession) == false, aborting.");
            }
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/> to the client.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            this.SendAsync(Message.ToJson(), Completed =>
            {
                Message.Log();
            });
        }
    }
}
