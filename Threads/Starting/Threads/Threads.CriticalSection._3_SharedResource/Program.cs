using System;
using System.Collections.Generic;
using System.Threading;

namespace Threads.CriticalSection._3_SharedResource
{
    internal class Program
    {
        private static List<string> _list = new();

        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("\tSecondary");

            PrintIterations("Primary");

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            lock (_list)
            {
                for (int i = 1; i <= 10; i++)
                {
                    _list.Add($"{arg} - {i}");
                }

                foreach (string value in _list)
                {
                    Console.WriteLine(value);
                    Thread.Sleep(100);
                }

                Console.WriteLine($"End of {arg}");
                Console.WriteLine(new string('=', 20));
            }
        }
    }
}