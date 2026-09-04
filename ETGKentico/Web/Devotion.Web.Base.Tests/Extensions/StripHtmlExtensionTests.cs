using Devotion.Web.Base.Providers;
using NUnit.Framework;

namespace Devotion.Web.Base.Tests.Extensions
{
    [TestFixture]
    public class StripHtmlExtensionTests
    {
        private HtmlSanitiserProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new HtmlSanitiserProvider();
        }

        [TestCase("<b>test1</b>", "<b>test1</b>")]
        [TestCase("<i>test1</i>", "<i>test1</i>")]
        [TestCase("<em>test1</em>", "<em>test1</em>")]
        [TestCase("<ul>test1</ul>", "<ul>test1</ul>")]
        [TestCase("<ul><li>test1</li></ul>", "<ul><li>test1</li></ul>")]
        [TestCase("<ol><li>test1</li></ol>", "<ol><li>test1</li></ol>")]
        [TestCase("<strong><script>test1</script></strong>", "")]
        [TestCase("<script>test1</script>", "")]
        [TestCase("<sccript>test1</sccript>", "")]
        [TestCase("<sccript>test1</sccript>", "")]
        [TestCase("<script>alert('a')</script>", "")]
        [TestCase("<script><b>alert('a')</b></script>", "")]
        [TestCase("<ol><li><script>test1</script></li></ol>", "<ol><li></li></ol>")]
        public void RemoveUnwantedTagsTests(string input, string expectedOutput)
        {
            var allowedTags = new[] { "strong", "em", "u", "li", "ol", "b", "i", "ul", "p", "br" };
            var result = _provider.RemoveUnwantedTags(input, allowedTags);
            Assert.AreEqual(expectedOutput, result);
        }
    }
}
