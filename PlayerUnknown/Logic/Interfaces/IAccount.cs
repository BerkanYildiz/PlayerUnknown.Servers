namespace PlayerUnknown.Logic.Interfaces
{
    using Newtonsoft.Json.Linq;

    public interface IAccount
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        string AccountId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this player is a partner.
        /// </summary>
        bool IsPartner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the outer source.
        /// </summary>
        string OuterSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the partner identifier.
        /// </summary>
        string PartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the partner level.
        /// </summary>
        string PartnerLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the player net identifier.
        /// </summary>
        string PlayerNetId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user serial.
        /// </summary>
        string UserSerial
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        string Region
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
