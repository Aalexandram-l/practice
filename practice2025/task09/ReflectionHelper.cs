using System;
using System.IO;
using System.Reflection;
using System.Text;
using task07;

namespace task09
{
    public static class ReflectionHelper
    {
        public static string GetLibraryMetadata(string libraryPath)
        {
            try
            {
                if (!File.Exists(libraryPath))
                {
                    throw new FileNotFoundException($"Файл '{libraryPath}' не найден.");
                }

                Assembly assembly = Assembly.LoadFrom(libraryPath);
                StringBuilder metadata = new StringBuilder();
                metadata.AppendLine($"Метаданные библиотеки: {assembly.FullName}");

                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    metadata.AppendLine($"\nКласс: {type.FullName}");

                    metadata.AppendLine("  Методы:");
                    MethodInfo[] methods = type.GetMethods();
                    foreach (MethodInfo method in methods)
                    {
                        metadata.Append($"    {method.Name}(");
                        ParameterInfo[] parameters = method.GetParameters();
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            metadata.Append($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                            if (i < parameters.Length - 1)
                            {
                                metadata.Append(", ");
                            }
                        }
                        metadata.AppendLine(")");
                    }

                    metadata.AppendLine("  Конструкторы:");
                    ConstructorInfo[] constructors = type.GetConstructors();
                    foreach (ConstructorInfo constructor in constructors)
                    {
                        metadata.Append($"    {constructor.Name}(");
                        ParameterInfo[] parameters = constructor.GetParameters();
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            metadata.Append($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                            if (i < parameters.Length - 1)
                            {
                                metadata.Append(", ");
                            }
                        }
                        metadata.AppendLine(")");
                    }
                }
                return metadata.ToString();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (BadImageFormatException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}