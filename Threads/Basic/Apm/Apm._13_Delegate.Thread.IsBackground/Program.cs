using System;

namespace Apm._13_Delegate.Thread.IsBackground
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
            System.Threading.Thread.CurrentThread.IsBackground = false;

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
