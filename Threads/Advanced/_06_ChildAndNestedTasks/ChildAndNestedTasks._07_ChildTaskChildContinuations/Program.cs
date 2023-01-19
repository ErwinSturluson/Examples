using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChildAndNestedTasks._07_ChildTaskChildContinuations
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task parentTask = new(PrintAllIterations);
            parentTask.Start();

            Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
            parentTask.Wait();
            Console.WriteLine($"- Parent Task and Child Tasks have finished.");

            Console.ReadKey();
        }

        private static void PrintAllIterations()
        {
            Console.WriteLine($"+ ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            Task<int> childTask1 = Task.Factory.StartNew(PrintIterations, "ChildTask1", TaskCreationOptions.AttachedToParent);
            Task<int> childTask2 = Task.Factory.StartNew(PrintIterations, "ChildTask2", TaskCreationOptions.AttachedToParent);

            childTask1.ContinueWith(PrintIterationsContinuation, TaskContinuationOptions.AttachedToParent);
            childTask2.ContinueWith(PrintIterationsContinuation, TaskContinuationOptions.AttachedToParent);

            Thread.Sleep(100);

            Console.WriteLine($"> ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
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

        private static int PrintIterationsContinuation(Task<int> task)
        {
            Console.WriteLine($"++ Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");

            Thread.Sleep(200);

            Console.WriteLine($">> The Result of task with Id#{task.Id} is [{task.Result}].");

            Console.WriteLine($"-- Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");

            int newResult = task.Result * 10;

            return newResult;
        }
    }
}
