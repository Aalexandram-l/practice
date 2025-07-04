using Xunit;
using Task11;
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Task11.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public async Task CreateCalculatorAsync_ShouldReturnValidInstance()
        {
            var calculator = await CalculatorGenerator.CreateCalculatorAsync();
            
            Assert.NotNull(calculator);
            Assert.IsAssignableFrom<ICalculator>(calculator);
        }

        [Theory]
        [InlineData(2, 3, 5, "Add")]
        [InlineData(5, 3, 2, "Minus")]
        [InlineData(2, 3, 6, "Mul")]
        [InlineData(6, 3, 2, "Div")]
        [InlineData(0, 0, 0, "Add")]
        [InlineData(10, 10, 0, "Minus")]
        [InlineData(5, 0, 0, "Mul")]
        [InlineData(10, 2, 5, "Div")]
        [InlineData(-1, 1, 0, "Add")]
        [InlineData(0, 5, -5, "Minus")]
        [InlineData(-2, 4, -8, "Mul")]
        [InlineData(-10, 2, -5, "Div")]
        public async Task ArithmeticOperations_ShouldReturnCorrectResult(int a, int b, int expected, string operation)
        {
            var calculator = await CalculatorGenerator.CreateCalculatorAsync();
           
            switch (operation)
            {
                case "Add":
                    Assert.Equal(expected, calculator.Add(a, b));
                    break;
                case "Minus":
                    Assert.Equal(expected, calculator.Minus(a, b));
                    break;
                case "Mul":
                    Assert.Equal(expected, calculator.Mul(a, b));
                    break;
                case "Div":
                    if (b == 0)
                    {
                        Assert.Throws<DivideByZeroException>(() => calculator.Div(a, b));
                    }
                    else
                    {
                        Assert.Equal(expected, calculator.Div(a, b));
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown operation");
            }
        }
    }
}