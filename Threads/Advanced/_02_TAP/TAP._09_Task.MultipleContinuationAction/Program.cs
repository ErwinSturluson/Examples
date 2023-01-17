using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TAP._09_Task.MultipleContinuationAction
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task<IEnumerable<string>> task = new(MakeIterations, "AsyncTask");

            Task continuationTasks = task
                .ContinueWith(PrintIterationsSymbolsNumber)
                .ContinueWith(PrintIterationsReports)
                .ContinueWith(previousTask =>
                {
                    Console.WriteLine($"Lambda operator has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");

                    Console.WriteLine("Final continuation action is processing.");

                    Console.WriteLine($"Lambda operator has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");
                });

            task.Start();

            continuationTasks.Wait();
        }

        private static IEnumerable<string> MakeIterations(object state)
        {
            string taskName = state.ToString();
            Console.WriteLine($"{nameof(MakeIterations)} method has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");
            List<string> iterationReports = new(10);

            for (int i = 0; i < iterationReports.Count; i++)
            {
                Console.WriteLine($"{taskName} - Task#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{i}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"{nameof(MakeIterations)} method has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");

            return iterationReports;
        }

        private static IEnumerable<string> PrintIterationsSymbolsNumber(Task previousTask)
        {
            Console.WriteLine($"{nameof(PrintIterationsSymbolsNumber)} method has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");

            Task<IEnumerable<string>> castedPreviousTask = (Task<IEnumerable<string>>)previousTask;

            IEnumerable<string> iterationReports = castedPreviousTask.Result;

            int calculationResult = iterationReports.Select(x => x.Length).Sum();

            Console.WriteLine($"ContinuationAction {nameof(PrintIterationsSymbolsNumber)} - Task#{Task.CurrentId}- Thread#{Environment.CurrentManagedThreadId} - PreviousTask has Result of [{calculationResult}] symbols in all iterations.");

            Console.WriteLine($"{nameof(PrintIterationsSymbolsNumber)} method has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");

            return iterationReports;
        }

        private static void PrintIterationsReports(Task previousTask)
        {
            Console.WriteLine($"{nameof(PrintIterationsReports)} method has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");

            Task<IEnumerable<string>> castedPreviousTask = (Task<IEnumerable<string>>)previousTask;

            IEnumerable<string> iterationReports = castedPreviousTask.Result;

            foreach (string iterationReport in iterationReports)
            {
                Console.WriteLine(iterationReport);
            }

            Console.WriteLine($"{nameof(PrintIterationsReports)} method has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");
        }
    }
}
