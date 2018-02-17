namespace PlayerUnknown.Logic.Components
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public sealed class Inventory
    {
        [JsonProperty("Items")]      public List<object> Items;
        [JsonProperty("Equips")]     public List<object> Equips;
        [JsonProperty("Currencies")] public List<object> Currencies;
        [JsonProperty("History")]    public List<object> History;

        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class.
        /// </summary>
        public Inventory()
        {
            this.Items      = new List<object>();
            this.Equips     = new List<object>();
            this.Currencies = new List<object>();
            this.History    = new List<object>();
        }
    }
}
