namespace PlayerUnknown.Lobby.Database
{
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    using PlayerUnknown.Lobby.Database.Models;
    using PlayerUnknown.Logic;

    public class GameDb
    {
        /// <summary>
        /// The settings for the <see cref="JsonConvert" /> class.
        /// </summary>
        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.None,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            ReferenceLoopHandling       = ReferenceLoopHandling.Ignore,     Formatting              = Formatting.None
        };

        /// <summary>
        /// Gets the collection of players currently in the database.
        /// </summary>
        public IMongoCollection<PlayerDb> Players
        {
            get;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public GameDb()
        {
            var MongoClient     = new MongoClient("mongodb://GL.Servers.PUBG:DlzpSSAVkFYym6q9A0sJ7YhBTNXxnVycN74Eozl3@ns1.gobelinland.fr:27017");
            var MongoDb         = MongoClient.GetDatabase("PUBG");

            Logging.Info(this.GetType(), "GameDb is connected to " + MongoClient.Settings.Server.Host + ".");

            if (MongoDb.GetCollection<PlayerDb>("Players") == null)
            {
                MongoDb.CreateCollection("Players");
            }

            this.Players        = MongoDb.GetCollection<PlayerDb>("Players");
        }

        /// <summary>
        /// Creates the specified player.
        /// </summary>
        /// <param name="Player">The player.</param>
        public async Task Create(Player Player)
        {
            await this.Players.InsertOneAsync(new PlayerDb(Player));
        }

        /// <summary>
        /// Creates the player in the database.
        /// </summary>
        public async Task<PlayerDb> Save(Player Player)
        {
            var UpdatedEntity = await this.Players.FindOneAndUpdateAsync(PlayerDb =>
                PlayerDb._id == Player.Account.AccountId,
                Builders<PlayerDb>.Update.Set(PlayerDb => PlayerDb.Content, BsonDocument.Parse(JsonConvert.SerializeObject(Player, GameDb.JsonSettings)))
            );

            if (UpdatedEntity != null)
            {
                if (UpdatedEntity._id == Player.Account.AccountId)
                {
                    return UpdatedEntity;
                }
                else
                {
                    Logging.Error(typeof(PlayerDb), "UpdatedEntity.Ids != this.Ids at Save().");
                }
            }
            else
            {
                Logging.Error(typeof(PlayerDb), "UpdatedEntity == null at Save().");
            }

            return null;
        }

        /// <summary>
        /// Loads this instance from the database.
        /// </summary>
        public async Task<PlayerDb> Load(string AccountId)
        {
            if (!string.IsNullOrEmpty(AccountId))
            {
                var Entities = await this.Players.FindAsync(Player => Player._id == AccountId);

                if (Entities != null)
                {
                    var Entity = Entities.FirstOrDefault();

                    if (Entity != null)
                    {
                        return Entity;
                    }
                    else
                    {
                        Logging.Error(typeof(PlayerDb), "Entity == null at Load().");
                    }
                }
                else
                {
                    Logging.Error(typeof(PlayerDb), "Entities == null at Load().");
                }
            }
            else
            {
                Logging.Error(typeof(PlayerDb), "this.LowId < 0 at Load().");
            }

            return null;
        }

        /// <summary>
        /// Loads this instance from the database.
        /// </summary>
        public async Task<PlayerDb> Load(string Username, string Password)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                var Entities = await this.Players.FindAsync(Player => Player.Content["username"] == Username);

                if (Entities != null)
                {
                    var Entity = Entities.FirstOrDefault();

                    if (Entity != null)
                    {
                        return Entity;
                    }
                    else
                    {
                        Logging.Error(typeof(PlayerDb), "Entity == null at Load().");
                    }
                }
                else
                {
                    Logging.Error(typeof(PlayerDb), "Entities == null at Load().");
                }
            }
            else
            {
                Logging.Error(typeof(PlayerDb), "this.LowId < 0 at Load().");
            }

            return null;
        }

        /// <summary>
        /// Deletes this instance from the database.
        /// </summary>
        public async Task<bool> Delete(string AccountId)
        {
            if (!string.IsNullOrEmpty(AccountId))
            {
                var Result = await this.Players.DeleteOneAsync(PlayerDb => PlayerDb._id == AccountId);

                if (Result.IsAcknowledged)
                {
                    if (Result.DeletedCount > 0)
                    {
                        if (Result.DeletedCount == 1)
                        {
                            return true;
                        }
                        else
                        {
                            Logging.Error(typeof(PlayerDb), "Result.DeletedCount > 1 at Delete().");
                        }
                    }
                    else
                    {
                        Logging.Warning(typeof(PlayerDb), "Result.DeletedCount == 0 at Delete().");
                    }
                }
                else
                {
                    Logging.Error(typeof(PlayerDb), "Result.IsAcknowledged != true at Delete().");
                }
            }
            else
            {
                Logging.Error(typeof(PlayerDb), "LowId <= 0 at Delete(HighId, LowId).");
            }

            return false;
        }

        /// <summary>
        /// Deserializes the specified entity.
        /// </summary>
        public bool Deserialize(PlayerDb PlayerDb, out Player Player)
        {
            if (PlayerDb.Content != null)
            {
                Player = JsonConvert.DeserializeObject<Player>(PlayerDb.Content.ToJson(), GameDb.JsonSettings);

                if (Player != null)
                {
                    return true;
                }
            }
            else
            {
                Player = null;
            }

            return false;
        }
    }
}
