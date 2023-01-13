using System;
using System.Threading;

namespace Apm._02_Delegate.EndInvoke
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Action actionDelegate = new(PrintIterations);

            IAsyncResult actionResult = actionDelegate.BeginInvoke(null, null);

            actionDelegate.EndInvoke(actionResult);

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
