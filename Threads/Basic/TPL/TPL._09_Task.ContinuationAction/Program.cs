using System;
using System.Threading.Tasks;

namespace TPL._09_Task.ContinuationAction
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task task1 = new(PrintIterations, "AsyncTask1");
            Task task2 = new(PrintIterations, "AsyncTask2", TaskCreationOptions.RunContinuationsAsynchronously);

            task1.ContinueWith(PrintIterationsContinuationAction);
            task2.ContinueWith(PrintIterationsContinuationAction);

            task1.Start();
            task2.Start();

            Task.WaitAll(task1, task2);
        }

        private static void PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"{taskName} - TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void PrintIterationsContinuationAction(Task task)
        {
            string taskName = task.AsyncState.ToString();

            Console.WriteLine($"{taskName} has finished in Thread#{Environment.CurrentManagedThreadId}");
        }
    }
}
