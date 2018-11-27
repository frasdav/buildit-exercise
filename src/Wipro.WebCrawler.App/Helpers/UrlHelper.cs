using System;
using Wipro.WebCrawler.Common.Interfaces.Helpers;

namespace Wipro.WebCrawler.App.Helpers
{
    public class UrlHelper : IUrlHelper
    {
        public Uri GetAbsoluteUrl(string url, Uri parent)
        {
            if (url.Contains("://"))
            {
                return new Uri(url);
            }
            else
            {
                return new Uri(parent, url);
            }
        }

        public bool IsCrawlable(Uri url)
        {
            return url.Scheme == "http" || url.Scheme == "https";

        }
    }
}
