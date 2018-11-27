using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wipro.WebCrawler.Common;
using Wipro.WebCrawler.Common.Interfaces;
using Wipro.WebCrawler.Common.Interfaces.Helpers;

namespace Wipro.WebCrawler.App
{
    public class StandardWebCrawler : IWebCrawler
    {
        private readonly ILogger _logger;
        private readonly IRequestHelper _requestHelper;
        private readonly IUrlHelper _urlHelper;

        public StandardWebCrawler(ILogger<StandardWebCrawler> logger, IRequestHelper requestHelper, IUrlHelper urlHelper)
        {
            _logger = logger;
            _requestHelper = requestHelper;
            _urlHelper = urlHelper;
        }

        public async Task<IList<CrawlerResult>> CrawlAsync(Uri url)
        {
            if (!url.IsAbsoluteUri)
                throw new ApplicationException("Starting Url must be absolute.");

            _logger.LogInformation($"Starting {url.ToString()}.");

            // Invoke the recursive crawl, defining the top level domain scope as the host of the initial Uri.
            var crawlerResults = new List<CrawlerResult>();
            await CrawlAsync(url, crawlerResults, url.Host);

            _logger.LogInformation($"Finished {url.ToString()}.");

            return crawlerResults;
        }

        private async Task CrawlAsync(Uri url, IList<CrawlerResult> crawlerResults, string topLevelDomain)
        {
            // Strip the query string and trim trailing slash from the Url.
            var urlWithoutQuery = new Uri(url.GetLeftPart(UriPartial.Path).TrimEnd('/'));

            // If the Url isn't crawlable, return.
            if (!_urlHelper.IsCrawlable(urlWithoutQuery))
            {
                _logger.LogInformation($"Skipping {urlWithoutQuery.ToString()} - not crawlable.");
                return;
            }

            // If this Url has already been crawled, return.
            if (crawlerResults.Any(r => r.Url == urlWithoutQuery))
            {
                _logger.LogInformation($"Skipping {urlWithoutQuery.ToString()} - already been crawled.");
                return;
            }

            // If the Url isn't in the top level domain it shouldn't be crawled.
            if (!urlWithoutQuery.Host.EndsWith(topLevelDomain))
            {
                _logger.LogInformation($"Skipping {urlWithoutQuery.ToString()} - not under top level domain {topLevelDomain}.");

                // Add a crawler result with the Url and type external and return.
                crawlerResults.Add(new CrawlerResult { Url = urlWithoutQuery, Type = "external" });
                return;
            }

            _logger.LogInformation($"Crawling {urlWithoutQuery.ToString()}");

            // Get the response from the Url.
            var response = await _requestHelper.GetResponseAsync(urlWithoutQuery).ConfigureAwait(false);

            // If the response is null, return.
            if (response == null)
                return;

            // Load the Html content from the response into a document for parsing.
            var document = new HtmlDocument();
            document.LoadHtml(response.Item2);

            // Get the content type.
            var contentType = response.Item1.Content.Headers.ContentType;

            // Add a crawler result with the Url and content type.
            var crawlerResult = new CrawlerResult()
            {
                Url = urlWithoutQuery,
                Type = contentType?.ToString()
            };
            crawlerResults.Add(crawlerResult);

            // Use Xpath to get all anchor nodes that have an href.
            var anchorNodes = document.DocumentNode.SelectNodes("//a[@href]");

            _logger.LogDebug($"Found {(anchorNodes != null ? anchorNodes.Count : 0)} anchors with an href on page at {urlWithoutQuery.ToString()}.");

            // If no anchors are found, return.
            if (anchorNodes == null)
                return;

            foreach (var anchorNode in anchorNodes)
            {
                // Get the href attribute from the anchor.
                var href = anchorNode.Attributes["href"].Value;

                // Get the absolute Url for this anchor.
                var thisUrl = _urlHelper.GetAbsoluteUrl(href, urlWithoutQuery);

                // Add the Url as a page link.
                crawlerResult.AddLink(thisUrl);

                // Recurse.
                await CrawlAsync(thisUrl, crawlerResults, topLevelDomain);
            }
        }
    }
}
