using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._02_Task.ReturnValue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<int> coldAsyncTask = new(PrintIterations, "ColdAsyncTask");
            Task<int> coldSyncTask = new(PrintIterations, "ColdSyncTask");

            coldAsyncTask.Start();

            Console.WriteLine($"ColdAsyncTask completed with result of {coldAsyncTask.Result}");

            coldSyncTask.RunSynchronously();

            Console.WriteLine($"ColdAsyncTask completed with result of {coldSyncTask.Result}");

            Task<int> hotFactoryAsyncTask = Task.Factory.StartNew(PrintIterations, "HotFactoryAsyncTask");
            Task<int> hotRunMethodAsyncTask = Task.Run(() => PrintIterations("HotRunMethodAsyncTask"));

            Console.WriteLine($"HotFactoryAsyncTask completed with result of {hotFactoryAsyncTask.Result}");
            Console.WriteLine($"HotRunMethodAsyncTask completed with result of {hotRunMethodAsyncTask.Result}");

            Console.ReadKey();
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationNumber * 1000;

            return calculationResult;
        }
    }
}
