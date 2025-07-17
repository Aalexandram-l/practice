namespace task17
{
    public sealed class LongTask : ICommand
    {
        private int _steps = 10;
        public void Execute()
        {
            if (_steps > 0) _steps--;
        }
        public bool IsDone => _steps == 0;
    }
}