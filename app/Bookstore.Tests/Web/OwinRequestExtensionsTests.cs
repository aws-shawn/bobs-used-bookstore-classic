using Bookstore.Web.Helpers;
using Microsoft.Owin;
using Moq;
using Xunit;

namespace Bookstore.Tests.Web
{
    public class OwinRequestExtensionsTests
    {
        [Fact]
        public void GetReturnUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(r => r.Scheme).Returns("https");
            mockRequest.Setup(r => r.Host).Returns(new HostString("example.com"));
            
            // Act
            var result = mockRequest.Object.GetReturnUrl();
            
            // Assert
            Assert.Equal("https://example.com/signin-oidc", result);
        }
        
        [Fact]
        public void GetReturnUrl_WithDifferentSchemeAndHost_ReturnsCorrectUrl()
        {
            // Arrange
            var mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(r => r.Scheme).Returns("http");
            mockRequest.Setup(r => r.Host).Returns(new HostString("localhost:5000"));
            
            // Act
            var result = mockRequest.Object.GetReturnUrl();
            
            // Assert
            Assert.Equal("http://localhost:5000/signin-oidc", result);
        }
    }
}