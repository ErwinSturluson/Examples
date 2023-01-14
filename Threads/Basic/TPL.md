# **TPL (Task Parallel Library)**

**TPL (Task Parallel Library)** - **библиотека параллельных задач**, предназначенная для повышения производительности разработчиков за счёт упрощения процесса добавления параллелизма в приложения. 

**TPL в .NET** представлена пространством имён **System.Threading.Tasks**.

**TPL динамически масштабирует степень параллелизма** для наиболее эффективного использования всех доступных процессоров.

В **TPL** осуществляется секционирование работы, планирование потоков в пуле **ThreadPool**, поддержка отмены, управление состоянием и выполняются другие низкоуровневые задачи.

Используя библиотеку параллельных задач, можно **повысить производительность кода**, сосредоточившись на работе, для которой предназначена программа.

## **Класс Task**
---
Класс **Task** в **.NET** представляет собой задачу. Задача - **не паралельный поток** и не является аналогом Thread.

Задача является единицей работы, набором операций, которые необходимо выполнить. 

Задача **может выполниться как синхронно, так и асинхронно**. 

При выполнении асинхронно, задача **использует пул потоков** для выполнения **в контексте вторичного потока**.

Для выполнения метода в контексте задачи необходимо сообщить его с экземпляром делегата **Action** или **Func** и передать в конструктор класса **Task**, либо воспользоваться **техникой предположения делегата**.

Разница в использовании классов Task и Thread для асинхронного выполнения методов состоит в том, что класс Task обладает более широкой и удобной функциональностью, чем Thread, использует пул потоков вместо обычного запуска потока, а также обладает обширной вспомогательной инфрструктурой для асинхронного выполнения кода.

Для асинхронного запуска задачи используется метод **Start()**, для синхронного - **RunSynchronously()**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"{nameof(Main)} method has started in thread#{Environment.CurrentManagedThreadId}");

        Action printIterationsAction = new(PrintIterations);

        Task printIterationsTask1 = new(printIterationsAction);

        printIterationsTask1.Start();

        Task printIterationsTask2 = new(printIterationsAction);

        printIterationsTask2.RunSynchronously();

        PrintIterations();

        Console.WriteLine($"{nameof(PrintIterations)} method has finished in thread#{Environment.CurrentManagedThreadId}");
    }

    private static void PrintIterations()
    {
        Console.WriteLine($"{nameof(PrintIterations)} method has started in thread#{Environment.CurrentManagedThreadId}");

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(100);
        }

        Console.WriteLine($"{nameof(PrintIterations)} method has finished in thread#{Environment.CurrentManagedThreadId}");
    }
}
```

## **Статическое свойство CurrentId класса Task**
---
**Статическое свойство CurrentId класса Task** возвращает целочисленный идентификатор текущей задачи, в котором оно было прочитано. 

Метод Main не выполняется в контексте задачи, поэтому для него, и методов, которые были вызваны из него вне контекста задач, свойство вернёт null:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has started in thread#{Environment.CurrentManagedThreadId}");

        Action printIterationsAction = new(PrintIterations);

        Task printIterationsTask1 = new(printIterationsAction);

        printIterationsTask1.Start();

        Task printIterationsTask2 = new(printIterationsAction);

        printIterationsTask2.RunSynchronously();

        Task printIterationsTask3 = new(printIterationsAction);

        printIterationsTask3.Start();

        Task printIterationsTask4 = new(printIterationsAction);

        printIterationsTask4.RunSynchronously();

        PrintIterations();

        Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has finished in thread#{Environment.CurrentManagedThreadId}");
    }

    private static void PrintIterations()
    {
        Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has started in thread#{Environment.CurrentManagedThreadId}");

        int iterationNumber = 0;

        while (iterationNumber < 1)
        {
            iterationNumber++;

            Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId} [{iterationNumber}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has finished in thread#{Environment.CurrentManagedThreadId}");
    }
}
```

## **Свойство Status класса Task**
---
**Свойство Status класса Task** указывает на статус выполнения задачи, возвращая значение перечисления **TaskStatus**.

