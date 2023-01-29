using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeExceptions._09_Task.Result
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => PrintIterations("AsyncTask"));

            PrintIterations("SyncCall");

            int taskResult = task.Result;

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
                    throw new Exception($"[!!!EXCEPTION!!! {callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1}]");
                }
            }

            int result = iterationIndex * 1000;

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

            return result;
        }
    }
}
