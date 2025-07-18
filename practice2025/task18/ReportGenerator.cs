using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using task17;

namespace task18
{
    public static class ReportGenerator
    {
        private const string OutputDir = "/home/ome23/practice/practice2025";

        public static void Run()
        {
            Directory.CreateDirectory(OutputDir);

            const int stepsPerTask = 1000; 
            var data = new List<(int Tasks, double Single, double Parallel)>();

            foreach (int tasks in new[] { 1, 3, 5 })
            {
                var sw = Stopwatch.StartNew();
                foreach (var _ in Enumerable.Range(0, tasks))
                {
                    var lt = new CpuLongTask();
                    for (int i = 0; i < stepsPerTask; i++) lt.Execute();
                }
                sw.Stop();
                double single = sw.Elapsed.TotalMilliseconds;

                sw.Restart();
                var sch = new ThreadSafeRoundRobinScheduler();

                foreach (var _ in Enumerable.Range(0, tasks * stepsPerTask))
                    sch.Add(new CpuLongTask());

                for (int i = 0; i < tasks * stepsPerTask; i++)
                {
                    var cmd = sch.Select();
                    cmd?.Execute();
                }
                sw.Stop();
                data.Add((tasks, single, sw.Elapsed.TotalMilliseconds));
            }

            var best = data.OrderBy(d => d.Parallel).First();
            using (var w = new StreamWriter(Path.Combine(OutputDir, "results.txt")))
            {
                w.WriteLine($"Оптимальный шаг: 1.0E+0");
                w.WriteLine($"Оптимальное количество задач: {best.Tasks}");
                w.WriteLine($"Однопоточное время: {best.Single:F2} мс");
                w.WriteLine($"Псевдопараллельное время: {best.Parallel:F2} мс");
                w.WriteLine($"Улучшение: {(best.Single - best.Parallel) / best.Single * 100:F1}%");
            }

            using (var w = new StreamWriter(Path.Combine(OutputDir, "graph_data.txt")))
            {
                foreach (var d in data) w.WriteLine($"{d.Tasks}:{d.Single:F2}:{d.Parallel:F2}");
            }

            Console.WriteLine("Готово");
        }

        private sealed class CpuLongTask : ICommand
        {
            public void Execute()
            {
                for (int i = 0; i < 1000; i++) Math.Sqrt(i);
            }
            public bool IsDone => true; 
        }

        private sealed class ThreadSafeRoundRobinScheduler : IScheduler
        {
            private readonly Queue<ICommand> _pool = new();
            private readonly object _sync = new();

            public bool HasCommand() { lock (_sync) return _pool.Count > 0; }
            public ICommand Select()
            {
                lock (_sync)
                {
                    if (_pool.Count == 0) return null!;
                    var cmd = _pool.Dequeue();
                    _pool.Enqueue(cmd);
                    return cmd;
                }
            }
            public void Add(ICommand cmd) { lock (_sync) _pool.Enqueue(cmd); }
        }
    }
}