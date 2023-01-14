using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._03_Task.Status
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task printIterationsTask1 = new(PrintIterations);

            Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");

            printIterationsTask1.Start();

            Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");

            Thread.Sleep(500);

            Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");

            Thread.Sleep(1000);

            Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");
        }

        private static void PrintIterations()
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"Task#{Task.CurrentId.ToString() ?? "null"} - {iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}
