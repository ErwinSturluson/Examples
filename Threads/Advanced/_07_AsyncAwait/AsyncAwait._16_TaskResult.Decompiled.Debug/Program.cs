using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._16_TaskResult.Decompiled.Debug
{
    internal class Program
    {
        [SpecialName]
        private static void Main(string[] args)
        {
            Main_Async(args).GetAwaiter().GetResult();
        }

        [AsyncStateMachine(typeof(MainStateMachine))]
        [DebuggerStepThrough]
        private static Task Main_Async(string[] args)
        {
            MainStateMachine stateMachine = new();
            stateMachine._builder = AsyncTaskMethodBuilder.Create();
            stateMachine._args = args;
            stateMachine._state = -1;
            stateMachine._builder.Start(ref stateMachine);
            return stateMachine._builder.Task;
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

            int result = iterationIndex * 1000;

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

            return result;
        }

        [CompilerGenerated]
        private sealed class MainStateMachine : IAsyncStateMachine
        {
            public int _state;
            public AsyncTaskMethodBuilder _builder;
            public string[] _args;
            private int _asyncTaskResult;
            private int _syncCallResult;
            private TaskAwaiter<int> _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;
                try
                {
                    TaskAwaiter<int> awaiter;

                    if (localState != 0)
                    {
                        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

                        awaiter = PrintIterationsAsync("  AsyncTask").GetAwaiter();

                        if (!awaiter.IsCompleted)
                        {
                            _state = 0;
                            _awaiter = awaiter;
                            MainStateMachine stateMachine = this;

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

                    _asyncTaskResult = awaiter.GetResult();

                    Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Result of [asyncTaskResult] is [{_asyncTaskResult}]");

                    _syncCallResult = PrintIterations("   SyncCall");

                    Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Result of [syncCallResult] is [{_syncCallResult}]");

                    Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");
                }
                catch (Exception ex)
                {
                    _state = -2;
                    _builder.SetException(ex);

                    return;
                }

                _state = -2;
                _builder.SetResult();
            }

            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
            }
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
