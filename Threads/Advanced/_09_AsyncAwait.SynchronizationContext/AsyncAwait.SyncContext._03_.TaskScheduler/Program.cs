using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.SyncContext._03_.TaskScheduler
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Task<Task> task = new(PrintIterationsAsync, "AsyncTask");

            PrintCurrentTaskSchedulerName(nameof(Main));
            task.Start(new IndependentThreadTaskScheduler());

            await await task;

            PrintCurrentTaskSchedulerName(nameof(Main));
        }

        private static async Task PrintIterationsAsync(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

            Task printIterationsTask = new(PrintIterations, taskName);

            PrintCurrentTaskSchedulerName(nameof(PrintIterationsAsync));

            printIterationsTask.Start();

            await printIterationsTask;

            PrintCurrentTaskSchedulerName(nameof(PrintIterationsAsync));

            Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            PrintCurrentTaskSchedulerName(nameof(PrintIterations));

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }

        private static void PrintCurrentTaskSchedulerName(string methodName) =>
            Console.WriteLine($">>>Method [{methodName}] was executed in [{System.Threading.Tasks.TaskScheduler.Current.GetType().Name}].");
    }
}
