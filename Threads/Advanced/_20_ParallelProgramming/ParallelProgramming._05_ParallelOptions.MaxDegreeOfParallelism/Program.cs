using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._05_ParallelOptions.MaxDegreeOfParallelism
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = 4
            };

            Parallel.For(1, 10, options, PrintIteration);
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(500);
        }
    }
}
