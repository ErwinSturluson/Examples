using System;
using System.Threading;

namespace ThreadsSynchronisation._16_WaitHandle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WaitHandle[] waitHandles = new WaitHandle[2]
            {
                new AutoResetEvent(false),
                new AutoResetEvent(false)
            };

            PrintIterationArgs[] printIterationArgs = new PrintIterationArgs[2]
            {
                new PrintIterationArgs
                {
                    TaskName = "Secondary",
                    WaitHandle = waitHandles[0]
                },
                new PrintIterationArgs
                {
                    TaskName = "Secondary",
                    WaitHandle = waitHandles[1]
                }
            };

            Console.WriteLine($"2x {nameof(PrintIterations)} methods has started.");

            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[0]);
            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[1]);

            WaitHandle.WaitAll(waitHandles);

            Console.WriteLine($"Both {nameof(PrintIterations)} methods has finished.{Environment.NewLine}");

            Console.WriteLine($"2x new {nameof(PrintIterations)} methods has started.");

            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[0]);
            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[1]);

            int index = WaitHandle.WaitAny(waitHandles);

            Console.WriteLine($"Method {nameof(PrintIterations)} with index {index} has finished first.");

            WaitHandle.WaitAny(waitHandles);

            Console.WriteLine($"Both {nameof(PrintIterations)} methods has finished.");
        }

        private static void PrintIterations(object arg)
        {
            PrintIterationArgs printIterationArgs = arg as PrintIterationArgs ?? throw new ArgumentException(null, nameof(arg));

            AutoResetEvent autoResetEvent = printIterationArgs.WaitHandle as AutoResetEvent ?? throw new ArgumentException(null, nameof(arg));

            int iterationNumber = 0;

            while (iterationNumber < 3)
            {
                iterationNumber++;
                Console.WriteLine($"{printIterationArgs.TaskName} - Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(500);
            }

            autoResetEvent.Set();
        }
    }

    internal class PrintIterationArgs
    {
        public string TaskName { get; set; }

        public WaitHandle WaitHandle { get; set; }
    }
}
