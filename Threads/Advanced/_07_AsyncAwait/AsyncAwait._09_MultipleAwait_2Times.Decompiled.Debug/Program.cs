using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait._09_MultipleAwait_2Times.Decompiled.Debug
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
        private static Task PrintIterationsAsync(string taskName)
        {
            PrintIterationsAsyncStateMachine stateMachine = new();
            stateMachine._builder = AsyncTaskMethodBuilder.Create();
            stateMachine._taskName = taskName;
            stateMachine._state = -1;
            stateMachine._builder.Start(ref stateMachine);

            return stateMachine._builder.Task;
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
        private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
        {
            public int _state;
            public AsyncTaskMethodBuilder _builder;
            public string _taskName;
            private Task _printIterationsTask1;
            private Task _printIterationsTask2;
            private TaskAwaiter _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;

                try
                {
                    TaskAwaiter awaiter1;
                    TaskAwaiter awaiter2;

                    if (localState != 0)
                    {
                        if (localState != 1)
                        {
                            Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                            _printIterationsTask1 = new Task(PrintIterations, $"{_taskName}[1]");
                            _printIterationsTask1.Start();
                            awaiter1 = _printIterationsTask1.GetAwaiter();

                            if (!awaiter1.IsCompleted)
                            {
                                _state = 0;
                                _awaiter = awaiter1;
                                PrintIterationsAsyncStateMachine stateMachine = this;
                                _builder.AwaitUnsafeOnCompleted(ref awaiter1, ref stateMachine);

                                return;
                            }
                        }
                        else
                        {
                            awaiter2 = _awaiter;
                            _awaiter = new TaskAwaiter();
                            _state = -1;

                            goto Label_State1;
                        }
                    }
                    else
                    {
                        awaiter1 = _awaiter;
                        _awaiter = new TaskAwaiter();
                        _state = -1;
                    }

                    awaiter1.GetResult();

                    _printIterationsTask2 = new Task(PrintIterations, $"{_taskName}[2]");
                    _printIterationsTask2.Start();
                    awaiter2 = _printIterationsTask2.GetAwaiter();

                    if (!awaiter2.IsCompleted)
                    {
                        _state = 1;
                        _awaiter = awaiter2;
                        PrintIterationsAsyncStateMachine stateMachine = this;
                        _builder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);

                        return;
                    }

                Label_State1:
                    awaiter2.GetResult();
                    Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
                }
                catch (Exception ex)
                {
                    _state = -2;
                    _printIterationsTask1 = null;
                    _printIterationsTask2 = null;
                    _builder.SetException(ex);

                    return;
                }

                _state = -2;
                _printIterationsTask1 = null;
                _printIterationsTask2 = null;
                _builder.SetResult();
            }

            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
            }
        }
    }
}
