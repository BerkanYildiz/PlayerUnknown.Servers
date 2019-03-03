namespace PlayerUnknown.Crawler
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PlayerUnknown.Crawler.Helpers;
    using PlayerUnknown.Crawler.Logic;

    internal static class Program
    {
        /// <summary>
        /// Gets the official PUBG's game front-end hostname.
        /// </summary>
        private const string FrontEndHostname = "http://prod-live-front.playbattlegrounds.com/";

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static async Task Main()
        {
            var WebsiteToCrawl = new Uri(FrontEndHostname, UriKind.Absolute);
            var WebsiteCrawler = new Crawler(WebsiteToCrawl);

            await WebsiteCrawler.LoadAsync("index.html", async Page =>
            {
                var Update     = ContentHelpers.GetUriFromIndex(Page);

                if (!string.IsNullOrEmpty(Update))
                {
                    await ProcessWebsite(WebsiteCrawler, new Uri(Update));
                }
            });

            Console.ReadKey(false);
        }

        /// <summary>
        /// Processes the website.
        /// </summary>
        /// <param name="Crawler">The crawler.</param>
        /// <param name="Host">The host.</param>
        private static async Task ProcessWebsite(Crawler Crawler, Uri Host)
        {
            Log.Warning(typeof(Program), "Processing '" + Host.AbsoluteUri + "'.");

            if (Crawler == null)
            {
                throw new ArgumentNullException(nameof(Crawler));
            }

            if (Host == null)
            {
                throw new ArgumentNullException(nameof(Host));
            }

            if (Host.AbsoluteUri.EndsWith(".html"))
            {
                Crawler.SetUri(new Uri(Host.AbsoluteUri.Replace(Host.Segments.Last(), string.Empty)));

                await Crawler.LoadAsync(Host, async Page =>
                {
                    if (Page.IsEmpty)
                    {
                        return;
                    }

                    foreach (var Url in Page.DetectedUrls)
                    {
                        if (Url.Contains("{@gate}"))
                        {
                            continue;
                        }

                        var Uri = new Uri(Crawler.Uri, Url);

                        if (Url.EndsWith(".js"))
                        {
                            var LastSection = Url.Split('/').Last();
                            var FileName    = LastSection.Split('.').First();

                            switch (FileName)
                            {
                                case "main":
                                case "runtime":
                                case "polyfills":
                                {
                                    await Crawler.LoadAsync(Url, async JSFile =>
                                    {
                                        JSFile.SaveToFile();
                                    });

                                    break;
                                }

                                case "coherent":
                                {
                                    await Crawler.LoadAsync(Url, async JSLibrary =>
                                    {
                                        JSLibrary.SaveToFile();
                                    });

                                    break;
                                }

                                default:
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            await Crawler.LoadAsync(Url, async OtherFile =>
                            {
                                OtherFile.SaveToFile();
                            });
                        }

                        Log.Warning(typeof(Program), "-> " + Uri.AbsolutePath);
                    }
                });
            }
            else
            {
                throw new ArgumentException("Host doesnt end with .html extension.", nameof(Host));
            }
        }
    }
}
