namespace PlayerUnknown.Lobby.Proxy.Models.Sessions
{
    using System;
    using System.Reflection;
    using System.Security.Authentication;

    using PlayerUnknown.Lobby.Proxy.Services;
    using PlayerUnknown.Network;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public sealed class PubgSession
    {
        [Flags]
        private enum SslProtocolsHack
        {
            Tls     = 192,
            Tls11   = 768,
            Tls12   = 3072
        }

        /// <summary>
        /// Gets the parent of this instance, storing datas about the <see cref="IWebSocketSession"/>.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public IWebSocketSession Parent
        {
            get;
        }

        /// <summary>
        /// Gets the unique ID of the session.
        /// </summary>
        /// <value>
        /// A <see cref="T:System.String" /> that represents the unique ID of the session.
        /// </value>
        public string ID
        {
            get
            {
                return this.Parent.ID;
            }
        }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public WebSocket Server
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public UserProxy Client
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgSession"/> class.
        /// </summary>
        public PubgSession()
        {
            // PubgSession.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgSession"/> class.
        /// </summary>
        /// <param name="Session">The session.</param>
        /// <param name="Client">The client.</param>
        public PubgSession(IWebSocketSession Session, UserProxy Client)
        {
            this.Parent = Session;
            this.Client = Client;
        }

        /// <summary>
        /// Connects this <see cref="PubgSession"/> to official server.
        /// </summary>
        public void ConnectToServer(string Query)
        {
            Logging.Info(this.GetType(), "ConnectToServer(" + Query + ");");

            this.Server              = new WebSocket("wss://prod-live-entry.playbattlegrounds.com/userproxy?" + Query);
            this.Server.Origin       = "https://prod-live-front.playbattlegrounds.com";
            this.Server.OnOpen      += OnOpen;
            this.Server.OnMessage   += OnMessage;
            this.Server.OnError     += OnError;
            this.Server.SslConfiguration.EnabledSslProtocols = (SslProtocols) (SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
            this.Server.Connect();
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="ErrorEventArgs">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private void OnError(object Sender, ErrorEventArgs ErrorEventArgs)
        {
            Logging.Error(this.GetType(), ErrorEventArgs.Message);
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/> to the server.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessageToServer(Message Message)
        {
            this.Server.SendAsync(Message.ToJson(), Completed =>
            {
                Message.Log();
            });
        }

        /// <summary>
        /// Called when [connection].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="EventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnOpen(object Sender, EventArgs EventArgs)
        {
            Logging.Info(this.GetType(), "Connected to the official server.");
        }

        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The <see cref="MessageEventArgs"/> instance containing the event data.</param>
        private void OnMessage(object Sender, MessageEventArgs Args)
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

            this.Client.SendMessage(Message);
        }
    }
}
