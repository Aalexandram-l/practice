using Xunit;
using Moq;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }
    public string ReadOnlyProperty { get; }

    public void Method() { }
    public void MethodWithParams(int x, string y) { }
}

[Serializable]
public class AttributedClass { }

public class ClassWithAllMembers
{
    public int PublicField;
    private string _privateField;
    protected int ProtectedField;
    public int Property { get; set; }
    public string ReadOnlyProperty { get; }

    public void Method() { }
    public void MethodWithParams(int x, string y) { }
}

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods().ToList();

        Assert.Contains("Method", methods);
        Assert.Contains("MethodWithParams", methods);
        Assert.Equal(2, methods.Count);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields().ToList();

        Assert.Contains("PublicField", fields);
        Assert.Contains("_privateField", fields);
        Assert.Equal(2, fields.Count);
    }

    [Fact]
    public void GetMethodParams_ReturnsCorrectParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("MethodWithParams").ToList();

        Assert.Contains("x", parameters);
        Assert.Contains("y", parameters);
        Assert.Equal(2, parameters.Count);
    }

    [Fact]
    public void GetProperties_ReturnsAllProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties().ToList();

        Assert.Contains("Property", properties);
        Assert.Contains("ReadOnlyProperty", properties);
        Assert.Equal(2, properties.Count);
    }

    [Fact]
    public void HasAttribute_ReturnsTrueForAttributedClass()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }

    [Fact]
    public void HasAttribute_ReturnsFalseForRegularClass()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }

    [Fact]
    public void ComplexTest_ClassWithAllMembers()
    {
        var analyzer = new ClassAnalyzer(typeof(ClassWithAllMembers));

        var methods = analyzer.GetPublicMethods().ToList();
        var fields = analyzer.GetAllFields().ToList();
        var properties = analyzer.GetProperties().ToList();

        Assert.Equal(2, methods.Count);
        Assert.Equal(3, fields.Count); 
        Assert.Equal(2, properties.Count);
        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }
}