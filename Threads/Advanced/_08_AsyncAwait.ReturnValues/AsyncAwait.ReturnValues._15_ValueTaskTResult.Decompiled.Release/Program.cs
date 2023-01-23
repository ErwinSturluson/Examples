using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.ReturnValues._15_ValueTaskTResult
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

            ValueTask<int> asyncTask = PrintIterationsAsync("  AsyncTask");

            int syncCallResult = PrintIterations("   SyncCall");

            int asyncTaskResult = asyncTask.Result;

            Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

            Console.ReadKey();
        }

        [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
        [DebuggerStepThrough]
        private static ValueTask<int> PrintIterationsAsync(string _taskName)
        {
            PrintIterationsAsyncStateMachine stateMachine = new();
            stateMachine._builder = AsyncValueTaskMethodBuilder<int>.Create();
            stateMachine._taskName = _taskName;
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
        [StructLayout(LayoutKind.Auto)]
        private struct PrintIterationsAsyncStateMachine : IAsyncStateMachine
        {
            public int _state;
            public AsyncValueTaskMethodBuilder<int> _builder;
            public string _taskName;
            private ValueTaskAwaiter<int> _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;
                int taskResult;

                try
                {
                    ValueTaskAwaiter<int> awaiter;

                    if (localState != 0)
                    {
                        Console.WriteLine($"++ {_taskName ?? "null",-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                        Task<int> task = new(PrintIterations, _taskName);
                        ValueTask<int> valueTask = new(task);
                        task.Start();

                        awaiter = valueTask.GetAwaiter();

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
                        _awaiter = new ValueTaskAwaiter<int>();
                        _state = -1;
                    }

                    taskResult = awaiter.GetResult();

                    Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
                }
                catch (Exception ex)
                {
                    _state = -2;
                    _builder.SetException(ex);

                    return;
                }

                _state = -2;
                _builder.SetResult(taskResult);
            }

            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
                _builder.SetStateMachine(stateMachine);
            }
        }
    }
}
