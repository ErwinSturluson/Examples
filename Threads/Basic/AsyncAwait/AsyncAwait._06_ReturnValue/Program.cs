using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._06_ReturnValue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int printIterationsResult = PrintIterationsAsync("AsyncTask").GetAwaiter().GetResult();

            Console.WriteLine($"{nameof(PrintIterations)} Result: {printIterationsResult}");

            Console.ReadKey();
        }

        private static async Task<int> PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"{nameof(PrintIterationsAsync)} method before await has started in Thread#{Environment.CurrentManagedThreadId}");

            Task<int> printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            int printIterationsTaskResult = await printIterationsTask;

            Console.WriteLine($"{nameof(PrintIterationsAsync)} method after await (Continuation part) has finished in Thread#{Environment.CurrentManagedThreadId}");

            return printIterationsTaskResult;
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculatedResult = iterationNumber * 100;

            return calculatedResult;
        }
    }
}
