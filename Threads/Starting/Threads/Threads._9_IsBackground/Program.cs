using System;
using System.Threading;

namespace Threads._9_IsBackground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.IsBackground = true;

            thread.Start("Secondary");
        }

        private static void PrintIterations(object arg)
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{arg}|{nameof(iterationNumber)}:{iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}