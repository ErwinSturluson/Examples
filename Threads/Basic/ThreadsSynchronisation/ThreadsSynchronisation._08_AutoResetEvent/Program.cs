using System;
using System.Threading;

namespace ThreadsSynchronisation._08_AutoResetEvent
{
    internal class Program
    {
        private static AutoResetEvent _autoResetEvent = new(false);

        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("Secondary");

            Thread.Sleep(100);

            for (int i = 0; i < 3; i++)
            {
                Console.Write("Press any key to set the event: ");
                Console.ReadKey();
                _autoResetEvent.Set();
                Thread.Sleep(100);
            }
        }

        private static void PrintIterations(object arg)
        {
            int iterationNumber = 0;

            while (iterationNumber < 3)
            {
                _autoResetEvent.WaitOne();
                iterationNumber++;
                Console.WriteLine($"{Environment.NewLine}{arg} - {iterationNumber}");
            }

            Console.WriteLine();
        }
    }
}
