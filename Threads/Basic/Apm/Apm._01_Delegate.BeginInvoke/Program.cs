using System;
using System.Threading;

namespace Apm._01_Delegate.BeginInvoke
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Action actionDelegate = new(PrintIterations);

            actionDelegate.BeginInvoke(null, null);

            PrintIterations();

            Console.ReadKey();
        }

        private static void PrintIterations()
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}
