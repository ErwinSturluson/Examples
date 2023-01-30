using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeCancellation._05_CancellationToken.Register
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts = new(500);

            CancellationToken ct = cts.Token;

            string callName = "AsyncTask";

            Task task = Task.Run(() => PrintIterations(callName, ct), ct);

            ct.Register(() => CancellationContinuation(task, callName));

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

        private static void CancellationContinuation(Task cancelledTask, string taskName)
        {
            Console.WriteLine($"Operation [{taskName}] was canceled. Task [{cancelledTask.Id}]. Method [{nameof(CancellationContinuation)}].");
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
