# **Parallel Programming**

**Параллельное программирование** - это подход, при котором вычисление разбивается на части, выполняемые параллельно с цельу улучшения производитьельности системы.

**.NET** предлагает вам несколько стандартных решений для распараллеливания кода:
- Типы ```System.Threading.Tasks.Task``` и ```System.Threading.Tasks.Task<TResult>```;
- Тип ```System.Threading.Tasks.Parallel```;
- Параллельный ```LINQ (PLINQ)```, в виде типов ```System.Linq.ParallelEnumerable``` и
```System.Linq.ParallelQuery<T>```.

### **Класс ```Parallel```**

Класс ```Parallel``` – это класс, который упрощает параллельное выполнение кода. Благодаря его методам можно быстро распараллелить вычислительный процесс.

Класс ```Parallel``` использует внутри себя механизмы задач, представленных классом ```Task``` и забирает необходимость по написанию большого количества кода по созданию, настройке и поддержке работы этих задач, которые распараллеливают выполнение, вместо чего просто вызывается 1 метод.

Методы класса ```Parallel``` блокирующие, то есть

У класса ```Parallel``` доступно всего 3 метода для параллельной обработки:

- ```Invoke``` – параллельное выполнение делегатов ```Action```;
- ```For``` – параллельное выполнение итераций;
- ```ForEach``` – параллельный перебор коллекций.

Для параллельныхъ циклов не были добавлены отдельные ключенвые слова, так как большинство разработчиков могут вообще не пользоваться параллельными циклами.

При этом, параллельное выполнение этих методов можно настраивать с помощью выбора перегрузки, которая принимает в качестве параметров класс ```ParallelOptions```.

### **Параллельное выполнение класса ```Parallel```**

Класс ```Parallel``` пытается выполнить параллельный вызов, итерирование или перебор с помощью наименьшего количества задач, необходимых для максимально быстрого завершения, при этом учитывая ваши ограничения по распараллеливанию (```MaxDegreeOfParallelism```). Для этого класс ```Parallel``` использует самореплицирующиеся задачи **(Self-Replicating Tasks)**.

**Реплицируемая задача** - это задача, делегат которой может быть выполнен несколькими потоками одновременно. На количество саморепликаций задачи влияет указание максимального значения по распараллеливанию (```MaxDegreeOfParallelism```). Если оно указано, то система не будет превышать число реплицирований задачи более чем значение ```MaxDegreeOfParallelism```.

Именно самореплицирующиеся задачи используются параллельными циклами ```For()``` и ```ForEach()```. 

Метод ```Invoke()``` использует самореплицирующиеся задачи, если количество делегатов на выполнение превышает **10** единиц или если указана настройка максимального распараллеливания.

## **01 Метод ```Invoke()``` класса ```Parallel```**
---
Метод ```Invoke()``` класса ```Parallel``` предназначен для параллельного выполнения произвольного количества любых методов, сообщённых с экземплярами класса-делегата ```Action```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Stopwatch stopwatch = new();
        Console.WriteLine($"Operations have started sequentially...");
        stopwatch.Start();

        PrintOperation();
        PrintOperation();
        PrintOperation();

        Console.WriteLine($"Operations have finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");
        stopwatch.Restart();
        Console.WriteLine($"Operations have started parallel...");

        Parallel.Invoke(PrintOperation, PrintOperation, PrintOperation);

        Console.WriteLine($"Iterations have finished parallel after {stopwatch.ElapsedMilliseconds} milliseconds.");
        stopwatch.Stop();
    }

    private static void PrintOperation()
    {
        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(200);
        }
    }
}
```

Его преимущество состоит в том, что он позволяет быстро и просто запрограммировать параллельное выполнение нескольких задач и их синхронное ожидание.

## **02 Метод ```For()``` класса ```Parallel```**
---
Метод ```For()``` класса ```Parallel``` предназначен для параллельного выполнения установленного количества итераций, представленных методами или лямбда-операторами, сообщёнными с экземплярами класса-делегата ```Action<int>```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Stopwatch stopwatch = new();
        Console.WriteLine($"Iterations have started sequentially...");
        stopwatch.Start();

        for (int i = 0; i < 10; i++)
        {
            PrintIteration(i);
        }

        Console.WriteLine($"Iterations have finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");
        stopwatch.Restart();
        Console.WriteLine($"Iterations have started parallel...");

        Parallel.For(1, 10, PrintIteration);

        Console.WriteLine($"Iterations have finished parallel after {stopwatch.ElapsedMilliseconds} milliseconds.");
        stopwatch.Stop();
    }

    private static void PrintIteration(int iterationIndex)
    {
        Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
        Thread.Sleep(100);
    }
}
```

Его преимущество состоит в том, что он позволяет быстро и просто запрограммировать параллельное выполнение повторяющихся итераций, что увеличивает производительность в сравнении с обработкой в последовательном цикле ```for```.


