using Xunit;
using System;
using System.IO;
using System.Text;
using CommandLib;
using FileSystemCommands;

namespace task08tests
{
    public class FileSystemCommandsTests : IDisposable
    {
        private readonly string _testDir;
        private readonly StringWriter _output;

        public FileSystemCommandsTests()
        {
            _testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);
        
            _output = new StringWriter();
            Console.SetOut(_output);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDir))
            {
                Directory.Delete(_testDir, true);
            }
            
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            command.Execute();

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldCalculateCorrectSize()
        {
            var file1 = Path.Combine(_testDir, "test1.txt");
            var file2 = Path.Combine(_testDir, "test2.txt");
            File.WriteAllText(file1, new string('a', 1000));
            File.WriteAllText(file2, new string('b', 2000));
            
            var command = new DirectorySizeCommand(_testDir);
            command.Execute();
            var output = _output.ToString();

            Assert.Contains("3000 bytes", output);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldHandleEmptyDirectory()
        {
            var command = new DirectorySizeCommand(_testDir);
            command.Execute();
            var output = _output.ToString();

            Assert.Contains("0 bytes", output);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldShowErrorForInvalidPath()
        {
            var invalidPath = Path.Combine(_testDir, "nonexistent");
            var command = new DirectorySizeCommand(invalidPath);
            command.Execute();
            var output = _output.ToString();

            Assert.Contains("not found", output, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFilesWithOutputCheck()
        {
            File.WriteAllText(Path.Combine(_testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(_testDir, "file2.log"), "Log");
            File.WriteAllText(Path.Combine(_testDir, "notes.txt"), "Notes");
            
            var command = new FindFilesCommand(_testDir, "*.txt");
            command.Execute();
            var output = _output.ToString();

            Assert.Contains("Found 2 files", output);
            Assert.Contains("file1.txt", output);
            Assert.Contains("notes.txt", output);
            Assert.DoesNotContain("file2.log", output);
        }

        [Fact]
        public void FindFilesCommand_ShouldHandleNoMatches()
        {
            File.WriteAllText(Path.Combine(_testDir, "file1.log"), "Log");
            
            var command = new FindFilesCommand(_testDir, "*.txt");
            command.Execute();
            var output = _output.ToString();

            Assert.Contains("Found 0 files", output);
        }

        [Fact]
        public void FindFilesCommand_ShouldShowErrorForInvalidPath()
        {
            var invalidPath = Path.Combine(_testDir, "nonexistent");
            var command = new FindFilesCommand(invalidPath, "*.*");
            command.Execute();
            var output = _output.ToString();

            Assert.Contains("not found", output, StringComparison.OrdinalIgnoreCase);
        }
    }
}