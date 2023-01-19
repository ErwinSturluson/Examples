using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChildAndNestedTasks._04_AwaitedNestedTasks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task mainTask = new(PrintAllIterations);
            mainTask.Start();

            Console.WriteLine($"Main Thread is waiting for MainTask finishes.");
            mainTask.Wait();
            Console.WriteLine($"- MainTask Task has finished.");

            Console.ReadKey();
        }

        private static void PrintAllIterations()
        {
            Console.WriteLine($"+ MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            Task nestedTask1 = Task.Factory.StartNew(PrintIterations, "NestedTask1");
            Task nestedTask2 = Task.Factory.StartNew(PrintIterations, "NestedTask2");

            Console.WriteLine($"> MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} is waiting for nested tasks completion in Thread#{Environment.CurrentManagedThreadId}.");

            Task.WaitAll(nestedTask1, nestedTask2);

            Console.WriteLine($"> MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
                Thread.Sleep(100);
            }

            int calculationResult = iterationIndex * 1000;

            Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

            return calculationResult;
        }
    }
}
