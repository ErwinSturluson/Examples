using System;
using System.Threading;

namespace ThreadsSynchronisation._14_EventWaitHandle
{
    internal class Program
    {
        private static EventWaitHandle _eventWaitHandle = new(false, EventResetMode.ManualReset, typeof(Program).FullName);

        private static void Main(string[] args)
        {
            Thread thread = new(PrintIterations);

            thread.Start("Secondary");

            Thread.Sleep(100);

            Console.Write("Press any key to set the event: ");
            Console.ReadKey();
            _eventWaitHandle.Set();
            Thread.Sleep(1000);
        }

        private static void PrintIterations(object arg)
        {
            _eventWaitHandle.WaitOne();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;
                Console.WriteLine($"{Environment.NewLine}{arg} - {iterationNumber}");
                Thread.Sleep(100);
            }

            Console.WriteLine();
        }
    }
}
