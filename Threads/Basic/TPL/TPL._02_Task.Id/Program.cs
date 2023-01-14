using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._02_Task.Id
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has started in thread#{Environment.CurrentManagedThreadId}");

            Task printIterationsTask1 = new(PrintIterations);

            printIterationsTask1.Start();

            Task printIterationsTask2 = new(PrintIterations);

            printIterationsTask2.RunSynchronously();

            Task printIterationsTask3 = new(PrintIterations);

            printIterationsTask3.Start();

            Task printIterationsTask4 = new(PrintIterations);

            printIterationsTask4.RunSynchronously();

            PrintIterations();

            Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has finished in thread#{Environment.CurrentManagedThreadId}");
        }

        private static void PrintIterations()
        {
            Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has started in thread#{Environment.CurrentManagedThreadId}");

            int iterationNumber = 0;

            while (iterationNumber < 3)
            {
                iterationNumber++;

                Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId} [{iterationNumber}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has finished in thread#{Environment.CurrentManagedThreadId}");
        }
    }
}
