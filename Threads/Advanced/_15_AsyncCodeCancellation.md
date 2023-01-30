# **Скоординированная отмена в асинхронном коде**

### **Скоординированная отмена**

**Скоординированная отмена** – отмена, которая требует помимо команды на прерывание выполнения операции, подтверждения в месте операции, которая должна быть отменена.

Два участника в скоординированной операции необходимы потому, что в случае отправки сигнала на отмену в одностороннем порядке, не всегда возможно адекватно прервать выполняющуюся операцию.

Чтобы операция была отменена правильно и корректно, она должна подтвердить отмену своего выполнения. 

Тем самым, операция укажет, что на текущем моменте действительно можно прервать выполнение. 

Таким образом, скоординированная отмена приносит больше безопасности.

Для обеспечения шаблона скооридинированной отмены в .NET есть три специальных типа:
- ```CancellationTokenSource``` – источник токенов отмены, класс для управления отменами;
- ```CancellationToken``` – токен (некий талон/жетон отмены), проверяет наличие приказа на отмену, а так же подтверждает, что нужно совершить отмену;
- ```OperationCanceledException``` – исключение, представляющее в .NET отмену.

## **01 Скоординированная отмена при помощи CancellationToken**
---
Для того, чтобы выполнить скоординированную отмену асинхронной операции, запущенной в контексте экземпляра класса ```Task```, необходимо получить токен отмены, представленный экземплячром структуры ```CancellationToken``` из экземпляра класса ```CancellationTokenSource```, реализовать логику проверки этого токена на предмет запроса отмены в выполняющемся коде и с помощью проверки его свойства ```IsCancellationRequested```, и в случае обнаруженяи запроса на отмену, реализовать возбуждение исключения ```OperationCanceledException```, после чего передать этот токен через замыкание или параметр асинхронной операции в тело асинхронного метода, и в необходимый момент вызвать на упомянутом ранее экземпляре класса ```CancellationTokenSource``` метод ```Cancel()```. 

Для объединения операций проверки и возмбуждения исключения структура ```CancellationToken``` имеет метод ```ThrowIfCancellationRequested()```.

В результате в асинхронной операции будет возбуждено исключение ```OperationCanceledException```, которое будет добавлено в исключение ```AggregateException``` и она будет прервана.

