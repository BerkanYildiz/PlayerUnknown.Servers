namespace PlayerUnknown.Files.Traductions.Values
{
    using Newtonsoft.Json.Linq;

    public class ItemTranslation
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        public int ItemId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the game item short identifier.
        /// </summary>
        public string GameItemShortId
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTranslation"/> class.
        /// </summary>
        public ItemTranslation()
        {
            // ItemTranslation.
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            if (Json.ContainsKey("item_id"))
            {
                this.ItemId = Json.GetValue("item_id").ToObject<int>();
            }

            if (Json.ContainsKey("name"))
            {
                this.Name = Json.GetValue("name").ToObject<string>();
            }

            if (Json.ContainsKey("description"))
            {
                this.Description = Json.GetValue("description").ToObject<string>();
            }

            if (Json.ContainsKey("game_item_short_id"))
            {
                this.GameItemShortId = Json.GetValue("game_item_short_id").ToObject<string>();
            }
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("item_id", this.ItemId);
            Json.Add("name", this.Name);
            Json.Add("description", this.Description);
            Json.Add("game_item_short_id", this.GameItemShortId);

            return Json;
        }
    }
}
