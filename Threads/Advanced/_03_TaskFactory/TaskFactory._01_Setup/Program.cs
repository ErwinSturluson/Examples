using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskFactory._01_Setup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Threading.Tasks.TaskFactory taskFactory = new();

            Task<int> task1 = taskFactory.StartNew(PrintIterations, "AsyncTask1");
            Task<int> task2 = taskFactory.StartNew(PrintIterations, "AsyncTask2");

            Task continuationTask = taskFactory.ContinueWhenAll(new Task<int>[] { task1, task2 }, completedTasks =>
            {
                int completedTasksResutSum = completedTasks.Select(t => t.Result).Sum();

                Console.WriteLine($"All the Tasks completed with sum of results of [{completedTasksResutSum}].");
            });

            continuationTask.Wait();
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationNumber * 1000;

            return calculationResult;
        }
    }
}
