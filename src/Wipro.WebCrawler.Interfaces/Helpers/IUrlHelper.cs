using System;

namespace Wipro.WebCrawler.Interfaces.Helpers
{
    public interface IUrlHelper
    {
        Uri GetAbsoluteUrl(string url, Uri parent);
    }
}
