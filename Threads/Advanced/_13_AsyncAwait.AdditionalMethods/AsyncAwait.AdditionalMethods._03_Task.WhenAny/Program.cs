using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdditionalMethods._03_Task.WhenAny
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Task<IEnumerable<string>> iterationsCompletedTask = await Task.WhenAny(PrintIterationsAsync("AsyncTask1"), PrintIterationsAsync("AsyncTask2"));

            IEnumerable<string> iterations = iterationsCompletedTask.Result;

            Console.WriteLine($"Tasks with Id#{iterationsCompletedTask.Id} has completed first.");

            foreach (var iteration in iterations)
            {
                Console.WriteLine(iteration);
            }
        }

        private static async Task<IEnumerable<string>> PrintIterationsAsync(string taskName)
        {
            Task<IEnumerable<string>> printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            return await printIterationsTask;
        }

        private static IEnumerable<string> PrintIterations(object state)
        {
            string callName = state.ToString();

            List<string> iterationList = new();

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                iterationList.Add($"{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");

                Thread.Sleep(100);
            }

            return iterationList;
        }
    }
}
