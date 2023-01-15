using System;
using System.Threading;

namespace AsyncProgramming._01_Thread
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("\tSecondary");

            Thread.Sleep(100);

            PrintIterations("Primary");

            thread.Join();

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{arg} thread - {iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}
