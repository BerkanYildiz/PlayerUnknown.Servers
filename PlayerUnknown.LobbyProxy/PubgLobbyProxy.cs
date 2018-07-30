namespace PlayerUnknown.LobbyProxy
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Security.Authentication;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    using PlayerUnknown.Files;
    using PlayerUnknown.LobbyProxy.Collections;
    using PlayerUnknown.LobbyProxy.Services;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    public class PubgLobbyProxy : IDisposable
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
        /// Gets the sessions that are currently in memory.
        /// </summary>
        public Sessions Sessions
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
        /// Initializes a new instance of the <see cref="PubgLobbyProxy"/> class.
        /// </summary>
        public PubgLobbyProxy() : this(Config.Default)
        {
            // PubgLobbyProxy.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgLobbyProxy"/> class.
        /// </summary>
        public PubgLobbyProxy(Config Config)
        {
            if (Config == null)
            {
                throw new Exception("Configuration for PubgLobbyProxy can't be null.");
            }

            this.Configuration      = Config;

            this.Default            = new Default();
            this.Translations       = new Translations();
            this.Sessions           = new Sessions();
            this.Network            = new WebSocketServer(81, false);

            this.Network.Log.Level  = LogLevel.Trace;
            this.Network.Log.Output = (Data, Message) =>
            {
                Logging.Info(typeof(WebSocket), Data.Message);
            };

            this.Network.AddWebSocketService("/userproxy", () =>
            {
                return new ClientProxy(this);
            });

            // Tls..

            this.Network.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;

            // Callbacks..

            this.Network.SslConfiguration.ClientCertificateValidationCallback = (Sender, Certificate, Chain, Errors) =>
            {
                return true;
            };

            var Cert = new X509Certificate2(File.ReadAllBytes("Certs\\ssl.pfx"), "rekt");

            this.Network.SslConfiguration.ServerCertificate = Cert;
            this.Network.SslConfiguration.ClientCertificateRequired = false;

            Logging.Info(this.GetType(), "Lobby has been initialized.");
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

            Logging.Info(this.GetType(), "Lobby has been started.");
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

            Logging.Warning(typeof(PubgLobbyProxy), "Stopped waiting !");
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

            Logging.Info(this.GetType(), "Lobby has been stopped.");
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

            this.Configuration  = null;
            this.Default        = null;
            this.Translations   = null;
            this.Sessions       = null;
            this.Network        = null;

            this.IsDisposed     = true;

            Logging.Info(this.GetType(), "Lobby has been disposed.");
        }
    }
}
