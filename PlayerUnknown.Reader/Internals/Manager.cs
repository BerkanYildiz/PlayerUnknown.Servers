namespace PlayerUnknown.Reader.Internals
{
    using System.Collections.Generic;

    /// <summary>
    /// Class managing objects implementing <see cref="INamedElement"/> interface.
    /// </summary>
    public abstract class Manager<T>
        where T : INamedElement
    {
        /// <summary>
        /// The collection of the elements (writable).
        /// </summary>
        protected Dictionary<string, T> InternalItems = new Dictionary<string, T>();

        /// <summary>
        /// The collection of the elements.
        /// </summary>
        public IReadOnlyDictionary<string, T> Items
        {
            get
            {
                return this.InternalItems;
            }
        }

        /// <summary>
        /// Disables all items in the manager.
        /// </summary>
        public void DisableAll()
        {
            foreach (var item in this.InternalItems)
            {
                item.Value.Disable();
            }
        }

        /// <summary>
        /// Enables all items in the manager.
        /// </summary>
        public void EnableAll()
        {
            foreach (var item in this.InternalItems)
            {
                item.Value.Enable();
            }
        }

        /// <summary>
        /// Removes an element by its name in the manager.
        /// </summary>
        /// <param name="Name">The name of the element to remove.</param>
        public void Remove(string Name)
        {
            // Check if the element exists in the dictionary
            if (this.InternalItems.ContainsKey(Name))
            {
                try
                {
                    // Dispose the element
                    this.InternalItems[Name].Dispose();
                }
                finally
                {
                    // Remove the element from the dictionary
                    this.InternalItems.Remove(Name);
                }
            }
        }

        /// <summary>
        /// Remove a given element.
        /// </summary>
        /// <param name="Item">The element to remove.</param>
        public void Remove(T Item)
        {
            this.Remove(Item.Name);
        }

        /// <summary>
        /// Removes all the elements in the manager.
        /// </summary>
        public void RemoveAll()
        {
            // For each element
            foreach (var item in this.InternalItems)
            {
                // Dispose it
                item.Value.Dispose();
            }

            // Clear the dictionary
            this.InternalItems.Clear();
        }
    }
}