using System;
using System.Threading;

namespace Threads._01_Join
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("\tSecondary");

            Thread.Sleep(100);

            thread.Join();

            PrintIterations("Primary");

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{arg} - {iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}
