using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._01_KeywordsUsage
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(Main)} method has started in Thread#{Environment.CurrentManagedThreadId}");

            PrintIterationsAsync("AsyncTask");

            Console.ReadKey();

            Console.WriteLine($"{nameof(Main)} method has finished in Thread#{Environment.CurrentManagedThreadId}");
        }

        private static async void PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"{nameof(PrintIterationsAsync)} method has started in Thread#{Environment.CurrentManagedThreadId}");

            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;

            Console.WriteLine($"{nameof(PrintIterationsAsync)} method has finished in Thread#{Environment.CurrentManagedThreadId}");
        }

        private static void PrintIterations(object state)
        {
            Console.WriteLine($"{nameof(PrintIterations)} method has started in Thread#{Environment.CurrentManagedThreadId}");

            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"{nameof(PrintIterations)} method has finished in Thread#{Environment.CurrentManagedThreadId}");
        }
    }
}
