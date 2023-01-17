using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._08_Task.ContinuationAction
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<int> task = new(PrintIterations, "AsyncTask");
            Task continuationTask = task.ContinueWith(PrintIterationsContinuation);

            task.Start();

            continuationTask.Wait();
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationNumber * 1000;

            return calculationResult;
        }

        private static void PrintIterationsContinuation(Task previousTask)
        {
            Task<int> castedPreviousTask = (Task<int>)previousTask;

            Console.WriteLine($"ContinuationAction - Thread#{Environment.CurrentManagedThreadId} - PreviousTask has Result of {castedPreviousTask.Result}.");
        }
    }
}
