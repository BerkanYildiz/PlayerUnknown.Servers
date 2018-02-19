namespace PlayerUnknown.Logic.Matchmaking
{
    using System;
    using System.Collections.Generic;

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
        /// Gets or sets the region of this <see cref="GameServer"/>.
        /// </summary>
        public string Region
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the available game modes for this <see cref="GameServer"/>.
        /// </summary>
        public List<string> GameModes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current version of this <see cref="GameServer"/>.
        /// </summary>
        public string Version
        {
            get
            {
                return DateTime.UtcNow.ToString("yyyy-dd");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServer"/> class.
        /// </summary>
        public GameServer()
        {
            this.GameModes = new List<string>(4);
        }

        /// <summary>
        /// Sets the available modes.
        /// </summary>
        /// <param name="Modes">The modes.</param>
        public void SetAvailableModes(params string[] Modes)
        {
            if (Modes == null || Modes.Length < 1)
            {
                return;
            }

            this.GameModes.Clear();
            this.GameModes.AddRange(Modes);
        }

        /// <summary>
        /// Sets the region.
        /// </summary>
        /// <param name="Region">The region.</param>
        public void SetRegion(string Region)
        {
            if (string.IsNullOrEmpty(Region))
            {
                return;
            }

            this.Region = Region;
        }
    }
}