Задачи имеют множество статусов, 4 основных из них:
- **Created** - задача создана, но ещё не запущена;
- **WaitingToRun** - задача в процессе запуска;
- **Running** - задача выполняется;
- **RanToCompletion** - задача пришла к завершению.

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task printIterationsTask1 = new(PrintIterations);

        Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");

        printIterationsTask1.Start();

        Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");

        Thread.Sleep(500);

        Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");

        Thread.Sleep(1000);

        Console.WriteLine($"Task#{printIterationsTask1.Id} Status:{printIterationsTask1.Status}");
    }

    private static void PrintIterations()
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"Task#{Task.CurrentId.ToString() ?? "null"} - {iterationNumber}");
            Thread.Sleep(100);
        }
    }
}
```

## **Свойство IsBackground класса Thread, в котором выполняется задача**
---
По умолчанию потоки, в которых выполняются задачи, являются фоновыми и **свойство IsBackground класса Thread, в котором выполняется задача**, возвращает значение **true**, если задача запущена не синхронно из главного потока.

Поэтому, в случае завершения главного потока раньше выполняющихся задач, программа завершится, не дожидаясь завершения выполняющихся в этот момент задач. Чтобы это изменить, необходимо в коде метода, выполняющегося асинхронно в контексте задачи, присвоить свойству **IsBackground** класса **Thread** значение **false**.

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has started in thread#{Environment.CurrentManagedThreadId}");

        Task printIterationsTask1 = new(PrintIterations);

        printIterationsTask1.Start();

        System.Threading.Thread.Sleep(100);

        Console.WriteLine($"Method {nameof(Main)} in Task#{Task.CurrentId?.ToString() ?? "null"} has finished in thread#{Environment.CurrentManagedThreadId}");
    }

    private static void PrintIterations()
    {
        System.Threading.Thread.CurrentThread.IsBackground = false;

        Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has started in thread#{Environment.CurrentManagedThreadId}");

        int iterationNumber = 0;

        while (iterationNumber < 5)
        {
            iterationNumber++;

            Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId} [{iterationNumber}]");
            System.Threading.Thread.Sleep(100);
        }

        Console.WriteLine($"Method {nameof(PrintIterations)} in Task#{Task.CurrentId} has finished in thread#{Environment.CurrentManagedThreadId}");
    }
}
```

## **Передача аргументов в задачу**
---
**Передавать аргументы в задачу** можно во втором параметре конструктора класса **Task** - **object state**:  

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task printIterationsTask1 = new(PrintIterations, "AsyncTask");
        Task printIterationsTask2 = new(PrintIterations, "SyncTask");

        printIterationsTask1.Start();
        printIterationsTask2.RunSynchronously();

        Console.ReadKey();
    }

    private static void PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            System.Threading.Thread.Sleep(100);
        }
    }
}
```

## **Метод Wait() класса Task**
---
**Метод Wait() класса Task** используется для **синхронизации задач**. При этом выполнение, вызывающего метод **Wait()** потока блокируется до завершения ожидаемой задачи:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task printIterationsTask1 = new(PrintIterations, "AsyncTask");
        Task printIterationsTask2 = new(PrintIterations, "SyncTask1");

        printIterationsTask1.Start();

        printIterationsTask2.RunSynchronously();

        printIterationsTask1.Wait();

        Task printIterationsTask3 = new(PrintIterations, "SyncTask2");

        Console.WriteLine();

        printIterationsTask3.RunSynchronously();
    }

    private static void PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 5)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            System.Threading.Thread.Sleep(100);
        }
    }
}
```

## **Методы WaitAll() и WaitAny() класса Task**
---
**Методы WaitAll() и WaitAny() класса Task** предназначены для ожидания множества задач.

Метод **WaitAll()** блокирует вызывающий поток до тех пор, пока не будут выполнены все задачи, ссылки на экземпляры которых переданы в этот метод.

Метод **WaitAny()** блокирует вызывающий поток до тех пор, пока не будет выполнена хотя бы одна задача из списка задач, ссылки на экземпляры которых переданы в этот метод, после чего вызывающий поток будет разблокирован и продолжит выполнение.

