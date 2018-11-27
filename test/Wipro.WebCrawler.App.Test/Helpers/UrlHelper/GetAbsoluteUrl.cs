using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wipro.WebCrawler.App.Test.Helpers.UrlHelper
{
    [TestClass]
    public class GetAbsoluteUrl
    {
        [TestMethod]
        public void UrlHelper_GetAbsoluteUrl_ReturnsAbsoluteUrlFromAbsoluteUrl()
        {
            // Arrange.

            var urlHelper = new App.Helpers.UrlHelper();

            // Act.

            var actual = urlHelper.GetAbsoluteUrl("http://test.com/abc", new System.Uri("http://other.com"));

            // Assert.

            Assert.AreEqual("http://test.com/abc", actual.ToString());
        }

        [TestMethod]
        public void UrlHelper_GetAbsoluteUrl_ReturnsAbsoluteUrlFromRelativeUrl()
        {
            // Arrange.

            var urlHelper = new App.Helpers.UrlHelper();

            // Act.

            var actual = urlHelper.GetAbsoluteUrl("/abc", new System.Uri("http://test.com"));

            // Assert.

            Assert.AreEqual("http://test.com/abc", actual.ToString());
        }
    }
}
