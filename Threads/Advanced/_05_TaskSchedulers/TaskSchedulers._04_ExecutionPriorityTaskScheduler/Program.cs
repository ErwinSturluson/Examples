using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._04_ExecutionPriorityTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ExecutionPriorityTaskScheduler taskScheduler = new();

            Task<int>[] tasks = Enumerable.Range(1, 10)
                .Select(i => new Task<int>(PrintIterations, $"AsyncTask{i}"))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task<int> highPriorityTask = new(PrintIterations, "High-Priority Task 31");
            highPriorityTask.Start(taskScheduler);
            taskScheduler.TryIncreasePriority(highPriorityTask);

            Task<int> lowPriorityTask = new(PrintIterations, "Low-Priority Task 32");
            lowPriorityTask.Start(taskScheduler);
            taskScheduler.TryDecreasePriority(lowPriorityTask);

            Task.WaitAll(tasks);
            Task.WaitAll(highPriorityTask, lowPriorityTask);
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Thread.Sleep(100);
            }

            int calculationResult = iterationIndex * 1000;

            return calculationResult;
        }
    }
}
