using System;
using System.Threading;

namespace Threads.CriticalSection._4_UnmanagedSharedResource
{
    internal class Program
    {
        private static object _lock = new();

        private static void Main(string[] args)
        {
            Thread thread = new(SafePrintRedText);

            thread.Start("\tSecondary");

            SafePrintGreenText("Primary");

            Console.ReadKey();
        }

        private static void SafePrintRedText(object arg)
        {
            for (int i = 0; i < 10; i++)
            {
                lock (_lock)
                {
                    PrintColoredText(arg.ToString(), ConsoleColor.Red);
                }
                Thread.Sleep(100);
            }
        }

        private static void SafePrintGreenText(object arg)
        {
            for (int i = 0; i < 10; i++)
            {
                lock (_lock)
                {
                    PrintColoredText(arg.ToString(), ConsoleColor.Green);
                }
                Thread.Sleep(100);
            }
        }

        private static void PrintColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}