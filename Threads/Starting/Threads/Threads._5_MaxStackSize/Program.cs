using System;
using System.Threading;

namespace Threads._5_MaxStackSize
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread = new(PrintSecondary, 10_000_000);

            Thread threadParametrized = new(PrintSecondaryParametrized, 10_000_000);
        }

        private static void PrintSecondary()
        {
            while (true)
            {
                Console.WriteLine("\tSecondary");
            }
        }

        private static void PrintSecondaryParametrized(object arg)
        {
            while (true)
            {
                Console.WriteLine($"\tSecondary, arg = \"{arg}\"");
                Thread.Sleep(100);
            }
        }
    }
}
