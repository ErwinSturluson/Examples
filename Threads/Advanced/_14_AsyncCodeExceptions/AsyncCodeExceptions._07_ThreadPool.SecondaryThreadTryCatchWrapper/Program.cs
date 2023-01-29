using System;
using System.Threading;

namespace AsyncCodeExceptions._07_ThreadPool.SecondaryThreadTryCatchWrapper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(PrintIterationsWrapper, "ThreadPoolCall");
            }
            catch (Exception ex)
            {
                ReportExceptionDetails(ex);
            }

            PrintIterations("PrimaryThreadCall");

            Console.ReadKey();
        }

        private static void PrintIterationsWrapper(object state)
        {
            try
            {
                PrintIterations(state);
            }
            catch (Exception ex)
            {
                ReportExceptionDetails(ex);
            }
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);

                if (Thread.CurrentThread.IsThreadPoolThread && iterationIndex > 5)
                {
                    throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
                }
            }

            Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }

        private static void ReportExceptionDetails(Exception ex)
        {
            string exceptionDetails = $"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message: {ex.Message}";

            Console.WriteLine(exceptionDetails);
        }
    }
}
