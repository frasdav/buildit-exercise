using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wipro.WebCrawler.Interfaces
{
    public interface IWebCrawler
    {
        Task<Dictionary<Uri, string>> CrawlAsync(Uri uri);
    }
}
