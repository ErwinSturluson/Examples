using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeExceptions._03_Task
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task task = Task.Run(() =>
            {
                PrintIterations("AsyncTask");
            });

            PrintIterations("SyncCall");

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

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
}
