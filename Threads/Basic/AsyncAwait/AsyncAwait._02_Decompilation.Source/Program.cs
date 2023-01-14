using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._02_Decompilation.Source
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
            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;
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
