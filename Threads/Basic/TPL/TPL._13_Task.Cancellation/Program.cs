using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._13_Task.Cancellation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts = new();

            CancellationToken ct = cts.Token;

            PrintIterationsArgs taskArgs = new()
            {
                TaskName = "AsyncTask",
                CancellationToken = ct
            };

            Task task = new(PrintIterations, taskArgs, ct);

            task.Start();

            Thread.Sleep(500);

            cts.Cancel();

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                PrintExceptionReport(ex);
            }
        }

        private static void PrintIterations(object state)
        {
            if (state is null) throw new ArgumentException("null", nameof(state));

            PrintIterationsArgs args = (PrintIterationsArgs)state;

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                args.CancellationToken.ThrowIfCancellationRequested();

                iterationIndex++;

                Console.WriteLine($"{args.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }
        }

        private static void PrintExceptionReport(AggregateException ex)
        {
            Exception taskException = ex.InnerException;

            string reportMessage =
                Environment.NewLine +
                new string('=', 39) +
                Environment.NewLine +
                $"Exception Type: {taskException.GetType().Name}" +
                Environment.NewLine +
                $"Exception Message: {taskException.Message}" +
                Environment.NewLine +
                new string('=', 39);

            Console.WriteLine(reportMessage);
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
}
