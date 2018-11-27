using System;

namespace Wipro.WebCrawler.Common.Interfaces.Helpers
{
    public interface IUrlHelper
    {
        Uri GetAbsoluteUrl(string url, Uri parent);

        bool IsCrawlable(Uri url);
    }
}
