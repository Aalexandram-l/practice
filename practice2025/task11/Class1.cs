using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Task11
{
    public interface ICalculator
    {
        int Add(int a, int b);
        int Minus(int a, int b);
        int Mul(int a, int b);
        int Div(int a, int b);
    }

    public static class CalculatorGenerator
    {
        private const string CalculatorCode = @"
            using Task11;
            
            public class DynamicCalculator : ICalculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }
            
            new DynamicCalculator()
        ";

        public static async Task<ICalculator> CreateCalculatorAsync()
        {
            try
            {
                var options = ScriptOptions.Default
                    .WithReferences(typeof(ICalculator).Assembly)
                    .WithImports("Task11");

                var result = await CSharpScript.EvaluateAsync<ICalculator>(CalculatorCode, options);
                return result;
            }
            catch (CompilationErrorException ex)
            {
                throw new Exception($"Compilation error: {string.Join("\n", ex.Diagnostics)}");
            }
        }
    }
}