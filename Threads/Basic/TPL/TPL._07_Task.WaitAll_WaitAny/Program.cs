using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._07_Task.WaitAll_WaitAny
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int parallelCalculationsPerTask = 3;

            Task[] parallelCalculationTasks = new Task[3];

            for (int i = 1; i < parallelCalculationTasks.Length; i++)
            {
                parallelCalculationTasks[i] = new Task(PrintIterations, parallelCalculationsPerTask);
            }

            parallelCalculationTasks[0] = new Task(PrintIterations, 1);

            for (int i = 0; i < parallelCalculationTasks.Length; i++)
            {
                parallelCalculationTasks[i].Start();
            }

            Task.WaitAll(parallelCalculationTasks);

            Console.WriteLine();

            Task[] concurrentTasks = new Task[3];

            for (int i = 0; i < concurrentTasks.Length; i++)
            {
                int concurrentCalculationsPerTask = i + 1;

                concurrentTasks[i] = new Task(PrintIterations, concurrentCalculationsPerTask);
            }

            for (int i = 0; i < concurrentTasks.Length; i++)
            {
                concurrentTasks[i].Start();
            }

            Task.WaitAny(concurrentTasks);
        }

        private static void PrintIterations(object state)
        {
            int calculationsNumber = (int)state;

            int calculationIndex = 0;

            while (calculationIndex < calculationsNumber)
            {
                calculationIndex++;

                Console.WriteLine($"Task in Thread#{Environment.CurrentManagedThreadId} - [{calculationIndex}]");
                Thread.Sleep(100);
            }
        }
    }
}
