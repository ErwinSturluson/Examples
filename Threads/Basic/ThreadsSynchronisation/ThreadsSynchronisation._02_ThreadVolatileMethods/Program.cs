using System;
using System.Threading;

namespace ThreadsSynchronisation._02_ThreadVolatileMethods
{
    internal class Program
    {
        private static int _secondaryThreadsEnable = 1;

        private static void Main(string[] args)
        {
            Thread thread = new(CalculateIterations);

            thread.Start("\tSecondary");

            Thread.Sleep(1000);

            Thread.VolatileWrite(ref _secondaryThreadsEnable, 0);

            thread.Join();
        }

        private static void CalculateIterations(object arg)
        {
            Console.WriteLine($"Thread {arg} has started.");

            long iterationNumber = 1;

            while (Thread.VolatileRead(ref _secondaryThreadsEnable) != 0)
            {
                iterationNumber++;

                Console.WriteLine($"Thread \"{arg}\" is executing iteration #{iterationNumber}.");
                Thread.Sleep(100);
            }

            Console.WriteLine($"Thread \"{arg}\" has stopped after {iterationNumber} iterations.");
        }
    }
}
