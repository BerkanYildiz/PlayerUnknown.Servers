namespace PlayerUnknown.Lobby.Models.Sessions
{
    using System;
    using System.Threading.Tasks;

    using PlayerUnknown.Logic;

    using WebSocketSharp;
    using WebSocketSharp.Net.WebSockets;
    using WebSocketSharp.Server;

    public sealed class PubgSession
    {
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
                return this.Player?.Account;
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
                return this.Player?.Profile;
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
        /// <param name="Session">The session.</param>
        public PubgSession(IWebSocketSession Session)
        {
            this.Parent = Session;
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
        public async Task<bool> Authenticate(string Provider, string Ticket, string Username, string Password, string PlayerNetId, string CountryCode, string Version)
        {
            this.Player = new Player();

            if (Provider == "bro")
            {
                this.Account.Password   = Password;
                this.Account.Region     = "EU";
            }
            else if (Provider == "steam")
            {
                // Steam, use Ticket + NetId
            }

            return true;
        }
    }
}
