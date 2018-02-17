namespace PlayerUnknown.Logic.Components.Inventories
{
    using Newtonsoft.Json;

    public sealed class Currency : InventoryObject
    {
        [JsonProperty("CurrencyId")]    public string CurrencyId;
        [JsonProperty("Amount")]        public int Amount;

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
    }
}
