using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._01_Task
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(Main)} method has started in thread#{Environment.CurrentManagedThreadId}");

            Action printIterationsAction = new(PrintIterations);

            Task printIterationsTask1 = new(printIterationsAction);

            printIterationsTask1.Start();

            Task printIterationsTask2 = new(printIterationsAction);

            printIterationsTask2.RunSynchronously();

            PrintIterations();

            Console.WriteLine($"{nameof(PrintIterations)} method has finished in thread#{Environment.CurrentManagedThreadId}");
        }

        private static void PrintIterations()
        {
            Console.WriteLine($"{nameof(PrintIterations)} method has started in thread#{Environment.CurrentManagedThreadId}");

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(100);
            }

            Console.WriteLine($"{nameof(PrintIterations)} method has finished in thread#{Environment.CurrentManagedThreadId}");
        }
    }
}