Примеры множественного ожидания задач:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        int parallelCalculationsPerTask = 3;

        Task[] parallelCalculationTasks = new Task[3];

        for (int i = 1; i < parallelCalculationTasks.Length; i++)
        {
            parallelCalculationTasks[i] = new Task(PrintIterations, parallelCalculationsPerTask);
        }

        parallelCalculationTasks[0] = new Task(PrintIterations, 1);

        for (int i = 0; i < parallelCalculationTasks.Length; i++)
        {
            parallelCalculationTasks[i].Start();
        }

        Task.WaitAll(parallelCalculationTasks);

        Console.WriteLine();

        Task[] concurrentTasks = new Task[3];

        for (int i = 0; i < concurrentTasks.Length; i++)
        {
            int concurrentCalculationsPerTask = i + 1;

            concurrentTasks[i] = new Task(PrintIterations, concurrentCalculationsPerTask);
        }

        for (int i = 0; i < concurrentTasks.Length; i++)
        {
            concurrentTasks[i].Start();
        }

        Task.WaitAny(concurrentTasks);
    }

    private static void PrintIterations(object state)
    {
        int calculationsNumber = (int)state;

        int calculationIndex = 0;

        while (calculationIndex < calculationsNumber)
        {
            calculationIndex++;

            Console.WriteLine($"Task in Thread#{Environment.CurrentManagedThreadId} - [{calculationIndex}]");
            System.Threading.Thread.Sleep(100);
        }
    }
}
```

## **Фабрика задач (TaskFactory)**
---
**Класс TaskFactory** используется для порождения задач.

**Свойство Factory** класса **Task** ссылается на фабрику задач **TaskFactory**.

**Метод StartNew()** экземпляра класса **TaskFactory**, сообщённый, в который передан экземпляр делегата, сообщённый с методом, который нужно выполнить в контексте задачи, создаёт и запускает новую задачу.

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task1 = Task.Factory.StartNew(PrintIterations, "AsyncTask1");

        Task task2 = Task.Factory.StartNew((state) =>
        {
            string taskName = state.ToString();

            int iterationNumber = 0;

            while (iterationNumber < 5)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                System.Threading.Thread.Sleep(100);
            }
        }, "AsyncTask2");

        Task.WaitAll(task1, task2);
    }

    private static void PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 5)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            System.Threading.Thread.Sleep(100);
        }
    }
}
```

## **Метод продолжения (ContinuationAction)**
---
**С задачей можно сообщить метод продолжения**, вызвав на задаче метод **ContinueWith()**, и передав ему в качестве аргумента метод, сообщённый с делегатом **Action<Task>**.

