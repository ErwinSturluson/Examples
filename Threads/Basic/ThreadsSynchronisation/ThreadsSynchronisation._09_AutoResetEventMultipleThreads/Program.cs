using System;
using System.Threading;

namespace ThreadsSynchronisation._09_AutoResetEventMultipleThreads
{
    internal class Program
    {
        private static AutoResetEvent _autoResetEvent = new(false);

        private static void Main(string[] args)
        {
            Thread thread1 = new(PrintIterations);
            Thread thread2 = new(PrintIterations);

            thread1.Start("Secondary");
            thread2.Start("Secondary");
            Thread.Sleep(100);

            Console.Write("Press any key to set the event for the first thread: ");
            Console.ReadKey();
            Console.WriteLine();
            _autoResetEvent.Set();
            Thread.Sleep(1000);

            Console.Write("Press any key to set the event for the second thread: ");
            Console.ReadKey();
            Console.WriteLine();
            _autoResetEvent.Set();
        }

        private static void PrintIterations(object arg)
        {
            _autoResetEvent.WaitOne();

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
