namespace PlayerUnknown.Crawler.Logic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    using PlayerUnknown.Crawler.Logic.Structures;

    internal class Crawler
    {
        /// <summary>
        /// Gets or sets the event invoked when a page has been crawled.
        /// </summary>
        internal EventHandler<APage> OnPageCrawled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the event invoked when this instance is disposed.
        /// </summary>
        internal EventHandler Disposed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the website URI.
        /// </summary>
        internal Uri Uri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the web browser.
        /// </summary>
        internal WebClient WebClient
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        internal bool IsLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        internal bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crawler"/> class.
        /// </summary>
        internal Crawler()
        {
            this.WebClient = new WebClient();
            var Extensions = new List<string>
             {
                 "text/css",
                 "text/html",
                 "text/plain",

                 "application/javascript",
                 "application/json",

                 "image/png",
                 "image/jpeg"
             };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crawler"/> class.
        /// </summary>
        /// <param name="Uri">The URI.</param>
        internal Crawler(Uri Uri) : this()
        {
            this.SetUri(Uri);
        }

        /// <summary>
        /// Sets the URI.
        /// </summary>
        /// <param name="Uri">The URI.</param>
        internal void SetUri(Uri Uri)
        {
            this.Uri = Uri;
        }

        /// <summary>
        /// Loads the specified page asynchronously.
        /// </summary>
        /// <param name="PageUrl">The page URL.</param>
        internal async Task<APage> LoadAsync(string PageUrl)
        {
            var RequestedUrl = Path.Combine(this.Uri.AbsoluteUri, PageUrl);
            var RequestedUri = new Uri(RequestedUrl);
            var Page         = new APage(RequestedUri);

            try
            {
                var Content  = await this.WebClient.DownloadStringTaskAsync(RequestedUri.AbsoluteUri);

                if (!string.IsNullOrEmpty(Content))
                {
                    Page.SetContent(Content);
                }
            }
            catch (Exception Exception)
            {
                Page.SetError(Exception);
            }

            if (this.OnPageCrawled != null)
            {
                try
                {
                    this.OnPageCrawled.Invoke(this, Page);
                }
                catch (Exception)
                {
                    // ..
                }
            }

            return Page;
        }

        /// <summary>
        /// Loads the specified page asynchronously and executes the callback.
        /// </summary>
        /// <param name="PageUrl">The page URL.</param>
        /// <param name="Callback">The callback.</param>
        internal async Task LoadAsync(string PageUrl, Action<APage> Callback)
        {
            var RequestedUrl = Path.Combine(this.Uri.AbsoluteUri, PageUrl);
            var RequestedUri = new Uri(RequestedUrl);

            await LoadAsync(RequestedUri, Callback);
        }

        /// <summary>
        /// Loads the specified page asynchronously and executes the callback.
        /// </summary>
        /// <param name="PageUrl">The page URL.</param>
        /// <param name="Callback">The callback.</param>
        internal async Task LoadAsync(Uri PageUrl, Action<APage> Callback)
        {
            var Page         = new APage(PageUrl);

            try
            {
                var Content  = await this.WebClient.DownloadStringTaskAsync(PageUrl.AbsoluteUri);

                if (!string.IsNullOrEmpty(Content))
                {
                    Page.SetContent(Content);
                }
            }
            catch (Exception Exception)
            {
                Page.SetError(Exception);
            }

            if (Callback != null)
            {
                try
                {
                    Callback(Page);
                }
                catch (Exception)
                {
                    // ..
                }
            }
        }

        /// <summary>
        /// Exécute les tâches définies par l'application associées à la
        /// libération ou à la redéfinition des ressources non managées.
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.IsDisposed = true;

            // ..

            if (this.WebClient != null)
            {
                this.WebClient.Dispose();
            }

            // ..

            if (this.Disposed != null)
            {
                try
                {
                    this.Disposed.Invoke(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                    // ...
                }
            }
        }
    }
}
