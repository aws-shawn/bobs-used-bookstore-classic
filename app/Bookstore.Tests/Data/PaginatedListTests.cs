using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Domain;
using Moq;
using Xunit;

namespace Bookstore.Tests.Data
{
    public class PaginatedListTests
    {
        private class TestEntity : Entity
        {
            public string Name { get; set; }
        }
        
        [Fact]
        public async Task PopulateAsync_SetsCorrectPageProperties()
        {
            // Arrange
            var testData = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Item 1" },
                new TestEntity { Id = 2, Name = "Item 2" },
                new TestEntity { Id = 3, Name = "Item 3" },
                new TestEntity { Id = 4, Name = "Item 4" },
                new TestEntity { Id = 5, Name = "Item 5" },
                new TestEntity { Id = 6, Name = "Item 6" },
                new TestEntity { Id = 7, Name = "Item 7" },
                new TestEntity { Id = 8, Name = "Item 8" },
                new TestEntity { Id = 9, Name = "Item 9" },
                new TestEntity { Id = 10, Name = "Item 10" }
            };
            
            var queryable = testData.AsQueryable();
            var pageIndex = 2;
            var pageSize = 3;
            
            // Create a mock DbSet
            var mockDbSet = new Mock<System.Data.Entity.DbSet<TestEntity>>();
            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            
            // Create a PaginatedList with the mock DbSet
            var paginatedList = new PaginatedList<TestEntity>(queryable, pageIndex, pageSize);
            
            // Act
            await paginatedList.PopulateAsync();
            
            // Assert
            Assert.Equal(pageIndex, paginatedList.PageIndex);
            Assert.Equal(4, paginatedList.TotalPages); // 10 items / 3 per page = 4 pages (ceiling)
            Assert.True(paginatedList.HasPreviousPage);
            Assert.True(paginatedList.HasNextPage);
            Assert.Equal(3, paginatedList.Count); // Should have 3 items on page 2
            
            // Check that we have the correct items (4, 5, 6)
            Assert.Equal(4, paginatedList[0].Id);
            Assert.Equal(5, paginatedList[1].Id);
            Assert.Equal(6, paginatedList[2].Id);
        }
        
        [Fact]
        public async Task PopulateAsync_OnLastPage_HasNextPageIsFalse()
        {
            // Arrange
            var testData = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Item 1" },
                new TestEntity { Id = 2, Name = "Item 2" },
                new TestEntity { Id = 3, Name = "Item 3" },
                new TestEntity { Id = 4, Name = "Item 4" },
                new TestEntity { Id = 5, Name = "Item 5" }
            };
            
            var queryable = testData.AsQueryable();
            var pageIndex = 2;
            var pageSize = 3;
            
            // Create a PaginatedList
            var paginatedList = new PaginatedList<TestEntity>(queryable, pageIndex, pageSize);
            
            // Act
            await paginatedList.PopulateAsync();
            
            // Assert
            Assert.Equal(pageIndex, paginatedList.PageIndex);
            Assert.Equal(2, paginatedList.TotalPages); // 5 items / 3 per page = 2 pages (ceiling)
            Assert.True(paginatedList.HasPreviousPage);
            Assert.False(paginatedList.HasNextPage); // On last page
            Assert.Equal(2, paginatedList.Count); // Should have 2 items on page 2
            
            // Check that we have the correct items (4, 5)
            Assert.Equal(4, paginatedList[0].Id);
            Assert.Equal(5, paginatedList[1].Id);
        }
        
        [Fact]
        public async Task PopulateAsync_OnFirstPage_HasPreviousPageIsFalse()
        {
            // Arrange
            var testData = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Item 1" },
                new TestEntity { Id = 2, Name = "Item 2" },
                new TestEntity { Id = 3, Name = "Item 3" },
                new TestEntity { Id = 4, Name = "Item 4" },
                new TestEntity { Id = 5, Name = "Item 5" }
            };
            
            var queryable = testData.AsQueryable();
            var pageIndex = 1;
            var pageSize = 3;
            
            // Create a PaginatedList
            var paginatedList = new PaginatedList<TestEntity>(queryable, pageIndex, pageSize);
            
            // Act
            await paginatedList.PopulateAsync();
            
            // Assert
            Assert.Equal(pageIndex, paginatedList.PageIndex);
            Assert.Equal(2, paginatedList.TotalPages); // 5 items / 3 per page = 2 pages (ceiling)
            Assert.False(paginatedList.HasPreviousPage); // On first page
            Assert.True(paginatedList.HasNextPage);
            Assert.Equal(3, paginatedList.Count); // Should have 3 items on page 1
            
            // Check that we have the correct items (1, 2, 3)
            Assert.Equal(1, paginatedList[0].Id);
            Assert.Equal(2, paginatedList[1].Id);
            Assert.Equal(3, paginatedList[2].Id);
        }
    }
}