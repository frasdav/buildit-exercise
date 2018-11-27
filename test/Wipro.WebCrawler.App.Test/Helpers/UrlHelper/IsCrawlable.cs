using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Wipro.WebCrawler.App.Test.Helpers.UrlHelper
{
    [TestClass]
    public class IsCrawlable
    {
        [TestMethod]
        public void UrlHelper_IsCrawlable_ReturnsTrueForHttpUrl()
        {
            // Arrange.

            var urlHelper = new App.Helpers.UrlHelper();

            // Act.

            var actual = urlHelper.IsCrawlable(new Uri("http://test.com"));

            // Assert.

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void UrlHelper_IsCrawlable_ReturnsTrueForHttpsUrl()
        {
            // Arrange.

            var urlHelper = new App.Helpers.UrlHelper();

            // Act.

            var actual = urlHelper.IsCrawlable(new Uri("https://test.com"));

            // Assert.

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void UrlHelper_IsCrawlable_ReturnsFalseForNontHttpOrHttpsUrl()
        {
            // Arrange.

            var urlHelper = new App.Helpers.UrlHelper();

            // Act.

            var actual = urlHelper.IsCrawlable(new Uri("file://test.com"));

            // Assert.

            Assert.IsFalse(actual);
        }
    }
}
