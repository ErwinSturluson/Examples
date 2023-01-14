using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._10_Task.ComplexArgument
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintIterationsArgs task1Args = new()
            {
                TaskName = "AsyncTask1",
                IterationsNumber = 5,
                IterationsDelay = 200
            };

            PrintIterationsArgs task2Args = new()
            {
                TaskName = "AsyncTask2",
                IterationsNumber = 10,
                IterationsDelay = 100
            };

            Task task1 = Task.Factory.StartNew(PrintIterations, task1Args);
            Task task2 = Task.Factory.StartNew(PrintIterations, task2Args);

            Task.WaitAll(task1, task2);
        }

        private static void PrintIterations(object state)
        {
            PrintIterationsArgs taskArgs = state as PrintIterationsArgs;

            int iterationIndex = 0;

            while (iterationIndex < taskArgs.IterationsNumber)
            {
                iterationIndex++;

                Console.WriteLine($"TaskName:{taskArgs.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(taskArgs.IterationsDelay);
            }
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public int IterationsNumber { get; set; }

        public int IterationsDelay { get; set; }
    }
}
