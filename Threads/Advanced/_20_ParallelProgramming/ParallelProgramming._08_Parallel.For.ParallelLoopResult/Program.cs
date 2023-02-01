using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._08_Parallel.For.ParallelLoopResult
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Action<int, ParallelLoopState> loopAction = (i, loopState) =>
            {
                if (i > 10)
                {
                    loopState.Break();
                    Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{i}] - [BROKEN]");
                    return;
                }

                PrintIteration(i);
            };

            System.Threading.Tasks.ParallelLoopResult loopResult = Parallel.For(0, 15, loopAction);

            if (loopResult.IsCompleted)
            {
                Console.WriteLine($"{Environment.NewLine}All the Iterations have finished successfully.");
            }
            else
            {
                Console.WriteLine($"{Environment.NewLine}Iterations have failed on [{loopResult.LowestBreakIteration}] iteration.");
            }
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(500);
        }
    }
}
