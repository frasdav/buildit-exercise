using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wipro.WebCrawler.Interfaces;
using Wipro.WebCrawler.Interfaces.Helpers;

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

        public async Task<Dictionary<Uri, string>> CrawlAsync(Uri url)
        {
            if (!url.IsAbsoluteUri)
                throw new ApplicationException("Starting Url must be absolute.");

            // Invoke the recursive crawl, defining the top level domain scope as the host of the initial Uri.
            var crawlerResults = new Dictionary<Uri, string>();
            await CrawlAsync(url, crawlerResults, url.Host);

            return crawlerResults;
        }

        private async Task CrawlAsync(Uri url, Dictionary<Uri, string> crawlerResults, string topLevelDomain)
        {
            // Strip the query string and trim trailing slash from the Url.
            var urlWithoutQuery = new Uri(url.GetLeftPart(UriPartial.Path).TrimEnd('/'));

            // If the Url isn't http or https, return.
            if (urlWithoutQuery.Scheme != "http" && urlWithoutQuery.Scheme != "https")
            {
                _logger.LogInformation($"Skipping Url {urlWithoutQuery.ToString()} - not crawlable.");
                return;
            }

            // If this Url has already been crawled, return.
            if (crawlerResults.Any(r => r.Key == url))
            {
                _logger.LogInformation($"Skipping Url {urlWithoutQuery.ToString()} - already been crawled.");
                return;
            }

            // If the Url isn't in the top level domain, return.
            if (!url.Host.EndsWith(topLevelDomain))
            {
                _logger.LogInformation($"Skippiung Url {urlWithoutQuery.ToString()} - not under top level domain {topLevelDomain}.");
                return;
            }

            _logger.LogInformation($"Crawling {urlWithoutQuery.ToString()}");

            // Get the response from the Url.
            var response = await _requestHelper.GetResponseAsync(url).ConfigureAwait(false);

            // If the response is null, return.
            if (response == null)
                return;

            // Load the Html content from the response into a document for parsing.
            var document = new HtmlDocument();
            document.LoadHtml(response.Item2);

            // Get the content type.
            var contentType = response.Item1.Content.Headers.ContentType;

            // Add the Url and content type to the dictionary of crawler results.
            crawlerResults.Add(url, contentType.ToString());

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

                // Recurse.
                await CrawlAsync(_urlHelper.GetAbsoluteUrl(href, url), crawlerResults, topLevelDomain);
            }
        }
    }
}