Однако, по умолчанию асинхронная операция будет считаться проваленной, а не отменённой. Чтобы она считалась отменённой, токен отмены также должен быть передан в качестве аргумента при создании или запуске асинхронной операции в конструктор класса ```Task``` или метод создания и запуска ```Run()```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        CancellationTokenSource cts = new();

        CancellationToken ct = cts.Token;

        Task task = Task.Run(() => PrintIterations("AsyncTask", ct), ct);

        try
        {
            Thread.Sleep(500);

            cts.Cancel();

            task.Wait();
        }
        catch (AggregateException ex)
        {
            ReportTaskExceptionDetails(ex, task);
        }

        Console.ReadKey();
    }

    private static void PrintIterations(string callName, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            ct.ThrowIfCancellationRequested();

            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportTaskExceptionDetails(AggregateException ex, Task task = null, string indent = null)
    {
        string details = string.Empty;

        if (task is not null)
        {
            if (task.IsCanceled)
            {
                details += $"[Exception][A Task was canceled]{Environment.NewLine}";
            }
            else if (task.IsFaulted)
            {
                details += $"[Exception][A Task was faulted]{Environment.NewLine}";
            }
        }

        details += $"Exception Type: [{ex.GetType().Name}]{Environment.NewLine}Exception Message: [{ex.Message}]{Environment.NewLine}{new string('-', 40)}";

        Console.WriteLine(details);

        foreach (var innerException in ex.InnerExceptions)
        {
            if (innerException is AggregateException aggregateException)
            {
                ReportTaskExceptionDetails(aggregateException, null, indent + "  ");
            }
            else
            {
                Console.WriteLine($"[InnerException] Exception Type: [{innerException.GetType().Name}]{Environment.NewLine}Exception Message: [{innerException.Message}]{Environment.NewLine}{new string('-', 40)}");
            }
        }
    }
}
```

## **02 Скоординированная отмена при помощи CancellationToken до запуска задачи в планировщике задач**
---
Асинхронная операция в контексте ```Task``` также может быть отменена ещё до запуска, если планировщик задач перед отправкой задачи на запуск обнаружит, что она уже отменена и нет смысла пытаться её запускать.

Для демонтрации такого эффекта без нагрузки приложения множеством задач, создадим собственный планировщик задач ```DelayTaskScheduler```, запускающий задачи после необольшой задержки и проверки их состояния.

Для того, чтобы иметь возможность отменять задачи до их запуска, необходимо также передать токен отмены в качестве аргумента при создании или запуске асинхронной операции в конструктор класса ```Task``` или метод создания и запуска ```Run()```:

> Program.cs
```cs
namespace AsyncCodeCancellation._02_CancellationToken.TaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts = new();

            CancellationToken ct = cts.Token;

            Task task = new(() => PrintIterations("AsyncTask", ct), ct);

            task.Start(new DelayTaskScheduler());

            try
            {
                cts.Cancel();

                task.Wait();
            }
            catch (AggregateException ex)
            {
                ReportTaskExceptionDetails(ex, task);
            }

            Console.ReadKey();
        }

        private static void PrintIterations(string callName, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                ct.ThrowIfCancellationRequested();

                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }

        private static void ReportTaskExceptionDetails(AggregateException ex, Task task = null, string indent = null)
        {
            string details = string.Empty;

            if (task is not null)
            {
                if (task.IsCanceled)
                {
                    details += $"[Exception][A Task was canceled]{Environment.NewLine}";
                }
                else if (task.IsFaulted)
                {
                    details += $"[Exception][A Task was faulted]{Environment.NewLine}";
                }
            }

            details += $"Exception Type: [{ex.GetType().Name}]{Environment.NewLine}Exception Message: [{ex.Message}]{Environment.NewLine}{new string('-', 40)}";

            Console.WriteLine(details);

            foreach (var innerException in ex.InnerExceptions)
            {
                if (innerException is AggregateException aggregateException)
                {
                    ReportTaskExceptionDetails(aggregateException, null, indent + "  ");
                }
                else
                {
                    Console.WriteLine($"[InnerException] Exception Type: [{innerException.GetType().Name}]{Environment.NewLine}Exception Message: [{innerException.Message}]{Environment.NewLine}{new string('-', 40)}");
                }
            }
        }
    }
}
```

> DelayTaskScheduler.cs
```cs
namespace AsyncCodeCancellation._02_CancellationToken.TaskScheduler
{
    internal class DelayTaskScheduler : System.Threading.Tasks.TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task)
        {
            new Thread(() =>
            {
                Thread.Sleep(1000);

                if (!task.IsCanceled)
                {
                    Thread taskThread = new(() => TryExecuteTask(task));

                    taskThread.IsBackground = true;

                    taskThread.Start();
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);
    }
}
```

## **03 Скоординированная отмена при помощи CancellationToken и метода CancelAfter()**
---
Для того, чтобы инициировать скоординированную отмену по истечении определённого времени, класс ```CancellationTokenSource``` имеет экземплярный метод ```CancelAfter()```, который позволяет инициировать скооридинированную отмену по истечении указанного в параметре этого метода интервала времени:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        CancellationTokenSource cts = new();

        CancellationToken ct = cts.Token;

        Task task = Task.Run(() => PrintIterations("AsyncTask", ct), ct);

        cts.CancelAfter(500);

        try
        {
            task.Wait();
        }
        catch (AggregateException ex)
        {
            ReportTaskExceptionDetails(ex, task);
        }

        Console.ReadKey();
    }

    private static void PrintIterations(string callName, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            ct.ThrowIfCancellationRequested();

            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportTaskExceptionDetails(AggregateException ex, Task task = null, string indent = null)
    {
        string details = string.Empty;

        if (task is not null)
        {
            if (task.IsCanceled)
            {
                details += $"[Exception][A Task was canceled]{Environment.NewLine}";
            }
            else if (task.IsFaulted)
            {
                details += $"[Exception][A Task was faulted]{Environment.NewLine}";
            }
        }

        details += $"Exception Type: [{ex.GetType().Name}]{Environment.NewLine}Exception Message: [{ex.Message}]{Environment.NewLine}{new string('-', 40)}";

        Console.WriteLine(details);

        foreach (var innerException in ex.InnerExceptions)
        {
            if (innerException is AggregateException aggregateException)
            {
                ReportTaskExceptionDetails(aggregateException, null, indent + "  ");
            }
            else
            {
                Console.WriteLine($"[InnerException] Exception Type: [{innerException.GetType().Name}]{Environment.NewLine}Exception Message: [{innerException.Message}]{Environment.NewLine}{new string('-', 40)}");
            }
        }
    }
}
```

### **Класс CancellationTokenSource**

Класс ```CancellationTokenSource``` выпускает токены отмены ```CancellationToken```, на нём инициируется отмена через выпущенные токены.

Если потребуется новый маркер 

### **Члены класса ```CancellationTokenSource```**

Конструкторы:
- ```CancellationTokenSource()``` - конструктор, позволяющий создать источник токенов отмены без дополнительных настроек;
- ```CancellationTokenSource(TimeSpan delay)``` - конструктор, позволяющий создать источник токенов отмены c инициацией отмены на выпущенных токенах через временной интервал, представленный экземпляром структуры ```TimeSpan```;
- ```CancellationTokenSource(int millisecondsDelay)``` - конструктор, позволяющий создать источник токенов отмены c инициацией отмены на выпущенных токенах через временной интервал, представленный целочисленным значением в миллисекундах;

Свойства:
- ```bool IsCancellationRequested``` - указывает, была ли запрошена отмена;
- ```CancellationToken Token``` - выпускает токен отмены, в который приходит запрос на выполнение отмены, и при помощи которого должно производиться подтверждение этой отмены. Каждый экземпляр класса ```CancellationTokenSource``` будет возвращать экземпляр структуры ```CancellationToken```, который связан с неим. Если необходим новый маркер отмены, которым можно будет управлять отдельно, то необходимо будет создать новый экземпляр класса ```CancellationTokenSource```;

Методы:
- ```(static) CancellationTokenSource CreateLinkedTokenSource(CancellationToken ct1, CancellationToken ct2)``` - позволяет создать **связанный источник маркеров отмены**, представленный новым экземпляром ```CancellationTokenSource```, благодаря которому можно инициировать отмену на двух маркерах, созданных различными источниками маркеров отмены;
- ```(static) CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens)``` - позволяет создать **связанный источник маркеров отмены**, представленный новым экземпляром ```CancellationTokenSource``` благодаря которому можно инициировать отмену на произвольном количестве маркеров, созданных различными источниками маркеров отмены;
- ```void Cancel()``` - инициирует отмену на выпущенных маркерах;
- ```void Cancel(bool throwOnFirstException)``` - инициирует отмену на выпущенных маркерах с параметром ```bool throwOnFirstException```: если он имеет значение **true**, то инициированная отмена будет предотвращать обработку оставшихся обратных вызовов и операций, которые можно отменить, если **false**, то эта перегрузка будет объединять любые исключения, созданные в <see cref="AggregateException"/>, так что один обратный вызов, вызывающий исключение, не будет препятствовать выполнению других зарегистрированных обратных вызовов;
- ```void CancelAfter(TimeSpan delay)``` - позволяет инициировать отмену на выпущенных токенах через временной интервал, представленный экземпляром структуры ```TimeSpan```;
- ```void CancelAfter(int millisecondsDelay)``` - позволяет инициировать отмену на выпущенных токенах через временной интервал, представленный целочисленным значением в миллисекундах.

## **04 Множественная скоординированная отмена с использованием метода CreateLinkedTokenSource()**
---
Метод ```CreateLinkedTokenSource()``` принимает в качестве аргумента набор из токенов отмены, которые могут быть выпущены различными источниками ```CancellationTokenSource``` и возвращает новый источник токенов отмены, токены которого будут получать запрос на отмену уже не только от источника, который их выпустил, но и от источников, чьи токены были переданы в метод ```CreateLinkedTokenSource()```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        CancellationTokenSource cts1 = new();
        CancellationTokenSource cts2 = new();

        CancellationToken ct1 = cts1.Token;
        CancellationToken ct2 = cts2.Token;

        CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct1, ct2);

        CancellationToken linkedToken = linkedTokenSource.Token;

        Task task1 = Task.Run(() => PrintIterations("AsyncTask1", linkedToken), linkedToken);
        Task task2 = Task.Run(() => PrintIterations("AsyncTask2", linkedToken), linkedToken);

        cts1.CancelAfter(500);

        try
        {
            task1.Wait();
        }
        catch (AggregateException ex)
        {
            ReportTaskExceptionDetails(ex, task1);
        }

        try
        {
            task2.Wait();
        }
        catch (AggregateException ex)
        {
            ReportTaskExceptionDetails(ex, task2);
        }

        Console.ReadKey();
    }

    private static void PrintIterations(string callName, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            ct.ThrowIfCancellationRequested();

            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportTaskExceptionDetails(AggregateException ex, Task task = null, string indent = null)
    {
        string details = string.Empty;

        if (task is not null)
        {
            if (task.IsCanceled)
            {
                details += $"[Exception][A Task was canceled]{Environment.NewLine}";
            }
            else if (task.IsFaulted)
            {
                details += $"[Exception][A Task was faulted]{Environment.NewLine}";
            }
        }

        details += $"Exception Type: [{ex.GetType().Name}]{Environment.NewLine}Exception Message: [{ex.Message}]{Environment.NewLine}{new string('-', 40)}";

        Console.WriteLine(details);

        foreach (var innerException in ex.InnerExceptions)
        {
            if (innerException is AggregateException aggregateException)
            {
                ReportTaskExceptionDetails(aggregateException, null, indent + "  ");
            }
            else
            {
                Console.WriteLine($"[InnerException] Exception Type: [{innerException.GetType().Name}]{Environment.NewLine}Exception Message: [{innerException.Message}]{Environment.NewLine}{new string('-', 40)}");
            }
        }
    }
}
```

### **Структура CancellationToken**

Представляет маркер отмены, на котором инициируется отмена, и который предоставляет необходимую инфраструктуру для выполнения адекватного завершения отменяемой операции.

### **Члены стурктуры ```CancellationToken```**

Конструкторы:
- ```CancellationToken(bool canceled)``` - конструктор, позволяющий создать токен отмены, и принимающий булевый аргумент в параметр ```bool canceled```, который инициализирует значения свойств ```bool CanBeCanceled``` и ```bool IsCancellationRequested``` соответствующим значением;

Свойства:
- ```(static) CancellationToken None``` - возвращает маркер отмены по умолчанию, аналогичный значению оператора ```default```, который может понадобиться, если необходимо вызвать метод, требующий передачи в один из параметров экземпляр структуры ```CancellationToken```, но логика программы не подразумевает инициации отмены;
- ```bool IsCancellationRequested``` - указывает значениями **true** или **false**, была ли запрошена отмена;
- ```bool CanBeCanceled``` - указывает значениями **true** или **false**,, может ли асинхронная операция быть отменена с использованием текущего маркера отмены. Например, если маркер отмены был получен из свойства ```None```, то значение этого свойства будет **false**;
- ```WaitHandle WaitHandle``` - возвращает объект синхронизации потоков, на котором можно вызвать метод ```WaitOne()```, который заблокирует вызвавший поток до того момента, пока не будет инициирована отмена. Если отмена не произойдёт, то заблокированный поток не будет разблокирован;

Методы:
- ```CancellationTokenRegistration Register(Action callback)``` - регестрирует экземпляр делегата ```Action``` как продолжение операции в случае отмены;
- ```CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext)``` - регестрирует экземпляр делегата ```Action``` как продолжение операции в случае отмены, с помощью флага ```bool useSynchronizationContext``` можно указать, нужно ли захватывать для выполнения продолжения контекст синхронизации: **true** - нужно, **false** - не нужно;
- ```CancellationTokenRegistration Register(Action<object> callback)``` - регестрирует экземпляр делегата ```Action<object>``` как продолжение операции в случае отмены;
- ```CancellationTokenRegistration Register(Action<object> callback, bool useSynchronizationContext)``` - регестрирует экземпляр делегата ```Action<object>``` как продолжение операции в случае отмены, с помощью флага ```bool useSynchronizationContext``` можно указать, нужно ли захватывать для выполнения продолжения контекст синхронизации: **true** - нужно, **false** - не нужно;
- ```void ThrowIfCancellationRequested``` - в месте вызова проверяет, была ли запрошена отмена. Если да, то возбуждает исключение ```OperationCanceledException```, если нет, то ничего не делает.

## **04 Продолжение скоординированной отмены с использованием метода Register()**
---
Метод ```Register()``` позволяет зарегистрировать продолжение задачи, которое будет запущено, если операция будет отменена. Если в продолжение необходимо передать параметры, то это можно сделать через перевызов его в контексте другого метода или лямбда-оператора с замыканием:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        CancellationTokenSource cts = new(500);

        CancellationToken ct = cts.Token;

        string callName = "AsyncTask";

        Task task = Task.Run(() => PrintIterations(callName, ct), ct);

        ct.Register(() => CancellationContinuation(task, callName));

        Console.ReadKey();
    }

    private static void PrintIterations(string callName, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            ct.ThrowIfCancellationRequested();

            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void CancellationContinuation(Task cancelledTask, string taskName)
    {
        Console.WriteLine($"Operation [{taskName}] was canceled. Task [{cancelledTask.Id}]. Method [{nameof(CancellationContinuation)}].");
    }

    private static void ReportTaskExceptionDetails(AggregateException ex, Task task = null, string indent = null)
    {
        string details = string.Empty;

        if (task is not null)
        {
            if (task.IsCanceled)
            {
                details += $"[Exception][A Task#{task.Id} was canceled]{Environment.NewLine}";
            }
            else if (task.IsFaulted)
            {
                details += $"[Exception][A Task#{task.Id} was faulted]{Environment.NewLine}";
            }
        }

        details += $"Exception Type: [{ex.GetType().Name}]{Environment.NewLine}Exception Message: [{ex.Message}]{Environment.NewLine}{new string('-', 40)}";

        Console.WriteLine(details);

        foreach (var innerException in ex.InnerExceptions)
        {
            if (innerException is AggregateException aggregateException)
            {
                ReportTaskExceptionDetails(aggregateException, null, indent + "  ");
            }
            else
            {
                Console.WriteLine($"[InnerException] Exception Type: [{innerException.GetType().Name}]{Environment.NewLine}Exception Message: [{innerException.Message}]{Environment.NewLine}{new string('-', 40)}");
            }
        }
    }
}
```

> TODO: 6. Coordinated Cancellation & Async Await

> TODO: 7. Coordinated Cancellation in Continuations via check of TaskStatus

> TODO: 8. Coordinated Cancellation of Continuations via CancellationToken as a parameter of ContinueWith() method

> TODO: 9. Continuation of Cancelled Operations via TaskContinuationOptions.LazyCancellation and TaskScheduler arguments as a parameters of ContinueWith() method

> TODO: 10. Continuation of Cancelled Operations via TaskContinuationOptions.OnlyOnCanceled argument as a parameter of ContinueWith() method
