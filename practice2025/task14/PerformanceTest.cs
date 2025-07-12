using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class PerformanceTest
{
    public static void Main()
    {
        double a = -100;
        double b = 100;
        Func<double, double> function = Math.Sin;
        double[] steps = {1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        int[] threadCounts = { 1, 2, 4, 8, 16, 32 };

        double optimalStep = 0;              
        foreach (var step in steps)
        {
            double result = DefiniteIntegral.SolveSingleThread(a, b, function, step);
            if (Math.Abs(result-0.0) < 1e-4)
            {
                optimalStep = step;
                break;
            }
        }


        if (optimalStep == 0)
        {
            Console.WriteLine("Не удалось достичь точности 1e-4.");
            return;
        }

        Dictionary<int, double> timeByThreads = new();
        double singleTime = 0;
        for (int i = 0; i < 5; i++)
        {
            var sw = Stopwatch.StartNew();
            DefiniteIntegral.SolveSingleThread(a, b, function, optimalStep);
            sw.Stop();
            singleTime += sw.Elapsed.TotalMilliseconds;
        }
        singleTime /= 5;
        foreach (var threads in threadCounts)
        {
            double total = 0;
            for (int i = 0; i < 5; i++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.Solve(a, b, function, optimalStep, threads);
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            timeByThreads[threads] = total / 5;
        }

        var best = timeByThreads.OrderBy(kv => kv.Value).First();

        using (var writer = new StreamWriter("results.txt"))
        {
            writer.WriteLine($"Оптимальный шаг: {optimalStep:E1}");
            writer.WriteLine($"Оптимальное количество потоков: {best.Key}");
            writer.WriteLine($"Однопоточное время: {singleTime:F2} мс");
            writer.WriteLine($"Многопоточное время: {best.Value:F2} мс");
            writer.WriteLine($"Улучшение: {(timeByThreads[1] - best.Value) / timeByThreads[1] * 100:F1}%");
        }

        using (var writer = new StreamWriter("graph_data.txt"))
        {
            foreach (var kv in timeByThreads)
                writer.WriteLine($"{kv.Key}:{kv.Value:F2}");
        }

        Console.WriteLine($"Оптимальный шаг: {optimalStep:E1}");
        Console.WriteLine($"Оптимальное количество потоков: {best.Key}");
        Console.WriteLine($"Улучшение: {(timeByThreads[1] - best.Value) / timeByThreads[1] * 100:F1}%");
    }
}