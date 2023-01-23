using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.ReturnValues._03_Void.Decompiled.Release
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

            PrintIterationsAsync("  AsyncTask");

            PrintIterations("   SyncCall");

            Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

            Console.ReadKey();
        }

        [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
        [DebuggerStepThrough]
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
        }

        [CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct PrintIterationsAsyncStateMachine : IAsyncStateMachine

        {
            public int _state;
            public AsyncVoidMethodBuilder _builder;
            public string _taskName;
            private TaskAwaiter _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;
                try
                {
                    TaskAwaiter awaiter;

                    if (localState != 0)
                    {
                        Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                        Task task = new(PrintIterations, _taskName);
                        task.Start();

                        awaiter = task.GetAwaiter();

                        if (!awaiter.IsCompleted)
                        {
                            _state = 0;
                            _awaiter = awaiter;

                            _builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);

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

                    Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
                _builder.SetStateMachine(stateMachine);
            }
        }
    }
}
