namespace PlayerUnknown.Test
{
    using System;

    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Lobby;
    using PlayerUnknown.LobbyProxy;
    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Interfaces;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Program.StartServer(new Config(ServerPort: 11111));
            Console.ReadKey();
        }

        /// <summary>
        /// Tests the player save.
        /// </summary>
        private static void TestPlayerSave()
        {
            IPlayer Player  = new Player();
            JObject Json    = Player.Save();

            if (Json != null)
            {
                Console.WriteLine(Json);
            }
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        private static async void StartServer(Config Config)
        {
            using (PubgLobbyServer PubgLobbyServer = new PubgLobbyServer(Config))
            {
                PubgLobbyServer.Start();

                if (PubgLobbyServer.IsListening)
                {
                    await PubgLobbyServer.Wait();
                }
            }
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        private static async void StartProxy()
        {
            using (PubgLobbyProxy PubgLobbyProxy = new PubgLobbyProxy())
            {
                PubgLobbyProxy.Start();

                if (PubgLobbyProxy.IsListening)
                {
                    await PubgLobbyProxy.Wait();
                }
            }
        }
    }
}
