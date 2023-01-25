using System;
using System.Threading;

namespace AsyncAwait.SyncContext._05_ConfigureAwait
{
    internal class ConsoleSynchronizationContext : SynchronizationContext
    {
        private static readonly object _consoleLock = new();

        public override void OperationStarted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(ConsoleSynchronizationContext.OperationStarted)}");
            Console.ResetColor();
        }

        public override void OperationCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(ConsoleSynchronizationContext.OperationCompleted)}");
            Console.ResetColor();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (_consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{nameof(Post)} method has been executed.");
                    Console.ResetColor();
                }

                ConsoleMessage message = new(d, state);
                ConsoleMessageListenter.AddMessage(message);
            }, null);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{nameof(Send)} method has been executed.");
                Console.ResetColor();
            }

            ConsoleMessage message = new(d, state);
            ConsoleMessageListenter.AddMessage(message);
        }
    }
}
