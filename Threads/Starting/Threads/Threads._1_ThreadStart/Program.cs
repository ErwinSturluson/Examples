using System;
using System.Threading;

namespace Threads._1_ThreadStart
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ThreadStart threadStart = new(PrintSecondary);

            Thread thread = new(threadStart);

            thread.Start();

            while (true)
            {
                Console.WriteLine("Primary");
                Thread.Sleep(100);
            }
        }

        private static void PrintSecondary()
        {
            while (true)
            {
                Console.WriteLine("\tSecondary");
                Thread.Sleep(100);
            }
        }
    }
}
