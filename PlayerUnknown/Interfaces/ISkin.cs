namespace PlayerUnknown.Interfaces
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
        /// Saves this instance into a json object.
        /// </summary>
        JObject ToJson();
    }
}
