namespace PlayerUnknown.Lobby
{
    using System;
    using System.Threading.Tasks;

    using PlayerUnknown.Files;
    using PlayerUnknown.Lobby.Collections;
    using PlayerUnknown.Lobby.Database;
    using PlayerUnknown.Lobby.Services;
    using PlayerUnknown.Logic;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public class PubgLobbyServer : IDisposable
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public Config Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default inventory and profile variables.
        /// </summary>
        public Default Default
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the translations for cross-languages.
        /// </summary>
        public Translations Translations
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the game database where players are stored.
        /// </summary>
        public GameDb Database
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sessions that are currently in memory.
        /// </summary>
        public Sessions Sessions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the players that are currently in memory.
        /// </summary>
        public Players Players
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the network class used to send and receive messages.
        /// </summary>
        public WebSocketServer Network
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is listening.
        /// </summary>
        public bool IsListening
        {
            get
            {
                if (this.IsDisposed)
                {
                    return false;
                }

                return this.Network.IsListening;
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
        /// Initializes a new instance of the <see cref="PubgLobbyServer"/> class.
        /// </summary>
        public PubgLobbyServer() : this(Config.Default)
        {
            // PubgLobbyServer.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgLobbyServer"/> class.
        /// </summary>
        public PubgLobbyServer(Config Config)
        {
            if (Config == null)
            {
                throw new Exception("Configuration for PubgLobbyServer can't be null.");
            }

            this.Configuration      = Config;

            this.Default            = new Default();
            this.Translations       = new Translations();
            this.Database           = new GameDb();
            this.Sessions           = new Sessions();
            this.Players            = new Players(this);
            this.Network            = new WebSocketServer(Config.ServerPort);

            this.Network.Log.Level  = LogLevel.Trace;
            this.Network.Log.Output = (Data, Message) =>
            {
                Log.Info(typeof(WebSocket), Data.Message);
            };

            this.Network.AddWebSocketService("/userproxy", () =>
            {
                return new UserProxy(this);
            });

            this.Network.AddWebSocketService("/", () =>
            {
                return new DefaultProxy(this);
            });

            Log.Info(this.GetType(), "Lobby has been initialized.");
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (this.IsListening)
            {
                return;
            }

            this.Network.Start();

            Log.Info(this.GetType(), "Lobby has been started.");
        }

        /// <summary>
        /// Waits this instance.
        /// </summary>
        public async Task Wait()
        {
            while (this.IsListening)
            {
                await Task.Delay(1000);
            }

            Log.Warning(typeof(PubgLobbyServer), "Stopped waiting !");
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (!this.IsListening)
            {
                return;
            }

            this.Network.Stop();

            Log.Info(this.GetType(), "Lobby has been stopped.");
        }

        /// <summary>
        /// Exécute les tâches définies par l'application associées à la libération ou à la redéfinition des ressources non managées.
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.Stop();

            if (this.Sessions != null)
            {
                this.Sessions.Dispose();
            }

            if (this.Players != null)
            {
                this.Players.Dispose();
            }

            this.Configuration  = null;
            this.Default        = null;
            this.Translations   = null;
            this.Database       = null;
            this.Sessions       = null;
            this.Players        = null;
            this.Network        = null;

            this.IsDisposed     = true;

            Log.Info(this.GetType(), "Lobby has been disposed.");
        }
    }
}
