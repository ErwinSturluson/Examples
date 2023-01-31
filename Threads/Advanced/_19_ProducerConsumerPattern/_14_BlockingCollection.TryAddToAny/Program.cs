using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _14_BlockingCollection.TryAddToAny
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure1 = new(new ConcurrentQueue<int>(), 5);
            using BlockingCollection<int> dataStructure2 = new(new ConcurrentQueue<int>(), 5);
            using BlockingCollection<int> dataStructure3 = new(new ConcurrentQueue<int>(), 5);

            BlockingCollection<int>[] collections = new[] { dataStructure1, dataStructure2, dataStructure3 };

            Producer producer1 = new(collections);
            Producer producer2 = new(collections);

            CancellationTokenSource cts = new();
            Task producer1Task = producer1.AddDataAsync(0, 10);
            Task producer2Task = producer2.AddDataAsync(10, 5);

            await Task.WhenAll(producer1Task, producer2Task);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            Task enumerateTask1 = EnumerateCollection(dataStructure1, nameof(dataStructure1), cts.Token);
            Task enumerateTask2 = EnumerateCollection(dataStructure2, nameof(dataStructure2), cts.Token);
            Task enumerateTask3 = EnumerateCollection(dataStructure3, nameof(dataStructure3), cts.Token);

            cts.CancelAfter(1000);

            await Task.Delay(1000);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}Consumer Enumerable Task was canceled.");
        }

        private static async Task EnumerateCollection(BlockingCollection<int> dataStructure, string dataStructureName, CancellationToken ct)
        {
            IEnumerable<int> consumingEnumerable = dataStructure.GetConsumingEnumerable(ct);

            await Task.Run(async () =>
            {
                foreach (int item in consumingEnumerable)
                {
                    Console.WriteLine($"=Element [{item}] is read from [{consumingEnumerable.GetType().Name}] with a name [{dataStructureName}].");
                    await Task.Delay(100);
                }
            });
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int>[] _collections;

        public Producer(params BlockingCollection<int>[] collections)
        {
            _collections = collections;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                int collectionIndex = BlockingCollection<int>.TryAddToAny(_collections, i);

                if (collectionIndex != -1)
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] with Index [{collectionIndex}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }
}
