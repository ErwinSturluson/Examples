using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncOperationsProgress.IProgressT
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IProgress<string> progress = new Progress<string>((report) => Console.WriteLine($"[Progress]: {report}"));

            await Task.Run(() => PrintIterations("AsyncTask", progress));
        }

        private static void PrintIterations(string callName, IProgress<string> progress)
        {
            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                progress.Report($"{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
}
