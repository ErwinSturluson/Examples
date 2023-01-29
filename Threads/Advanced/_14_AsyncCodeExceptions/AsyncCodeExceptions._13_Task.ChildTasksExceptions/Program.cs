using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeExceptions._13_Task.ChildTasksExceptions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task task = new(PrintIterationsCollective, "AsyncTask");

            task.Start();

            PrintIterations("SyncCall");

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                ReportExceptionDetails(ex);
            }

            Console.ReadKey();
        }

        private static void PrintIterationsCollective(object state)
        {
            string callName = state.ToString();

            Task task1 = new(() => PrintIterations($"Child{callName}"), TaskCreationOptions.AttachedToParent);
            Task task2 = new(() => PrintIterations($"Child{callName}"), TaskCreationOptions.AttachedToParent);

            task1.Start();
            task2.Start();

            throw new Exception($"[!!!EXCEPTION!!! Parent{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1}]");
        }

        private static void PrintIterations(string callName)
        {
            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);

                if (Task.CurrentId != null && iterationIndex > 5)
                {
                    throw new Exception($"[!!!EXCEPTION!!! {callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1}]");
                }
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }

        private static void ReportExceptionDetails(AggregateException parentException, string indent = null)
        {
            Console.WriteLine(
                $"Exception Type: {parentException.GetType().Name}" +
                $"{Environment.NewLine}" +
                $"Exception Message: {parentException.Message}" +
                $"{Environment.NewLine}" +
                $"{new string('-', 40)}");

            foreach (var innerException in parentException.InnerExceptions)
            {
                if (innerException is AggregateException aggregateException)
                {
                    ReportExceptionDetails(aggregateException, indent + "  ");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
