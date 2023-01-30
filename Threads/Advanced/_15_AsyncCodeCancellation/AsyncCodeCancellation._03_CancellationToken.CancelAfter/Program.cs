using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeCancellation._03_CancellationToken.CancelAfter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts = new();

            CancellationToken ct = cts.Token;

            Task task = Task.Run(() => PrintIterations("AsyncTask", ct), ct);

            cts.CancelAfter(500);

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                ReportTaskExceptionDetails(ex, task);
            }

            Console.ReadKey();
        }

        private static void PrintIterations(string callName, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                ct.ThrowIfCancellationRequested();

                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }

        private static void ReportTaskExceptionDetails(AggregateException ex, Task task = null, string indent = null)
        {
            string details = string.Empty;

            if (task is not null)
            {
                if (task.IsCanceled)
                {
                    details += $"[Exception][A Task#{task.Id} was canceled]{Environment.NewLine}";
                }
                else if (task.IsFaulted)
                {
                    details += $"[Exception][A Task#{task.Id} was faulted]{Environment.NewLine}";
                }
            }

            details += $"Exception Type: [{ex.GetType().Name}]{Environment.NewLine}Exception Message: [{ex.Message}]{Environment.NewLine}{new string('-', 40)}";

            Console.WriteLine(details);

            foreach (var innerException in ex.InnerExceptions)
            {
                if (innerException is AggregateException aggregateException)
                {
                    ReportTaskExceptionDetails(aggregateException, null, indent + "  ");
                }
                else
                {
                    Console.WriteLine($"[InnerException] Exception Type: [{innerException.GetType().Name}]{Environment.NewLine}Exception Message: [{innerException.Message}]{Environment.NewLine}{new string('-', 40)}");
                }
            }
        }
    }
}
