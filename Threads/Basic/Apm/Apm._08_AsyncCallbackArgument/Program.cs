using System;
using System.Threading;

namespace Apm._08_AsyncCallbackArgument
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Action<string, int> actionDelegate = new(PrintIterations);

            AsyncCallback actionDelegateCallback = new(PrintIterationsCallback);

            IAsyncResult actionResult = actionDelegate.BeginInvoke("AsyncTask", 100, actionDelegateCallback, "Callback of AsyncTask");

            PrintIterations("SyncTask", 200);

            Console.ReadKey();
        }

        private static void PrintIterations(string taskName, int iterationsDelay)
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - ThreadId:{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(iterationsDelay);
            }

            Console.WriteLine($"TaskName:{taskName} completed in Thread#{Environment.CurrentManagedThreadId}");
        }

        private static void PrintIterationsCallback(IAsyncResult ar)
        {
            Console.WriteLine($"{ar.AsyncState} has executed in Thread#{Environment.CurrentManagedThreadId}");
        }
    }
}
