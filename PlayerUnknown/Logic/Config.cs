namespace PlayerUnknown.Logic
{
    public class Config
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="MaxPlayers">The maximum players.</param>
        /// <param name="ServerId">The server identifier.</param>
        /// <param name="ServerPort">The server port.</param>
        public Config(int MaxPlayers = 1, int ServerId = 1, int ServerPort = 81)
        {
            this.MaxPlayers = MaxPlayers;
            this.ServerId   = ServerId;
            this.ServerPort = ServerPort;
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
        /// Gets or sets the server port.
        /// </summary>
        public int ServerPort
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
