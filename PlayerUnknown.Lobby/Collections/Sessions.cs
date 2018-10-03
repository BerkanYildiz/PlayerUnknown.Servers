namespace PlayerUnknown.Lobby.Collections
{
    using System;
    using System.Collections.Concurrent;

    using PlayerUnknown.Lobby.Models.Sessions;

    public class Sessions : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.Entities.Count;
            }
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        public ConcurrentDictionary<string, PubgSession> Entities
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sessions"/> class.
        /// </summary>
        public Sessions()
        {
            this.Entities = new ConcurrentDictionary<string, PubgSession>();
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public bool TryAdd(PubgSession Entity)
        {
            if (this.Entities.ContainsKey(Entity.ID))
            {
                Log.Error(typeof(Sessions), "ContainsKey(Entity.ID) != false at Add(Entity).");
            }
            else
            {
                if (this.Entities.TryAdd(Entity.ID, Entity))
                {
                    return true;
                }
                else
                {
                    Log.Error(typeof(Sessions), "TryAdd(Entity.ID, Entity) != true at Add(Entity).");
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public bool TryRemove(PubgSession Entity)
        {
            if (this.Entities.TryRemove(Entity.ID, out PubgSession TmpEntity))
            {
                if (TmpEntity.ID == Entity.ID)
                {
                    return true;
                }
                else
                {
                    Log.Error(typeof(Sessions), "TmpEntity.ID != Entity.ID at Remove(Entity).");
                }
            }
            else
            {
                Log.Error(typeof(Sessions), "TryRemove(Entity.ID, out TmpEntity) != true at Remove(Entity).");
            }

            return false;
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        public bool TryRemove(string ID)
        {
            if (this.Entities.TryRemove(ID, out PubgSession TmpEntity))
            {
                if (TmpEntity.ID == ID)
                {
                    return true;
                }
                else
                {
                    Log.Error(typeof(Sessions), "TmpEntity.ID != ID at Remove(ID).");
                }
            }
            else
            {
                Log.Error(typeof(Sessions), "TryRemove(ID, out TmpEntity) != true at Remove(ID).");
            }

            return false;
        }

        /// <summary>
        /// Gets the entity using the specified identifier.
        /// </summary>
        /// <param name="Identifier">The identifier.</param>
        public PubgSession Get(string Identifier)
        {
            if (this.Entities.TryGetValue(Identifier, out PubgSession Entity))
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
        public void ForEach(Action<PubgSession> Action, bool Connected = true)
        {
            var Entities = this.Entities.Values;

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

        /// <summary>
        /// Exécute les tâches définies par l'application associées à
        /// la libération ou à la redéfinition des ressources non managées.
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.IsDisposed = true;

            if (this.Entities != null)
            {
                this.Entities.Clear();
            }

            this.Entities = null;
        }
    }
}