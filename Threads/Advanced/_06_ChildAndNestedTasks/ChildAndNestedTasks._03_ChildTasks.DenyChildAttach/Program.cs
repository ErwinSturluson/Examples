using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChildAndNestedTasks._03_ChildTasks.DenyChildAttach
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task nonParentTask = new(PrintAllIterations, TaskCreationOptions.DenyChildAttach);
            nonParentTask.Start();

            Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
            nonParentTask.Wait();
            Console.WriteLine($"- Parent Task has finished.");

            Console.ReadKey();
        }

        private static void PrintAllIterations()
        {
            Console.WriteLine($"+ NonParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            Task.Factory.StartNew(PrintIterations, "NonChildTask1", TaskCreationOptions.AttachedToParent);
            Task.Factory.StartNew(PrintIterations, "NonChildTask2", TaskCreationOptions.AttachedToParent);

            Thread.Sleep(100);

            Console.WriteLine($"> NonParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
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
