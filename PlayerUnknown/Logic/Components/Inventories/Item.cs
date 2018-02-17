namespace PlayerUnknown.Logic.Components.Inventories
{
    using Newtonsoft.Json;

    public sealed class Item : InventoryObject
    {
        [JsonProperty("ItemDescId")]            public string ItemDescId;
        [JsonProperty("PartDescId")]            public string PartDescId;
        [JsonProperty("Name")]                  public string Name;
        [JsonProperty("Desc")]                  public string Desc;
        [JsonProperty("PresetId")]              public string PresetId;
        [JsonProperty("Quality")]               public string Quality;

        [JsonProperty("Count")]                 public int Count;
        [JsonProperty("BuyPrice")]              public int BuyPrice;
        [JsonProperty("SellPrice")]             public int SellPrice;
        [JsonProperty("PriceInCents")]          public int PriceInCents;
        [JsonProperty("WeeklyPurchaseLimit")]   public int WeeklyPurchaseLimit;

        [JsonProperty("InEquip")]               public bool InEquip;
        [JsonProperty("Doubling")]              public bool Doubling;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        public Item()
        {
            // Item.
        }
    }
}
