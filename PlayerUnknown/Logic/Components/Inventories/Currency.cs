namespace PlayerUnknown.Logic.Components.Inventories
{
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Exceptions;
    using PlayerUnknown.Logic.Interfaces;
    using PlayerUnknown.Logic.Interfaces.Players;

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
        /// Gets a value indicating whether the amount is inferior to zero.
        /// </summary>
        public bool IsNegative
        {
            get
            {
                return this.Amount < 0;
            }
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
            var NewAmount = this.Amount -= Amount;

            if (NewAmount < 0)
            {
                throw new PubgCurrencyException("The amount to remove makes the currency count negative.");
            }

            this.Amount = NewAmount;
        }

        /// <summary>
        /// Tries to remove the specified amount of this currency.
        /// </summary>
        /// <param name="Amount">The amount.</param>
        public bool TryRemoveAmount(int Amount)
        {
            var HasEnough = this.HasEnough(Amount);

            if (HasEnough)
            {
                this.RemoveAmount(Amount);
            }

            return HasEnough;
        }

        /// <summary>
        /// Determines whether the user has enough amount of this currency.
        /// </summary>
        /// <param name="Amount">The amount.</param>
        public bool HasEnough(int Amount)
        {
            if (this.Amount - Amount >= 0)
            {
                return true;
            }

            return false;
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