## **03 Метод ```Foreach()``` класса ```Parallel```**
---
Метод ```For()``` класса ```Parallel``` предназначен для параллельного перебора коллекций ```IEnumerable<T>```, действиями, представленными методами или лямбда-операторами, сообщёнными с экземплярами класса-делегата ```Action<T>```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        IEnumerable<int> collection = Enumerable.Range(1, 10);

        Stopwatch stopwatch = new();
        Console.WriteLine($"Iterations have started sequentially...");
        stopwatch.Start();

        foreach (var item in collection)
        {
            PrintIteration(item);
        }

        Console.WriteLine($"Iterations have finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");
        stopwatch.Restart();
        Console.WriteLine($"Iterations have started parallel...");

        Parallel.ForEach(collection, PrintIteration);

        Console.WriteLine($"Iterations have finished parallel after {stopwatch.ElapsedMilliseconds} milliseconds.");
        stopwatch.Stop();
    }

    private static void PrintIteration(int iterationIndex)
    {
        Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
        Thread.Sleep(100);
    }
}
```

Его преимущество состоит в том, что он позволяет быстро и просто запрограммировать параллельное выполнение перебора коллекций, что увеличивает производительность в сравнении с обработкой в последовательном цикле ```foreach```.

### **Класс ```ParallelOptions```**

С помощью класса ```ParallelOptions``` можно настроить выполнение параллельных методов. Для этого необходимо создать экземпляр класса ```ParallelOptions``` и задать необходимые параметры.

Класс ```ParallelOptions``` имеет 3 различных настройки:

- ```CancellationToken``` – указание CancellationToken для возможности отмены выполнения параллельных операций;
- ```MaxDegreeOfParallelism```– возможность для установки или получения максимального количества одновременных задач;
- ```TaskScheduler``` – установка своего планировщика задач для параллельного выполнения.

## **04 Свойство ```CancellationToken``` класса ```ParallelOptions```**
---
При помощи свойство ```CancellationToken``` класса ```ParallelOptions``` можно осуществить отмену выполнения параллельного цикла. При этом, будет возбуждено исключение ```OperationCanceledException```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        CancellationTokenSource cts = new();

        ParallelOptions options = new()
        {
            CancellationToken = cts.Token
        };

        cts.CancelAfter(50);

        try
        {
            Parallel.For(1, 20, options, PrintIteration);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"{Environment.NewLine}[Parallel.For() method was canceled]");
        }
    }

    private static void PrintIteration(int iterationIndex)
    {
        Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
        Thread.Sleep(200);
    }
}
```

## **05 Свойство ```MaxDegreeOfParallelism``` класса ```ParallelOptions```**
---
При помощи свойство ```CancellationToken``` класса ```ParallelOptions``` можно установить максимально допустимое количество параллельно выполняемых задач итераций:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = 4
        };

        Parallel.For(1, 10, options, PrintIteration);
    }

    private static void PrintIteration(int iterationIndex)
    {
        Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
        Thread.Sleep(500);
    }
}
```

## **06 Свойство ```TaskScheduler``` класса ```ParallelOptions```**
---
При помощи свойство ```TaskScheduler``` класса ```ParallelOptions``` можно установить планироващика задач, который будет использоваться для планирования и запуска задач итераций:

```cs
namespace ParallelProgramming._06_ParallelOptions.TaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ParallelOptions options = new()
            {
                TaskScheduler = new ParallelTaskScheduler()
            };

            Parallel.For(1, 10, options, PrintIteration);
        }

        private static void PrintIteration(int iterationIndex)
        {
            Console.WriteLine($">TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(500);
        }
    }

    internal class ParallelTaskScheduler : System.Threading.Tasks.TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks() => null;

        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"=[Task#{task.Id}] is processing through [{nameof(ParallelTaskScheduler)}.{nameof(QueueTask)}].");

            ThreadPool.QueueUserWorkItem((_) =>
            {
                TryExecuteTask(task);
            });
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"-[Task#{task.Id}] is processing through [{nameof(ParallelTaskScheduler)}.{nameof(TryExecuteTaskInline)}].");
            return TryExecuteTask(task);
        }
    }
}
```

### **Класс ```ParallelLoopState```**

Класс ```ParallelLoopState``` позволяет отдельным параллельным итерациям параллельных циклов взаимодействовать друг с другом, поскольку методы ```Parallel.For()``` и ```Parallel.ForEach()``` отдельные итерации выполняют параллельно.

Методы:
- ```Break()``` – предотвращает выполнение любых итераций с индексом, превышающим текущий. Не влияет на выполняющиеся итерации.
- ```Stop()``` – заканчивает текущую итерацию и запрещает запуск дополнительных итераций как только это возможно. Не влияет на выполняющиеся итерации.
Свойства:
- ```ShouldExitCurrentIteration``` – определяет, вызывала ли какая-то из итераций метод Stop, Break или было выброшено исключение;
- ```IsExceptional``` – определяет, произошло ли исключение в какой-то из итераций цикла;
- ```IsStopped``` – определяет, вызывала ли какая-то итерация цикла метод ```Stop()```;
- ```LowestBreakIteration``` – отдает индекс итерации, в которой был вызван ```Break()```.

## **07 Класс ```ParallelLoopState```**
---
Пример использования класса ```ParallelLoopState```, где индекс операции извлекается с помощью экземпляра этого класса, и для всех индексов больше **10** операция прерывается:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Action<int, System.Threading.Tasks.ParallelLoopState> loopAction = (i, loopState) =>
        {
            if (i > 10)
            {
                loopState.Break();
                Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{i}] - [BROKEN]");
                return;
            }

            PrintIteration(i);
        };

        Parallel.For(0, 15, loopAction);
    }

    private static void PrintIteration(int iterationIndex)
    {
        Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
        Thread.Sleep(500);
    }
}
```

