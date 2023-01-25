using System;
using System.Threading;

namespace AsyncAwait.SyncContext._07_Void
{
    internal class VoidSynchronizationContext : SynchronizationContext
    {
        private static readonly object _consoleLock = new();

        public override void OperationStarted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(VoidSynchronizationContext.OperationStarted)}");
            Console.ResetColor();
        }

        public override void OperationCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(VoidSynchronizationContext.OperationCompleted)}");
            Console.ResetColor();
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (_consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{nameof(Post)} method has been executed.");
                    Console.ResetColor();
                }

                callback(state);
            }, null);
        }
    }
}
