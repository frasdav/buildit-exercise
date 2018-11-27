using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wipro.WebCrawler.Interfaces.Helpers
{
    public interface IRequestHelper
    {
        Task<Tuple<HttpResponseMessage, string>> GetResponseAsync(Uri url);
    }
}
