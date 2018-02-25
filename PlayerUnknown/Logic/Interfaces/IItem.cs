namespace PlayerUnknown.Logic.Interfaces
{
    using Newtonsoft.Json.Linq;

    public interface IItem
    {
        /// <summary>
        /// Gets the item description identifier.
        /// </summary>
        string ItemDescId
        {
            get;
        }

        /// <summary>
        /// Gets the part description identifier.
        /// </summary>
        string PartDescId
        {
            get;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Desc
        {
            get;
        }

        /// <summary>
        /// Gets the preset identifier.
        /// </summary>
        string PresetId
        {
            get;
        }

        /// <summary>
        /// Gets the quality.
        /// </summary>
        string Quality
        {
            get;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Gets the buy price.
        /// </summary>
        int BuyPrice
        {
            get;
        }

        /// <summary>
        /// Gets the sell price.
        /// </summary>
        int SellPrice
        {
            get;
        }

        /// <summary>
        /// Gets the price in cents.
        /// </summary>
        int PriceInCents
        {
            get;
        }

        /// <summary>
        /// Gets the weekly purchase limit.
        /// </summary>
        int WeeklyPurchaseLimit
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this item is equipped.
        /// </summary>
        bool InEquip
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this item is doubled.
        /// </summary>
        bool Doubling
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
