using System;
using System.Threading;

namespace ThreadsSynchronisation._11_ManualResetEventSlim
{
    internal class Program
    {
        private static ManualResetEventSlim _manualResetEventSlim = new(false);

        private static void Main(string[] args)
        {
            Thread thread1 = new(PrintIterations);
            Thread thread2 = new(PrintIterations);

            thread1.Start("Secondary 1");
            thread2.Start("Secondary 2");
            Thread.Sleep(100);

            Console.Write("Press any key to set the event for all threads: ");
            Console.ReadKey();
            Console.WriteLine();
            _manualResetEventSlim.Set();
            Thread.Sleep(1000);

            Console.Write("Press any key to reset the event: ");
            Console.ReadKey();
            Console.WriteLine();
            _manualResetEventSlim.Reset();

            Thread thread3 = new(PrintIterations);
            Thread thread4 = new(PrintIterations);

            thread3.Start("Secondary 3");
            thread4.Start("Secondary 4");
            Thread.Sleep(100);

            Console.Write("Press any key to set the event for all remained threads: ");
            Console.ReadKey();
            Console.WriteLine();
            _manualResetEventSlim.Set();
            Thread.Sleep(1000);
        }

        private static void PrintIterations(object arg)
        {
            _manualResetEventSlim.Wait();

            int iterationNumber = 0;

            while (iterationNumber < 3)
            {
                iterationNumber++;
                Console.WriteLine($"{arg} - {iterationNumber}");
                Thread.Sleep(200);
            }

            Console.WriteLine();
        }
    }
}
