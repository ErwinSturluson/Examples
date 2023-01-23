using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.ReturnValues._08_TaskTResult.Decompiled.Debug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

            Task<int> asyncTask = PrintIterationsAsync("  AsyncTask");

            int syncCallResult = PrintIterations("   SyncCall");

            int asyncTaskResult = asyncTask.Result;

            Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

            Console.ReadKey();
        }

        [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
        [DebuggerStepThrough]
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
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

            int result = iterationIndex * 1000;

            return iterationIndex;
        }

        [CompilerGenerated]
        private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
        {
            public int _state;
            public AsyncTaskMethodBuilder<int> _builder;
            public string _taskName;
            private Task<int> _printIterationsTask;
            private int _taskResult;
            private TaskAwaiter<int> _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;

                try
                {
                    TaskAwaiter<int> awaiter;

                    if (localState != 0)
                    {
                        Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                        _printIterationsTask = new Task<int>(PrintIterations, _taskName);
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

                    _taskResult = awaiter.GetResult();

                    Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
                _builder.SetResult(_taskResult);
            }

            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
            }
        }
    }
}
