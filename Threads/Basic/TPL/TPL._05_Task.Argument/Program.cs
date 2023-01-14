using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._05_Task.Argument
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task printIterationsTask1 = new(PrintIterations, "AsyncTask");
            Task printIterationsTask2 = new(PrintIterations, "SyncTask");

            printIterationsTask1.Start();
            printIterationsTask2.RunSynchronously();

            Console.ReadKey();
        }

        private static void PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }
        }
    }
}
