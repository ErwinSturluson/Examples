using System;
using System.Threading;

namespace ThreadsSynchronisation._03_ThreadPool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Program has started.");
            Report();

            ThreadPool.QueueUserWorkItem(new WaitCallback(Task1));
            Report();

            ThreadPool.QueueUserWorkItem(Task2);
            Report();

            Thread.Sleep(3000);
            Console.WriteLine("Program has finished.");
            Report();
        }

        private static void Task1(object state)
        {
            Thread.CurrentThread.Name = "1";
            Console.WriteLine("Thread {0} has started\n", Thread.CurrentThread.Name);
            Thread.Sleep(2000);
            Console.WriteLine("Thread {0} has finished\n", Thread.CurrentThread.Name);
        }

        private static void Task2(object state)
        {
            Thread.CurrentThread.Name = "2";
            Console.WriteLine("Thread {0} has started\n", Thread.CurrentThread.Name);
            Thread.Sleep(500);
            Console.WriteLine("Thread {0} has finished\n", Thread.CurrentThread.Name);
        }

        private static void Report()
        {
            Thread.Sleep(200);
            ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableIoThreads);
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxIoThreads);

            Console.WriteLine("Available worker threads in the ThreadPool: {0} of {1}", availableWorkerThreads, maxWorkerThreads);
            Console.WriteLine("Available IO threads in the ThreadPool:     {0} of {1}\n", availableIoThreads, maxIoThreads);
        }
    }
}
