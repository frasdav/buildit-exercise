using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Wipro.WebCrawler.App.Test.Helpers.RequestHelper
{
    [TestClass]
    public class GetContentAsync
    {
        private const string Content = "<html><head><title>test.com</title><head><body></body></html>";

        [TestMethod]
        public async Task RequestHelper_GetContentAsync_MakesHttpRequest()
        {
            // Arrange.

            var mockHttpMessageHandler = GetMockHttpMessageHandler();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var requestHelper = new App.Helpers.RequestHelper(httpClient);

            // Act.

            var actual = await requestHelper.GetResponseAsync(new Uri("http://test.com"));

            // Assert.

            mockHttpMessageHandler
                .Protected()
                .Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task RequestHelper_GetContentAsync_ReturnsHttpResponseMessage()
        {
            // Arrange.

            var mockHttpMessageHandler = GetMockHttpMessageHandler();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var requestHelper = new App.Helpers.RequestHelper(httpClient);

            // Act.

            var actual = await requestHelper.GetResponseAsync(new Uri("http://test.com"));

            // Assert.

            Assert.IsInstanceOfType(actual.Item1, typeof (HttpResponseMessage));
        }

        [TestMethod]
        public async Task RequestHelper_GetContentAsync_ReturnsContent()
        {
            // Arrange.

            var mockHttpMessageHandler = GetMockHttpMessageHandler();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var requestHelper = new App.Helpers.RequestHelper(httpClient);

            // Act.

            var actual = await requestHelper.GetResponseAsync(new Uri("http://test.com"));

            // Assert.

            Assert.AreEqual(Content, actual.Item2);
        }

        private Mock<HttpMessageHandler> GetMockHttpMessageHandler()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Content)
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(httpResponseMessage));

            return mockHttpMessageHandler;
        }
    }
}
