using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgrammingPatterns._02_EAP
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EapExecuter<IEnumerable<string>> eapExecuter = new();

            eapExecuter.OnExecutionCompleted += EapExecuter_OnExecutionCompleted;

            eapExecuter.ExecuteAsync(() => PrintIterations("AsyncOperation_EAP"));

            Console.ReadKey();
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
