using System;
using System.Linq;
using Bookstore.Domain.Carts;
using Xunit;

namespace Bookstore.Tests.Domain
{
    public class ShoppingCartTests
    {
        [Fact]
        public void Constructor_SetsCorrelationId()
        {
            // Arrange
            var correlationId = "test-correlation-id";
            
            // Act
            var cart = new ShoppingCart(correlationId);
            
            // Assert
            Assert.Equal(correlationId, cart.CorrelationId);
        }
        
        [Fact]
        public void AddItemToShoppingCart_AddsItemWithCorrectProperties()
        {
            // Arrange
            var cart = new ShoppingCart("test-id");
            var bookId = 123;
            var quantity = 2;
            
            // Act
            cart.AddItemToShoppingCart(bookId, quantity);
            
            // Assert
            var item = cart.ShoppingCartItems.FirstOrDefault();
            Assert.NotNull(item);
            Assert.Equal(bookId, item.BookId);
            Assert.Equal(quantity, item.Quantity);
            Assert.True(item.WantToBuy);
        }
        
        [Fact]
        public void AddItemToWishlist_AddsItemWithCorrectProperties()
        {
            // Arrange
            var cart = new ShoppingCart("test-id");
            var bookId = 123;
            
            // Act
            cart.AddItemToWishlist(bookId);
            
            // Assert
            var item = cart.ShoppingCartItems.FirstOrDefault();
            Assert.NotNull(item);
            Assert.Equal(bookId, item.BookId);
            Assert.Equal(1, item.Quantity);
            Assert.False(item.WantToBuy);
        }
        
        [Fact]
        public void GetWishListItems_ReturnsOnlyWishlistItems()
        {
            // Arrange
            var cart = new ShoppingCart("test-id");
            cart.AddItemToShoppingCart(1, 1);
            cart.AddItemToWishlist(2);
            cart.AddItemToWishlist(3);
            
            // Act
            var wishlistItems = cart.GetWishListItems().ToList();
            
            // Assert
            Assert.Equal(2, wishlistItems.Count);
            Assert.All(wishlistItems, item => Assert.False(item.WantToBuy));
            Assert.Contains(wishlistItems, item => item.BookId == 2);
            Assert.Contains(wishlistItems, item => item.BookId == 3);
        }
        
        [Fact]
        public void MoveWishListItemToShoppingCart_ChangesWantToBuyToTrue()
        {
            // Arrange
            var cart = new ShoppingCart("test-id");
            cart.AddItemToWishlist(1);
            var wishlistItemId = cart.ShoppingCartItems.First().Id;
            
            // Act
            cart.MoveWishListItemToShoppingCart(wishlistItemId);
            
            // Assert
            var item = cart.ShoppingCartItems.First();
            Assert.True(item.WantToBuy);
        }
        
        [Fact]
        public void MoveWishListItemToShoppingCart_WithInvalidId_DoesNothing()
        {
            // Arrange
            var cart = new ShoppingCart("test-id");
            cart.AddItemToWishlist(1);
            var invalidId = 999;
            
            // Act
            cart.MoveWishListItemToShoppingCart(invalidId);
            
            // Assert
            var item = cart.ShoppingCartItems.First();
            Assert.False(item.WantToBuy);
        }
    }
}