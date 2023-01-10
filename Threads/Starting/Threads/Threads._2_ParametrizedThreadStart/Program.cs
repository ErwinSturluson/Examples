using System;
using System.Threading;

namespace Threads._2_ParametrizedThreadStart
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ParameterizedThreadStart threadStartParametrized = new(PrintSecondaryParametrized);

            Thread threadParametrized = new(threadStartParametrized);

            threadParametrized.Start("Test Argument");

            while (true)
            {
                Console.WriteLine("Primary");
                Thread.Sleep(100);
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