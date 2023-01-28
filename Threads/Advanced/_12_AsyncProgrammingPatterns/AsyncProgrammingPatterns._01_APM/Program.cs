using System;
using System.Collections.Generic;
using System.Threading;

namespace AsyncProgrammingPatterns._00_APM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Func<string, IEnumerable<string>> asyncOperation_Apm = PrintIterations;

            IAsyncResult asyncResult = asyncOperation_Apm.BeginInvoke("AsyncOperation_APM", null, null);

            IEnumerable<string> iterations = asyncOperation_Apm.EndInvoke(asyncResult);

            foreach (string iteration in iterations)
            {
                Console.WriteLine(iteration);
            }

            Console.ReadKey();
        }

        private static IEnumerable<string> PrintIterations(string callName)
        {
            List<string> iterationList = new List<string>();

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                iterationList.Add($"{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");

                Thread.Sleep(100);
            }

            return iterationList;
        }
    }
}
