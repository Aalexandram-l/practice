using System;
using task17;

namespace task17
{
    public sealed class RepeatableStepCommand : ICommand
    {
        private readonly ICommand _step;
        private readonly int _totalSteps;
        private int _done;
        private readonly ServerThread _scheduler;

        public RepeatableStepCommand(ICommand step, int totalSteps, ServerThread scheduler)
        {
            _step        = step;
            _totalSteps  = totalSteps;
            _scheduler   = scheduler;
        }

        public void Execute()
        {
            _step.Execute();
            _done++;

            if (_done < _totalSteps)
            {
                _scheduler.Enqueue(this);
            }
        }
    }
}