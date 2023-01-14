# **Async Await**

**Async Await** - 2 ключевых слова, которые применяются к методам, позволяющие организовать их асинхронное выполнение.

**Async Await** в разных контекстах (Console/WPF/Web App) используется по-разному (с небольшими отличиями).

Ключевым словом **async** помечается метод, который должен быть вызван асинхронно, а ключевым словом **await** строка, ожидающая асинхронного завершения.

Операции метода, расположенные до await, выполняются синхронно, а операции, расположенные после, могут выполняться как синхронно, так и асинхронно, например:

- Стандартно, операции в асинхронном методе, помеченным ключевым словом **async**, расположенные до ключевого слова **await**, выполняются синхронно с вызывающим метод потоком. Асинхронная задача, ожидаемая ключевым словом **await**, выполняется асинхронно. Операции, расположенные после ключевого слова **await**, выполняются асинхронно по отношению к потоку, вызвавшему асинхронный метод и синхронно по отношению к задаче, которая ожидалась ключевым словом **await**.
- В приложениях **WPF** операции, расположенные после ключевого слова **await**, выполняются синхронно по отношению к вызвавшему асинхронный метод потоку, даже несмотря на то, что await ожидал операцию, выполняемую асинхронно, то есть в контексте другого потока.

Разница в способе выполнения кода, который расположен после ключевого слова **await** обусловлена тем, что его выполнение может регулироваться контекстом синхронизации **SynchronisationContext**.

Асинхронные методы, помеченные ключевым словом **async**, могут иметь в качестве возвращаемого значения типы **void**, **Task** и **Task<T>**. 

В следующих примерах в качестве возвращаемого значения будут использованы типы **void** и **Task<T>**, однако, если из асинхронного метода не требуется возвращать какое-либо значение, следует всё равно указывать в качестве возвращаемого значения тип **Task** вместо **void**, и использовать тип **void** только при необходимости, например, в событийных моделях, где требуются методы с возвращаемым типом **void**.

## **Ключевые слова async await**
---
Пример использования ключевых слов **async** и **await**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"{nameof(Main)} method has started in Thread#{Environment.CurrentManagedThreadId}");

        PrintIterationsAsync("AsyncTask");

        Console.ReadKey();

        Console.WriteLine($"{nameof(Main)} method has finished in Thread#{Environment.CurrentManagedThreadId}");
    }

    private static async void PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"{nameof(PrintIterationsAsync)} method has started in Thread#{Environment.CurrentManagedThreadId}");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"{nameof(PrintIterationsAsync)} method has finished in Thread#{Environment.CurrentManagedThreadId}");
    }

    private static void PrintIterations(object state)
    {
        Console.WriteLine($"{nameof(PrintIterations)} method has started in Thread#{Environment.CurrentManagedThreadId}");

        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"{nameof(PrintIterations)} method has finished in Thread#{Environment.CurrentManagedThreadId}");
    }
}
```

## **Преобразование компилятором асинхронного метода, помеченного ключевым словом async**
---
На самом деле, C# компилятор переписывает асинхронный метод, помеченный ключевым словом **async** в код "конечного автомата", который может выполнять одну задачу последовательно в разных потоках, переключая состояние каждый раз при выполнении очередной части асинхронного метода.

C# компилятор создаёт вложенный класс, наследующий интерфейс **IAsyncStateMachine**, содержащий методы **MoveNext()** и **SetStateMachine()**. 

Вся логика работы конечного автомата содержится в методе **MoveNext()**, а состояние (числовой идентификатор состояния и значения локальных переменных исходного метода) хранятся в полях созданного класса.

Также созданный класс кроме полей для хранения идентификатора состояния и локальных переменных исходного метода имеет следующие поля:
- **AsyncVoidMethodBuilder _builder** - поле, в которое помещается экземпляр структуры типа **AsyncVoidMethodBuilder**, отвечающий за запуск асинхронной задачи;
- **TaskAwaiter awaiter** - поле, в которое помещается экземпляр структуры типа **TaskAwaiter**, полученного из ожидаемой ключевым словом **await** исходного кода задачи через метод **GetAwaiter()** класса **Task**.

Пример исходного кода с **async await**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        PrintIterationsAsync("AsyncTask");

        Console.ReadKey();
    }

    private static async void PrintIterationsAsync(string taskName)
    {
        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;
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
}
```

Пример декомпилированного исходного кода из языка программирования IL обратно в язык программирования C# после его преобразования C# компилятором:

```cs
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
```

Для повышения читаемости и понимания примера из следующего кода были удалены не относящиеся к теме сгенерированные конструкции и частично изменены названия полей, переменных и классов.

## **Continuation part**

Часть асинхронного метода, помеченного ключевым словом **async**, выполняемая **после** ключевого слова **await**, или часть продолжения, помещается после вызова метода **GetResult()** на экземпляре структуры **TaskAwaiter**.

Часть асинхронного метода, помеченного ключевым словом **async**, выполняемая **до** ключевого слова **await**, помещается перед созданием и запуском ожидаемой в асинхронном методе задачи.

Пример исходного кода с **частью продолжения**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        PrintIterationsAsync("AsyncTask");

        Console.ReadKey();
    }

    private static async void PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"{nameof(PrintIterationsAsync)} method before await has started in Thread#{Environment.CurrentManagedThreadId}");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"{nameof(PrintIterationsAsync)} method after await (Continuation) has finished in Thread#{Environment.CurrentManagedThreadId}");
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
}
```

Пример декомпилированного исходного кода из языка программирования IL обратно в язык программирования C# после его преобразования C# компилятором:

```cs
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
```

## **Return value**
---

Для возврата значения из асинхронного метода, помеченного ключевым словом **async**, необходимо в качестве возвращаемого из метода значения указать тип **Task<T>**, где **T** - тип, экземпляр которого требуется вернуть.

В теле асинхронного метода возврат значения из метода осуществляется обычным способом через оператор **return**.

Для возврата значения из задачи в сгенерированном типе cоздаётся дополнительное поле для сохранения возвращаемого значения асинхронного метода, и, для возврата этого значения вызывающему коду, оно передаётся в метод **_builder.SetResult(iterationsTaskResult);**.

Пример исходного кода с **возвращаемым значением**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        int printIterationsResult = PrintIterationsAsync("AsyncTask").GetAwaiter().GetResult();

        Console.WriteLine($"{nameof(PrintIterations)} Result: {printIterationsResult}");

        Console.ReadKey();
    }

    private static async Task<int> PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"{nameof(PrintIterationsAsync)} method before await has started in Thread#{Environment.CurrentManagedThreadId}");

        Task<int> printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        int printIterationsTaskResult = await printIterationsTask;

        Console.WriteLine($"{nameof(PrintIterationsAsync)} method after await (Continuation part) has finished in Thread#{Environment.CurrentManagedThreadId}");

        return printIterationsTaskResult;
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
}
```

Пример декомпилированного исходного кода из языка программирования IL обратно в язык программирования C# после его преобразования C# компилятором:

```cs
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
```
