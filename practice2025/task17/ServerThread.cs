using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public sealed class ServerThread : IDisposable
    {
        private readonly BlockingCollection<ICommand> _queue = new();
        private volatile bool _hardStop;
        private volatile bool _softStop;
        public bool IsCurrentThread => Thread.CurrentThread == Worker;

        public Thread Worker { get; }

        public ServerThread()
        {
            Worker = new Thread(Run) { IsBackground = true };
            Worker.Start();
        }

        public void Enqueue(ICommand cmd) => _queue.Add(cmd);

        private void Run()
        {
            while (true)
            {
                if (_queue.TryTake(out var cmd, Timeout.Infinite))
                {
                    if (_hardStop) break;
                    if (_softStop && _queue.Count == 0) break;

                    try
                    {
                        cmd.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка в команде {cmd}: {ex}");
                    }

                    if (_hardStop) break;
                    if (_softStop && _queue.Count == 0) break;
                }
            }
        }

        public void RequestStop(bool hard)
        {
            _hardStop = hard;
            _softStop = !hard;
            Enqueue(new EmptyCommand()); 
        }

        public void Join() => Worker.Join();

        public void Dispose()
        {
            _queue.CompleteAdding();
            Worker.Join();
            _queue.Dispose();
        }

        private sealed record EmptyCommand : ICommand
        {
            public void Execute() { }
        }
    }
} 