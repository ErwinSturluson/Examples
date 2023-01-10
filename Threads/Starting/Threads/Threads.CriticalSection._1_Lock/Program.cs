using System;
using System.Threading;

namespace Threads.CriticalSection._1_Lock
{
    internal class Program
    {
        private static object _lock = new();

        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("\tSecondary");

            PrintIterations("Primary");

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            lock (_lock)
            {
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine($"{arg} - {i}");
                    Thread.Sleep(100);
                }
            }
        }
    }
}
