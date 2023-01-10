using System;
using System.Collections.Generic;
using System.Threading;

namespace Threads._02_MultipleJoin
{
    internal class Program
    {
        private static List<string> _list = new();

        private static void Main(string[] args)
        {
            Thread thread = new(WriteIterations);

            thread.Start("\tSecondary");

            Thread.Sleep(100);

            thread.Join();

            WriteIterations("Primary");

            PrintIterations();

            Console.ReadKey();
        }

        private static void WriteIterations(object arg)
        {
            Thread thread = new(Write);
            thread.Start($"\t{arg} - Secondary");
            thread.Join();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                _list.Add($"{arg} - {iterationNumber}");
            }
        }

        private static void Write(object arg)
        {
            _list.Add($"{arg}");
        }

        private static void PrintIterations()
        {
            foreach (string item in _list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
