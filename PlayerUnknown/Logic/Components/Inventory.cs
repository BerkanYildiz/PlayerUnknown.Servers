namespace PlayerUnknown.Logic.Components
{
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Interfaces;
    using PlayerUnknown.Logic.Components.Inventories;

    public sealed class Inventory : IInventory
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<Item> Items
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
        public List<Currency> Currencies
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
            this.Items      = new List<Item>();
            this.Equips     = new List<object>();
            this.Currencies = new List<Currency>();
            this.History    = new List<object>();
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void AddItem(Item Item)
        {
            // TODO
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void RemoveItem(Item Item)
        {
            // TODO
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
            return false; // TODO
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject ToJson()
        {
            JObject Json = new JObject();

            Json.Add("Items", null);
            Json.Add("Equips", null);
            Json.Add("Currencies", null);
            Json.Add("History", null);

            return Json;
        }
    }
}
