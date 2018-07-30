namespace PlayerUnknown.LobbyProxy.Services
{
    using Newtonsoft.Json;

    using PlayerUnknown.LobbyProxy.Models.Sessions;
    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Network;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public sealed class ClientProxy : WebSocketBehavior
    {
        /// <summary>
        /// Gets the proxy.
        /// </summary>
        private PubgLobbyProxy Proxy
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientProxy"/> class.
        /// </summary>
        /// <param name="Proxy">The proxy.</param>
        public ClientProxy(PubgLobbyProxy Proxy)
        {
            this.Proxy = Proxy;
        }

        /// <summary>
        /// Called when the WebSocket connection used in a session has been established.
        /// </summary>
        protected override async void OnOpen()
        {
            Logging.Info(this.GetType(), " - " + string.Join("    ", this.Context.QueryString.ToString().Split('&')));

            string Provider     = this.Context.QueryString.Get("provider");
            string Ticket       = this.Context.QueryString.Get("ticket");
            string Username     = this.Context.QueryString.Get("id");
            string Password     = this.Context.QueryString.Get("password");
            string PlayerId     = this.Context.QueryString.Get("playerNetId");
            string Country      = this.Context.QueryString.Get("cc");
            string Version      = this.Context.QueryString.Get("clientGameVersion");
            string FullVersion  = this.Context.QueryString.Get("fullClientGameVersion");

            var PubgSession     = new PubgSession(this);

            if (this.Proxy.Sessions.TryAdd(PubgSession))
            {
                PubgSession.Player = new Player
                {
                    Username = Username,
                    Password = Password
                };

                PubgSession.ConnectToOfficialServer(this.Context.RequestUri.PathAndQuery);
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

            Message Message = new Message(Args.Data);

            Logging.Info(this.GetType(), "Received a message from the client.");
            Logging.Info(this.GetType(), Args.Data);
        }

        /// <summary>
        /// Called when the WebSocket connection used in a session has been closed.
        /// </summary>
        /// <param name="Args">A <see cref="T:WebSocketSharp.CloseEventArgs" /> that represents the event data passed to
        /// a <see cref="E:WebSocketSharp.WebSocket.OnClose" /> event.</param>
        protected override void OnClose(CloseEventArgs Args)
        {
            base.OnClose(Args);

            bool Cleaned = this.Proxy.Sessions.TryRemove(this.ID);

            if (Cleaned == false)
            {
                Logging.Fatal(this.GetType(), "Something is wrong with the proxy, please put the maintenance mode and check the logs.");
            }

            Logging.Warning(this.GetType(), "The client closed the connection.");
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            this.SendAsync(Message.Save().ToString(Formatting.None), Completed =>
            {
                Message.Log();

                if (Completed)
                {
                    Logging.Info(this.GetType(), "Sent a message to the client.");
                }
                else
                {
                    Logging.Warning(this.GetType(), "Completed != true at Client.SendMessage(" + Message.Identifier + ").");
                }
            });
        }
    }
}
