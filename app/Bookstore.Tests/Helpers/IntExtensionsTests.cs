using System;
using Bookstore.Web.Helpers;
using Xunit;

namespace Bookstore.Tests.Helpers
{
    public class IntExtensionsTests
    {
        [Theory]
        [InlineData(500, "500 B")]
        [InlineData(1024, "1 KB")]
        [InlineData(1500, "1.46 KB")]
        [InlineData(1048576, "1 MB")]
        [InlineData(1073741824, "1 GB")]
        [InlineData(1099511627776, "1 TB")]
        public void ToStorageSize_ReturnsCorrectFormattedString(int value, string expected)
        {
            // Act
            var result = value.ToStorageSize();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}