using System;
using System.Threading;

namespace Threads._05_Priority
{
    internal class Program
    {
        private static bool _secondaryThreadsEnable = false;

        private static void Main(string[] args)
        {
            int processorNumber = Environment.ProcessorCount;

            if (processorNumber > 1)
            {
                Thread[] threads = new Thread[processorNumber - 1];

                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i] = new Thread(CalculateIterations);
                    threads[i].Priority = ThreadPriority.Highest;
                }

                threads[0].Priority = ThreadPriority.Lowest;

                _secondaryThreadsEnable = true;

                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i].Start($"Secondary - {threads[i].Priority}");
                }

                Thread.Sleep(3000);

                Console.WriteLine("Thread \"Primary\" has stopped \"Secondary\" threads.");

                _secondaryThreadsEnable = false;
            }
            else
            {
                throw new NotSupportedException($"{nameof(processorNumber)}:{processorNumber}");
            }

            Console.ReadKey();
        }

        private static void CalculateIterations(object arg)
        {
            Console.WriteLine($"Thread {arg} has started.");

            long iterationNumber = 1;

            while (_secondaryThreadsEnable)
            {
                iterationNumber++;
            }

            decimal result = Convert.ToDecimal(iterationNumber) / 1_000_000;

            Console.WriteLine($"Thread \"{arg}\" has stopped with result of \"{decimal.Round(result).ToString("#,#")}\".");
        }
    }
}
