using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgrammingPatterns._03_TAP
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IEnumerable<string> iterations = await PrintIterationsAsync("AsyncTask_TAP");

            foreach (string iteration in iterations)
            {
                Console.WriteLine(iteration);
            }

            Console.ReadKey();
        }

        private static async Task<IEnumerable<string>> PrintIterationsAsync(string taskName)
        {
            return await Task.Run(() => PrintIterations(taskName));
        }

        private static IEnumerable<string> PrintIterations(string callName)
        {
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
