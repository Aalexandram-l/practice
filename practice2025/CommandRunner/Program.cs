using System;
using System.Reflection;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllPath = "FileSystemCommands.dll";

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                Type commandInterface = typeof(ICommand);

                foreach (Type type in assembly.GetTypes())
                {
                    if (commandInterface.IsAssignableFrom(type) && !type.IsInterface)
                    {
                        if (Activator.CreateInstance(type) is ICommand command)
                        {
                            command.Execute();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}