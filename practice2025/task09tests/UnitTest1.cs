using System;
using System.IO;
using Xunit;
using task09;

namespace task09tests
{
    public class UnitTest1
    {
        [Fact]
        public void GetLibraryMetadata_WithValidPath_ReturnsMetadata()
        {
            string libraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "task07.dll"); // Убедитесь, что это корректный путь к существующей библиотеке
            string expectedOutputStart = "Метаданные библиотеки: ";

            string result = ReflectionHelper.GetLibraryMetadata(libraryPath);

            Assert.StartsWith(expectedOutputStart, result);
        }

        [Fact]
        public void GetLibraryMetadata_WithInvalidPath_ThrowsException()
        {
            string libraryPath = "path/to/invalid/library.dll";

            var exception = Assert.Throws<FileNotFoundException>(() => ReflectionHelper.GetLibraryMetadata(libraryPath));
            Assert.Contains("не найден", exception.Message);
        }
    }
}