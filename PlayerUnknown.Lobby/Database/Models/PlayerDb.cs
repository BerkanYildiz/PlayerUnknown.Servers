namespace PlayerUnknown.Lobby.Database.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using Newtonsoft.Json;

    using PlayerUnknown.Logic.Interfaces;

    public class PlayerDb
    {
        [BsonId]                    public BsonObjectId _id;
        [BsonElement("profile")]    public BsonDocument Content;

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
        public PlayerDb(IPlayer Player)
        {
            if (string.IsNullOrEmpty(Player.Account.AccountId))
            {
                Player.Account.AccountId = ObjectId.GenerateNewId().ToString();
            }

            this._id     = ObjectId.Parse(Player.Account.AccountId);
            this.Content = BsonDocument.Parse(JsonConvert.SerializeObject(Player, GameDb.JsonSettings));
        }
    }
}