При создании задачи в дополнительный параметре конструктора **TaskCreationOptions creationOptions** класса **Task** можно передать значение **TaskCreationOptions.RunContinuationsAsynchronously** для того, чтобы метод продолжения был выполнен асинхронно относительно самой задачи, с которой он сообщён:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task1 = new(PrintIterations, "AsyncTask1");
        Task task2 = new(PrintIterations, "AsyncTask2", TaskCreationOptions.RunContinuationsAsynchronously);

        task1.ContinueWith(PrintIterationsContinuationAction);
        task2.ContinueWith(PrintIterationsContinuationAction);

        task1.Start();
        task2.Start();

        Task.WaitAll(task1, task2);
    }

    private static void PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 5)
        {
            iterationNumber++;

            Console.WriteLine($"{taskName} - TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            System.Threading.Thread.Sleep(100);
        }
    }

    private static void PrintIterationsContinuationAction(Task task)
    {
        string taskName = task.AsyncState.ToString();

        Console.WriteLine($"{taskName} has finished in Thread#{Environment.CurrentManagedThreadId}");
    }
}
```

## **Передача нескольких аргументов в задачу**
---
**Чтобы передать несколько аргументов в задачу**, необходимо создать составной тип, который будет содержать все необходимые параметры в качестве полей и/или свойств, затем создать экземпляр этого типа, записать в его поля и/или свойства данные, которые нужно передать в задачу, и после этого преедать ссылку на этот экземпляр во второй параметр конструктора класса **Task** - **object state**:

```cs
namespace TPL._10_Task.ComplexArgument
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintIterationsArgs task1Args = new()
            {
                TaskName = "AsyncTask1",
                IterationsNumber = 5,
                IterationsDelay = 200
            };

            PrintIterationsArgs task2Args = new()
            {
                TaskName = "AsyncTask2",
                IterationsNumber = 10,
                IterationsDelay = 100
            };

            Task task1 = Task.Factory.StartNew(PrintIterations, task1Args);
            Task task2 = Task.Factory.StartNew(PrintIterations, task2Args);

            Task.WaitAll(task1, task2);
        }

        private static void PrintIterations(object state)
        {
            PrintIterationsArgs taskArgs = state as PrintIterationsArgs;

            int iterationIndex = 0;

            while (iterationIndex < taskArgs.IterationsNumber)
            {
                iterationIndex++;

                Console.WriteLine($"TaskName:{taskArgs.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(taskArgs.IterationsDelay);
            }
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public int IterationsNumber { get; set; }

        public int IterationsDelay { get; set; }
    }
}
```

## **Возврат значений из задачи**
---
**Возврат значений из задачи** осуществляется через обращение к свойству **Result** класса **Task**. 

При обращении к свойству **Result** класса **Task** вызывающий поток блокируется до завершения ожидаемой задачи.

Для того, чтобы вернуть значение из задачи, задача должна быть параметризирована параметром нужного типа возвращаемого значения.

В самом методе, который сообщается с экземпляром делегата **Func**, ссылка на который передаётся конструктор класса **Task**, для возврата значения при асинхронном выполнении достаточно просто вернуть значение с помощью ключевого слова **return**, так же, как и в случае синхронного выполнения:

```cs
namespace TPL._11_Task.ReturnValue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintIterationsArgs task1Args = new()
            {
                TaskName = "AsyncTask1",
                IterationsNumber = 5,
                IterationsDelay = 200
            };

            PrintIterationsArgs task2Args = new()
            {
                TaskName = "AsyncTask2",
                IterationsNumber = 10,
                IterationsDelay = 100
            };

            Task<string> task1 = new Task<string>(PrintIterations, task1Args);
            Task<string> task2 = Task<string>.Factory.StartNew(PrintIterations, task2Args);

            task1.Start();

            string task1Result = task1.Result;
            string task2Result = task2.Result;

            Console.WriteLine($"Task1 Result: {task1Result}");
            Console.WriteLine($"Task2 Result: {task2Result}");
        }

        private static string PrintIterations(object state)
        {
            PrintIterationsArgs taskArgs = state as PrintIterationsArgs;

            int iterationIndex = 0;

            while (iterationIndex < taskArgs.IterationsNumber)
            {
                iterationIndex++;

                Console.WriteLine($"{taskArgs.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(taskArgs.IterationsDelay);
            }

            string resultMessage = $"{taskArgs.TaskName} completed sucsessfully after {iterationIndex} iterations.";

            return resultMessage;
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public int IterationsNumber { get; set; }

        public int IterationsDelay { get; set; }
    }
}
```

## **Исключения в задачах**
---
При возникновении исключений в задачах, выполняемых асинхронно, исключение можно поймать только при вызове в родительском потоке метода **Wait()** или свойства **Result** на экземпляре класса **Task** ожидаемой задачи, которые блокируют вызывающий поток до завершения асинхронной задачи.

Выбрасываемое в местах вызова метода **Wait()** и обращения к свойству **Result** на на экземпляре класса **Task** ожидаемой задачи имеет тип **AggregateException**.

Экземпляр исключения, возникшего внутри асинхронной задачи, помещается в свойство **InnerException**.

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<string> task1 = new(PrintIterations, null);
        Task<string> task2 = new(PrintIterations, null);

        task1.Start();
        task2.Start();

        try
        {
            task1.Wait();

            string taskResult = task1.Result;

            Console.WriteLine($"Task1 Result: {taskResult}");
        }
        catch (AggregateException ex)
        {
            PrintExceprionReport(ex);
        }

        try
        {
            string taskResult = task2.Result;

            Console.WriteLine($"Task2 Result: {taskResult}");
        }
        catch (AggregateException ex)
        {
            PrintExceprionReport(ex);
        }
    }

    private static string PrintIterations(object state)
    {
        if (state is null) throw new ArgumentException("null", nameof(state));

        string taskName = state.ToString();

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($"{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        string resultMessage = $"{taskName} completed sucsessfully after {iterationIndex} iterations.";

        return resultMessage;
    }

    private static void PrintExceprionReport(AggregateException ex)
    {
        System.Exception taskException = ex.InnerException;

        string reportMessage =
            Environment.NewLine +
            new string('=', 43) +
            Environment.NewLine +
            $"Exception Type: {taskException.GetType().Name}" +
            Environment.NewLine +
            $"Exception Message: {taskException.Message}" +
            Environment.NewLine +
            new string('=', 43);

        Console.WriteLine(reportMessage);
    }
}
```

## **Отмена задач**
---
Для отмены задач используются токены отмены, представленные экземплярами структуры **CancellationToken**, создаваемые экземплярами скласса **CancellationTokenSource**.

