using System;
using System.Collections.Generic;
using System.Security.Claims;
using Bookstore.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Bookstore.Tests.Web
{
    public class HttpContextExtensionsTests
    {
        [Fact]
        public void GetShoppingCartCorrelationId_WithExistingCookie_ReturnsCookieValue()
        {
            // Arrange
            var cookieKey = "ShoppingCartId";
            var expectedId = "existing-cart-id";
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Cookies = new RequestCookieCollection(
                new Dictionary<string, string> { { cookieKey, expectedId } });
            
            // Act
            var result = httpContext.GetShoppingCartCorrelationId();
            
            // Assert
            Assert.Equal(expectedId, result);
            
            // Verify cookie was set in response
            Assert.True(httpContext.Response.Cookies.Cookies.ContainsKey(cookieKey));
            Assert.Equal(expectedId, httpContext.Response.Cookies.Cookies[cookieKey].Value);
        }
        
        [Fact]
        public void GetShoppingCartCorrelationId_WithAuthenticatedUser_ReturnsUserSub()
        {
            // Arrange
            var userSub = "user-sub-id";
            
            var httpContext = new DefaultHttpContext();
            var claims = new List<Claim>
            {
                new Claim("sub", userSub)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;
            
            // Act
            var result = httpContext.GetShoppingCartCorrelationId();
            
            // Assert
            Assert.Equal(userSub, result);
            
            // Verify cookie was set in response
            Assert.True(httpContext.Response.Cookies.Cookies.ContainsKey("ShoppingCartId"));
            Assert.Equal(userSub, httpContext.Response.Cookies.Cookies["ShoppingCartId"].Value);
        }
        
        [Fact]
        public void GetShoppingCartCorrelationId_WithoutCookieOrAuth_GeneratesNewGuid()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            
            // Act
            var result = httpContext.GetShoppingCartCorrelationId();
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            // Verify it's a valid GUID
            Assert.True(Guid.TryParse(result, out _));
            
            // Verify cookie was set in response
            Assert.True(httpContext.Response.Cookies.Cookies.ContainsKey("ShoppingCartId"));
            Assert.Equal(result, httpContext.Response.Cookies.Cookies["ShoppingCartId"].Value);
        }
    }
}