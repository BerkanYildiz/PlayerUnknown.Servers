namespace PlayerUnknown.Logic
{
    using Newtonsoft.Json;

    public sealed class Player
    {
        [JsonProperty("profile")]   public Profile Profile;
        [JsonProperty("inventory")] public Inventory Inventory;
        [JsonProperty("record")]    public Record Record;
        [JsonProperty("account")]   public Account Account;

        [JsonProperty("inviteAllow")]
        public object InviteAllow
        {
            get
            {
                return this.Profile.InviteAllow;
            }
        }

        [JsonProperty("playinggame")]
        public object PlayingGame
        {
            get
            {
                return null;
            }
        }
        
        [JsonProperty("avatarUrl")]
        public string AvatarUrl
        {
            get
            {
                return string.Empty;
            }
        }

        [JsonProperty("lobbyAppConfig")]
        public object LobbyAppConfig
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            this.Profile    = new Profile();
            // this.Inventory  = new Inventory();
            // this.Record     = new Record();
            this.Account    = new Account();
        }
    }
}
