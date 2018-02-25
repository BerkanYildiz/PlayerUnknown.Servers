namespace PlayerUnknown.Logic.Interfaces
{
    using Newtonsoft.Json.Linq;

    public interface ICurrency
    {
        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        string CurrencyId
        {
            get;
        }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        int Amount
        {
            get;
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
