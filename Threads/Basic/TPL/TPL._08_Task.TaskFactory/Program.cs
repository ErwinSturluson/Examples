using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL._08_TaskFactory
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task task1 = Task.Factory.StartNew(PrintIterations, "AsyncTask1");

            Task task2 = Task.Factory.StartNew((state) =>
            {
                string taskName = state.ToString();

                int iterationNumber = 0;

                while (iterationNumber < 5)
                {
                    iterationNumber++;

                    Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                    Thread.Sleep(100);
                }
            }, "AsyncTask2");

            Task.WaitAll(task1, task2);
        }

        private static void PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }
        }
    }
}
