using System;
using System.Threading;

namespace Threads._08_SharedResourceWithInterlocked
{
    internal class Program
    {
        private static object _sharedFieldLock = new();
        private static int _sharedField = 0;

        private static void Main(string[] args)
        {
            Thread[] threads = new Thread[10];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(CalculateIterations);
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start($"Secondary#{i}");
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine($"Expected result of Shared Field: 10,000,000, Actual result: {_sharedField.ToString("#,#")}");

            Console.ReadKey();
        }

        private static void CalculateIterations(object arg)
        {
            Console.WriteLine($"Thread {arg} has started.");

            for (int i = 0; i < 1_000_000; i++)
            {
                Interlocked.Increment(ref _sharedField);
            }

            Console.WriteLine($"Thread \"{arg}\" has finished with result of \"1,000,000\" iterations.");
        }
    }
}
