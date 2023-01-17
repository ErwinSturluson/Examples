using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._10_Task.TaskContinuationOptions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task task = new(PrintIterations, -1);

            Task continuationTasks = task
                .ContinueWith(HandleExceptionContinuation, System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted);

            task.Start();

            continuationTasks.Wait();
        }

        private static void PrintIterations(object state)
        {
            Console.WriteLine($"{nameof(PrintIterations)} - Thread#{Environment.CurrentManagedThreadId} - Task has started.");

            int iterationsNumber = (int)state;

            if (iterationsNumber < 0) throw new ArgumentOutOfRangeException(nameof(iterationsNumber), $"Incorrect Value: {iterationsNumber}.");

            int iterationIndex = 0;

            while (iterationIndex < iterationsNumber)
            {
                iterationIndex++;

                Console.WriteLine($"{nameof(PrintIterations)} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }
        }

        private static void HandleExceptionContinuation(Task previousTask)
        {
            Console.WriteLine($"{nameof(HandleExceptionContinuation)} - Thread#{Environment.CurrentManagedThreadId} - PreviousTask has finished with an exception.");

            Exception taskInnerException = previousTask.Exception.InnerException;

            Console.WriteLine($"Exception Type: {taskInnerException.GetType()}{Environment.NewLine}Exception Message: {taskInnerException.Message}");
        }
    }
}
