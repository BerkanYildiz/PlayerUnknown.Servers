namespace PlayerUnknown.Logic.Components.Inventories
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Interfaces;

    public sealed class Item : IItem
    {
        /// <summary>
        /// Gets the item description identifier.
        /// </summary>
        public string ItemDescId
        {
            get
            {
                return "itemdesc." + this._ItemDescId;
            }
        }

        /// <summary>
        /// Gets the part description identifier.
        /// </summary>
        public string PartDescId
        {
            get
            {
                return "partdesc." + this._PartDescId;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Desc
        {
            get
            {
                return this._Desc;
            }
        }

        /// <summary>
        /// Gets the preset identifier.
        /// </summary>
        public string PresetId
        {
            get
            {
                return this._PresetId;
            }
        }

        /// <summary>
        /// Gets the quality.
        /// </summary>
        public string Quality
        {
            get
            {
                return this._Quality;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this._Count;
            }
        }

        /// <summary>
        /// Gets the buy price.
        /// </summary>
        public int BuyPrice
        {
            get
            {
                return this._BuyPrice;
            }
        }

        /// <summary>
        /// Gets the sell price.
        /// </summary>
        public int SellPrice
        {
            get
            {
                return this._SellPrice;
            }
        }

        /// <summary>
        /// Gets the price in cents.
        /// </summary>
        public int PriceInCents
        {
            get
            {
                return this._PriceInCents;
            }
        }

        /// <summary>
        /// Gets the weekly purchase limit.
        /// </summary>
        public int WeeklyPurchaseLimit
        {
            get
            {
                return this._WeeklyPurchaseLimit;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this item is equipped.
        /// </summary>
        public bool InEquip
        {
            get
            {
                return this._InEquip;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this item is doubled.
        /// </summary>
        public bool Doubling
        {
            get
            {
                return this._Doubling;
            }
        }

        private int _ItemDescId;
        private int _PartDescId;

        private string _PresetId;
        private string _Name;
        private string _Desc;
        private string _Quality;

        private int _Count;
        private int _BuyPrice;
        private int _SellPrice;
        private int _PriceInCents;
        private int _WeeklyPurchaseLimit;

        private bool _InEquip;
        private bool _Doubling;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        public Item()
        {
            // Item.
        }
        
        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject ToJson()
        {
            JObject Json = new JObject();

            Json.Add("ItemDescId", this.ItemDescId);
            Json.Add("PartDescId", this.PartDescId);
            Json.Add("Name", this.Name);
            Json.Add("Desc", this.Desc);
            Json.Add("PresetId", this.PresetId);
            Json.Add("Quality", this.Quality);

            Json.Add("Count", this.Count);
            Json.Add("BuyPrice", this.BuyPrice);
            Json.Add("SellPrice", this.SellPrice);
            Json.Add("PriceInCents", this.PriceInCents);
            Json.Add("WeeklyPurchaseLimit", this.WeeklyPurchaseLimit);

            Json.Add("InEquip", this.InEquip);
            Json.Add("Doubling", this.Doubling);

            return Json;
        }
    }
}
