namespace PlayerUnknown.Logic.Interfaces.Players
{
    public interface ISkin : IObject
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
    }
}