Для инициации отмены задачи через токен, необходимо вызвать метод **Cancel()** на экземпляре класса **CancellationTokenSource**, который создал этот токен.

Существует 2 способа отменить выполнение задачи:
- Преждевременно завершить метод, обратившись внутри асинхронной задачи к свойству **IsCancellationRequested** или вызвав метод **ThrowIfCancellationRequested** на экземпляра структуры **CancellationToken**, если этот экземпляр был передан вызывающим кодом в качестве аргумента задачи;
- Удалить задачу из очереди на выполнение в планировщике задач. Это может произойти в том случае, если токен отмены был передан в параметр соответствующего типа в конструктор класса **Task**, и отмена произошла до того,как планировщик задач отправил задачу на выполнение.

Пример отмены задачи:

```cs
namespace TPL._13_Task.Cancellation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts = new();

            CancellationToken ct = cts.Token;

            PrintIterationsArgs taskArgs = new()
            {
                TaskName = "AsyncTask1",
                CancellationToken = ct
            };

            Task task = new(PrintIterations, taskArgs, ct);

            task.Start();

            Thread.Sleep(500);

            cts.Cancel();

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                PrintExceprionReport(ex);
            }
        }

        private static void PrintIterations(object state)
        {
            if (state is null) throw new ArgumentException("null", nameof(state));

            PrintIterationsArgs args = (PrintIterationsArgs)state;

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                args.CancellationToken.ThrowIfCancellationRequested();

                iterationIndex++;

                Console.WriteLine($"{args.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }
        }

        private static void PrintExceprionReport(AggregateException ex)
        {
            System.Exception taskException = ex.InnerException;

            string reportMessage =
                Environment.NewLine +
                new string('=', 43) +
                Environment.NewLine +
                $"Exception Type: {taskException.GetType().Name}" +
                Environment.NewLine +
                $"Exception Message: {taskException.Message}" +
                Environment.NewLine +
                new string('=', 43);

            Console.WriteLine(reportMessage);
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
}
```

## **Опции выполнения метода продолжения**
---
Методы продолжения могут быть настроены для выполнения при определённых условиях, таких, как, например, успешное или неудачное завершение задачи:

```cs
namespace TPL._14_Task.ContinuationOptions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintIterationsArgs task2Args = new()
            {
                TaskName = "AsyncTask2",
                CancellationToken = null
            };

            Task<int> task1 = new(CalculateIterations, null);
            Task<int> task2 = new(CalculateIterations, task2Args);

            task1.ContinueWith(OnFaultedContinuation, TaskContinuationOptions.OnlyOnFaulted);
            task2.ContinueWith(OnRanToCompletionContinuation, TaskContinuationOptions.OnlyOnRanToCompletion);

            task1.Start();
            task2.Start();

            Console.ReadKey();
        }

        private static int CalculateIterations(object state)
        {
            if (state is null) throw new ArgumentException("null", nameof(state));

            PrintIterationsArgs args = (PrintIterationsArgs)state;

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                args.CancellationToken?.ThrowIfCancellationRequested();

                iterationIndex++;

                Console.WriteLine($"{args.TaskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int result = iterationIndex * 100;

            return result;
        }

        private static void OnRanToCompletionContinuation(Task task)
        {
            Task<int> castedTask = task as Task<int>;

            int taskResult = castedTask.Result;

            string reportMessage =
                Environment.NewLine +
                new string('=', 43) +
                Environment.NewLine +
                $"Task result: {taskResult}" +
                Environment.NewLine +
                new string('=', 43);

            Console.WriteLine(reportMessage);
        }

        private static void OnFaultedContinuation(Task task)
        {
            Exception taskException = task.Exception.InnerException;

            string reportMessage =
                Environment.NewLine +
                new string('=', 43) +
                Environment.NewLine +
                $"Exception Type: {taskException.GetType().Name}" +
                Environment.NewLine +
                $"Exception Message: {taskException.Message}" +
                Environment.NewLine +
                new string('=', 43);

            Console.WriteLine(reportMessage);
        }
    }

    internal class PrintIterationsArgs
    {
        public string TaskName { get; set; }

        public CancellationToken? CancellationToken { get; set; }
    }
}
```

## **Планировщик задач (TaskScheduler)**
---
**Планировщик задач (TaskScheduler)** - TODO: create an example of limited parallelism TaskScheduler

```cs

```

