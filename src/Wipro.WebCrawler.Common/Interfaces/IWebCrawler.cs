using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wipro.WebCrawler.Common.Interfaces
{
    public interface IWebCrawler
    {
        Task<IList<CrawlerResult>> CrawlAsync(Uri uri);
    }
}
