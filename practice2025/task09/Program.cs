using System;
using task09;

namespace task09
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Использование: Program <путь_к_библиотеке>");
                return;
            }

            string libraryPath = args[0];
            string metadata = ReflectionHelper.GetLibraryMetadata(libraryPath);
        }
    }
}