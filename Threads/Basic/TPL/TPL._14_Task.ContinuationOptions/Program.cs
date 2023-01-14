using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._14_Task.ContinuationOptions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintIterationsArgs task2Args = new()
            {
                TaskName = "AsyncTask2",
                CancellationToken = null
            };

            Task<int> task1 = new(CalculateIterations, null);
            Task<int> task2 = new(CalculateIterations, task2Args);

            task1.ContinueWith(OnFaultedContinuation, TaskContinuationOptions.OnlyOnFaulted);
            task2.ContinueWith(OnRanToCompletionContinuation, TaskContinuationOptions.OnlyOnRanToCompletion);

            task1.Start();
            task2.Start();

            Console.ReadKey();
        }

        private static int CalculateIterations(object state)
        {
            if (state is null) throw new ArgumentException("null", nameof(state));

            PrintIterationsArgs args = (PrintIterationsArgs)state;

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                args.CancellationToken?.ThrowIfCancellationRequested();

                iterationIndex++;

                Console.WriteLine($"{args.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int result = iterationIndex * 100;

            return result;
        }

        private static void OnRanToCompletionContinuation(Task task)
        {
            Task<int> castedTask = task as Task<int>;

            int taskResult = castedTask.Result;

            string reportMessage =
                Environment.NewLine +
                new string('=', 43) +
                Environment.NewLine +
                $"Task result: {taskResult}" +
                Environment.NewLine +
                new string('=', 43);

            Console.WriteLine(reportMessage);
        }

        private static void OnFaultedContinuation(Task task)
        {
            Exception taskException = task.Exception.InnerException;

            string reportMessage =
                Environment.NewLine +
                new string('=', 43) +
                Environment.NewLine +
                $"Exception Type: {taskException.GetType().Name}" +
                Environment.NewLine +
                $"Exception Message: {taskException.Message}" +
                Environment.NewLine +
                new string('=', 43);

            Console.WriteLine(reportMessage);
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public CancellationToken? CancellationToken { get; set; }
    }
}
