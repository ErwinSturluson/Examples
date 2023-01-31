using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _02_IProducerConsumerCollection.ConcurrentStack
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IProducerConsumerCollection<int> dataStructure = new ConcurrentStack<int>();

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(100);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(producer1Task, producer2Task, consumer1Task, consumer2Task, consumer3Task);

            await Console.Out.WriteLineAsync($"All the Tasks were completed successfully.{Environment.NewLine}");
        }
    }

    internal class Producer
    {
        private readonly IProducerConsumerCollection<int> _dataStructure;

        public Producer(IProducerConsumerCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(IProducerConsumerCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(IProducerConsumerCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly IProducerConsumerCollection<int> _dataStructure;

        public Consumer(IProducerConsumerCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (_dataStructure.TryTake(out int result))
            {
                Console.WriteLine($"-Element [{result}] was removed from [{nameof(IProducerConsumerCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(200);
            }
        }
    }
}
