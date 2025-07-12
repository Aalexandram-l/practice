using Xunit;

public class BDefiniteIntegralTests
{
    [Fact]
    public void TestIntegralOfSin()
    {
        var SIN = (double x) => Math.Sin(x);
        Assert.Equal(0, DefiniteIntegral.Solve(-100, 100, SIN, 1e-4, 8), 1e-4);
    }
}