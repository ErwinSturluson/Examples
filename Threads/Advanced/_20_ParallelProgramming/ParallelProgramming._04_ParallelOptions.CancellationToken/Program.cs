using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._04_ParallelOptions.CancellationToken
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts = new();

            ParallelOptions options = new()
            {
                CancellationToken = cts.Token
            };

            cts.CancelAfter(50);

            try
            {
                Parallel.For(1, 20, options, PrintIteration);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"{Environment.NewLine}[Parallel.For() method was canceled]");
            }
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(200);
        }
    }
}
