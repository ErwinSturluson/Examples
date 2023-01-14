using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._07_ReturnValue.Decompiled
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int printIterationsResult = PrintIterationsAsync("AsyncTask").GetAwaiter().GetResult();

            Console.WriteLine($"{nameof(PrintIterations)} Result: {printIterationsResult}");

            Console.ReadKey();
        }

        private static Task<int> PrintIterationsAsync(string taskName)
        {
            PrintIterationsAsyncStateMachine stateMachine = new();
            stateMachine._builder = AsyncTaskMethodBuilder<int>.Create();
            stateMachine._taskName = taskName;
            stateMachine._state = -1;
            stateMachine._builder.Start(ref stateMachine);
            return stateMachine._builder.Task;
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }

            int calculatedResult = iterationNumber * 100;

            return calculatedResult;
        }

        private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
        {
            public int _state;
            public AsyncTaskMethodBuilder<int> _builder;
            public string _taskName; // The local variable of the source method.
            private Task<int> _printIterationsTask; // The local variable of the source method.
            private int _printIterationsTaskResult; // The local variable of the source method.
            private int _taskAwaiterResult; // A variable to save task result locally.
            private TaskAwaiter<int> _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;
                int iterationsTaskResult;

                try
                {
                    TaskAwaiter<int> awaiter;

                    if (localState != 0)
                    {
                        Console.WriteLine($"{nameof(PrintIterationsAsync)} method before await has started in Thread#{Environment.CurrentManagedThreadId}");

                        _printIterationsTask = new Task<int>(new Func<object, int>(PrintIterations), _taskName);
                        _printIterationsTask.Start();
                        awaiter = _printIterationsTask.GetAwaiter();
                        if (!awaiter.IsCompleted)
                        {
                            _state = 0;
                            _awaiter = awaiter;
                            PrintIterationsAsyncStateMachine stateMachine = this;

                            _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                            return;
                        }
                    }
                    else
                    {
                        awaiter = _awaiter;
                        _awaiter = new TaskAwaiter<int>();
                        _state = -1;
                    }

                    _taskAwaiterResult = awaiter.GetResult();
                    _printIterationsTaskResult = _taskAwaiterResult;

                    Console.WriteLine($"{nameof(PrintIterationsAsync)} method after await (Continuation part) has finished in Thread#{Environment.CurrentManagedThreadId}");

                    iterationsTaskResult = _printIterationsTaskResult;
                }
                catch (Exception ex)
                {
                    _state = -2;
                    _printIterationsTask = null;
                    _builder.SetException(ex);

                    return;
                }
                _state = -2;
                _printIterationsTask = null;
                _builder.SetResult(iterationsTaskResult);
            }

            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
            }
        }
    }
}
