using System;
using System.Threading;

namespace ThreadsSynchronisation._04_Mutex
{
    internal class Program
    {
        private static Mutex _mutex = new(false, typeof(Program).FullName);

        private static void Main(string[] args)
        {
            Thread[] threads = new Thread[3];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(PrintIterations);
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

        private static void PrintIterations(object arg)
        {
            _mutex.WaitOne();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{arg} - {iterationNumber}");
                Thread.Sleep(100);
            }

            Console.WriteLine();

            _mutex.ReleaseMutex();
        }
    }
}
