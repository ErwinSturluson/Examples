using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._03_Task.TaskCreationOptions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<int> task = new(PrintIterations, "AsyncTask", System.Threading.Tasks.TaskCreationOptions.LongRunning);
            task.Start();

            Thread.Sleep(100);

            int rmcResult = PrintIterations("RegularMethodCall");

            Console.WriteLine($"AsyncTask Result: {task.Result}.");
            Console.WriteLine($"RegularMethodCall Result: {rmcResult}.");
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationNumber * 1000;

            return calculationResult;
        }
    }
}
