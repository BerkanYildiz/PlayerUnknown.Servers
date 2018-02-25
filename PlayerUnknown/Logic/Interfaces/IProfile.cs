namespace PlayerUnknown.Logic.Interfaces
{
    using Newtonsoft.Json.Linq;

    public interface IProfile
    {
        /// <summary>
        /// Gets or sets the nickname.
        /// </summary>
        string Nickname
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the profile status.
        /// </summary>
        string ProfileStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the invite allow.
        /// </summary>
        string InviteAllow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        ISkin Skin
        {
            get;
            set;
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        void Load(JObject Json);

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        JObject Save();
    }
}
