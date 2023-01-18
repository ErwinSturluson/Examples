using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._07_Default_ThreadPoolTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TaskScheduler defaultTaskScheduler = TaskScheduler.Default;
            Console.WriteLine($"Default TaskScheduler is {defaultTaskScheduler.GetType()}.");

            TaskScheduler currentTaskScheduler = TaskScheduler.Default;
            Console.WriteLine($"Current TaskScheduler is {currentTaskScheduler.GetType()}.{Environment.NewLine}");

            ReportThreadPoolState();

            Task<int> task1 = Task.Factory.StartNew(PrintIterations, "AsyncTask1");

            Thread.Sleep(100);
            ReportThreadPoolState();

            Task<int> task2 = new(PrintIterations, "AsyncTask2");
            task2.Start();

            Thread.Sleep(100);
            ReportThreadPoolState();

            Task<int> task3 = Task.Factory.StartNew(PrintIterations, "AsyncTask3");

            Thread.Sleep(100);
            ReportThreadPoolState();

            task1.Wait();
            ReportThreadPoolState();

            task2.Wait();
            ReportThreadPoolState();

            task3.Wait();
            ReportThreadPoolState();
        }

        private static void ReportThreadPoolState()
        {
            ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableIoThreads);
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxIoThreads);

            Console.WriteLine("Available worker threads in the ThreadPool: {0} of {1}", availableWorkerThreads, maxWorkerThreads);
            Console.WriteLine("Available IO threads in the ThreadPool:     {0} of {1}\n", availableIoThreads, maxIoThreads);
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine(
                $"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} " +
                $"has started in Thread#{Environment.CurrentManagedThreadId}. " +
                $"Is this Thread from the ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}.");

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Thread.Sleep(100);
            }

            int calculationResult = iterationIndex * 1000;

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

            return calculationResult;
        }
    }
}
