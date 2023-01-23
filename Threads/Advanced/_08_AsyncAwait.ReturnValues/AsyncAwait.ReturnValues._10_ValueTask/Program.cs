using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.ReturnValues._10_ValueTask
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

            ValueTask asyncTask = PrintIterationsAsync("  AsyncTask");

            PrintIterations("   SyncCall");

            asyncTask.GetAwaiter().GetResult();

            Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

            Console.ReadKey();
        }

        private static async ValueTask PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"++ {taskName ?? "null",-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

            Task printIterationsTask = new(PrintIterations, taskName);

            ValueTask printIterationsValueTask = new(printIterationsTask);

            printIterationsTask.Start();

            await printIterationsValueTask;

            Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
}
