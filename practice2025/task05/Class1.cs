using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ClassAnalyzer
{
    private readonly Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
        => _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
               .Where(m => !m.IsSpecialName) 
               .Select(m => m.Name);

    public IEnumerable<string> GetMethodParams(string methodName)
    {
        var method = _type.GetMethod(methodName);
        return method?.GetParameters()
                   .Select(p => p.Name)
               ?? Enumerable.Empty<string>();
    }

    public IEnumerable<string> GetAllFields()
        => _type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
               .Where(f => !f.Name.Contains("<")) 
               .Select(f => f.Name);

    public IEnumerable<string> GetProperties()
        => _type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Select(p => p.Name);

    public bool HasAttribute<T>() where T : Attribute
        => _type.IsDefined(typeof(T));
}