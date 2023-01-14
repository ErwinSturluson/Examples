using System;
using System.Threading.Tasks;

namespace TPL._04_Task.Thread.IsBackground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has started in thread#{Environment.CurrentManagedThreadId}");

            Task printIterationsTask1 = new(PrintIterations);

            printIterationsTask1.Start();

            System.Threading.Thread.Sleep(100);

            Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has finished in thread#{Environment.CurrentManagedThreadId}");
        }

        private static void PrintIterations()
        {
            System.Threading.Thread.CurrentThread.IsBackground = false;

            Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has started in thread#{Environment.CurrentManagedThreadId}");

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId} [{iterationNumber}]");
                System.Threading.Thread.Sleep(100);
            }

            Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has finished in thread#{Environment.CurrentManagedThreadId}");
        }
    }
}
