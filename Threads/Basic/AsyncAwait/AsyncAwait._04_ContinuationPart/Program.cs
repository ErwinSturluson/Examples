using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._04_ContinuationPart
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintIterationsAsync("AsyncTask");

            Console.ReadKey();
        }

        private static async void PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"{nameof(PrintIterationsAsync)} method before await has started in Thread#{Environment.CurrentManagedThreadId}");

            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;

            Console.WriteLine($"{nameof(PrintIterationsAsync)} method after await (Continuation part) has finished in Thread#{Environment.CurrentManagedThreadId}");
        }

        private static void PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }
        }
    }
}
