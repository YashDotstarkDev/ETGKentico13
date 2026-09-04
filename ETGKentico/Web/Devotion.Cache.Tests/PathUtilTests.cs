using Devotion.Cache.Common;
using NUnit.Framework;

namespace Devotion.Cache.Tests
{
    [TestFixture]
    public class PathUtilTests
    {
        [Test]
        public void TrimAliasPath_Trims_Correctly()
        {
            // Arrange
            var path = "/home/%";

            // Act
            var trimmed = PathUtil.TrimAliasPath(path);

            // Assert
            Assert.AreEqual("/home", trimmed);
        }
    }
}
