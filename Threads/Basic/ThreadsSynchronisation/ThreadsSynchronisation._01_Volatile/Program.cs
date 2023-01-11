using System;
using System.Threading;

namespace ThreadsSynchronisation._01_Volatile
{
    internal class Program
    {
        private static volatile bool _secondaryThreadsEnable = true; // Without JIT optimization. Works only in Release mode.
        //private static bool _secondaryThreadsEnable = false; // With JIT optimization.

        private static void Main(string[] args)
        {
            Thread thread = new(CalculateIterations);

            thread.Start("\tSecondary");

            Thread.Sleep(1000);

            _secondaryThreadsEnable = false;

            thread.Join();
        }

        private static void CalculateIterations(object arg)
        {
            Console.WriteLine($"Thread {arg} has started.");

            long iterationNumber = 1;

            while (_secondaryThreadsEnable)
            {
                iterationNumber++;

                Console.WriteLine($"Thread \"{arg}\" is executing iteration #{iterationNumber}.");
                Thread.Sleep(100);
            }

            Console.WriteLine($"Thread \"{arg}\" has stopped after {iterationNumber} iterations.");
        }
    }
}
