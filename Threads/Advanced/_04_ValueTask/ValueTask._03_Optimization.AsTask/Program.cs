using System;
using System.Threading;
using System.Threading.Tasks;

namespace ValueTask._03_Optimization.AsTask
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Incorrect value.
            ValueTask<int> valueTask1 = PrintIterationsAsync(0);

            Console.WriteLine($"The \"{nameof(valueTask1)}\" with input of \"{0}\" has finished with the Result of \"{valueTask1.Result}\".{Environment.NewLine}");

            if (!valueTask1.IsCompleted)
            {
                Task continuationTask = valueTask1.AsTask()
                    .ContinueWith((task, state) => Console.WriteLine($"The Task {state} with Id#{task.Id} took too long."), nameof(valueTask1));
                continuationTask.Wait();
            }

            // Correct value.
            ValueTask<int> valueTask2 = PrintIterationsAsync(10);

            if (!valueTask2.IsCompleted)
            {
                Task continuationTask = valueTask2.AsTask()
                    .ContinueWith((task, state) => Console.WriteLine($"The Task {state} with Id#{task.Id} took too long."), nameof(valueTask2));
                continuationTask.Wait();
            }

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
