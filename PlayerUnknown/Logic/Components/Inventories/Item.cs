namespace PlayerUnknown.Logic.Components.Inventories
{
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Interfaces;
    using PlayerUnknown.Logic.Interfaces.Players;

    public sealed class Item : IItem, IObject
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
            private set
            {
                this._ItemDescId = value;
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
            private set
            {
                this._PartDescId = value;
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
            private set
            {
                this._Name = value;
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
            private set
            {
                this._Desc = value;
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
            private set
            {
                this._PresetId = value;
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
            private set
            {
                this._Quality = value;
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
            private set
            {
                this._Count = value;
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
            private set
            {
                this._BuyPrice = value;
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
            private set
            {
                this._SellPrice = value;
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
            private set
            {
                this._PriceInCents = value;
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
            private set
            {
                this._WeeklyPurchaseLimit = value;
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
            private set
            {
                this._InEquip = value;
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
            private set
            {
                this._Doubling = value;
            }
        }

        private string _ItemDescId;
        private string _PartDescId;
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
        /// Increases the amount.
        /// </summary>
        public void IncreaseAmount(int Amount = 1)
        {
            this._Count += Amount;
        }

        /// <summary>
        /// Increases the amount.
        /// </summary>
        public void DecreaseAmount(int Amount = 1)
        {
            if (this.Count - Amount < 0)
            {
                this._Count = 0;
            }
            else
            {
                this._Count -= Amount;
            }
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            if (Json.ContainsKey("ItemDescId"))
            {
                this._ItemDescId = Json.GetValue("ItemDescId").ToObject<string>();
            }

            if (Json.ContainsKey("PartDescId"))
            {
                this._PartDescId = Json.GetValue("PartDescId").ToObject<string>();
            }

            if (Json.ContainsKey("Name"))
            {
                this._Name = Json.GetValue("Name").ToObject<string>();
            }

            if (Json.ContainsKey("Desc"))
            {
                this._Desc = Json.GetValue("Desc").ToObject<string>();
            }

            if (Json.ContainsKey("PresetId"))
            {
                this._PresetId = Json.GetValue("PresetId").ToObject<string>();
            }

            if (Json.ContainsKey("Quality"))
            {
                this._Quality = Json.GetValue("Quality").ToObject<string>();
            }

            if (Json.ContainsKey("Count"))
            {
                this._Count = Json.GetValue("Count").ToObject<int>();
            }

            if (Json.ContainsKey("BuyPrice"))
            {
                this._BuyPrice = Json.GetValue("BuyPrice").ToObject<int>();
            }

            if (Json.ContainsKey("SellPrice"))
            {
                this._SellPrice = Json.GetValue("SellPrice").ToObject<int>();
            }

            if (Json.ContainsKey("PriceInCents"))
            {
                this._PriceInCents = Json.GetValue("PriceInCents").ToObject<int>();
            }

            if (Json.ContainsKey("WeeklyPurchaseLimit"))
            {
                this._WeeklyPurchaseLimit = Json.GetValue("WeeklyPurchaseLimit").ToObject<int>();
            }

            if (Json.ContainsKey("InEquip"))
            {
                this._InEquip = Json.GetValue("InEquip").ToObject<bool>();
            }

            if (Json.ContainsKey("Doubling"))
            {
                this._Doubling = Json.GetValue("Doubling").ToObject<bool>();
            }
        }
        
        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
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
