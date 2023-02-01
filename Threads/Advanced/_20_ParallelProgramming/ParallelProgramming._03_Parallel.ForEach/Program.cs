using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._03_Parallel.ForEach
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<int> collection = Enumerable.Range(1, 10);

            Stopwatch stopwatch = new();
            Console.WriteLine($"Iterations have started sequentially...");
            stopwatch.Start();

            foreach (var item in collection)
            {
                PrintIteration(item);
            }

            Console.WriteLine($"Iterations have finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");
            stopwatch.Restart();
            Console.WriteLine($"Iterations have started parallel...");

            Parallel.ForEach(collection, PrintIteration);

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
