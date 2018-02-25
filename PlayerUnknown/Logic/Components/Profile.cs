namespace PlayerUnknown.Logic.Components
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Interfaces;

    public sealed class Profile : IProfile
    {
        /// <summary>
        /// Gets or sets the nickname.
        /// </summary>
        public string Nickname
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the profile status.
        /// </summary>
        public string ProfileStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the invite allow.
        /// </summary>
        public string InviteAllow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        public ISkin Skin
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        public Profile()
        {
            this.Skin = new Skin();
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            if (Json.ContainsKey("Nickname"))
            {
                this.Nickname = Json.GetValue("Nickname").ToObject<string>();
            }

            if (Json.ContainsKey("ProfileStatus"))
            {
                this.ProfileStatus = Json.GetValue("ProfileStatus").ToObject<string>();
            }

            if (Json.ContainsKey("InviteAllow"))
            {
                this.InviteAllow = Json.GetValue("InviteAllow").ToObject<string>();
            }

            if (Json.ContainsKey("Skin"))
            {
                this.Skin.Load(Json.GetValue("Skin").ToObject<JObject>());
            }
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("Nickname", this.Nickname);
            Json.Add("ProfileStatus", this.ProfileStatus);
            Json.Add("InviteAllow", this.InviteAllow);
            Json.Add("Skin", this.Skin.Save());

            return Json;
        }
    }
}
