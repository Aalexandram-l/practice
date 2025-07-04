using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginAttributes;
using PluginLibrary1;

namespace PluginLoader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string pluginsDirectory = "plugins"; 
            var plugins = new List<PluginInfo>();

            foreach (var dll in Directory.GetFiles(pluginsDirectory, "*.dll"))
            {
                var assembly = Assembly.LoadFrom(dll);
                foreach (var type in assembly.GetTypes())
                {
                    var pluginLoadAttr = type.GetCustomAttribute<PluginLoadAttribute>();
                    if (pluginLoadAttr != null)
                    {
                        plugins.Add(new PluginInfo
                        {
                            Type = type,
                            Dependencies = pluginLoadAttr.Dependencies
                        });
                    }
                }
            }

            var sortedPlugins = SortPluginsByDependencies(plugins);

            foreach (var plugin in sortedPlugins)
            {
                var instance = Activator.CreateInstance(plugin.Type) as ICommand;
                instance.Execute();
            }
        }

        static List<PluginInfo> SortPluginsByDependencies(List<PluginInfo> plugins)
        {
            var sorted = new List<PluginInfo>();
            var visited = new HashSet<string>();

            foreach (var plugin in plugins)
            {
                if (!visited.Contains(plugin.Type.FullName))
                {
                    SortPlugin(plugin, plugins, sorted, visited);
                }
            }

            return sorted;
        }

        static void SortPlugin(PluginInfo plugin, List<PluginInfo> plugins, List<PluginInfo> sorted, HashSet<string> visited)
        {
            visited.Add(plugin.Type.FullName);

            foreach (var dependency in plugin.Dependencies)
            {
                var depPlugin = plugins.FirstOrDefault(p => p.Type.FullName == dependency);
                if (depPlugin != null && !visited.Contains(depPlugin.Type.FullName))
                {
                    SortPlugin(depPlugin, plugins, sorted, visited);
                }
            }

            sorted.Add(plugin);
        }
    }

    class PluginInfo
    {
        public Type Type { get; set; }
        public string[] Dependencies { get; set; }
    }
}