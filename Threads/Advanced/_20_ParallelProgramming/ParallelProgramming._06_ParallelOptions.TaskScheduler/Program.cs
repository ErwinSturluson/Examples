using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._06_ParallelOptions.TaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ParallelOptions options = new()
            {
                TaskScheduler = new ParallelTaskScheduler()
            };

            Parallel.For(1, 10, options, PrintIteration);
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($">TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(500);
        }
    }

    internal class ParallelTaskScheduler : System.Threading.Tasks.TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks() => null;

        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"=[Task#{task.Id}] is processing through [{nameof(ParallelTaskScheduler)}.{nameof(QueueTask)}].");

            ThreadPool.QueueUserWorkItem((_) =>
            {
                TryExecuteTask(task);
            });
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"-[Task#{task.Id}] is processing through [{nameof(ParallelTaskScheduler)}.{nameof(TryExecuteTaskInline)}].");
            return TryExecuteTask(task);
        }
    }
}
