using System;
using System.Threading;
using Xunit;
using task17;

namespace task17tests
{
    public class ServerThreadTests2
    {
        [Fact(Timeout = 1000)] 
        public void NewCommand_IsAdded_AndExecuted()
        {
            using var server = new ServerThread();
            var latch = new ManualResetEventSlim(false);
            
            server.Enqueue(new LambdaCommand(() => latch.Set()));
            server.Enqueue(new SoftStop(server));
            
            Assert.True(latch.Wait(500));
            server.Join();
        }

        [Fact(Timeout = 1000)]
        public void RoundRobin_SeveralCommands_ExecutedInTurn()
        {
            using var server = new ServerThread();
            var latch1 = new ManualResetEventSlim(false);
            var latch2 = new ManualResetEventSlim(false);

            server.Enqueue(new LambdaCommand(() => latch1.Set()));
            server.Enqueue(new LambdaCommand(() => latch2.Set()));
            server.Enqueue(new SoftStop(server));

            Assert.True(latch1.Wait(500));
            Assert.True(latch2.Wait(500));
            server.Join();
        }

        [Fact(Timeout = 1000)]
        public void Stop_ImmediatelyFinishes_Thread()
        {
            using var server = new ServerThread();
         
            server.RequestStop(hard: false);
       
            server.Enqueue(new LambdaCommand(() => {}));
            
            server.Join();
            Assert.False(server.Worker.IsAlive);
        }

        private sealed record LambdaCommand(Action Action) : ICommand
        {
            public void Execute() => Action();
        }
    }
}