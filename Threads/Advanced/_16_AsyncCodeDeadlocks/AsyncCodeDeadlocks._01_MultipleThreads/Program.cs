using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeDeadlocks._01_MultipleThreads
{
    internal class Program
    {
        private static readonly object _sharedResource1Lock = new();
        private static readonly object _sharedResource2Lock = new();

        private static async Task Main(string[] args)
        {
            Task task1 = Task.Run(() =>
            {
                Console.WriteLine("Task1 has started.");

                lock (_sharedResource1Lock)
                {
                    Console.WriteLine("  Task1 has locked Shared Resource 1.");
                    Thread.Sleep(500);

                    lock (_sharedResource2Lock)
                    {
                        Console.WriteLine("    Task1 has locked Shared Resource 1.");
                        Thread.Sleep(500);
                        Console.WriteLine("    Task1 has unlocked Shared Resource 1.");
                    }

                    Console.WriteLine("  Task1 has unlocked Shared Resource 1.");
                }

                Console.WriteLine("Task1 has finished.");
            });

            Task task2 = Task.Run(() =>
            {
                Console.WriteLine("Task2 has started.");

                lock (_sharedResource2Lock)
                {
                    Console.WriteLine("  Task2 has locked Shared Resource 1.");
                    Thread.Sleep(500);

                    lock (_sharedResource1Lock)
                    {
                        Console.WriteLine("    Task2 has locked Shared Resource 1.");
                        Thread.Sleep(500);
                        Console.WriteLine("    Task2 has unlocked Shared Resource 1.");
                    }

                    Console.WriteLine("  Task2 has unlocked Shared Resource 1.");
                }

                Console.WriteLine("Task2 has finished.");
            });

            await Task.WhenAll(task1, task2);

            await Console.Out.WriteLineAsync("All the Tasks were completed.");
        }
    }
}
