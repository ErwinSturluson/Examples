using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._02_Parallel.For
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopwatch = new();
            Console.WriteLine($"Iterations have started sequentially...");
            stopwatch.Start();

            for (int i = 0; i < 10; i++)
            {
                PrintIteration(i);
            }

            Console.WriteLine($"Iterations have finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");
            stopwatch.Restart();
            Console.WriteLine($"Iterations have started parallel...");

            Parallel.For(1, 10, PrintIteration);

            Console.WriteLine($"Iterations have finished parallel after {stopwatch.ElapsedMilliseconds} milliseconds.");
            stopwatch.Stop();
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(100);
        }
    }
}
