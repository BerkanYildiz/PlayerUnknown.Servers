namespace PlayerUnknown.LobbyProxy
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class WebSocketClient
    {
        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;

        private readonly ClientWebSocket _ws;
        private readonly Uri _uri;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        private Action<WebSocketClient> _onConnected;
        private Action<string, WebSocketClient> _onMessage;
        private Action<WebSocketClient> _onDisconnected;

        public ClientWebSocketOptions Options
        {
            get
            {
                return this._ws.Options;
            }
        }

        protected WebSocketClient(string uri)
        {
            this._ws = new ClientWebSocket();
            this._ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
            this._uri = new Uri(uri);
            this._cancellationToken = this._cancellationTokenSource.Token;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="uri">The URI of the WebSocket server.</param>
        /// <returns></returns>
        public static WebSocketClient Create(string uri)
        {
            return new WebSocketClient(uri);
        }

        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        /// <returns></returns>
        public WebSocketClient Connect()
        {
            this.ConnectAsync();
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been established.
        /// </summary>
        /// <param name="onConnect">The Action to call.</param>
        /// <returns></returns>
        public WebSocketClient OnConnect(Action<WebSocketClient> onConnect)
        {
            this._onConnected = onConnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been terminated.
        /// </summary>
        /// <param name="onDisconnect">The Action to call</param>
        /// <returns></returns>
        public WebSocketClient OnDisconnect(Action<WebSocketClient> onDisconnect)
        {
            this._onDisconnected = onDisconnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns></returns>
        public WebSocketClient OnMessage(Action<string, WebSocketClient> onMessage)
        {
            this._onMessage = onMessage;
            return this;
        }

        /// <summary>
        /// Send a message to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(string message)
        {
            this.SendMessageAsync(message);
        }

        private async Task SendMessageAsync(string message)
        {
            if (this._ws.State != WebSocketState.Open)
            {
                throw new Exception("Connection is not open.");
            }

            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var messagesCount = (int)Math.Ceiling((double)messageBuffer.Length / WebSocketClient.SendChunkSize);

            for (var i = 0; i < messagesCount; i++)
            {
                var offset = (WebSocketClient.SendChunkSize * i);
                var count = WebSocketClient.SendChunkSize;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length)
                {
                    count = messageBuffer.Length - offset;
                }

                await this._ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text, lastMessage, this._cancellationToken);
            }
        }

        private async Task ConnectAsync()
        {
            await this._ws.ConnectAsync(this._uri, this._cancellationToken);
            this.CallOnConnected();
            this.StartListen();
        }

        private async Task StartListen()
        {
            var buffer = new byte[WebSocketClient.ReceiveChunkSize];

            try
            {
                while (this._ws.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();


                    WebSocketReceiveResult result;
                    do
                    {
                        result = await this._ws.ReceiveAsync(new ArraySegment<byte>(buffer), this._cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                this._ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            this.CallOnDisconnected();
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    this.CallOnMessage(stringResult);

                }
            }
            catch (Exception)
            {
                this.CallOnDisconnected();
            }
            finally
            {
                this._ws.Dispose();
            }
        }

        private void CallOnMessage(StringBuilder stringResult)
        {
            if (this._onMessage != null)
                WebSocketClient.RunInTask(() => this._onMessage(stringResult.ToString(), this));
        }

        private void CallOnDisconnected()
        {
            if (this._onDisconnected != null)
                WebSocketClient.RunInTask(() => this._onDisconnected(this));
        }

        private void CallOnConnected()
        {
            if (this._onConnected != null)
                WebSocketClient.RunInTask(() => this._onConnected(this));
        }

        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }
    }
}