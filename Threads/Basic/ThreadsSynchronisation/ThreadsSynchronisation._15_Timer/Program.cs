using System;
using System.Threading;

namespace ThreadsSynchronisation._15_Timer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TimerCallback callback = new(PrintIterations);

            Timer timer = new(callback, "Secondary", 1000, 500);

            Console.WriteLine("Timer has started." + Environment.NewLine);

            Thread.Sleep(3000);

            timer.Change(500, 1000);

            Thread.Sleep(3000);

            timer.Dispose();

            Console.WriteLine("Timer has finished.");

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            int iterationNumber = 0;

            while (iterationNumber < 3)
            {
                iterationNumber++;
                Console.WriteLine($"{arg} - {iterationNumber}");
                Thread.Sleep(100);
            }

            Console.WriteLine();
        }
    }
}
