namespace PlayerUnknown.Lobby.Proxy.Collections
{
    using System;
    using System.Collections.Concurrent;

    using PlayerUnknown.Lobby.Proxy.Models.Sessions;

    public static class Sessions
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Sessions"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public static int Count
        {
            get
            {
                return Sessions.Entities.Count;
            }
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        private static ConcurrentDictionary<string, PubgSession> Entities;

        /// <summary>
        /// Initializes the slot.
        /// </summary>
        public static void Initialize()
        {
            if (Sessions.Initialized)
            {
                return;
            }

            Sessions.Entities    = new ConcurrentDictionary<string, PubgSession>();
            Sessions.Initialized = true;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static bool TryAdd(PubgSession Entity)
        {
            if (Sessions.Entities.ContainsKey(Entity.ID))
            {
                Logging.Error(typeof(Sessions), "ContainsKey(Entity.ID) != false at Add(Entity).");
            }
            else
            {
                if (Sessions.Entities.TryAdd(Entity.ID, Entity))
                {
                    return true;
                }
                else
                {
                    Logging.Error(typeof(Sessions), "TryAdd(Entity.ID, Entity) != true at Add(Entity).");
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static bool TryRemove(PubgSession Entity)
        {
            if (Sessions.Entities.TryRemove(Entity.ID, out PubgSession TmpEntity))
            {
                if (TmpEntity.ID == Entity.ID)
                {
                    return true;
                }
                else
                {
                    Logging.Error(typeof(Sessions), "TmpEntity.ID != Entity.ID at Remove(Entity).");
                }
            }
            else
            {
                Logging.Error(typeof(Sessions), "TryRemove(Entity.ID, out TmpEntity) != true at Remove(Entity).");
            }

            return false;
        }

        /// <summary>
        /// Gets the entity using the specified identifier.
        /// </summary>
        /// <param name="Identifier">The identifier.</param>
        public static PubgSession Get(string Identifier)
        {
            if (Sessions.Entities.TryGetValue(Identifier, out PubgSession Entity))
            {
                return Entity; 
            }

            return null;
        }

        /// <summary>
        /// Executes an action on every entity in the collection.
        /// </summary>
        /// <param name="Action">The action to execute on the entities.</param>
        /// <param name="Connected">if set to true, only execute the action on connected entities.</param>
        public static void ForEach(Action<PubgSession> Action, bool Connected = true)
        {
            var Entities = Sessions.Entities.Values;

            if (Connected)
            {
                // TODO.

                foreach (var Entity in Entities)
                {
                    Action.Invoke(Entity);
                }
            }
            else
            {
                foreach (var Entity in Entities)
                {
                    Action.Invoke(Entity);
                }
            }
        }
    }
}