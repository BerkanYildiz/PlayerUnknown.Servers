namespace PlayerUnknown.LobbyProxy.Models.Sessions
{
    using PlayerUnknown.LobbyProxy.Services;
    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Components;

    using WebSocketSharp.Server;
    using WebSocketSharp;

    public sealed class PubgSession
    {
        /// <summary>
        /// Gets the client <see cref="ClientProxy"/> of this instance, storing datas about the <see cref="IWebSocketSession"/>.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public ClientProxy Client
        {
            get;
        }

        /// <summary>
        /// Gets the server <see cref="WebSocket"/> of this instance, storing datas about the <see cref="IWebSocketSession"/>.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public ServerProxy Server
        {
            get;
            private set;
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
                return this.Client?.ID;
            }
        }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public Account Account
        {
            get
            {
                return (Account) this.Player?.Account;
            }
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public Profile Profile
        {
            get
            {
                return (Profile) this.Player?.Profile;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return this.Player != null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgSession"/> class.
        /// </summary>
        public PubgSession()
        {
            this.Server = new ServerProxy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgSession"/> class.
        /// </summary>
        /// <param name="UserProxy">The client.</param>
        public PubgSession(ClientProxy UserProxy) : this()
        {
            this.Client = UserProxy;
        }

        /// <summary>
        /// Connects to the official server.
        /// </summary>
        public void ConnectToOfficialServer(string Query)
        {
            Logging.Warning(this.GetType(), $"Connect(\"{Query}\").");
            this.Server.Connect(Query);
        }
    }
}
