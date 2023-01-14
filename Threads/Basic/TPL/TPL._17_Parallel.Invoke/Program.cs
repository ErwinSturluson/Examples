using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._17_Parallel.Invoke
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parallel.Invoke(PrintIterations, PrintIterations, PrintIterations);
        }

        private static void PrintIterations()
        {
            int iterationNumber = 0;

            while (iterationNumber < 3)
            {
                iterationNumber++;

                Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(200);
            }
        }
    }
}
