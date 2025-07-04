using PluginAttributes;
using PluginLibrary1;

namespace PluginLibrary2
{
    [PluginLoad("PluginLibrary1.Plugin1")]
    public class Plugin2 : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Plugin2 executed.");
        }
    }
}