using System;
using Bookstore.Domain;
using Xunit;

namespace Bookstore.Tests.Domain
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void StartOfMonth_ReturnsFirstDayOfMonth()
        {
            // Arrange
            var date = new DateTime(2023, 5, 15, 14, 30, 45);
            
            // Act
            var result = date.StartOfMonth();
            
            // Assert
            Assert.Equal(new DateTime(2023, 5, 1, 0, 0, 0), result);
        }
        
        [Fact]
        public void OneSecondToMidnight_ReturnsLastSecondOfDay()
        {
            // Arrange
            var date = new DateTime(2023, 5, 15, 14, 30, 45);
            
            // Act
            var result = date.OneSecondToMidnight();
            
            // Assert
            Assert.Equal(new DateTime(2023, 5, 15, 23, 59, 59), result);
        }
    }
}