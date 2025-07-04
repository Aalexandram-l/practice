using PluginAttributes;
using PluginLibrary1;

namespace PluginLibrary1
{
    [PluginLoad]
    public class Plugin1 : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Plugin1 executed.");
        }
    }
}
