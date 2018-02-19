namespace PlayerUnknown.Logic
{
    using Newtonsoft.Json;

    using PlayerUnknown.Logic.Components;

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Player
    {
        [JsonProperty("username")]  public string Username;
        [JsonProperty("password")]  public string Password;

        [JsonProperty("profile")]   public Profile Profile;
        [JsonProperty("inventory")] public Inventory Inventory;
        [JsonProperty("record")]    public Record Record;
        [JsonProperty("account")]   public Account Account;

        [JsonProperty("inviteAllow")]
        public string InviteAllow
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
                return "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/ee/ee38d09b29f12fc63630d5df4d5eb1c313f075ff_full.jpg";
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
            this.Inventory  = new Inventory();
            this.Record     = new Record();
            this.Account    = new Account();
        }
    }
}
