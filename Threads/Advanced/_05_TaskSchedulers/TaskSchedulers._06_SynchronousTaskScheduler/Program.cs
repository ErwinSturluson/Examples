using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._06_SynchronousTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SynchronousTaskScheduler taskScheduler = new();

            Task<int>[] tasks = Enumerable.Range(1, 3)
                .Select(i => new Task<int>(PrintIterations, 100))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task.WaitAll(tasks);
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
