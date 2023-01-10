using System;
using System.Threading;

namespace Threads._7_Properties
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("Secondary");

            PrintIterations("Primary");

            Console.ReadKey();
        }

        private static void PrintIterations(object arg)
        {
            Thread currentThread = Thread.CurrentThread;

            int id = currentThread.ManagedThreadId;

            currentThread.Name = arg.ToString();

            string name = currentThread.Name;

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"{nameof(id)}:{id}|{nameof(name)}:{name}|{nameof(iterationNumber)}:{iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}