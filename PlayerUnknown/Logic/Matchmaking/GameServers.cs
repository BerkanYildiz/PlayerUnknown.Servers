namespace PlayerUnknown.Logic.Matchmaking
{
    using System;
    using System.Collections.Generic;

    public static class GameServers
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameServers"/> is initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the servers.
        /// </summary>
        public static Dictionary<string, List<GameServer>> Servers
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (GameServers.Initialized)
            {
                return;
            }

            GameServers.Servers     = new Dictionary<string, List<GameServer>>();
            GameServers.Initialized = true;
        }

        /// <summary>
        /// Adds the server.
        /// </summary>
        /// <param name="Server">The server.</param>
        /// <exception cref="Exception">
        /// Server == null at AddServer(Server).
        /// or
        /// RegionServers == null at AddServer(Server).
        /// </exception>
        public static void AddServer(GameServer Server)
        {
            if (Server == null)
            {
                throw new Exception("Server == null at AddServer(Server).");
            }

            if (GameServers.Servers.ContainsKey(Server.Region) == false)
            {
                GameServers.Servers.Add(Server.Region, new List<GameServer>());
            }

            var RegionServers = GameServers.Servers[Server.Region];

            if (RegionServers != null)
            {
                RegionServers.Add(Server);
            }
            else
            {
                throw new Exception("RegionServers == null at AddServer(Server).");
            }
        }

        /// <summary>
        /// Adds the servers.
        /// </summary>
        /// <param name="Servers">The servers.</param>
        public static void AddServers(params GameServer[] Servers)
        {
            if (Servers == null || Servers.Length < 1)
            {
                return;
            }

            foreach (var Server in Servers)
            {
                GameServers.AddServer(Server);
            }
        }
    }
}
