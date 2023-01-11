using System;
using System.Threading;

namespace ThreadsSynchronisation._12_RegisteredWaitHandle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AutoResetEvent autoResetEvent = new(false);
            WaitOrTimerCallback callbackMethodDelegate = new(PrintIterations);

            RegisteredWaitHandle registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(autoResetEvent, callbackMethodDelegate, "Secondary", 2000, false);

            char command;

            while (true)
            {
                Thread.Sleep(800);
                Console.Write("Press any key to set the event for associated RegisteredWaitHandle instances, 'x' to unregister the event and exit: ");

                command = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (command == 'x')
                {
                    Console.WriteLine("Associated RegisteredWaitHandle instances was unregistered.");
                    registeredWaitHandle.Unregister(autoResetEvent);
                    break;
                }
                else
                {
                    autoResetEvent.Set();
                }
            }
        }

        private static void PrintIterations(object state, bool timedOut)
        {
            int iterationNumber = 0;

            Console.WriteLine();

            while (iterationNumber < 3)
            {
                iterationNumber++;
                Console.WriteLine($"{state} - {iterationNumber}");
                Thread.Sleep(200);
            }
        }
    }
}
