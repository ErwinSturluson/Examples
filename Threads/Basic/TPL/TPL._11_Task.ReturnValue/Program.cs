using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._11_Task.ReturnValue
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

            Task<string> task1 = new Task<string>(PrintIterations, task1Args);
            Task<string> task2 = Task<string>.Factory.StartNew(PrintIterations, task2Args);

            task1.Start();

            string task1Result = task1.Result;
            string task2Result = task2.Result;

            Console.WriteLine($"Task1 Result: {task1Result}");
            Console.WriteLine($"Task2 Result: {task2Result}");
        }

        private static string PrintIterations(object state)
        {
            PrintIterationsArgs taskArgs = state as PrintIterationsArgs;

            int iterationIndex = 0;

            while (iterationIndex < taskArgs.IterationsNumber)
            {
                iterationIndex++;

                Console.WriteLine($"{taskArgs.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(taskArgs.IterationsDelay);
            }

            string resultMessage = $"{taskArgs.TaskName} completed sucsessfully after {iterationIndex} iterations.";

            return resultMessage;
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public int IterationsNumber { get; set; }

        public int IterationsDelay { get; set; }
    }
}
