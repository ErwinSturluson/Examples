using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace _15_BlockingCollection.TryTakeFromAny
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

            Consumer consumer1 = new(collections);
            Consumer consumer2 = new(collections);

            Task producer1Task = producer1.AddDataAsync(0, 10);
            Task producer2Task = producer2.AddDataAsync(10, 10);

            await Task.Delay(500);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            dataStructure1.CompleteAdding();
            dataStructure2.CompleteAdding();
            dataStructure3.CompleteAdding();

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(BlockingCollection<int>)}'s elements are read.{Environment.NewLine}");

            await Task.WhenAll(consumer1Task, consumer2Task, consumer3Task);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Consumer)}'s Tasks were completed successfully.{Environment.NewLine}");
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

    internal class Consumer
    {
        private readonly BlockingCollection<int>[] _collections;

        public Consumer(params BlockingCollection<int>[] collections)
        {
            _collections = collections;
        }

        public async Task TakeDataAsync()
        {
            while (true)
            {
                int collectionIndex = BlockingCollection<int>.TryTakeFromAny(_collections, out int item);

                if (collectionIndex != -1)
                {
                    Console.WriteLine($"-Element [{item}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                }
                else if (collectionIndex == -1 && _collections.All(x => x.IsAddingCompleted))
                {
                    string report = Environment.NewLine;

                    for (int i = 0; i < _collections.Length; i++)
                    {
                        report += $"{Environment.NewLine}>Any element can not be removed anymore from [{nameof(BlockingCollection<int>)}] with Index [{i}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.IsAddingCompleted)}] property is [{_collections[i].IsAddingCompleted}] " +
                        $"and [{nameof(BlockingCollection<int>.Count)}] property is [{_collections[i].Count}].";
                    }

                    await Console.Out.WriteLineAsync(report);

                    break;
                }
                else
                {
                    string report = Environment.NewLine;

                    for (int i = 0; i < _collections.Length; i++)
                    {
                        report += $"{Environment.NewLine}>Any element was not removed from [{nameof(BlockingCollection<int>)}] with Index [{i}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.Count)}] property is [{_collections[i].Count}].";
                    }

                    await Console.Out.WriteLineAsync(report);
                }

                await Task.Delay(200);
            }
        }
    }
}
