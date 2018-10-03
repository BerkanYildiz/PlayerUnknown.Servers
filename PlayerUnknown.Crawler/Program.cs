namespace PlayerUnknown.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    using Abot.Crawler;
    using Abot.Poco;

    internal static class Program
    {
        /// <summary>
        /// Gets the official PUBG's game front-end hostname.
        /// </summary>
        private const string FrontEndHostname = "http://prod-live-front.playbattlegrounds.com/index.html";

        /// <summary>
        /// Gets the official PUBG's game front-end hostname, from the latest update.
        /// </summary>
        private static string UpdateFrontEndHostname;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            var WebsiteToCrawl = new Uri(FrontEndHostname, UriKind.Absolute);
            var WebsiteCrawler = GetCrawler();

            using (var CancellationSource = new CancellationTokenSource())
            {
                CrawlResult WebsiteResult;

                try
                {
                    WebsiteResult = WebsiteCrawler.Crawl(WebsiteToCrawl, CancellationSource);

                    if (WebsiteResult != null)
                    {
                        Console.WriteLine("[*] The pre-crawling process has ended.");

                        if (WebsiteResult.ErrorOccurred == false)
                        {
                            Console.WriteLine("[*] No error(s) occured.");

                            if (string.IsNullOrEmpty(Program.UpdateFrontEndHostname) == false)
                            {
                                Console.WriteLine("[*] Crawling again..");

                                WebsiteResult = WebsiteCrawler.Crawl(WebsiteResult.RootUri);
                            }
                            else
                            {
                                
                            }
                        }
                        else
                        {
                            Console.WriteLine("[*] " + WebsiteResult.ErrorException.GetType().Name + " : " + WebsiteResult.ErrorException.Message + ".");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[*] Failed to crawl the website.");
                    }
                }
                catch (Exception Exception)
                {
                    Console.WriteLine("[*] " + Exception.GetType().Name + " : " + Exception.Message + ".");
                }
            }

            Console.ReadKey(false);
        }

        /// <summary>
        /// Called when a crawling task has been processed.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Args">The arguments.</param>
        private static void OnCrawlingProcessed(object Sender, PageCrawlCompletedArgs Args)
        {
            if (Args.CrawledPage.Uri.AbsoluteUri == FrontEndHostname)
            {
                var RedString = "location.href=";
                var RedIndex  = Args.CrawledPage.Content.Text.IndexOf(RedString, StringComparison.Ordinal);

                if (RedIndex == -1)
                {
                    return;
                }

                RedIndex += RedString.Length;

                if (Args.CrawledPage.Content.Text[RedIndex] != '"')
                {
                    return;
                }

                var CurrentChar = ++RedIndex;
                var HostName    = new StringBuilder();

                while (Args.CrawledPage.Content.Text[CurrentChar] != '"')
                {
                    HostName.Append(Args.CrawledPage.Content.Text[CurrentChar]);

                    if (CurrentChar++ >= Args.CrawledPage.Content.Text.Length)
                    {
                        break;
                    }
                }

                if (HostName[HostName.Length - 1] == '?')
                {
                    HostName.Remove(HostName.Length - 1, 1);
                }

                Program.UpdateFrontEndHostname = "http://" + Args.CrawledPage.Uri.Host + HostName;
            }

            Log.Warning(typeof(Program), Args.CrawledPage.Uri.AbsoluteUri);
        }

        /// <summary>
        /// Gets the crawler.
        /// </summary>
        private static IWebCrawler GetCrawler()
        {
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

            var Config = new CrawlConfiguration
            {
                CrawlTimeoutSeconds                 = 0,
                DownloadableContentTypes            = string.Join(", ", Extensions),
                IsExternalPageCrawlingEnabled       = false,
                IsExternalPageLinksCrawlingEnabled  = false,
                IsRespectRobotsDotTextEnabled       = false,
                IsUriRecrawlingEnabled              = false,
                MaxConcurrentThreads                = 10,
                MaxPagesToCrawl                     = 0,
                MaxPagesToCrawlPerDomain            = 0,
                MinCrawlDelayPerDomainMilliSeconds  = 1000
            };

            var Crawler = new PoliteWebCrawler(Config, null, null, null, null, null, null, null, null);

            Crawler.PageCrawlCompleted             += OnCrawlingProcessed;

            return Crawler;
        }
    }
}
