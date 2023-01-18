using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._03_LimitedConcurrencyTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LimitedConcurrencyTaskScheduler taskScheduler = new(2);

            Task<int>[] tasks = Enumerable.Range(1, 4)
                .Select(i => new Task<int>(PrintIterations, $"AsyncTask{i}"))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task.WaitAll(tasks);
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"Task with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationIndex * 1000;

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

            return calculationResult;
        }
    }
}
