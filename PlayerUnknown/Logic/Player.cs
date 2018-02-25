namespace PlayerUnknown.Logic
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Interfaces;
    using PlayerUnknown.Logic.Components;

    public sealed class Player : IPlayer
    {
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        public IProfile Profile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        public IInventory Inventory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        public IRecord Record
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        public IAccount Account
        {
            get;
            set;
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
        }

        /// <summary>
        /// Gets the playing game.
        /// </summary>
        public string PlayingGame
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the avatar URL.
        /// </summary>
        public string AvatarUrl
        {
            get
            {
                return "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/ee/ee38d09b29f12fc63630d5df4d5eb1c313f075ff_full.jpg";
            }
        }

        /// <summary>
        /// Gets the lobby application configuration.
        /// </summary>
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

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject ToJson()
        {
            JObject Json = new JObject();

            Json.Add("username", this.Username);
            Json.Add("password", this.Password);
            Json.Add("profile", this.Profile.ToJson());
            Json.Add("inventory", this.Inventory.ToJson());
            Json.Add("record", this.Record.ToJson());
            Json.Add("account", this.Account.ToJson());
            Json.Add("inviteAllow", this.Profile.InviteAllow);
            Json.Add("playinggame", this.PlayingGame);
            Json.Add("avatarUrl", this.AvatarUrl);
            Json.Add("LobbyAppConfig", null);

            return Json;
        }
    }
}
