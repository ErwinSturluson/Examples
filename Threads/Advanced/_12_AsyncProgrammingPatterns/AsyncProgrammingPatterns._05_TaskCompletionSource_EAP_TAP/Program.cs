using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgrammingPatterns._05_TaskCompletionSource_EAP_TAP
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IEnumerable<string> iterations = await PrintIterationsAsync_EAP_TAP("AsyncTask_EAP_TAP");

            foreach (string iteration in iterations)
            {
                Console.WriteLine(iteration);
            }

            Console.ReadKey();
        }

        private static Task<IEnumerable<string>> PrintIterationsAsync_EAP_TAP(string taskName)
        {
            TaskCompletionSource<IEnumerable<string>> tcs = new();

            EapExecuter<IEnumerable<string>> eapExecuter = new();

            eapExecuter.OnExecutionCompleted += (iterations) =>
            {
                try
                {
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

            eapExecuter.ExecuteAsync(() => PrintIterations(taskName));

            return tcs.Task;
        }

        private static void EapExecuter_OnExecutionCompleted(IEnumerable<string> iterations)
        {
            foreach (string iteration in iterations)
            {
                Console.WriteLine(iteration);
            }
        }

        private static IEnumerable<string> PrintIterations(object state)
        {
            string callName = state.ToString();

            List<string> iterationList = new();

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

    internal class EapExecuter<TResult>
    {
        public event Action<TResult> OnExecutionCompleted;

        public void ExecuteAsync(Func<TResult> operation)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ExecutionWaitCallback), operation);
        }

        private void ExecutionWaitCallback(object state)
        {
            Func<TResult> operation = (Func<TResult>)state;

            TResult result = operation.Invoke();

            OnExecutionCompleted?.Invoke(result);
        }
    }
}
