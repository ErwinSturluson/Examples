using System;
using System.Threading;

namespace AsyncProgramming._03_ThreadPoolWrapper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ThreadPoolWrapper<int, int> task1 = new(PrintIterations);
            task1.Start(10);

            int syncTask1Resut = PrintIterations(5);
            Console.WriteLine($"The result of sync task:{syncTask1Resut}.");

            int asyncTaskResut = task1.Result;
            Console.WriteLine($"The result of async task 1:{asyncTaskResut}.");

            try
            {
                ThreadPoolWrapper<int, int> task = new(PrintIterations);
                task.Start(-1);

                int asyncTask2Resut = task.Result;
                Console.WriteLine($"The result of async task 2:{asyncTaskResut}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception was occured in task 2: {ex.GetType()}{Environment.NewLine}{ex.Message}");
            }
        }

        private static int PrintIterations(int iterationsNumber)
        {
            if (iterationsNumber < 0) throw new ArgumentOutOfRangeException(nameof(iterationsNumber), $"Incorrect value: {iterationsNumber}.");

            int iterationIndex = 0;

            while (iterationIndex < iterationsNumber)
            {
                iterationIndex++;

                Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int calculatedResult = iterationIndex * 100;

            return calculatedResult;
        }
    }
}
