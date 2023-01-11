using System;
using System.Threading;

namespace ThreadsSynchronisation._05_MutexRecursiveLocking
{
    internal class Program
    {
        private static Mutex _mutex = new();

        private static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(Main)} in Thread#{Environment.CurrentManagedThreadId} has started.");

            Thread thread1 = new(Method1);
            Thread thread2 = new(Method2);

            thread1.Start();
            Thread.Sleep(100);
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"{nameof(Main)} in Thread#{Environment.CurrentManagedThreadId} has finished.");
        }

        private static void Method1()
        {
            _mutex.WaitOne();
            Thread.Sleep(100);
            Console.WriteLine($"{nameof(Method1)} in Thread#{Environment.CurrentManagedThreadId} has started.");
            Method2();
            _mutex.ReleaseMutex();
            Console.WriteLine($"{nameof(Method1)} in Thread#{Environment.CurrentManagedThreadId} has finished.");
        }

        private static void Method2()
        {
            _mutex.WaitOne();
            Console.WriteLine($"{nameof(Method2)} in Thread#{Environment.CurrentManagedThreadId} has started.");
            Thread.Sleep(1000);
            _mutex.ReleaseMutex();
            Console.WriteLine($"{nameof(Method2)} in Thread#{Environment.CurrentManagedThreadId} has finished.");
        }
    }
}
