using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._01_Task
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task printIterationsTask1 = new(PrintIterations, "ColdAsyncTask");
            Task printIterationsTask2 = new(PrintIterations, "ColdSyncTask");

            printIterationsTask1.Start();
            printIterationsTask2.RunSynchronously();

            Task.Factory.StartNew(PrintIterations, "HotFactoryAsyncTask");
            Task.Run(() => PrintIterations("HotRunMethodAsyncTask"));

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
