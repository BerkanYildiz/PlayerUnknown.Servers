namespace PlayerUnknown.Lobby.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using PlayerUnknown.Logic;

    using PlayerDb  = PlayerUnknown.Lobby.Database.Models.PlayerDb;

    public class Players
    {
        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        public ConcurrentDictionary<string, Player> Entities
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        private PubgLobbyServer Server
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the count of players currently in the memory.
        /// </summary>
        public int Count
        {
            get
            {
                if (this.IsDisposed)
                {
                    return 0;
                }

                return this.Entities.Count;
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Players"/> class.
        /// </summary>
        /// <param name="Server">The server.</param>
        public Players(PubgLobbyServer Server)
        {
            this.Server     = Server;
            this.Entities   = new ConcurrentDictionary<string, Player>();
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public void Add(Player Entity)
        {
            if (this.Entities.ContainsKey(Entity.Account.AccountId))
            {
                if (!this.Entities.TryUpdate(Entity.Account.AccountId, Entity, Entity))
                {
                    Logging.Error(typeof(Players), "TryUpdate(EntityId, Entity, Entity) != true at Add(Entity).");
                }
            }
            else
            {
                if (!this.Entities.TryAdd(Entity.Account.AccountId, Entity))
                {
                    Logging.Error(typeof(Players), "TryAdd(EntityId, Entity) != true at Add(Entity).");
                }
            }
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public async Task Remove(Player Entity)
        {
            Player TmpEntity;

            if (Entity != null)
            {
                if (!this.Entities.TryRemove(Entity.Account.AccountId, out TmpEntity))
                {
                    Logging.Warning(typeof(Players), "Entities.TryRemove(Entity, TmpEntity) != true at Remove(Entity).");
                }
            }

            await this.Server.Database.Save(Entity);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public async Task<Player> Create(Player Entity = null, bool Store = true)
        {
            if (Entity == null)
            {
                Entity = JsonConvert.DeserializeObject<Player>(this.Server.Default.HomeJson.ToString());
            }
            else
            {
                JsonConvert.PopulateObject(this.Server.Default.HomeJson.ToString(), Entity);
            }

            await this.Server.Database.Create(Entity);

            if (Store)
            {
                this.Add(Entity);
            }

            return Entity;
        }

        /// <summary>
        /// Gets the entity using the specified identifier.
        /// </summary>
        /// <param name="AccountId">The account identifier.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public async Task<Player> Get(string AccountId, bool Store = true)
        {
            PlayerDb PlayerDb = await this.Server.Database.Load(AccountId);
            Player Player     = null;

            if (this.Entities.TryGetValue(AccountId, out Player))
            {
                return Player;
            }
            else
            {
                if (PlayerDb != null)
                {
                    if (this.Server.Database.Deserialize(PlayerDb, out Player))
                    {
                        if (Store)
                        {
                            this.Add(Player);
                        }

                        return Player;
                    }
                    else
                    {
                        Logging.Error(typeof(Players), "PlayerDb.Deserialize(out Player) != true at Get(" + AccountId + ").");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Players), "PlayerDb == null at Get(HighId, LowId).");
                }
            }

            return Player;
        }
        
        /// <summary>
        /// Gets the entity using the specified identifier.
        /// </summary>
        /// <param name="AccountId">The account identifier.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public async Task<Player> Get(string Username, string Password, bool Store = true)
        {
            PlayerDb PlayerDb = await this.Server.Database.Load(Username, Password);
            Player Player     = null;

            /* if (Players.Entities.TryGetValue(PlayerDb., out Player))
            {
                return Player;
            }
            else */
            {
                if (PlayerDb != null)
                {
                    if (this.Server.Database.Deserialize(PlayerDb, out Player))
                    {
                        if (Store)
                        {
                            this.Add(Player);
                        }

                        return Player;
                    }
                    else
                    {
                        Logging.Error(typeof(Players), "PlayerDb.Deserialize(out Player) != true at Get(" + Username + ", " + Password + ").");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Players), "PlayerDb == null at Get(HighId, LowId).");
                }
            }

            return Player;
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public async Task Save(Player Entity)
        {
            var Result = await this.Server.Database.Save(Entity);

            if (Result == null)
            {
                Logging.Error(typeof(Players), "Result == null at Save(Entity).");
            }
        }

        /// <summary>
        /// Saves every entities in this slot.
        /// </summary>
        public async Task SaveAll()
        {
            var Players         = this.Entities.Values.ToArray();
            var RequestsTasks   = new Task[Players.Length];

            for (var I = 0; I < Players.Length; I++)
            {
                RequestsTasks[I] = this.Save(Players[I]);
            }

            await Task.WhenAll(RequestsTasks);
        }

        /// <summary>
        /// Executes an action on every players in the collection.
        /// </summary>
        /// <param name="Action">The action to execute on the players.</param>
        public void ForEach(Action<Player> Action)
        {
            var Entities = this.Entities.Values;
            
            foreach (var Entity in Entities)
            {
                Action.Invoke(Entity);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
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