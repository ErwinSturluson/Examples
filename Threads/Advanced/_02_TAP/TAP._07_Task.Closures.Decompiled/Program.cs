using System;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._07_Task.Closures.Decompiled
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string taskName = "AsyncTask";
            int iterationNumber = 10;
            int iterationDelayMilliseconds = 100;

            DisplayClass displayClass = new()
            {
                _taskName = taskName,
                _iterationNumber = iterationNumber,
                _iterationDelayMilliseconds = iterationDelayMilliseconds
            };

            Task<int> task = new(state => ((DisplayClass)state).Main(), displayClass);
            task.Start();

            task.Wait();

            Console.WriteLine($"AsyncTask has finished. Task's Result: {task.Result}");
        }

        private static int PrintIterations(string taskName, int iterationNumber, int iterationDelayMilliseconds)
        {
            int iterationIndex = 0;

            while (iterationIndex < iterationNumber)
            {
                iterationIndex++;

                Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(iterationDelayMilliseconds);
            }

            int calculationResult = iterationIndex * 1000;

            return calculationResult;
        }

        private sealed class DisplayClass
        {
            public string _taskName;
            public int _iterationNumber;
            public int _iterationDelayMilliseconds;

            internal int Main()
            {
                return PrintIterations(_taskName, _iterationNumber, _iterationDelayMilliseconds);
            }
        }
    }
}
