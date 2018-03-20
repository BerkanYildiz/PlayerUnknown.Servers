namespace PlayerUnknown.LobbyProxy.Services
{
    using System;

    using Newtonsoft.Json;

    using PlayerUnknown.Logic.Network;

    using WebSocketSharp;

    public class ServerProxy
    {
        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        private WebSocket Server
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
        public void Connect(string Query)
        {
            this.Server = new WebSocket(Query);

            this.Server.OnOpen      += this.OnOpen;
            this.Server.OnMessage   += this.OnMessage;
            this.Server.OnClose     += this.OnClose;

            this.Server.ConnectAsync();
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

            this.Server.SendAsync(Message.Save().ToString(Formatting.None), Completed =>
            {
                if (Completed)
                {
                    Logging.Info(this.GetType(), "Sent a message to the server.");
                }
                else
                {
                    Logging.Warning(this.GetType(), "Completed != true at Server.SendMessage(" + Message.Identifier + ").");
                }
            });
        }
    }
}
