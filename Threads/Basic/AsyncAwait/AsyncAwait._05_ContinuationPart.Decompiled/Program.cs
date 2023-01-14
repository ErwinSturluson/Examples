using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._05_ContinuationPart.Decompiled
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintIterationsAsync("AsyncTask");
            Console.ReadKey();
        }

        private static void PrintIterationsAsync(string taskName)
        {
            PrintIterationsAsyncStateMachine stateMachine = new();

            stateMachine._builder = AsyncVoidMethodBuilder.Create();
            stateMachine._taskName = taskName;
            stateMachine._state = -1;

            stateMachine._builder.Start(ref stateMachine);
        }

        private static void PrintIterations(object state)
        {
            string taskName = state.ToString();
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }
        }

        private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
        {
            public int _state;
            public AsyncVoidMethodBuilder _builder;
            public string _taskName; // The local variable of the source method.
            private Task _printIterationsTask; // The local variable of the source method.
            private TaskAwaiter _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;

                try
                {
                    TaskAwaiter awaiter;

                    if (localState != 0)
                    {
                        Console.WriteLine($"{nameof(PrintIterationsAsync)} method before await has started in Thread#{Environment.CurrentManagedThreadId}");

                        _printIterationsTask = new Task(PrintIterations, _taskName);

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
                        _awaiter = new TaskAwaiter();
                        _state = -1;
                    }

                    awaiter.GetResult();

                    Console.WriteLine($"{nameof(PrintIterationsAsync)} method after await (Continuation part) has finished in Thread#{Environment.CurrentManagedThreadId}");
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
                _builder.SetResult();
            }

            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
            }
        }
    }
}
