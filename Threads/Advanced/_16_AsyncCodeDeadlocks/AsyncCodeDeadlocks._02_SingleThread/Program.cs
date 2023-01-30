using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeDeadlocks._02_SingleThread
{
    internal class Program
    {
        private static readonly object _sharedResource1Lock = new object();
        private static readonly object _sharedResource2Lock = new object();

        private static async Task Main(string[] args)
        {
            Task task = Task.Run(() =>
            {
                Console.WriteLine("Task has started.");

                lock (_sharedResource1Lock)
                {
                    Console.WriteLine("  Task has locked Shared Resource 1.");
                    Thread.Sleep(500);

                    lock (_sharedResource2Lock)
                    {
                        Console.WriteLine("    Task has locked Shared Resource 1.");
                        Thread.Sleep(500);
                        Console.WriteLine("    Task has unlocked Shared Resource 1.");
                    }

                    Console.WriteLine("  Task has unlocked Shared Resource 1.");
                }

                Console.WriteLine("Task has finished.");
            });

            await task;

            await Console.Out.WriteLineAsync("All the operations were completed.");
        }
    }
}
