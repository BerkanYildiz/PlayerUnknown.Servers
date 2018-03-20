namespace PlayerUnknown.Logic.Matchmaking
{
    public class GameServer
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameServer"/> is initialized.
        /// </summary>
        public bool IsAvailable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        public string Hostname
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version
        {
            get;
            set;
        }
    }
}