## **Метод Run класса Task**
---
**Статический метод Run класса Task** используется для быстрого создания и запуска задач. 

При таком создании и запуске задач не могут быть переданы аргументы, и выполниться в контексте таких задач могут только методы, сигнатура которых соответствует классам-делегатам **Action** и **Func<T>**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task1 = Task.Run(PrintIterations);

        Task task2 = Task.Run(() =>
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskId:{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }
        });

        Task.WaitAll(task1, task2);
    }

    private static void PrintIterations()
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskId:{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(100);
        }
    }
}
```

## **Метод Invoke() класса Parallel**
---
**Метод Invoke() класса Parallel** параллельно запускает набор задач, переданный ему в качестве аргумента. На время выполнения этих задач вызывающий поток блокируется.

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Parallel.Invoke(PrintIterations, PrintIterations, PrintIterations);
    }

    private static void PrintIterations()
    {
        int iterationNumber = 0;

        while (iterationNumber < 3)
        {
            iterationNumber++;

            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(200);
        }
    }
}
```

## **Метод For() класса Parallel**
---
**Метод For() класса Parallel** позволяет выполнять цикл for параллельно. В массивных вычислениях, кторые могут быть разделены на части для выполнения параллельно, **метод For() класса Parallel** может привести к значительному приросту производительности. На время выполнения параллеького цикла **For** вызывающий поток блокируется.

Рассмотрим искусственный пример, где производятся вычисления, стилизованные под отрисовку видеокадров экрана, а также фиксируется время выполнения схожей операции последовательно и параллельно:

```cs
namespace TPL._18_Parallel.For
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopwatch = new();

            Frame[] frames = Renderer.MakeEmptyFrames(60, 3820, 2160);

            Console.WriteLine($"Frames rendering has started sequentially...");

            stopwatch.Start();

            for (int i = 0; i < frames.Length; i++)
            {
                Renderer.RenderFrame(frames[i]);
            }

            Console.WriteLine($"Frames rendering has finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");

            stopwatch.Reset();

            Console.WriteLine($"Frames rendering has started parallel...");

            stopwatch.Start();

            Parallel.For(0, frames.Length, (i) => Renderer.RenderFrame(frames[i]));

            Console.WriteLine($"Frames rendering has finished parallel after {stopwatch.ElapsedMilliseconds} milliseconds.");

            stopwatch.Stop();
        }
    }

    internal struct Frame
    {
        public int Width { get; }

        public int Height { get; }

        public Pixel[] Pixels { get; }

        public Frame(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Pixel[Width * Height];
        }
    }

    internal struct Pixel
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }

    internal static class Renderer
    {
        internal static void RenderFrame(Frame frame)
        {
            Pixel[] framePixels = frame.Pixels;

            for (int i = 0; i < framePixels.Length; i++)
            {
                RenderPixel(framePixels[i], i);
            }
        }

        internal static void RenderPixel(Pixel pixel, int index)
        {
            byte coefficient = (byte)(index % byte.MaxValue);

            pixel.R = (byte)(byte.MaxValue - coefficient);
            pixel.G = (byte)(pixel.R - coefficient);
            pixel.B = (byte)(pixel.G - coefficient);
            pixel.A = (byte)(pixel.B - coefficient);
        }

        internal static Frame[] MakeEmptyFrames(int number, int resWidth, int resHeight) =>
            Enumerable.Range(0, number)
            .Select(i => new Frame(resWidth, resHeight))
            .ToArray();
    }
}
```

**Метод For() класса Parallel** на многоядерных компьютерах выполняется значительно быстрее, чем обычный цикл **for**.

## **ParallelLoopState и ParallelLoopResult в контексте метода For() класса Parallel**

Для **управления параллельным циклом For** и **обработки результата его выполнения** можно использовать экземпляры класса **ParallelLoopState** и структуры **ParallelLoopResult**.

Экзмепляр **ParallelLoopState** создаётся автоматически внутри метода **For** и передаётся в каждую итерацию, если экземпляр делегата, с которым сообщён метод для выполнения в цикле, вторым параметром имеет параметр типа **ParallelLoopState**. Экземпляр **ParallelLoopState** можно использовать, например, для остановки выполнения текущей и последующих итераций цикла через вызов на нём метода **Break()**.

