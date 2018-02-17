namespace PlayerUnknown.Logic.Components
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using PlayerUnknown.Logic.Components.Inventories;

    public sealed class Inventory
    {
        [JsonProperty("Items")]      public List<Item> Items;
        [JsonProperty("Equips")]     public List<object> Equips;
        [JsonProperty("Currencies")] public List<Currency> Currencies;
        [JsonProperty("History")]    public List<object> History;

        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class.
        /// </summary>
        public Inventory()
        {
            this.Items      = new List<Item>();
            this.Equips     = new List<object>();
            this.Currencies = new List<Currency>();
            this.History    = new List<object>();
        }
    }
}
