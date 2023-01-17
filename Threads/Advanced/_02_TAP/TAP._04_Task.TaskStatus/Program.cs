using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._04_Task.TaskStatus
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<int> task = new(PrintIterations, "AsyncTask");

            PrintTaskStatus(task);

            task.Start();

            PrintTaskStatus(task);

            Thread.Sleep(100);

            PrintTaskStatus(task);

            task.Wait();

            PrintTaskStatus(task);

            Console.WriteLine($"AsyncTask Result: {task.Result}.");
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationNumber * 1000;

            return calculationResult;
        }

        private static void PrintTaskStatus(Task task) => Console.WriteLine($"[AsyncTask Status: {task.Status}]");
    }
}
