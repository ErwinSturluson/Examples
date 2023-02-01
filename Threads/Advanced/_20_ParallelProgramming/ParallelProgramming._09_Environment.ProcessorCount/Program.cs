using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._09_Environment.ProcessorCount
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Processor Count of the Current Machine is [{Environment.ProcessorCount}].");

            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount - 1
            };

            Parallel.For(1, 50, options, PrintIteration);
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(500);
        }
    }
}
