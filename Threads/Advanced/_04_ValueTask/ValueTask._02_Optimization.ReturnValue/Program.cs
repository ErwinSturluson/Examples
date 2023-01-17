using System;
using System.Threading;
using System.Threading.Tasks;

namespace ValueTask._02_Optimization.ReturnValue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Incorrect value.
            ValueTask<int> valueTask1 = PrintIterationsAsync(0);

            Console.WriteLine($"The \"{nameof(valueTask1)}\" with input of \"{0}\" has finished with the Result of \"{valueTask1.Result}\".{Environment.NewLine}");

            // Correct value.
            ValueTask<int> valueTask2 = PrintIterationsAsync(10);

            Console.WriteLine($"The \"{nameof(valueTask2)}\" with input of \"{10}\" has finished with the Result of \"{valueTask2.Result}\".");
        }

        private static ValueTask<int> PrintIterationsAsync(int iterationsNumber)
        {
            if (iterationsNumber < 1)
            {
                Console.WriteLine($"Invalid value {nameof(iterationsNumber)}: [{iterationsNumber}] is less than \"1\".");
                return new ValueTask<int>(0);
            }
            else
            {
                Console.WriteLine($"Value {nameof(iterationsNumber)}: [{iterationsNumber}] is valid. A Task instance will be created.");
                return new ValueTask<int>(Task.Factory.StartNew(PrintIterations, iterationsNumber));
            }
        }

        private static int PrintIterations(object state)
        {
            int iterationNumber = (int)state;

            int iterationIndex = 0;

            while (iterationIndex < iterationNumber)
            {
                iterationIndex++;

                Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationNumber * 1000;

            return calculationResult;
        }
    }
}
