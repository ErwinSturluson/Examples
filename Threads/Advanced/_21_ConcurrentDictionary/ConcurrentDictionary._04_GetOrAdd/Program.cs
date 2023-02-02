using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentDictionary._04_GetOrAdd
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            ConcurrentDictionary<int, string> dictionary = new();

            Task fillingTask1 = FillConcurrentDictionaryAsync(dictionary, 0, 5);
            Task fillingTask2 = FillConcurrentDictionaryAsync(dictionary, 5, 5);

            dictionary[10] = $"Text value of key <{5}>.";

            dictionary[0] = $"Text value of key <{0}> (CHANGED).";
            dictionary[5] = $"Text value of key <{5}> (CHANGED).";

            await Task.WhenAll(fillingTask1, fillingTask2);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Filling Tasks were completed successfully.{Environment.NewLine}");

            Task removingTask = Task.Run(() =>
            {
                for (int i = 0; i < dictionary.Count; i++)
                {
                    RemoveConcurrentDictionaryElement(dictionary, i);
                }
            });

            await Task.Delay(300);

            Task readingTask1 = ReadEnumeratorConcurrentDictionaryAsync(dictionary);
            Task readingTask2 = Task.Run(() =>
            {
                int dictionaryLength = dictionary.Count;

                for (int i = 0; i < dictionaryLength; i++)
                {
                    string gottenOrAddedValue = dictionary.GetOrAdd(i, (_) => $"Text value of key <{i}>. (ADDED).");

                    if (gottenOrAddedValue.EndsWith(" (ADDED)."))
                    {
                        Console.WriteLine($"++Element [{i}][{gottenOrAddedValue}] was added (recovered) in [{nameof(ConcurrentDictionary<int, string>)}].");
                    }
                    else
                    {
                        Console.WriteLine($">>Element [{i}][{gottenOrAddedValue}] was gotten in [{nameof(ConcurrentDictionary<int, string>)}].");
                    }

                    if (dictionary.TryUpdate(i, $"{gottenOrAddedValue} (UPDATED).", gottenOrAddedValue))
                    {
                        Console.WriteLine($"^Element [{i}][{gottenOrAddedValue}] was updated in [{nameof(ConcurrentDictionary<int, string>)}].");
                    }
                    else
                    {
                        Console.WriteLine($"!Element with Key [{i}] cannot be updated in [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
                    }

                    if (i > 3)
                    {
                        Thread.Sleep(100);
                    }
                }
            });

            await Task.WhenAll(removingTask, readingTask1, readingTask2);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Reading and Removing Tasks were completed successfully.");

            dictionary.Clear();
            await Console.Out.WriteLineAsync($"{Environment.NewLine}Cleanup Operation was completed successfully. Current Dictionary size is [{dictionary.Count}].");
        }

        private static Task FillConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
        {
            return Task.Run(() => FillConcurrentDictionary(dictionary, startIndex, elementsNumber));
        }

        private static void FillConcurrentDictionary(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                int key = i;
                string value = $"Text value of key <{i}>.";

                if (dictionary.TryAdd(key, value))
                {
                    Console.WriteLine($"+Element [{key}][{value}] was added to [{nameof(ConcurrentDictionary<int, string>)}].");
                }
                else
                {
                    Console.WriteLine($"!Element with Key [{key}] cannot be added to [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
                }

                Thread.Sleep(100);
            }
        }

        private static Task ReadEnumeratorConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary)
        {
            return Task.Run(() =>
            {
                foreach (var item in dictionary)
                {
                    Console.WriteLine($"+Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
                    Thread.Sleep(200);
                }
            });
        }

        private static void ReadEnumeratorConcurrentDictionary(ConcurrentDictionary<int, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($">Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
                Thread.Sleep(200);
            }
        }

        private static void RemoveConcurrentDictionaryElement(ConcurrentDictionary<int, string> dictionary, int key)
        {
            if (dictionary.TryRemove(key, out string value))
            {
                Console.WriteLine($"-Element [{key}][{value}] was removed from [{nameof(ConcurrentDictionary<int, string>)}].");
            }
            else
            {
                Console.WriteLine($"!Element with Key [{key}] cannot be removed from [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
            }

            Thread.Sleep(100);
        }
    }
}
