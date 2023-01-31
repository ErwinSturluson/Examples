using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _12_BlockingCollection.GetEnumerator
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

            await Task.Delay(200);

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            foreach (var item in dataStructure)
            {
                Console.WriteLine($"=Element [{item}] is read from [{nameof(BlockingCollection<int>)}].");

                await Task.Delay(100);
            }

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(BlockingCollection<int>)}'s elements are read.{Environment.NewLine}");

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(consumer1Task, consumer2Task, consumer3Task);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Consumer)}'s Tasks were completed successfully.{Environment.NewLine}");
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
            while (_dataStructure.TryTake(out int result))
            {
                Console.WriteLine($"-Element [{result}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(100);
            }
        }
    }
}
