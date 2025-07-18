using System;
using task17;

namespace task17
{
    public sealed class TestStepCommand : ICommand
    {
        private readonly int _id;
        private int _counter;

        public TestStepCommand(int id) => _id = id;

        public void Execute()
        {
            _counter++;
            var msg = $"Поток {_id} вызов {_counter}";
            Console.WriteLine(msg);
            Reporter.Write(msg);
            Reporter.WriteCsv(_id, _counter);
        }
    }
}