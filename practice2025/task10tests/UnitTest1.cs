using Xunit;
using System;
using System.IO;
using PluginLoader;

namespace task10tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestPluginLoadingAndExecution()
        {
            string pluginsDirectory = "plugins";
            Directory.CreateDirectory(pluginsDirectory);

            File.Copy("PluginLibrary1.dll", Path.Combine(pluginsDirectory, "PluginLibrary1.dll"), true);
            File.Copy("PluginLibrary2.dll", Path.Combine(pluginsDirectory, "PluginLibrary2.dll"), true);

            using (var consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                Program.Main(new string[] { pluginsDirectory });  
                var output = consoleOutput.ToString();

                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                Assert.Equal(2, lines.Length);
                Assert.Contains("Plugin1 executed.", lines[0]);
                Assert.Contains("Plugin2 executed.", lines[1]);
            }

            Directory.Delete(pluginsDirectory, true);
        }
    }
}