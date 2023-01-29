using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeExceptions._14_Task.Await
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                await PrintIterationsAsync("AsyncTask");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message{ex.Message}");
            }

            Console.ReadKey();
        }

        private static async Task PrintIterationsAsync(string callName)
        {
            Task iterationsTask = new(PrintIterations, callName);

            iterationsTask.Start();

            await iterationsTask;
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

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
    }
}
