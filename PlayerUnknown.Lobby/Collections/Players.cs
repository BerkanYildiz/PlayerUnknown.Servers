namespace PlayerUnknown.Lobby.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using PlayerUnknown.Logic;

    using GameDb    = PlayerUnknown.Lobby.Database.GameDb;
    using PlayerDb  = PlayerUnknown.Lobby.Database.Models.PlayerDb;

    public static class Players
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Players"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        private static ConcurrentDictionary<string, Player> Entities;

        /// <summary>
        /// Initializes the slot.
        /// </summary>
        public static void Initialize()
        {
            if (Players.Initialized)
            {
                return;
            }

            Players.Entities    = new ConcurrentDictionary<string, Player>();
            Players.Initialized = true;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static void Add(Player Entity)
        {
            if (Players.Entities.ContainsKey(Entity.Account.AccountId))
            {
                if (!Players.Entities.TryUpdate(Entity.Account.AccountId, Entity, Entity))
                {
                    Logging.Error(typeof(Players), "TryUpdate(EntityId, Entity, Entity) != true at Add(Entity).");
                }
            }
            else
            {
                if (!Players.Entities.TryAdd(Entity.Account.AccountId, Entity))
                {
                    Logging.Error(typeof(Players), "TryAdd(EntityId, Entity) != true at Add(Entity).");
                }
            }
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static async Task Remove(Player Entity)
        {
            Player TmpEntity;

            if (Entity != null)
            {
                if (!Players.Entities.TryRemove(Entity.Account.AccountId, out TmpEntity))
                {
                    Logging.Warning(typeof(Players), "Entities.TryRemove(Entity, TmpEntity) != true at Remove(Entity).");
                }
            }

            await Players.Save(Entity);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<Player> Create(Player Entity = null, bool Store = true)
        {
            if (Entity == null)
            {
                Entity = new Player();
            }

            await PlayerDb.Create(Entity);

            if (Store)
            {
                Players.Add(Entity);
            }

            return Entity;
        }

        /// <summary>
        /// Gets the entity using the specified identifier.
        /// </summary>
        /// <param name="AccountId">The account identifier.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<Player> Get(string AccountId, bool Store = true)
        {
            PlayerDb PlayerDb = await PlayerDb.Load(AccountId);
            Player Player     = null;

            if (Players.Entities.TryGetValue(AccountId, out Player))
            {
                return Player;
            }
            else
            {
                if (PlayerDb != null)
                {
                    if (PlayerDb.Deserialize(out Player))
                    {
                        if (Store)
                        {
                            Players.Add(Player);
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
        public static async Task<Player> Get(string Username, string Password, bool Store = true)
        {
            PlayerDb PlayerDb = await PlayerDb.Load(Username, Password);
            Player Player     = null;

            /* if (Players.Entities.TryGetValue(PlayerDb., out Player))
            {
                return Player;
            }
            else */
            {
                if (PlayerDb != null)
                {
                    if (PlayerDb.Deserialize(out Player))
                    {
                        if (Store)
                        {
                            Players.Add(Player);
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
        public static async Task Save(Player Entity)
        {
            var Result = await PlayerDb.Save(Entity);

            if (Result == null)
            {
                Logging.Error(typeof(Players), "Result == null at Save(Entity).");
            }
        }

        /// <summary>
        /// Saves every entities in this slot.
        /// </summary>
        public static async Task SaveAll()
        {
            var Players = Collections.Players.Entities.Values.ToArray();
            var RequestsTasks = new Task[Players.Length];

            for (var I = 0; I < Players.Length; I++)
            {
                RequestsTasks[I] = Collections.Players.Save(Players[I]);
            }

            await Task.WhenAll(RequestsTasks);
        }

        /// <summary>
        /// Executes an action on every players in the collection.
        /// </summary>
        /// <param name="Action">The action to execute on the players.</param>
        public static void ForEach(Action<Player> Action)
        {
            var Entities = Players.Entities.Values;
            
            foreach (var Entity in Entities)
            {
                Action.Invoke(Entity);
            }
        }
    }
}