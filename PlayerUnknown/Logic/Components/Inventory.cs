namespace PlayerUnknown.Logic.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Components.Inventories;
    using PlayerUnknown.Logic.Interfaces;
    using PlayerUnknown.Logic.Interfaces.Players;

    public sealed class Inventory : IInventory
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<IItem> Items
        {
            get;
        }

        /// <summary>
        /// Gets the equips.
        /// </summary>
        public List<object> Equips
        {
            get;
        }

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        public List<ICurrency> Currencies
        {
            get;
        }

        /// <summary>
        /// Gets the history.
        /// </summary>
        public List<object> History
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class.
        /// </summary>
        public Inventory()
        {
            this.Items      = new List<IItem>();
            this.Equips     = new List<object>();
            this.Currencies = new List<ICurrency>();
            this.History    = new List<object>();
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void AddItem(Item Item)
        {
            Item CurrentItem = (Item) this.Items.Find(T => T.ItemDescId == Item.ItemDescId);
            
            if (CurrentItem != null)
            {
                if (Item.Count != 0)
                {
                    CurrentItem.IncreaseAmount(Item.Count);
                }
                else
                {
                    CurrentItem.IncreaseAmount();
                }
            }
            else
            {
                if (Item.Count == 0)
                {
                    Item.IncreaseAmount();
                }

                this.Items.Add(Item);
            }
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void RemoveItem(Item Item, int Amount = 1)
        {
            Item CurrentItem = (Item) this.Items.Find(T => T.ItemDescId == Item.ItemDescId);

            if (CurrentItem != null)
            {
                CurrentItem.DecreaseAmount(Amount);

                if (CurrentItem.Count == 0)
                {
                    Log.Warning(this.GetType(), "CurrentItem.Count == 0 at RemoveItem(" + Item.PartDescId + ", " + Amount + "), should we remove the item ?");
                }
            }
            else
            {
                Log.Warning(this.GetType(), "CurrentItem == null at RemoveItem(" + Item.PartDescId + ").");
            }
        }

        /// <summary>
        /// Equips the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void EquipItem(Item Item)
        {
            // TODO
        }

        /// <summary>
        /// Unequips the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void UnequipItem(Item Item)
        {
            // TODO
        }

        /// <summary>
        /// Determines whether the player has the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <returns></returns>
        public bool HasItem(Item Item)
        {
            if (this.Items.Any(T => T.ItemDescId == Item.ItemDescId))
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
            if (Json.ContainsKey("Items"))
            {
                foreach (var ItemJson in Json.GetValue("Items"))
                {
                    var Item = new Item();
                    Item.Load(ItemJson.ToObject<JObject>());
                    this.Items.Add(Item);
                }
            }

            if (Json.ContainsKey("Equips"))
            {
                // TODO : Equips.
            }

            if (Json.ContainsKey("Currencies"))
            {
                foreach (var CurrencyJson in Json.GetValue("Currencies"))
                {
                    var Currency = new Currency();
                    Currency.Load(CurrencyJson.ToObject<JObject>());
                    this.Currencies.Add(Currency);
                }
            }

            if (Json.ContainsKey("History"))
            {
                // TODO : History.
            }
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("Items", null);
            Json.Add("Equips", null);
            Json.Add("Currencies", null);
            Json.Add("History", null);

            if (this.Items.Count > 0)
            {
                var ItemsJson = new JArray();

                foreach (var Item in this.Items)
                {
                    ItemsJson.Add(Item.Save());
                }

                Json["Items"] = ItemsJson;
            }

            // TODO : Equips.

            if (this.Currencies.Count > 0)
            {
                var CurrenciesJson = new JArray();

                foreach (var Currency in this.Currencies)
                {
                    CurrenciesJson.Add(Currency.Save());
                }

                Json["Currencies"] = CurrenciesJson;
            }

            // TODO : History.

            return Json;
        }
    }
}
