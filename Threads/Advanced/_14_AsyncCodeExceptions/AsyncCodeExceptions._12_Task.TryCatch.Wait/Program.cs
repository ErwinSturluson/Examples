using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeExceptions._12_Task.TryCatch.Wait
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task task = Task.Run(() => PrintIterations("AsyncTask"));

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
                    throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
                }
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }

        private static void ReportExceptionDetails(Exception ex)
        {
            Console.WriteLine(
                $"Exception Type: {ex.GetType().Name}" +
                $"{Environment.NewLine}" +
                $"Exception Message: {ex.Message}" +
                $"{Environment.NewLine}" +
                $"{new string('-', 40)}");
        }
    }
}
