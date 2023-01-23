using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.ReturnValues._11_ValueTask.Decompiled.Debug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

            ValueTask asyncTask = PrintIterationsAsync("  AsyncTask");

            PrintIterations("   SyncCall");

            asyncTask.GetAwaiter().GetResult();

            Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

            Console.ReadKey();
        }

        [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
        [DebuggerStepThrough]
        private static ValueTask PrintIterationsAsync(string _taskName)
        {
            PrintIterationsAsyncStateMachine stateMachine = new();
            stateMachine._builder = AsyncValueTaskMethodBuilder.Create();
            stateMachine._taskName = _taskName;
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
            public AsyncValueTaskMethodBuilder _builder;
            public string _taskName;
            private Task _printIterationsTask;
            private ValueTask _printIterationsValueTask;
            private ValueTaskAwaiter _awaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int localState = _state;

                try
                {
                    ValueTaskAwaiter awaiter;

                    if (localState != 0)
                    {
                        Console.WriteLine($"++ {_taskName ?? "null",-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                        _printIterationsTask = new Task(PrintIterations, _taskName);
                        _printIterationsValueTask = new ValueTask(_printIterationsTask);
                        _printIterationsTask.Start();

                        awaiter = _printIterationsValueTask.GetAwaiter();

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
                        _awaiter = new ValueTaskAwaiter();
                        _state = -1;
                    }

                    awaiter.GetResult();

                    Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
                }
                catch (Exception ex)
                {
                    _state = -2;
                    _printIterationsTask = null;
                    _printIterationsValueTask = new ValueTask();
                    _builder.SetException(ex);

                    return;
                }

                _state = -2;
                _printIterationsTask = null;
                _printIterationsValueTask = new ValueTask();
                _builder.SetResult();
            }

            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
            }
        }
    }
}
