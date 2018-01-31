namespace PlayerUnknown.Lobby.Database.Models
{
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    using PlayerUnknown.Logic;

    public class PlayerDb
    {
        /// <summary>
        /// The settings for the <see cref="JsonConvert" /> class.
        /// </summary>
        [BsonIgnore]
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.None,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            ReferenceLoopHandling       = ReferenceLoopHandling.Ignore,     Formatting              = Formatting.None
        };

        [BsonId]                    public BsonObjectId _id;
        [BsonElement("profile")]    public BsonDocument Profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDb"/> class.
        /// </summary>
        public PlayerDb()
        {
            // PlayerDb.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDb"/> class.
        /// </summary>
        /// <param name="Player">The player.</param>
        public PlayerDb(Player Player)
        {
            if (string.IsNullOrEmpty(Player.Account.AccountId))
            {
                Player.Account.AccountId = ObjectId.GenerateNewId().ToString();
            }

            this._id     = ObjectId.Parse(Player.Account.AccountId);
            this.Profile = BsonDocument.Parse(JsonConvert.SerializeObject(Player, PlayerDb.JsonSettings));
        }

        /// <summary>
        /// Creates the specified player.
        /// </summary>
        /// <param name="Player">The player.</param>
        public static async Task Create(Player Player)
        {
            await GameDb.Players.InsertOneAsync(new PlayerDb(Player));
        }

        /// <summary>
        /// Creates the player in the database.
        /// </summary>
        public static async Task<PlayerDb> Save(Player Player)
        {
            var UpdatedEntity = await GameDb.Players.FindOneAndUpdateAsync(PlayerDb =>
                PlayerDb._id == Player.Account.AccountId,
                Builders<PlayerDb>.Update.Set(PlayerDb => PlayerDb.Profile, BsonDocument.Parse(JsonConvert.SerializeObject(Player, PlayerDb.JsonSettings)))
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
        public static async Task<PlayerDb> Load(string AccountId)
        {
            if (!string.IsNullOrEmpty(AccountId))
            {
                var Entities = await GameDb.Players.FindAsync(Player => Player._id == AccountId);

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
        public static async Task<bool> Delete(string AccountId)
        {
            if (!string.IsNullOrEmpty(AccountId))
            {
                var Result = await GameDb.Players.DeleteOneAsync(PlayerDb => PlayerDb._id == AccountId);

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
        public bool Deserialize(out Player Player)
        {
            if (this.Profile != null)
            {
                Player = JsonConvert.DeserializeObject<Player>(this.Profile.ToJson(), PlayerDb.JsonSettings);

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