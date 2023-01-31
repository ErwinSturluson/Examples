﻿using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _08_BlockingCollection.CompleteAdding
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

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            dataStructure.CompleteAdding();

            try
            {
                await Task.WhenAll(producer1Task, producer2Task, consumer1Task, consumer2Task, consumer3Task);
                await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Tasks were completed successfully.{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(
                    $"{Environment.NewLine}An error was occurred while the Tasks is processing." +
                    $"{Environment.NewLine}Exception Type: {ex.GetType().Name}" +
                    $"{Environment.NewLine}Exception Message: {ex.Message}");
            }
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
                await Task.Delay(200);
            }
        }
    }
}
