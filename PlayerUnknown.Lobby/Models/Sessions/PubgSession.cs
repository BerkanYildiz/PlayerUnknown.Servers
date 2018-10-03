namespace PlayerUnknown.Lobby.Models.Sessions
{
    using PlayerUnknown.Lobby.Services;
    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Components;

    using WebSocketSharp.Server;

    public sealed class PubgSession
    {
        /// <summary>
        /// Gets the parent of this instance, storing datas about the <see cref="IWebSocketSession"/>.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public UserProxy Client
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
            // PubgSession.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgSession"/> class.
        /// </summary>
        /// <param name="UserProxy">The client.</param>
        public PubgSession(UserProxy UserProxy)
        {
            this.Client = UserProxy;
        }
    }
}
