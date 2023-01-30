using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LockFreeCollections._02_ConcurrentStack
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            ConcurrentStack<int> stack = new();

            Task fillingTask1 = Task.Run(() => FillStack(stack, 0, 5));
            Task fillingTask2 = Task.Run(() => FillStack(stack, 5, 5));

            await Task.WhenAll(fillingTask1, fillingTask2);
            await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

            Task cleanupTask = Task.Run(() => CleanupStack(stack));
            await Task.Delay(500);
            Task readingTask = Task.Run(() => ReadStack(stack));

            await Task.WhenAll(cleanupTask, readingTask);
            await Console.Out.WriteLineAsync("All the Reading and Cleanup Tasks were completed successfully.");
        }

        private static void FillStack(ConcurrentStack<int> stack, int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                stack.Push(i);
                Console.WriteLine($"+Element [{i}] was added to [{nameof(ConcurrentStack<int>)}].");
                Thread.Sleep(100);
            }
        }

        private static void ReadStack(ConcurrentStack<int> stack)
        {
            foreach (var item in stack)
            {
                Console.WriteLine($"+Element [{item}] was read from [{nameof(ConcurrentStack<int>)}].");
                Thread.Sleep(200);
            }
        }

        private static void CleanupStack(ConcurrentStack<int> stack)
        {
            while (stack.TryPop(out int result))
            {
                Console.WriteLine($"+Element [{result}] was removed from [{nameof(ConcurrentStack<int>)}].");
                Thread.Sleep(100);
            }
        }
    }
}
