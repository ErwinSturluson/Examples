using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _13_BlockingCollection.GetConsumingEnumerable
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentQueue<int>());

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            CancellationTokenSource cts = new();

            IEnumerable<int> consumingEnumerable = dataStructure.GetConsumingEnumerable(cts.Token);

            Task consumingEnumerableTask = Task.Run(async () =>
            {
                foreach (int item in consumingEnumerable)
                {
                    Console.WriteLine($"=Element [{item}] is read from [{consumingEnumerable.GetType().Name}].");
                    await Task.Delay(100);
                }
            });

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.");

            cts.Cancel();

            await Console.Out.WriteLineAsync($"{Environment.NewLine}Consumer Enumerable Task was canceled.");
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
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
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (true)
            {
                bool itemTaken = _dataStructure.TryTake(out int item);

                if (itemTaken)
                {
                    Console.WriteLine($"-Element [{item}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                }
                else if (!itemTaken && _dataStructure.IsAddingCompleted)
                {
                    Console.WriteLine(Environment.NewLine +
                        $">Any element can not be removed anymore from [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.IsAddingCompleted)}] property is [{_dataStructure.IsAddingCompleted}] " +
                        $"and [{nameof(BlockingCollection<int>.Count)}] property is [{_dataStructure.Count}].");

                    break;
                }
                else
                {
                    Console.WriteLine(
                        $">Any element was not removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.Count)}] property is [{_dataStructure.Count}].");
                }

                await Task.Delay(200);
            }
        }
    }
}
