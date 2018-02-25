namespace PlayerUnknown.Logic.Interfaces
{
    using Newtonsoft.Json.Linq;

    public interface ISkin
    {
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        string Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hair.
        /// </summary>
        string Hair
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the face.
        /// </summary>
        string Face
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the presets.
        /// </summary>
        string Presets
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
