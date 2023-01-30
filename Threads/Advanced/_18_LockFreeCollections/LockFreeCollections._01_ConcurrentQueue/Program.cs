using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LockFreeCollections._01_ConcurrentQueue
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            ConcurrentQueue<int> queue = new();

            Task fillingTask1 = Task.Run(() => FillQueue(queue, 0, 5));
            Task fillingTask2 = Task.Run(() => FillQueue(queue, 5, 5));

            await Task.WhenAll(fillingTask1, fillingTask2);
            await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

            Task cleanupTask = Task.Run(() => CleanupQueue(queue));
            await Task.Delay(500);
            Task readingTask = Task.Run(() => ReadQueue(queue));

            await Task.WhenAll(cleanupTask, readingTask);
            await Console.Out.WriteLineAsync("All the Reading and Cleanup Tasks were completed successfully.");
        }

        private static void FillQueue(ConcurrentQueue<int> queue, int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                queue.Enqueue(i);
                Console.WriteLine($"+Element [{i}] was added to [{nameof(ConcurrentQueue<int>)}].");
                Thread.Sleep(100);
            }
        }

        private static void ReadQueue(ConcurrentQueue<int> queue)
        {
            foreach (var item in queue)
            {
                Console.WriteLine($"+Element [{item}] was read from [{nameof(ConcurrentQueue<int>)}].");
                Thread.Sleep(200);
            }
        }

        private static void CleanupQueue(ConcurrentQueue<int> queue)
        {
            while (queue.TryDequeue(out int result))
            {
                Console.WriteLine($"+Element [{result}] was removed from [{nameof(ConcurrentQueue<int>)}].");
                Thread.Sleep(100);
            }
        }
    }
}
