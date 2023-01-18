using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._01_Implementation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Testing_QueueTask();

            Testing_TryExecuteTaskInline();

            Testing_TryDequeue();
        }

        private static void Testing_QueueTask()
        {
            Console.WriteLine($"Testing of QueueTask method:");

            ConcreteTaskScheduler taskScheduler = new();

            Task<int>[] tasks = Enumerable.Range(1, 3)
                .Select(i => new Task<int>(PrintIterations, 0))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task.WaitAll(tasks);

            Console.WriteLine();
        }

        private static void Testing_TryExecuteTaskInline()
        {
            Console.WriteLine($"Testing of TryExecuteTaskInline method:");

            ConcreteTaskScheduler taskScheduler = new();
            taskScheduler.ExecuteTasksDelayMilliseconds = 2000;

            Task<int>[] tasks = Enumerable.Range(1, 3)
                .Select(i => new Task<int>(PrintIterations, 100))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Array.ForEach(tasks, t => t.Wait());

            Console.WriteLine();
        }

        private static void Testing_TryDequeue()
        {
            Console.WriteLine($"Testing of TryDequeue method:");

            ConcreteTaskScheduler taskScheduler = new();

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            cts.CancelAfter(200);

            Task<int>[] tasks = Enumerable.Range(1, 3)
               .Select(i => new Task<int>(PrintIterations, 100, ct))
               .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            try
            {
                Task.WaitAll(tasks);
            }
            catch (Exception)
            {
                Console.WriteLine($"Some tasks were canceled.");
            }

            Console.WriteLine();
        }

        private static int PrintIterations(object state)
        {
            int iterationDelay = (int)state;

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"Task with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(iterationDelay);
            }

            int calculationResult = iterationIndex * iterationDelay;

            return calculationResult;
        }
    }
}
