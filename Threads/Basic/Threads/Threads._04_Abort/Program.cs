using System;
using System.Threading;

namespace Threads._04_Abort
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("\tSecondary");

            Thread.Sleep(500);

            thread.Abort();

            PrintIterations("Primary");

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            try
            {
                int iterationNumber = 0;

                while (iterationNumber < 10)
                {
                    iterationNumber++;

                    Console.WriteLine($"{arg} - {iterationNumber}");
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine($"Thread with Id#{Environment.CurrentManagedThreadId} was aborted.");
            }
        }
    }
}
