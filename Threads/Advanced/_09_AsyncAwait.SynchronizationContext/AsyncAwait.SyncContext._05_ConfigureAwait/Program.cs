using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.SyncContext._05_ConfigureAwait
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new ConsoleSynchronizationContext());

            ConsoleMessage message1 = new(PrintIterationsWrapperAsync, "AsyncTask1");
            ConsoleMessage message2 = new(PrintIterationsWrapperAsync, "AsyncTask2");
            ConsoleMessageListenter.AddMessage(message1);
            ConsoleMessageListenter.AddMessage(message2);

            ConsoleMessageListenter messageListenter = new();

            messageListenter.Listen();
        }

        private static async void PrintIterationsWrapperAsync(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"+  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsWrapperAsync)}]");

            await PrintIterationsAsync(taskName).ConfigureAwait(false);

            Console.WriteLine($"-  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsWrapperAsync)}]");
        }

        private static async Task PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask.ConfigureAwait(false);

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
