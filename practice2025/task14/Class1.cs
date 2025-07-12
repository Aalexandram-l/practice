using System;
using System.Threading;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        if (b <= a) return 0;
        if (step <= 0) throw new ArgumentException("Шаг должен быть положительным");
        if (threadsNumber <= 0) throw new ArgumentException("Количество потоков должно быть положительным");

        double totalSum = 0;
        double range = b - a;
        double chunkSize = range / threadsNumber;

        Barrier barrier = new Barrier(threadsNumber, (bar) =>
        {
            Console.WriteLine("Все потоки завершили вычисления.");
        });

        Thread[] threads = new Thread[threadsNumber];
        double[] partialSums = new double[threadsNumber];

        for (int i = 0; i < threadsNumber; i++)
        {
            int threadId = i;
            threads[i] = new Thread(() =>
            {
                double start = a + threadId * chunkSize;
                double end = (threadId == threadsNumber - 1) ? b : start + chunkSize;
                double sum = 0;

                double x = start;
                while (x < end)
                {
                    double nextX = Math.Min(x + step, end);
                    double f1 = function(x);
                    double f2 = function(nextX);
                    sum += (f1 + f2) * (nextX - x) / 2;
                    x = nextX;
                }

                partialSums[threadId] = sum;

                barrier.SignalAndWait();
            });
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        foreach (var sum in partialSums)
        {
            totalSum += sum;
        }

        return totalSum;
    }
    public static double SolveSingleThread(double a, double b, Func<double, double> f, double step)
    {
        if (b <= a) return 0;
        if (step <= 0) throw new ArgumentException("Шаг должен быть положительным");
        double sum = 0;
        for (double x = a; x < b; x += step)
        {
            double nextX = Math.Min(x + step, b);
            sum += (f(x) + f(nextX)) * (nextX - x) / 2;
        }
        return sum;
    }
}