namespace PlayerUnknown.Lobby.Services
{
    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Logic;

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
                        this.Send("[0,null,\"ClientApi\",\"ConnectionAccepted\",\"account.d97a9d0dc25948f18348816373392734\", " + JsonConvert.SerializeObject(PubgSession.Player) + "]");
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

            Logging.Info(this.GetType(), Args.Data + ".");
        }
    }
}
