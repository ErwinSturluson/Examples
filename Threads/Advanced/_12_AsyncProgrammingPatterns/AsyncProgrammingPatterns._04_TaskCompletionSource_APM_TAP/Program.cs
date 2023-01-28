using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgrammingPatterns._04_TaskCompletionSource_APM_TAP
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IEnumerable<string> iterationsTcs = await PrintIterationsAsync_APM_TAP_TCS("AsyncTask_APM_TAP_TCS");

            foreach (string iteration in iterationsTcs)
            {
                Console.WriteLine(iteration);
            }

            await Console.Out.WriteLineAsync();

            IEnumerable<string> iterationsTf = await PrintIterationsAsync_APM_TAP_TF("AsyncTask_APM_TAP_TF");

            foreach (string iteration in iterationsTf)
            {
                Console.WriteLine(iteration);
            }

            Console.ReadKey();
        }

        private static Task<IEnumerable<string>> PrintIterationsAsync_APM_TAP_TCS(string taskName)
        {
            TaskCompletionSource<IEnumerable<string>> tcs = new TaskCompletionSource<IEnumerable<string>>();

            Action<string> asyncOperation_Apm = (callName) =>
            {
                try
                {
                    IEnumerable<string> iterations = PrintIterations(callName);

                    tcs.TrySetResult(iterations);
                }
                catch (OperationCanceledException ex)
                {
                    tcs.TrySetCanceled(ex.CancellationToken);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };

            asyncOperation_Apm.BeginInvoke(taskName, null, null);

            return tcs.Task;
        }

        private static Task<IEnumerable<string>> PrintIterationsAsync_APM_TAP_TF(string taskName)
        {
            TaskFactory taskFactory = new TaskFactory();

            Func<string, IEnumerable<string>> asyncOperation_Apm = PrintIterations;

            return taskFactory.FromAsync(asyncOperation_Apm.BeginInvoke(taskName, null, null), (asyncResult) =>
            {
                IEnumerable<string> iterations = asyncOperation_Apm.EndInvoke(asyncResult);

                return iterations;
            });
        }

        private static IEnumerable<string> PrintIterations(string callName)
        {
            List<string> iterationList = new List<string>();

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                iterationList.Add($"{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");

                Thread.Sleep(100);
            }

            return iterationList;
        }
    }
}
