using System;
using System.IO;
using System.Threading.Tasks;
using Bookstore.Data.FileServices;
using Xunit;

namespace Bookstore.Tests.Data
{
    public class LocalFileServiceTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly LocalFileService _fileService;
        
        public LocalFileServiceTests()
        {
            // Create a temporary directory for testing
            _testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_testDirectory);
            _fileService = new LocalFileService(_testDirectory);
        }
        
        [Fact]
        public async Task SaveAsync_WithValidFile_SavesFileAndReturnsPath()
        {
            // Arrange
            var testContent = "Test file content";
            var testFileName = "test.txt";
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            writer.Write(testContent);
            writer.Flush();
            stream.Position = 0;
            
            // Act
            var result = await _fileService.SaveAsync(stream, testFileName);
            
            // Assert
            Assert.NotNull(result);
            Assert.Contains("/Content/images/coverimages/", result);
            
            // Verify the file exists in the expected location
            var expectedDir = Path.Combine(_testDirectory, "images", "coverimages");
            Assert.True(Directory.Exists(expectedDir));
            
            // Extract the filename from the result path
            var fileName = Path.GetFileName(result.Replace("/Content/images/coverimages/", ""));
            var filePath = Path.Combine(expectedDir, fileName);
            Assert.True(File.Exists(filePath));
        }
        
        [Fact]
        public async Task SaveAsync_WithNullFile_ReturnsNull()
        {
            // Act
            var result = await _fileService.SaveAsync(null, "test.txt");
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task DeleteAsync_WithExistingFile_DeletesFile()
        {
            // Arrange
            var testFilePath = Path.Combine(_testDirectory, "testDelete.txt");
            File.WriteAllText(testFilePath, "Test content");
            Assert.True(File.Exists(testFilePath));
            
            // Act
            await _fileService.DeleteAsync(testFilePath);
            
            // Assert
            Assert.False(File.Exists(testFilePath));
        }
        
        [Fact]
        public async Task DeleteAsync_WithNonExistentFile_DoesNotThrowException()
        {
            // Arrange
            var nonExistentPath = Path.Combine(_testDirectory, "nonexistent.txt");
            
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _fileService.DeleteAsync(nonExistentPath));
            Assert.Null(exception);
        }
        
        public void Dispose()
        {
            // Clean up the test directory
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }
    }
}