namespace PlayerUnknown.Logic.Components.Subs
{
    using Newtonsoft.Json;

    public sealed class Skin
    {
        [JsonProperty("Gender")]    public string Gender;
        [JsonProperty("Hair")]      public string Hair;
        [JsonProperty("Face")]      public string Face;
        [JsonProperty("Presets")]   public string Presets;

        /// <summary>
        /// Initializes a new instance of the <see cref="Skin"/> class.
        /// </summary>
        public Skin()
        {
            this.SetDefault();
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        public void SetDefault()
        {
            this.Gender     = "male";
            this.Hair       = "skindesc.male.hair.02.02";
            this.Face       = "skindesc.male.face.01.01";
            this.Presets    = "male:M_Hair_B_02:M_Face_01:M_NudeBody_01";
        }
    }
}
