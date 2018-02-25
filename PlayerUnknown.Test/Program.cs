namespace PlayerUnknown.Test
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Interfaces;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        internal static void Main()
        {
            IPlayer Player  = new Player();
            JObject Json    = Player.Save();

            if (Json != null)
            {
                Console.WriteLine(Json.ToString());
            }

            Console.ReadKey(false);
        }
    }
}
