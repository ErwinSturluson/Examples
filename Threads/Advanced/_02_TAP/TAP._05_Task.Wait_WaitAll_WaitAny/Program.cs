using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._02_Task.Wait_WaitAll_WaitAny
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // singleTask.Wait();
            Console.WriteLine("Creating 1 Single Task.");

            Task<int> singleTask = new(PrintIterations, "SingleAsyncTask");
            singleTask.Start();

            Console.WriteLine("Waiting for 1 Single Task.");

            singleTask.Wait();

            Console.WriteLine($"SingleAsyncTask has finished. Task's Result: {singleTask.Result}.{Environment.NewLine}");

            // Task.WaitAll();
            Console.WriteLine("Creating 2 Important Tasks.");

            Task<int>[] allTheImportantTasks = Enumerable.Range(1, 2)
                .Select(i => Task.Factory.StartNew(PrintIterations, $"ImportantAsyncTask{i}"))
                .ToArray();

            Console.WriteLine("Waiting for All 2 The Important Tasks.");

            Task.WaitAll(allTheImportantTasks);

            Console.WriteLine($"All 2 The Important Tasks have finished.");

            Array.ForEach(allTheImportantTasks, importantTask => Console.WriteLine($"Task's Result: {importantTask.Result}"));
            Console.WriteLine();

            // Task.WaitAny();
            Console.WriteLine("Creating Some 2 Tasks.");

            Task<int>[] someTasks = Enumerable.Range(1, 2)
               .Select(i => Task.Factory.StartNew(PrintIterations, $"SomeAsyncTask{i}"))
               .ToArray();

            Console.WriteLine("Waiting for Any 1 of 2 Some Tasks.");

            int taskIndex = Task.WaitAny(someTasks);

            Console.WriteLine($"AsyncTask with index [{taskIndex}] has finished first. Task's Result: {someTasks[taskIndex].Result}.");
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationNumber * 1000;

            return calculationResult;
        }
    }
}
