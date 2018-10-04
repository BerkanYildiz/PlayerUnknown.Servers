namespace PlayerUnknown.Crawler.Logic.Structures
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using PlayerUnknown.Crawler.Helpers;

    internal class APage
    {
        /// <summary>
        /// Gets or sets the event invoked when this instance is disposed.
        /// </summary>
        internal EventHandler Disposed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page URI.
        /// </summary>
        internal Uri Uri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the page content.
        /// </summary>
        internal string Content
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the detected urls.
        /// </summary>
        internal List<string> DetectedUrls
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        internal Exception Exception
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating this page content length.
        /// </summary>
        internal int Length
        {
            get
            {
                if (string.IsNullOrEmpty(this.Content))
                {
                    return 0;
                }

                return this.Content.Length;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this page is empty.
        /// </summary>
        internal bool IsEmpty
        {
            get
            {
                return this.Length == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance threw an exception.
        /// </summary>
        internal bool HasThrewError
        {
            get
            {
                return this.Exception != null;
            }
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
        /// Initializes a new instance of the <see cref="APage"/> class.
        /// </summary>
        internal APage()
        {
            this.DetectedUrls = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APage"/> class.
        /// </summary>
        /// <param name="Uri">The URI.</param>
        internal APage(Uri Uri) : this()
        {
            this.Uri = Uri;
        }

        /// <summary>
        /// Sets the content.
        /// </summary>
        /// <param name="Content">The content.</param>
        internal void SetContent(string Content)
        {
            this.Content = Content;

            if (!string.IsNullOrEmpty(Content))
            {
                // We don't want to find urls in pages heavier than 1MB.

                if (Content.Length >= 100000)
                {
                    return;
                }

                DetectedUrls.AddRange(ContentHelpers.GetAllUrls(this));
            }
        }

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="Exception">The exception.</param>
        internal void SetError(Exception Exception)
        {
            this.Exception = Exception;
        }

        /// <summary>
        /// Saves this <see cref="APage"/> to a file.
        /// </summary>
        internal void SaveToFile()
        {
            if (this.HasThrewError)
            {
                return;
            }

            var CurrentDirectory = Directory.GetCurrentDirectory();
            var FilePath         = Path.Combine(CurrentDirectory, this.Uri.AbsolutePath.Replace(@"/", @"\").TrimStart('\\'));
            var FileName         = FilePath.Split('\\').Last();
            var DirectoryPath    = FilePath.Replace(FileName, string.Empty);

            if (!Directory.Exists(DirectoryPath))
            {
                try
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                catch (Exception)
                {
                    // ..
                }
            }

            try
            {
                if (this.IsEmpty)
                {
                    File.Create(FilePath, 0).Dispose();
                }
                else
                {
                    File.WriteAllText(FilePath, this.Content);
                }
            }
            catch (Exception)
            {
                // ..
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
