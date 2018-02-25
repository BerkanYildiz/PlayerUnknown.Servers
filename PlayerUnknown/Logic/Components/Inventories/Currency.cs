namespace PlayerUnknown.Logic.Components.Inventories
{
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Interfaces;

    public sealed class Currency : ICurrency, IObject
    {
        /// <summary>
        /// Gets or sets the currency identifier.
        /// </summary>
        public string CurrencyId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public int Amount
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        public Currency()
        {
            // Currency.
        }

        /// <summary>
        /// Sets the amount.
        /// </summary>
        /// <param name="Amount">The amount.</param>
        public void SetAmount(int Amount)
        {
            this.Amount = Amount;
        }

        /// <summary>
        /// Adds the amount.
        /// </summary>
        /// <param name="Amount">The amount.</param>
        public void AddAmount(int Amount)
        {
            this.Amount += Amount;
        }

        /// <summary>
        /// Removes the amount.
        /// </summary>
        /// <param name="Amount">The amount.</param>
        public void RemoveAmount(int Amount)
        {
            this.Amount -= Amount;
        }

        /// <summary>
        /// Gets a value indicating whether the amount is equal to zero.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.Amount == 0;
            }
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            if (Json.ContainsKey("CurrencyId"))
            {
                this.CurrencyId = Json.GetValue("CurrencyId").ToObject<string>();
            }

            if (Json.ContainsKey("Amount"))
            {
                this.Amount = Json.GetValue("Amount").ToObject<int>();
            }
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("CurrencyId", this.CurrencyId);
            Json.Add("Amount", this.Amount);

            return Json;
        }
    }
}
