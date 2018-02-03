namespace PlayerUnknown.Lobby.Proxy
{
    using System;

    using PlayerUnknown.Lobby.Proxy.Collections;
    using PlayerUnknown.Lobby.Proxy.Services;

    using WebSocketSharp.Server;

    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            Sessions.Initialize();

            var Server = new WebSocketServer(81);
            Server.AddWebSocketService<UserProxy>("/userproxy");
            Server.Start();

            Console.ReadKey(false);
        }
    }
}
