namespace PlayerUnknown.Lobby
{
    using System;

    using PlayerUnknown.Lobby.Collections;
    using PlayerUnknown.Lobby.Database;
    using PlayerUnknown.Lobby.Services;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            GameDb.Initialize();
            Sessions.Initialize();
            Players.Initialize();

            var Server = new WebSocketServer(81);
            Server.AddWebSocketService<UserProxy>("/userproxy");
            Server.Start();

            Console.ReadKey(false);
        }
    }
}
