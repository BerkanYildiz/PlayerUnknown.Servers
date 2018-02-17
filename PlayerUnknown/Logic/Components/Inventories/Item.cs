namespace PlayerUnknown.Logic.Components.Inventories
{
    public sealed class Item : InventoryObject
    {
        public string ItemDescId;
        public string PartDescId;
        public string Name;
        public string Desc;
        public string PresetId;
        public string Quality;

        public int Count;
        public int BuyPrice;
        public int SellPrice;
        public int PriceInCents;
        public int WeeklyPurchaseLimit;

        public bool InEquip;
        public bool Doubling;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        public Item()
        {
            // Item.
        }
    }
}
