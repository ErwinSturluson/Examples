using System;
using System.Threading;

namespace Apm._04_Delegate.BeginInvokeParameters
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Func<string, int, int> funcDelegate = new(PrintIterations);

            IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, null, null);

            int funcIterationNumber = funcDelegate.EndInvoke(funcResult);

            Console.WriteLine($"Result of {nameof(funcIterationNumber)} from async call:{funcIterationNumber}");

            int iterationNumber = PrintIterations("SyncTask", 200);

            Console.WriteLine($"Result of {nameof(iterationNumber)} from sync call:{iterationNumber}");

            Console.ReadKey();
        }

        private static int PrintIterations(string taskName, int iterationsDelay)
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - {iterationNumber}");
                Thread.Sleep(iterationsDelay);
            }

            return iterationNumber;
        }
    }
}
