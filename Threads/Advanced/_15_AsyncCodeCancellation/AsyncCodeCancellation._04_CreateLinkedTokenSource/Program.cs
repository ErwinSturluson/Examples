using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeCancellation._04_CreateLinkedTokenSource
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts1 = new();
            CancellationTokenSource cts2 = new();

            CancellationToken ct1 = cts1.Token;
            CancellationToken ct2 = cts2.Token;

            CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct1, ct2);

            CancellationToken linkedToken = linkedTokenSource.Token;

            Task task1 = Task.Run(() => PrintIterations("AsyncTask1", linkedToken), linkedToken);
            Task task2 = Task.Run(() => PrintIterations("AsyncTask2", linkedToken), linkedToken);

            cts1.CancelAfter(500);

            try
            {
                task1.Wait();
            }
            catch (AggregateException ex)
            {
                ReportTaskExceptionDetails(ex, task1);
            }

            try
            {
                task2.Wait();
            }
            catch (AggregateException ex)
            {
                ReportTaskExceptionDetails(ex, task2);
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
