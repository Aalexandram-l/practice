using System;
using System.Reflection;

namespace task07
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public DisplayNameAttribute(string name)
        {
            DisplayName = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }

        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }

    [DisplayName("Пример класса")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Тестовый метод")]
        public void TestMethod()

        {
            Console.WriteLine("Метод работает!");
        }

        [DisplayName("Числовое свойство")]
        public int Number { get; set; }
    }

    public static class ReflectionHelper

    {
        public static void PrintTypeInfo(Type type)
        {
            var nameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
            if (nameAttr != null)
            {
                Console.WriteLine($"Имя класса: {nameAttr.DisplayName}");
            }

            var versionAttr = type.GetCustomAttribute<VersionAttribute>();
            if (versionAttr != null)
            {
                Console.WriteLine($"Версия: {versionAttr.Major}.{versionAttr.Minor}");
            }

            var allMembers = type.GetMembers();
            foreach (var member in allMembers)
            {
                var memberNameAttr = member.GetCustomAttribute<DisplayNameAttribute>();
                if (memberNameAttr != null)
                {
                    Console.WriteLine($"{member.GetType().Name} {member.Name}: {memberNameAttr.DisplayName}");
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            ReflectionHelper.PrintTypeInfo(typeof(SampleClass));
            Console.ReadKey(); 
        }
    }
}