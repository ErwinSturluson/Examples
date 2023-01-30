using System.Collections.Concurrent;

namespace LockFreeCollections._03_ConcurrentBag
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            ConcurrentBag<int> bag = new();

            Task fillingTask1 = Task.Run(() => FillBag(bag, 0, 5));
            Task fillingTask2 = Task.Run(() => FillBag(bag, 5, 5));

            await Task.WhenAll(fillingTask1, fillingTask2);
            await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

            Task cleanupTask = Task.Run(() => CleanupBag(bag));
            await Task.Delay(500);
            Task readingTask = Task.Run(() => ReadBag(bag));

            await Task.WhenAll(cleanupTask, readingTask);
            await Console.Out.WriteLineAsync("All the Reading and Cleanup Tasks were completed successfully.");
        }

        private static void FillBag(ConcurrentBag<int> bag, int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                bag.Add(i);
                Console.WriteLine($"+Element [{i}] was added to [{nameof(ConcurrentBag<int>)}].");
                Thread.Sleep(100);
            }
        }

        private static void ReadBag(ConcurrentBag<int> bag)
        {
            foreach (var item in bag)
            {
                Console.WriteLine($"+Element [{item}] was read from [{nameof(ConcurrentBag<int>)}].");
                Thread.Sleep(200);
            }
        }

        private static void CleanupBag(ConcurrentBag<int> bag)
        {
            while (bag.TryTake(out int result))
            {
                Console.WriteLine($"+Element [{result}] was removed from [{nameof(ConcurrentBag<int>)}].");
                Thread.Sleep(100);
            }
        }
    }
}
