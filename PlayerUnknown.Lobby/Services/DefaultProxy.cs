namespace PlayerUnknown.Lobby.Services
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public sealed class DefaultProxy : WebSocketBehavior
    {
        /// <summary>
        /// Gets the server.
        /// </summary>
        private PubgLobbyServer Server
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProxy"/> class.
        /// </summary>
        /// <param name="Server">The server.</param>
        public DefaultProxy(PubgLobbyServer Server)
        {
            this.Server     = Server;

            Lobby.Log.Info(this.GetType(), "DefaultProxy has been initalized.");
        }
        
        /// <summary>
        /// Called when the WebSocket connection used in a session has been established.
        /// </summary>
        protected override void OnOpen()
        {
            Lobby.Log.Warning(typeof(DefaultProxy), "Query : ");

            foreach (var Key in this.Context.QueryString.AllKeys)
            {
                Lobby.Log.Warning(typeof(DefaultProxy), " - " + Key + " -> " + this.Context.QueryString.Get(Key));
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

            var Json = JObject.Parse(Args.Data);
            Lobby.Log.Warning(typeof(DefaultProxy), "Json : " + Json.ToString(Formatting.Indented));

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

            if (Args.WasClean == true)
            {
                Lobby.Log.Warning(this.GetType(), "Args.WasClean == true at OnClose(Args).");
            }

            bool Cleaned = this.Server.Sessions.TryRemove(this.ID);

            if (Cleaned == false)
            {
                Lobby.Log.Fatal(this.GetType(), "Something is wrong with the server, please put the maintenance mode and check the logs.");
            }
        }
    }
}
