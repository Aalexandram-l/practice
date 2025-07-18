using System.Collections.Generic;

namespace task17
{
    public sealed class RoundRobinScheduler : IScheduler
    {
        private readonly Queue<ICommand> _pool = new();
        private readonly object _sync = new();

        public bool HasCommand() { lock (_sync) return _pool.Count > 0; }

        public ICommand Select()
        {
            lock (_sync)
            {
                if (_pool.Count == 0) return null;
                var cmd = _pool.Dequeue();
                _pool.Enqueue(cmd);
                return cmd;
            }
        }

        public void Add(ICommand cmd) { lock (_sync) _pool.Enqueue(cmd); }
    }
}