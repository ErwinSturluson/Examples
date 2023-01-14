using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._06_Task.Wait
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task printIterationsTask1 = new(PrintIterations, "AsyncTask");
            Task printIterationsTask2 = new(PrintIterations, "SyncTask1");

            printIterationsTask1.Start();

            printIterationsTask2.RunSynchronously();

            printIterationsTask1.Wait();

            Task printIterationsTask3 = new(PrintIterations, "SyncTask2");

            Console.WriteLine();

            printIterationsTask3.RunSynchronously();
        }

        private static void PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }
        }
    }
}
