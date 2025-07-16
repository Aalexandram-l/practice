using System;
using task17;
public sealed record HardStop(ServerThread Target) : ICommand
{
    public void Execute()
    {
        if (Thread.CurrentThread != Target.Worker)
            throw new InvalidOperationException("HardStop может выполняться только в своём потоке.");

        Target.RequestStop(hard: true);
    }
}

public sealed record SoftStop(ServerThread Target) : ICommand
{
    public void Execute()
    {
        if (Thread.CurrentThread != Target.Worker)
            throw new InvalidOperationException("SoftStop может выполняться только в своём потоке.");

        Target.RequestStop(hard: false);
    }
}