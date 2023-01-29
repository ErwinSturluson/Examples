using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeExceptions._11_Task.TryCatch.Result
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => PrintIterations("AsyncTask"));

            PrintIterations("SyncCall");

            try
            {
                int taskResult = task.Result;
            }
            catch (Exception ex)
            {
                ReportExceptionDetails(ex);
            }

            Console.ReadKey();
        }

        private static int PrintIterations(string callName)
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

            int result = iterationIndex * 1000;

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

            return result;
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
