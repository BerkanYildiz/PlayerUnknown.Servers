namespace PlayerUnknown.Lobby
{
    using System;

    using PlayerUnknown.Files;
    using PlayerUnknown.Lobby.Collections;
    using PlayerUnknown.Lobby.Database;
    using PlayerUnknown.Lobby.Services;
    using PlayerUnknown.Logic.Matchmaking;

    using WebSocketSharp.Server;

    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            Home.Initialize();
            GameDb.Initialize();
            Sessions.Initialize();
            Players.Initialize();
            GameServers.Initialize();

            var Server = new WebSocketServer(81);
            Server.AddWebSocketService<UserProxy>("/userproxy");
            Server.Start();

            Console.ReadKey(false);
        }
    }
}
