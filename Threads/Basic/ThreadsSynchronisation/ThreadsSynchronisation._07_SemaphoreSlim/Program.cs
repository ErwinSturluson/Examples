using System;
using System.Threading;

namespace ThreadsSynchronisation._07_SemaphoreSlim
{
    internal class Program
    {
        private static SemaphoreSlim _semaphoreSlim = new(2, 4, );

        private static void Main(string[] args)
        {
            MakeThreads(4);
            Console.WriteLine();

            _semaphoreSlim.Release(2);

            MakeThreads(4);

            Console.WriteLine();
        }

        private static void Calculate(object arg)
        {
            _semaphoreSlim.Wait();

            Thread.Sleep(100);

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} has entered the protected section.");

            Thread.Sleep(1000);

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} has left the protected section.");

            Thread.Sleep(100);

            _semaphoreSlim.Release();
        }

        private static void MakeThreads(int threadsNumber)
        {
            Thread[] threads = new Thread[threadsNumber];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(Calculate);
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start($"Secondary#{i + 1}");
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }
    }
}
