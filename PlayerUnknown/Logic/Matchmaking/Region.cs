namespace PlayerUnknown.Logic.Matchmaking
{
    using System.Collections.Generic;

    public class Region
    {
        /// <summary>
        /// Gets or sets the region of this <see cref="Region"/>.
        /// </summary>
        public string Location
        {
            get;
        }

        /// <summary>
        /// Gets or sets the available game modes for this <see cref="Region"/>.
        /// </summary>
        public Dictionary<string, GameServer> GameModes
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        public Region(string Region)
        {
            this.Location   = Region;
            this.GameModes  = new Dictionary<string, GameServer>(4);
        }
    }
}