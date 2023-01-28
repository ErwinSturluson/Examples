using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdditionalMethods._02_Task.WhenAll
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await Task.WhenAll(PrintIterationsAsync("AsyncTask1"), PrintIterationsAsync("AsyncTask2"));

            await Console.Out.WriteLineAsync("All The Async Tasks have completed.");
        }

        private static async Task PrintIterationsAsync(string taskName)
        {
            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($"{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");

                Thread.Sleep(100);
            }
        }
    }
}
