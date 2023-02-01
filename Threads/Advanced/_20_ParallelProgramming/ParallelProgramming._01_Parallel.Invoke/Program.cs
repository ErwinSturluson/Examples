using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._01_Parallel.Invoke
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopwatch = new();
            Console.WriteLine($"Operations have started sequentially...");
            stopwatch.Start();

            PrintOperation();
            PrintOperation();
            PrintOperation();

            Console.WriteLine($"Operations have finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");
            stopwatch.Restart();
            Console.WriteLine($"Operations have started parallel...");

            Parallel.Invoke(PrintOperation, PrintOperation, PrintOperation);

            Console.WriteLine($"Iterations have finished parallel after {stopwatch.ElapsedMilliseconds} milliseconds.");
            stopwatch.Stop();
        }

        private static void PrintOperation()
        {
            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(200);
            }
        }
    }
}