Обработать результат выполнения параллельного цикла **For** можно, получив из него возвращаемое значение в виде экземпляра структуры **ParallelLoopResult**. Экземпляр **ParallelLoopResult** можно использовать, например, для выяснения, был ли завершён параллельный цикл **For** и все его итерации успешно через его свойство **IsCompleted**, и, если нет, то через свойство **LowestBreakIteration** можно получить индекс итерации, на которой его выполнение было прервано.

Рассмотрим искусственный пример, где производятся вычисления, стилизованные под отрисовку видеокадров экрана, при этом, механизм отрисовки кадров не позволяет отрисовать более 30 кадров подряд и прерывает параллельный цикл **For**, если итераций больше: 

```cs
namespace TPL._19_Parallel.For.ParallelLoopState_ParallelLoopResult
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Frame[] frames = Renderer.MakeEmptyFrames(60, 3820, 2160);

            Action<int, ParallelLoopState> loopAction = (i, loopState) =>
            {
                if (i > 30)
                {
                    loopState.Break();
                }

                Renderer.RenderFrame(frames[i]);
            };

            Console.WriteLine($"Frames rendering has started...");

            ParallelLoopResult loopResult = Parallel.For(0, frames.Length, loopAction);

            if (loopResult.IsCompleted)
            {
                Console.WriteLine($"Frames rendering has finished succesfully.");
            }
            else
            {
                Console.WriteLine($"Frames rendering has failed on [{loopResult.LowestBreakIteration}] iteration.");
            }
        }
    }

    internal struct Frame
    {
        public int Width { get; }

        public int Height { get; }

        public Pixel[] Pixels { get; }

        public Frame(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Pixel[Width * Height];
        }
    }

    internal struct Pixel
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }

    internal static class Renderer
    {
        internal static void RenderFrame(Frame frame)
        {
            Pixel[] framePixels = frame.Pixels;

            for (int i = 0; i < framePixels.Length; i++)
            {
                RenderPixel(framePixels[i], i);
            }
        }

        internal static void RenderPixel(Pixel pixel, int index)
        {
            byte coefficient = (byte)(index % byte.MaxValue);

            pixel.R = (byte)(byte.MaxValue - coefficient);
            pixel.G = (byte)(pixel.R - coefficient);
            pixel.B = (byte)(pixel.G - coefficient);
            pixel.A = (byte)(pixel.B - coefficient);
        }

        internal static Frame[] MakeEmptyFrames(int number, int resWidth, int resHeight) =>
            Enumerable.Range(0, number)
            .Select(i => new Frame(resWidth, resHeight))
            .ToArray();
    }
}
```

## **Метод ForEach() класса Parallel**
---
**Метод ForEach() класса Parallel** работает аналогично методу **For()**, однако, предназначен для перебора коллекций. Также выполнение цикла возможно отменить через экземпляр токена отмены **CancellationToken**, тогда исключение **OperationCanceledException** возникнет в месте ожидания завершения параллельного цикла заблокированным вызывающим кодом:

```cs
namespace TPL._20_Parallel.ForEach
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Frame[] frames = Renderer.MakeEmptyFrames(60, 3820, 2160);

            Action<Frame, ParallelLoopState> loopAction = (frame, loopState) =>
            {
                Renderer.RenderFrame(frame);
            };

            Console.WriteLine($"Frames rendering has started...");

            CancellationTokenSource cts = new();

            ParallelOptions loopOptions = new()
            {
                CancellationToken = cts.Token
            };

            cts.CancelAfter(TimeSpan.FromMilliseconds(100));

            ParallelLoopResult loopResult;

            try
            {
                loopResult = Parallel.ForEach(frames, loopOptions, loopAction);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Frames rendering has failed due to operation was canceled.");
                throw;
            }

            if (loopResult.IsCompleted)
            {
                Console.WriteLine($"Frames rendering has finished succesfully.");
            }
            else
            {
                Console.WriteLine($"Frames rendering has failed on [{loopResult.LowestBreakIteration}] iteration.");
            }
        }
    }

    internal struct Frame
    {
        public int Width { get; }

        public int Height { get; }

        public Pixel[] Pixels { get; }

        public Frame(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Pixel[Width * Height];
        }
    }

    internal struct Pixel
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }

    internal static class Renderer
    {
        internal static void RenderFrame(Frame frame)
        {
            Pixel[] framePixels = frame.Pixels;

            for (int i = 0; i < framePixels.Length; i++)
            {
                RenderPixel(framePixels[i], i);
            }
        }

        internal static void RenderPixel(Pixel pixel, int index)
        {
            byte coefficient = (byte)(index % byte.MaxValue);

            pixel.R = (byte)(byte.MaxValue - coefficient);
            pixel.G = (byte)(pixel.R - coefficient);
            pixel.B = (byte)(pixel.G - coefficient);
            pixel.A = (byte)(pixel.B - coefficient);
        }

        internal static Frame[] MakeEmptyFrames(int number, int resWidth, int resHeight) =>
            Enumerable.Range(0, number)
            .Select(i => new Frame(resWidth, resHeight))
            .ToArray();
    }
}
```

