using System;
using System.Threading;

namespace AsyncCodeExceptions._02_ThreadPool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(PrintIterations, "ThreadPoolCall");

            PrintIterations("PrimaryThreadCall");

            Console.ReadKey();
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
    }
}
