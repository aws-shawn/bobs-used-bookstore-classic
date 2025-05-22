using System;
using System.ComponentModel;
using Bookstore.Domain;
using Xunit;

namespace Bookstore.Tests.Helpers
{
    public class EnumExtensionsTests
    {
        private enum TestEnum
        {
            [Description("First Value")]
            FirstValue,
            
            SecondValue,
            
            [Description("Third Custom Value")]
            ThirdValue
        }
        
        [Fact]
        public void GetDescription_WithDescriptionAttribute_ReturnsDescription()
        {
            // Arrange
            var enumValue = TestEnum.FirstValue;
            
            // Act
            var result = enumValue.GetDescription();
            
            // Assert
            Assert.Equal("First Value", result);
        }
        
        [Fact]
        public void GetDescription_WithoutDescriptionAttribute_ReturnsEnumName()
        {
            // Arrange
            var enumValue = TestEnum.SecondValue;
            
            // Act
            var result = enumValue.GetDescription();
            
            // Assert
            Assert.Equal("SecondValue", result);
        }
        
        [Fact]
        public void GetDescription_WithCustomDescription_ReturnsCustomDescription()
        {
            // Arrange
            var enumValue = TestEnum.ThirdValue;
            
            // Act
            var result = enumValue.GetDescription();
            
            // Assert
            Assert.Equal("Third Custom Value", result);
        }
    }
}