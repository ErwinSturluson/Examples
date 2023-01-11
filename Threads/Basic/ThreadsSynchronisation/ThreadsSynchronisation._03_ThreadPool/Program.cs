using System;
using System.Threading;

namespace ThreadsSynchronisation._03_ThreadPool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Program start.");
            Report();

            ThreadPool.QueueUserWorkItem(new WaitCallback(Task1));
            Report();

            ThreadPool.QueueUserWorkItem(Task2);
            Report();

            Thread.Sleep(3000);
            Console.WriteLine("Program end.");
            Report();
        }

        private static void Task1(object state)
        {
            Thread.CurrentThread.Name = "1";
            Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
            Thread.Sleep(2000);
            Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
        }

        private static void Task2(object state)
        {
            Thread.CurrentThread.Name = "2";
            Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
            Thread.Sleep(500);
            Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
        }

        private static void Report()
        {
            Thread.Sleep(200);
            int availableWorkerThreads, availableIoThreads, maxWorkerThreads, maxIoThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableIoThreads);
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIoThreads);

            Console.WriteLine("Available worker threads in the ThreadPool: {0} of {1}", availableWorkerThreads, maxWorkerThreads);
            Console.WriteLine("Available IO threads in the ThreadPool:     {0} of {1}\n", availableIoThreads, maxIoThreads);
        }
    }
}
