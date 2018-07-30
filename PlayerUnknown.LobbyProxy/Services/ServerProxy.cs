namespace PlayerUnknown.LobbyProxy.Services
{
    using System;
    using System.IO;
    using System.Net.WebSockets;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using PlayerUnknown.Logic.Network;
    using WebSocketSharp;

    // using WebSocketSharp;

    public class ServerProxy
    {
        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        private ClientWebSocket Server
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerProxy"/> class.
        /// </summary>
        /// <param name="Proxy">The proxy.</param>
        public ServerProxy()
        {
            // ServerProxy.
        }

        /// <summary>
        /// Connects to the official server using the specified query.
        /// </summary>
        /// <param name="Query">The query.</param>
        public async void Connect(string Query)
        {
            this.Server = new ClientWebSocket();

            if (this.Server.Options.ClientCertificates == null)
            {
                this.Server.Options.ClientCertificates = new X509CertificateCollection();
            }

            this.Server.Options.ClientCertificates.Add(new X509Certificate(File.ReadAllBytes("Certs\\ssl.pfx"), "rekt"));
            this.Server.Options.SetRequestHeader("Sec-WebSocket-Extensions", "x-webkit-deflate-frame");
            this.Server.Options.SetRequestHeader("Origin", "https://prod-live-front.playbattlegrounds.com");
            this.Server.Options.SetBuffer(2048, 2048);
            
            using (var Cancellation = new CancellationTokenSource())
            {
                await this.Server.ConnectAsync(new Uri(Query), Cancellation.Token);

                if (this.Server.State.HasFlag(System.Net.WebSockets.WebSocketState.Open))
                {
                    Logging.Warning(typeof(ServerProxy), "We are connected !");
                }
                else
                {
                    Logging.Error(typeof(ServerProxy), "We are not connected !");
                }
            }

            await Task.Delay(500);

            var Buffer = new ArraySegment<byte>(new byte[2048]);

            await this.Server.SendAsync(
                new ArraySegment<byte>(
                    new byte[]
                    {
                        0x01, 0x02
                    }),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
            await this.Server.ReceiveAsync(Buffer, CancellationToken.None);

            Logging.Warning(typeof(ServerProxy), Encoding.UTF8.GetString(Buffer.Array));

            Console.ReadKey(false);
        }

        /// <summary>
        /// Called when the WebSocket connection used in a session has been established.
        /// </summary>
        private void OnOpen(object Sender, EventArgs EventArgs)
        {
            Logging.Info(this.GetType(), "Connected to the official server.");
        }

        /// <summary>
        /// Called when a message has been received.
        /// </summary>
        private void OnMessage(object Sender, MessageEventArgs MessageEventArgs)
        {
            Logging.Info(this.GetType(), "Received a message from the server.");
        }

        /// <summary>
        /// Called when the <see cref="WebSocket"/> connection has been terminated.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="CloseEventArgs">The <see cref="CloseEventArgs"/> instance containing the event data.</param>
        private void OnClose(object Sender, CloseEventArgs CloseEventArgs)
        {
            Logging.Warning(this.GetType(), "The official server closed the connection.");
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            if (Message == null)
            {
                throw new Exception("Message == null at SendMessage(Message).");
            }

            /* this.Server.SendAsync(Message.Save().ToString(Formatting.None), Completed =>
            {
                if (Completed)
                {
                    Logging.Info(this.GetType(), "Sent a message to the server.");
                }
                else
                {
                    Logging.Warning(this.GetType(), "Completed != true at Server.SendMessage(" + Message.Identifier + ").");
                }
            }); */
        }
    }
}
