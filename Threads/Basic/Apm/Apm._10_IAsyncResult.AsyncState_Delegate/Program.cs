using System;
using System.Threading;

namespace Apm._10_IAsyncResult.AsyncState_Delegate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Func<string, int, int> funcDelegate = new(PrintIterations);

            IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, PrintIterationsCallback, funcDelegate);

            PrintIterations("SyncTask", 200);

            Console.ReadKey();
        }

        private static int PrintIterations(string taskName, int iterationsDelay)
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - ThreadId:{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(iterationsDelay);
            }

            Console.WriteLine($"TaskName:{taskName} completed in Thread#{Environment.CurrentManagedThreadId}");

            return iterationNumber;
        }

        private static void PrintIterationsCallback(IAsyncResult ar)
        {
            Func<int, int, int> funcDelegate = ar.AsyncState as Func<int, int, int>;

            int funcDelegateResult = funcDelegate.EndInvoke(ar);

            Console.WriteLine($"{ar.AsyncState} has executed in Thread#{Environment.CurrentManagedThreadId}, {nameof(funcDelegateResult)}:{funcDelegateResult}");
        }
    }
}
