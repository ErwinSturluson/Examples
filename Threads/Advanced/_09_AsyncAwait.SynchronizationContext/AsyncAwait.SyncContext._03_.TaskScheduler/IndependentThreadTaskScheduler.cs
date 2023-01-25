using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.SyncContext._03_.TaskScheduler
{
    internal class IndependentThreadTaskScheduler : System.Threading.Tasks.TaskScheduler
    {
        private static readonly object _consoleLock = new object();

        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{nameof(IndependentThreadTaskScheduler.QueueTask)} was executed for Task#{task.Id} in Thread#{Environment.CurrentManagedThreadId}");
                Console.ResetColor();
            }

            Thread taskThread = new(() => TryExecuteTask(task))
            {
                IsBackground = true
            };

            taskThread.Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{nameof(IndependentThreadTaskScheduler.TryExecuteTaskInline)} was executed for Task#{task.Id} in Thread#{Environment.CurrentManagedThreadId}");
                Console.ResetColor();
            }

            return TryExecuteTask(task);
        }
    }
}
