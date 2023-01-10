using System;
using System.Threading;

namespace Threads._03_ThreadStaticAttribute
{
    internal class Program
    {
        [ThreadStatic]
        private static int _iterationNumber = 0;

        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("\tSecondary");

            Thread.Sleep(100);

            thread.Join();

            PrintIterations("Primary");

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            for (int i = 0; i < 10; i++)
            {
                _iterationNumber++;

                Console.WriteLine($"{arg} - {_iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}
