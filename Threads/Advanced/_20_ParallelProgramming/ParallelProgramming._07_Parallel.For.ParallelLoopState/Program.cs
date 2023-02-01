using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming._07_Parallel.For.ParallelLoopState
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Action<int, System.Threading.Tasks.ParallelLoopState> loopAction = (i, loopState) =>
            {
                if (i > 10)
                {
                    loopState.Break();
                    Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{i}] - [BROKEN]");
                    return;
                }

                PrintIteration(i);
            };

            Parallel.For(0, 15, loopAction);
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(500);
        }
    }
}
