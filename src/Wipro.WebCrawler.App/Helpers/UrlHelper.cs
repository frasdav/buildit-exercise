using System;
using Wipro.WebCrawler.Interfaces.Helpers;

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
    }
}