### **08 Структура ```ParallelLoopResult```**

Структура ```ParallelLoopResult``` предоставляет возможность просмотреть статус выполнения параллельного цикла. Все перегрузки методов ```Parallel.For()``` и ```Parallel.ForEach()``` возвращают после своего выполнения экземпляр структуры ```ParallelLoopResult```.

Свойства:
- ```IsCompleted``` – возвращает значение **true**, если все параллельные итерации были выполнены, **false** – если параллельная обработка была нарушена вызовом метода ```Stop()```, Break или произошло исключение.
- ```LowestBreakIteration``` – возвращает значение **null**, если для остановки цикла был вызван метод ```Stop()```. Возвращает целочисленное значение, если для завершения цикла был использован метод ```Break()```.

## **Структура ```ParallelLoopResult```**
---
Пример использования структуры ```ParallelLoopResult```, где индекс операции извлекается с помощью экземпляра класса ```ParallelLoopState```, и для всех индексов больше **10** операция прерывается, а после завершения работы цикла с помощью экземпляра структуры ```ParallelLoopResult``` извлекается и отображается информация о завершённости цикла и, в случае его незавершённости, о самой нижней итерации, на которой был прерван цикл:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Action<int, ParallelLoopState> loopAction = (i, loopState) =>
        {
            if (i > 10)
            {
                loopState.Break();
                Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{i}] - [BROKEN]");
                return;
            }

            PrintIteration(i);
        };

        System.Threading.Tasks.ParallelLoopResult loopResult = Parallel.For(0, 15, loopAction);

        if (loopResult.IsCompleted)
        {
            Console.WriteLine($"{Environment.NewLine}All the Iterations have finished successfully.");
        }
        else
        {
            Console.WriteLine($"{Environment.NewLine}Iterations have failed on [{loopResult.LowestBreakIteration}] iteration.");
        }
    }

    private static void PrintIteration(int iterationIndex)
    {
        Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
        Thread.Sleep(500);
    }
}
```

## **09 Свойство ```ProcessorCount``` класса ```Environment```**
---
Для удобного и быстрого получения информации о количестве процессоров (или ядер в процессоре) на текущей машине и выставления оптимального для текущей машины уровня параллелизма, можно воспользоваться свойством ```ProcessorCount``` класса ```Environment```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"Processor Count of the Current Machine is [{Environment.ProcessorCount}].");

        ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount - 1
        };

        Parallel.For(1, 50, options, PrintIteration);
    }

    private static void PrintIteration(int iterationIndex)
    {
        Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
        Thread.Sleep(500);
    }
}
```

Уровень параллелизма был установлен в значение, на 1 меньше количества процессоров текущей машины для того, чтобы другие потоки и процессы при любой нагрузке параллельными вычислениями всегда могли воспользоваться оставшимся незанятым параллельным циклом ядром.

## **10 Исключения в методе ```Invoke()```**
---
Исключение, возникшее в одном из делегатов, выполняемого методом ```Invoke()```, не прервет работу других делегатов или потоков. Делегат, в котором произошло исключение, прервет свою работу. Все исключения, возникшие в делегатах, собираются и помещаются в исключение ```AggregateException```.

Оно будет выброшено через вызов метод ```Invoke()```.

Для обработки исключений метода ```Invoke()``` необходимо помещать его вызов в тело блока ```try``` конструкции ```try/catch```:

```
> TODO: Example with an Exception inside Parallel.Invoke() method
```

## **11 Исключения в параллельных циклах**
---
Исключение в одной из итераций параллельного цикла приводит к полному прерыванию работы всего цикла. Все исключения из итераций собираются и помещаются в исключение ```AggregateException```. 

Оно будет выброшено через вызов метода ```For()``` или ```ForEach()```.

Для обработки исключений параллельных циклов необходимо помещать вызов метода ```For( ``` или ```ForEach()``` в тело блока ```try``` конструкции ```try/catch```:

```
> TODO: Example with an Exception inside Parallel.For() and Parallel.ForEach() methods
```

## **12 Неблокирующий вызов методов класса ```Parallel```**
---
Для того, чтобы организовать неблокирующий вызов методов класса ```Parallel```, необходимо поместить этот вызов во вторичный поток, задачу или асинхронный метод:

```
> TODO: Example with asynchronous execution of Parallel class methods
```
