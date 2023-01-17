using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._06_Task.Closures
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string taskName = "AsyncTask";
            int iterationNumber = 10;
            int iterationDelayMilliseconds = 100;

            Task<int> task = Task.Run(() => PrintIterations(taskName, iterationNumber, iterationDelayMilliseconds));

            task.Wait();

            Console.WriteLine($"AsyncTask has finished. Task's Result: {task.Result}");
        }

        private static int PrintIterations(string taskName, int iterationNumber, int iterationDelayMilliseconds)
        {
            int iterationIndex = 0;

            while (iterationIndex < iterationNumber)
            {
                iterationIndex++;

                Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(iterationDelayMilliseconds);
            }

            int calculationResult = iterationIndex * 1000;

            return calculationResult;
        }
    }
}
