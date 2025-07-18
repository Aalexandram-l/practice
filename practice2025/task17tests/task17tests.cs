using System;
using System.Threading;
using Xunit;
using task17;

namespace task17tests
{
    public class ServerThreadTests
    {
        [Fact]
        public void HardStop_ExecutedInSameThread_StopsImmediately()
        {
            using var server = new ServerThread();
            server.Enqueue(new HardStop(server));
            server.Join();
            Assert.False(server.Worker.IsAlive);
        }

        [Fact]
        public void SoftStop_ExecutedInSameThread_StopsAfterQueueEmpty()
        {
            using var server = new ServerThread();
            var latch = new ManualResetEventSlim(false);

            server.Enqueue(new LambdaCommand(() => Thread.Sleep(10)));
            server.Enqueue(new LambdaCommand(() => latch.Set()));
            server.Enqueue(new SoftStop(server));

            server.Join();
            Assert.True(latch.IsSet);
        }
        
        [Fact]
        public void HardStop_FromAnotherThread_ThrowsException()
        {
            using var server = new ServerThread();
            var hardStop = new HardStop(server);
            
            var ex = Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
            Assert.Contains("только в своём потоке", ex.Message);
            
            var executedInServerThread = false;
            server.Enqueue(new LambdaCommand(() => {
                var exInThread = Record.Exception(() => hardStop.Execute());
                Assert.Null(exInThread);
                executedInServerThread = true;
            }));
            server.Enqueue(new SoftStop(server));
            server.Join();
    
            Assert.True(executedInServerThread);
        }

        [Fact]
        public void SoftStop_FromAnotherThread_ThrowsException()
        {
            using var server = new ServerThread();
            var softStop = new SoftStop(server);
            
            var ex = Assert.Throws<InvalidOperationException>(() => softStop.Execute());
            Assert.Contains("только в своём потоке", ex.Message);
    
            var executedInServerThread = false;
            server.Enqueue(new LambdaCommand(() => {
                var exInThread = Record.Exception(() => softStop.Execute());
                Assert.Null(exInThread); 
                executedInServerThread = true;
            }));
            
            server.Enqueue(new SoftStop(server));
            server.Join();
    
            Assert.True(executedInServerThread);
        }

        [Fact]
        public void EmptyQueue_DoesNotConsumeCPU()
        {
            using var server = new ServerThread();
            server.RequestStop(hard: false); 
            server.Join();
            Assert.False(server.Worker.IsAlive);
        }

        private sealed record LambdaCommand(Action Action) : ICommand
        {
            public void Execute() => Action();
        }
    }
} 