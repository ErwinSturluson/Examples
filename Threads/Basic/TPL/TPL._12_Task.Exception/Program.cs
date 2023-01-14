using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._12_Task.Exception
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<string> task1 = new(PrintIterations, null);
            Task<string> task2 = new(PrintIterations, null);

            task1.Start();
            task2.Start();

            try
            {
                task1.Wait();

                string taskResult = task1.Result;

                Console.WriteLine($"Task1 Result: {taskResult}");
            }
            catch (AggregateException ex)
            {
                PrintExceptionReport(ex);
            }

            try
            {
                string taskResult = task2.Result;

                Console.WriteLine($"Task2 Result: {taskResult}");
            }
            catch (AggregateException ex)
            {
                PrintExceptionReport(ex);
            }
        }

        private static string PrintIterations(object state)
        {
            if (state is null) throw new ArgumentException("null", nameof(state));

            string taskName = state.ToString();

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($"{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            string resultMessage = $"{taskName} completed sucsessfully after {iterationIndex} iterations.";

            return resultMessage;
        }

        private static void PrintExceptionReport(AggregateException ex)
        {
            System.Exception taskException = ex.InnerException;

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
}
