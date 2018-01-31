namespace PlayerUnknown.Logic
{
    using Newtonsoft.Json;

    public sealed class Profile
    {
        [JsonProperty("Nickname")]      public string Nickname;
        [JsonProperty("ProfileStatus")] public object ProfileStatus;
        [JsonProperty("InviteAllow")]   public object InviteAllow;
        [JsonProperty("Skin")]          public Skin Skin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        public Profile()
        {
            this.Nickname = "Developer";
            this.Skin = new Skin();
        }
    }
}
