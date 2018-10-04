namespace PlayerUnknown.Crawler.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using PlayerUnknown.Crawler.Logic.Structures;

    internal static class ContentHelpers
    {
        internal static string GetUriFromIndex(APage Page)
        {
            if (Page.HasThrewError || Page.IsEmpty)
            {
                return string.Empty;
            }

            var RedString = "location.href=";
            var RedIndex  = Page.Content.IndexOf(RedString, StringComparison.Ordinal);

            if (RedIndex == -1)
            {
                return string.Empty;
            }

            RedIndex += RedString.Length;

            if (Page.Content[RedIndex] != '"')
            {
                return string.Empty;
            }

            var CurrentChar = ++RedIndex;
            var HostName    = new StringBuilder();

            while (Page.Content[CurrentChar] != '"')
            {
                HostName.Append(Page.Content[CurrentChar]);

                if (CurrentChar++ >= Page.Content.Length)
                {
                    break;
                }
            }

            if (HostName[HostName.Length - 1] == '?')
            {
                HostName.Remove(HostName.Length - 1, 1);
            }

            return "http://" + Page.Uri.Host + HostName;
        }

        internal static IEnumerable<string> GetAllUrls(APage Page)
        {
            if (Page.HasThrewError || Page.IsEmpty)
            {
                yield break;
            }

            for (int i = 0; i < Page.Length; i++)
            {
                if (Page.Content[i] == '"')
                {
                    var Url = new StringBuilder();

                    while (Page.Content[++i] != '"')
                    {
                        if (i + 1 >= Page.Content.Length)
                        {
                            break;
                        }

                        Url.Append(Page.Content[i]);
                    }

                    var Uri = Url.ToString();

                    if (Uri.EndsWith(".html") || Uri.EndsWith(".css") || Uri.EndsWith(".js") || Uri.EndsWith(".jpg") || Uri.EndsWith(".jpeg") || Uri.EndsWith(".png"))
                    {
                        yield return Url.ToString();
                    }
                }
            }
        }
    }
}
