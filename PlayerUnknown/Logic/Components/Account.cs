namespace PlayerUnknown.Logic.Components
{
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Interfaces;
    using PlayerUnknown.Logic.Interfaces.Players;

    public sealed class Account : IAccount
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        public string AccountId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this player is a partner.
        /// </summary>
        public bool IsPartner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the outer source.
        /// </summary>
        public string OuterSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the partner identifier.
        /// </summary>
        public string PartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the partner level.
        /// </summary>
        public string PartnerLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the player net identifier.
        /// </summary>
        public string PlayerNetId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user serial.
        /// </summary>
        public string UserSerial
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region
        {
            get;
            set;
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            if (Json.ContainsKey("AccountId"))
            {
                this.AccountId = Json.GetValue("AccountId").ToObject<string>();
            }

            if (Json.ContainsKey("AppId"))
            {
                this.AppId = Json.GetValue("AppId").ToObject<string>();
            }

            if (Json.ContainsKey("IsPartner"))
            {
                this.IsPartner = Json.GetValue("IsPartner").ToObject<bool>();
            }

            if (Json.ContainsKey("OuterSource"))
            {
                this.OuterSource = Json.GetValue("OuterSource").ToObject<string>();
            }

            if (Json.ContainsKey("PartnerId"))
            {
                this.PartnerId = Json.GetValue("PartnerId").ToObject<string>();
            }

            if (Json.ContainsKey("PartnerLevel"))
            {
                this.PartnerLevel = Json.GetValue("PartnerLevel").ToObject<string>();
            }

            if (Json.ContainsKey("PlayerNetId"))
            {
                this.PlayerNetId = Json.GetValue("PlayerNetId").ToObject<string>();
            }

            if (Json.ContainsKey("UserSerial"))
            {
                this.UserSerial = Json.GetValue("UserSerial").ToObject<string>();
            }

            if (Json.ContainsKey("Region"))
            {
                this.Region = Json.GetValue("Region").ToObject<string>();
            }
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("AccountId",  this.AccountId);
            Json.Add("AppId", this.AppId);
            Json.Add("IsPartner", this.IsPartner);
            Json.Add("OuterSource", this.OuterSource);
            Json.Add("PartnerId", this.PartnerId);
            Json.Add("PartnerLevel", this.PartnerLevel);
            Json.Add("PlayerNetId", this.PlayerNetId);
            Json.Add("UserSerial", this.UserSerial);
            Json.Add("Region", this.Region);

            return Json;
        }
    }
}
