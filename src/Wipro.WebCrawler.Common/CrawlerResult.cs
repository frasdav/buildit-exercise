using System;
using System.Collections.Generic;
using System.Linq;

namespace Wipro.WebCrawler.Common
{
    public class CrawlerResult
    {
        public Uri Url { get; set; }

        public string Type { get; set; }

        public IReadOnlyCollection<Uri> Links {
            get
            {
                return _links.AsReadOnly();
            }
        }

        private List<Uri> _links;

        public CrawlerResult()
        {
            _links = new List<Uri>();
        }

        public void AddLink(Uri link)
        {
            if (!_links.Any(l => l.Equals(link)))
                _links.Add(link);
        }
    }
}
