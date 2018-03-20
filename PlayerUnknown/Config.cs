namespace PlayerUnknown
{
    public class Config
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="MaxPlayers">The maximum players.</param>
        /// <param name="ServerId">The server identifier.</param>
        public Config(int MaxPlayers = 1, int ServerId = 1)
        {
            this.MaxPlayers = MaxPlayers;
            this.ServerId   = ServerId;
        }

        /// <summary>
        /// Gets or sets the maximum players.
        /// </summary>
        public int MaxPlayers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the server identifier.
        /// </summary>
        public int ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default configuration.
        /// </summary>
        public static Config Default
        {
            get
            {
                return new Config();
            }
        }
    }
}
