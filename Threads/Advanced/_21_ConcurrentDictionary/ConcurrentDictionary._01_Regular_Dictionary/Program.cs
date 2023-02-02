using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrentDictionary._01_Regular_Dictionary
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Dictionary<int, string> dictionary = new()
            {
                { 0, $"Text value of key <{0}>." },
                { 1, $"Text value of key <{1}>." }
            };

            FillDictionary(dictionary, 2, 5);

            dictionary[5] = $"Text value of key <{5}>.";

            dictionary[0] = $"Text value of key <{0}> (CHANGED).";
            dictionary[3] = $"Text value of key <{3}> (CHANGED).";

            Console.WriteLine($"Filling operation was completed successfully.{Environment.NewLine}");

            for (int i = 0; i < dictionary.Count; i++)
            {
                Console.WriteLine($"+Element [{i}][{dictionary[i]}] was read from [{nameof(Dictionary<int, string>)}.[int]] indexer.");
                Thread.Sleep(100);
            }

            Console.WriteLine($"Reading operation by [{nameof(Dictionary<int, string>)}.[int]] indexer was completed successfully.{Environment.NewLine}");

            ReadDictionary(dictionary);

            Console.WriteLine($"Reading operation by {nameof(IEnumerator)} was completed successfully.{Environment.NewLine}");

            for (int i = 0; i < dictionary.Count; i++)
            {
                CleanupDictionaryElement(dictionary, i);
            }

            Console.WriteLine("Cleanup operation was completed successfully.");
        }

        private static void FillDictionary(Dictionary<int, string> dictionary, int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                int key = i;
                string value = $"Text value of key <{i}>.";

                if (dictionary.TryAdd(key, value))
                {
                    Console.WriteLine($"+Element [{key}][{value}] was added to [{nameof(Dictionary<int, string>)}].");
                }
                else if (dictionary.ContainsKey(key))
                {
                    Console.WriteLine($"!Element with Key [{key}] cannot be added to [{nameof(Dictionary<int, string>)}] due to some element with the same Key already exists.");
                }
                else
                {
                    Console.WriteLine($"!Element with Key [{key}] cannot be added to [{nameof(Dictionary<int, string>)}] for a some reason.");
                }

                Thread.Sleep(100);
            }
        }

        private static void ReadDictionary(Dictionary<int, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($">Element [{item.Key}][{item.Value}] was read from [{nameof(Dictionary<int, string>)}].");
                Thread.Sleep(100);
            }
        }

        private static void CleanupDictionaryElement(Dictionary<int, string> dictionary, int key)
        {
            if (dictionary.TryGetValue(key, out string value))
            {
                dictionary.Remove(key);
                Console.WriteLine($"-Element [{key}][{value}] was removed from [{nameof(Dictionary<int, string>)}].");
            }
            else if (!dictionary.ContainsKey(key))
            {
                Console.WriteLine($"!Element with Key [{key}] cannot be removed from [{nameof(Dictionary<int, string>)}] due to element with this Key does not exist.");
            }
            else
            {
                Console.WriteLine($"!Element with Key [{key}] cannot be removed from [{nameof(Dictionary<int, string>)}] for a some reason.");
            }
            Thread.Sleep(100);
        }
    }
}
