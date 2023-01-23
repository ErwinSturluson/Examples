# **Возвращаемые значения Async Await**

**Возвращаемые значения Async Await**:

## **Типы возвращаемых значений асинхронных методов:**

**Асинхронные методы** могут иметь следующие типы **возвращаемых значений**:
- Тип ```void``` – используется только для обработчиков событий;
- Тип ```Task``` – для асинхронной операции, которая не возвращает значение;
- Тип ```Task<TResult>``` - для асинхронной операции, которая возвращает знаение;
- Тип ```ValueTask``` – для асинхронной операции, которая не возвращает значения. Используется только тогда, когда действительно может привести к приросту производительности;
- Тип ```ValueTask<TResult>``` - для асинхронной операции, которая возвращает значение. Используется только тогда, когда действительно может привести к приросту производительности.

## **void**
---
Тип ```void``` – используется только для специфичных задач, таких, как обработчики событий.

Отличительные черты:
- Тип строителя асинхронного метода: ```AsyncVoidMethodBuilder```;
- При вызове ```_builder.SetResult();``` не передаётся никакого аргумента;
- Асинхронный метод не возвращает задачу-марионетку. 

Source:
```cs
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

    private static async void PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
}
```

Decompiled.Debug:
```cs
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
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncVoidMethodBuilder _builder;
        public string _taskName;
        private Task _printIterationsTask;
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
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

Decompiled.Release:
```cs
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
```

## **Task**
---
Тип ```Task``` - для асинхронной операции, которая не возвращает значение.

Отличительные черты:
- Тип строителя асинхронного метода: ```AsyncTaskMethodBuilder```;
- При вызове ```_builder.SetResult(taskResult);``` не передаётся никакого аргумента;
- Асинхронный метод возвращает задачу-марионетку в качестве экзмепляра структуры ```Task```.

Source:
```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        Task asyncTask = PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        asyncTask.GetAwaiter().GetResult();

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
}
```

Decompiled.Debug:
```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        Task asyncTask = PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        asyncTask.GetAwaiter().GetResult();

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
        private Task _printIterationsTask;
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
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

Decompiled.Release:
```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        Task asyncTask = PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        asyncTask.GetAwaiter().GetResult();

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
    [StructLayout(LayoutKind.Auto)]
    private struct PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
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
```

## **Task<TResult>**
---
Тип ```Task<TResult>``` - для асинхронной операции, которая возвращает знаение.

Отличительные черты:
- Тип строителя асинхронного метода: ```AsyncTaskMethodBuilder<TResult>```;
- При вызове ```_builder.SetResult(taskResult);``` передаётся аргумент - результат выполнения асинхронного метода;
- Асинхронный метод возвращает задачу-марионетку в качестве экзмепляра класса ```Task<TResult>```.

Source:
```cs
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

    private static async Task<int> PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task<int> printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        int taskResult = await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");

        return taskResult;
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
}
```

Decompiled.Debug:
```cs
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
```

Decompiled.Release:
```cs
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
    [StructLayout(LayoutKind.Auto)]
    private struct PrintIterationsAsyncStateMachine : IAsyncStateMachine

    {
        public int _state;
        public AsyncTaskMethodBuilder<int> _builder;
        public string _taskName;
        private TaskAwaiter<int> _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            int taskResult;

            try
            {
                TaskAwaiter<int> awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                    Task<int> task = new(PrintIterations, _taskName);
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
                    _awaiter = new TaskAwaiter<int>();
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
```

## **ValueTask**
---
Тип ```ValueTask``` - для асинхронной операции, которая не возвращает значения. Используется только тогда, когда действительно может привести к приросту производительности.

Отличительные черты:
- Тип строителя асинхронного метода: ```AsyncValueTaskMethodBuilder```;
- При вызове ```_builder.SetResult(taskResult);``` не передаётся никакого аргумента;
- Асинхронный метод возвращает задачу-марионетку в качестве экзмепляра структуры ```ValueTask```.

Source:
```cs
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

    private static async ValueTask PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName ?? "null",-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        ValueTask printIterationsValueTask = new(printIterationsTask);

        printIterationsTask.Start();

        await printIterationsValueTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
}
```

Decompiled.Debug:
```cs
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
```

Decompiled.Release:
```cs
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
    [StructLayout(LayoutKind.Auto)]
    private struct PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncValueTaskMethodBuilder _builder;
        public string _taskName;
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

                    Task task = new(PrintIterations, _taskName);
                    ValueTask valueTask = new(task);
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
                    _awaiter = new ValueTaskAwaiter();
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
```

## **ValueTask<TResult>**
---
Тип ```ValueTask<TResult>``` - для асинхронной операции, которая возвращает значение. Используется только тогда, когда действительно может привести к приросту производительности.

Отличительные черты:
- Тип строителя асинхронного метода: ```AsyncValueTaskMethodBuilder<TResult>```;
- При вызове ```_builder.SetResult(taskResult);``` передаётся аргумент - результат выполнения асинхронного метода;
- Асинхронный метод возвращает задачу-марионетку в качестве экзмепляра структуры ```ValueTask<TResult>```.

Source:
```cs
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

    private static async ValueTask<int> PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task<int> printIterationsTask = new(PrintIterations, taskName);

        ValueTask<int> printIterationsValueTask = new(printIterationsTask);

        printIterationsTask.Start();

        int taskResult = await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");

        return taskResult;
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
}
```

Decompiled.Debug:
```cs
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
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncValueTaskMethodBuilder<int> _builder;
        public string _taskName;
        private Task<int> _printIterationsTask;
        private ValueTask<int> _printIterationsValueTask;
        private int _taskResult;
        private ValueTaskAwaiter<int> _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;

            try
            {
                ValueTaskAwaiter<int> awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"++ {_taskName ?? "null",-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                    _printIterationsTask = new Task<int>(PrintIterations, _taskName);
                    _printIterationsValueTask = new ValueTask<int>(_printIterationsTask);
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
                    _awaiter = new ValueTaskAwaiter<int>();
                    _state = -1;
                }

                awaiter.GetResult();

                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _printIterationsTask = null;
                _printIterationsValueTask = new ValueTask<int>();
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _printIterationsTask = null;
            _printIterationsValueTask = new ValueTask<int>();
            _builder.SetResult(_taskResult);
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

Decompiled.Release:
```cs
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
```