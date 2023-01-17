using System;
using System.Threading;
using System.Threading.Tasks;

namespace ValueTask._01_Optimization.ResultAvailableSynchronously
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Incorrect value.
            PrintIterationsAsync(0).GetAwaiter().GetResult();

            // Correct value.
            PrintIterationsAsync(10).GetAwaiter().GetResult();
        }

        private static System.Threading.Tasks.ValueTask PrintIterationsAsync(int iterationsNumber)
        {
            if (iterationsNumber < 1)
            {
                Console.WriteLine($"Invalid value {nameof(iterationsNumber)}: [{iterationsNumber}] is less than \"1\".{Environment.NewLine}");
                return new System.Threading.Tasks.ValueTask();
            }
            else
            {
                Console.WriteLine($"Value {nameof(iterationsNumber)}: [{iterationsNumber}] is valid. A Task instance will be created.");
                return new System.Threading.Tasks.ValueTask(Task.Factory.StartNew(PrintIterations, iterationsNumber));
            }
        }

        private static void PrintIterations(object state)
        {
            int iterationNumber = (int)state;

            int iterationIndex = 0;

            while (iterationIndex < iterationNumber)
            {
                iterationIndex++;

                Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }
        }
    }
}
