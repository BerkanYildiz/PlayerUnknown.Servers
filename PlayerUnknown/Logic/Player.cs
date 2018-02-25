namespace PlayerUnknown.Logic
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Components;
    using PlayerUnknown.Logic.Configurations;
    using PlayerUnknown.Logic.Interfaces;

    public sealed class Player : IPlayer
    {
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        public IProfile Profile
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        public IInventory Inventory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        public IRecord Record
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        public IAccount Account
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the invite allow.
        /// </summary>
        public string InviteAllow
        {
            get
            {
                return this.Profile.InviteAllow;
            }
            private set
            {
                this.Profile.InviteAllow = value;
            }
        }

        /// <summary>
        /// Gets the playing game.
        /// </summary>
        public string PlayingGame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the avatar URL.
        /// </summary>
        public string AvatarUrl
        {
            get;
            private set;
        } = "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/ee/ee38d09b29f12fc63630d5df4d5eb1c313f075ff_full.jpg";

        /// <summary>
        /// Gets the lobby application configuration.
        /// </summary>
        public IAppConfig LobbyAppConfig
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            this.Profile        = new Profile();
            this.Inventory      = new Inventory();
            this.Record         = new Record();
            this.Account        = new Account();
            this.LobbyAppConfig = new LobbyAppConfig();
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            if (Json.ContainsKey("username"))
            {
                this.Username = Json.GetValue("username").ToObject<string>();
            }

            if (Json.ContainsKey("password"))
            {
                this.Password = Json.GetValue("password").ToObject<string>();
            }

            if (Json.ContainsKey("profile"))
            {
                this.Profile.Load(Json.GetValue("profile").ToObject<JObject>());
            }

            if (Json.ContainsKey("inventory"))
            {
                this.Inventory.Load(Json.GetValue("inventory").ToObject<JObject>());
            }

            if (Json.ContainsKey("record"))
            {
                this.Record.Load(Json.GetValue("record").ToObject<JObject>());
            }

            if (Json.ContainsKey("account"))
            {
                this.Account.Load(Json.GetValue("account").ToObject<JObject>());
            }

            if (Json.ContainsKey("inviteAllow"))
            {
                this.InviteAllow = Json.GetValue("inviteAllow").ToObject<string>();
            }

            if (Json.ContainsKey("playinggame"))
            {
                this.PlayingGame = Json.GetValue("playinggame").ToObject<string>();
            }

            if (Json.ContainsKey("avatarUrl"))
            {
                this.AvatarUrl = Json.GetValue("avatarUrl").ToObject<string>();
            }

            if (Json.ContainsKey("LobbyAppConfig"))
            {
                this.LobbyAppConfig.Load(Json.GetValue("LobbyAppConfig").ToObject<JObject>());
            }
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("username", this.Username);
            Json.Add("password", this.Password);
            Json.Add("profile", this.Profile.Save());
            Json.Add("inventory", this.Inventory.Save());
            Json.Add("record", this.Record.Save());
            Json.Add("account", this.Account.Save());
            Json.Add("inviteAllow", this.Profile.InviteAllow);
            Json.Add("playinggame", this.PlayingGame);
            Json.Add("avatarUrl", this.AvatarUrl);
            Json.Add("LobbyAppConfig", this.LobbyAppConfig.Save());

            return Json;
        }
    }
}
