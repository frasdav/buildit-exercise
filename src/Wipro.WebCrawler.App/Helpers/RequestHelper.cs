using System;
using System.Net.Http;
using System.Threading.Tasks;
using Wipro.WebCrawler.Interfaces.Helpers;

namespace Wipro.WebCrawler.App.Helpers
{
    public class RequestHelper : IRequestHelper
    {
        private readonly HttpClient _httpClient;

        public RequestHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Tuple<HttpResponseMessage, string>> GetResponseAsync(Uri url)
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);

            var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            var content = System.Text.Encoding.UTF8.GetString(bytes);

            return new Tuple<HttpResponseMessage, string>(response, content);
        }
    }
}
