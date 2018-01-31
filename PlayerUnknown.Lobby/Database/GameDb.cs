namespace PlayerUnknown.Lobby.Database
{
    using MongoDB.Driver;

    using PlayerUnknown.Lobby.Database.Models;

    public static class GameDb
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameDb"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        public static IMongoCollection<PlayerDb> Players;
        public static IMongoCollection<BattleDb> Battles;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (GameDb.Initialized)
            {
                return;
            }

            var MongoClient     = new MongoClient("mongodb://GL.Servers.PUBG:DlzpSSAVkFYym6q9A0sJ7YhBTNXxnVycN74Eozl3@ns1.gobelinland.fr:27017");
            var MongoDb         = MongoClient.GetDatabase("PUBG");

            Logging.Info(typeof(GameDb), "GameDb is connected to " + MongoClient.Settings.Server.Host + ".");

            if (MongoDb.GetCollection<PlayerDb>("Players") == null)
            {
                MongoDb.CreateCollection("Players");
            }

            if (MongoDb.GetCollection<BattleDb>("Battles") == null)
            {
                MongoDb.CreateCollection("Battles");
            }

            GameDb.Players      = MongoDb.GetCollection<PlayerDb>("Players");
            GameDb.Battles      = MongoDb.GetCollection<BattleDb>("Battles");
            GameDb.Initialized  = true;
        }
    }
}