## **PLinq, метод AsParallel()**
---
Параллельные запросы **PLinq** представлены классом **ParallelEnumerable**, который определён в пространстве имён **System.Linq**.

Для того, чтобы **Linq** запрос выполнялся параллельно, необходимо в **цепочке Linq-запроса** до его выполнения вызвать метод **AsParallel()**:

```cs
namespace TPL._20_PLinq.AsParallel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<Document> documentArchive = Enumerable.Range(0, 100_000)
                .Select(i => new Document(i, $"Document #{i + 1} by {DateTime.UtcNow}"));

            ParallelQuery<Document> filteredDocs = from doc in documentArchive.AsParallel()
                                                   where doc.Id > 50_000
                                                   select doc;

            List<Document> documents = filteredDocs.ToList();

            if (documents.Count != 0)
            {
                Console.WriteLine($"First document Name:{documents.First().Name}");
                Console.WriteLine($"Last document Name:{documents.Last().Name}");
            }
            else
            {
                Console.WriteLine($"There are not any documents for those conditions.");
            }
        }
    }

    internal readonly struct Document
    {
        public int Id { get; }

        public string Name { get; }

        public Document(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
```
При этом, элементы в результат выборки записываются в случайной последовательности, а не в той, в которой были в изначальном наборе.

## **PLinq, метод AsOrdered()**
---
Для того, чтобы **Linq** запрос выполнялся параллельно и результаты выборки располагались в исходной последовательсности, необходимо после вызова метода **AsParallel()** вызвать метод **AsOrdered()**:

```cs
namespace TPL._21_PLinq.AsParallel_AsOrdered
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<Document> documentArchive = Enumerable.Range(0, 100_000)
                .Select(i => new Document(i, $"Document #{i + 1} by {DateTime.UtcNow}"));

            ParallelQuery<Document> filteredDocs = from doc in documentArchive.AsParallel().AsOrdered()
                                                   where doc.Id > 50_000
                                                   select doc;

            List<Document> documents = filteredDocs.ToList();

            if (documents.Count != 0)
            {
                Console.WriteLine($"First document Name:{documents.First().Name}");
                Console.WriteLine($"Last document Name:{documents.Last().Name}");
            }
            else
            {
                Console.WriteLine($"There are not any documents for those conditions.");
            }
        }
    }

    internal readonly struct Document
    {
        public int Id { get; }

        public string Name { get; }

        public Document(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
```

## **PLinq, метод WithCancellation()**
---
Для того, чтобы отменить параллельный запрос **Linq**, необходимо после вызова метода **AsParallel()** вызвать метод **WithCancellation()**:

```cs
namespace TPL._22_PLinq.AsParallel_WithCancellation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<Document> documentArchive = Enumerable.Range(0, 100_000)
                .Select(i => new Document(i, $"Document #{i + 1} by {DateTime.UtcNow}"));

            CancellationTokenSource cts = new();

            ParallelQuery<Document> filteredDocs = from doc in documentArchive
                                                   .AsParallel()
                                                   .AsOrdered()
                                                   .WithCancellation(cts.Token)
                                                   where doc.Id > 50_000
                                                   select doc;

            cts.CancelAfter(100);

            List<Document> documents;

            try
            {
                documents = filteredDocs.ToList();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Document searching has failed due to operation was canceled.");
                throw;
            }

            if (documents.Count != 0)
            {
                Console.WriteLine($"First document Name:{documents.First().Name}");
                Console.WriteLine($"Last document Name:{documents.Last().Name}");
            }
            else
            {
                Console.WriteLine($"There are not any documents for those conditions.");
            }
        }
    }

    internal readonly struct Document
    {
        public int Id { get; }

        public string Name { get; }

        public Document(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
```