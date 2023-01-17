# **TAP (Task-Based Asynchronous Pattern)**

---
1) Библиотека TPL;
2) Шаблон TAP;
3) Задача (Task);
4) Фабрика задач (Task Factory);
5) Продолжения задач (Task Continuations);
6) Значимая задача (ValueTask);
7) Task vs ValueTask.
---

**TAP (Task-Based Asynchronous Pattern)** - шаблон асинхронного программирования, основанный на задачах.

**TAP** реализован в **.NET** в качестве библиотеки **TPL**.

**Библиотека TPL** - это набор открытых типов и интерфейсов в пространствах имён **System.Threading** и **System.Threading.Tasks**.

**System.Threading** содержит типы, представляющие собой потоки, пул потоков, объекты синхронизации доступа и объекты отмены выполнения.

**System.Threading.Tasks** содержит типы, реализующие асинхронные шаблоны, основанные на задачах.

Для работы с шаблоном TAP используется пространство имён **System.Threading.Tasks**.

**TAP** основан на типах **Task** и **Task<TResult>**. Сейчяас это самый популярный и рекомендованный **Microsoft** шаблон для разработки новых компонентов, работающих с асинхронными задачами.

**Преимущества TAP**:
- Простая инициализация и завершение асинхронной операции - для представления инициализации и завершения асинхронной операцуии используется только 1 метод, когда старые асинхронные шаблоны потребляли более 1 метода. Например, для шаблона APM (Asynchronous Programming Model) требовалось 2 метода: BeginXXX и EndXXX, а для шаблона EAP (Event-Based Asynchronous Pattern) - были необходимы: асинхроннаяч операция, событие, подписчики на событие;
- Удобный способ получения возвращаемого значения операции. Для этого тип Task<TResult> имеет свойство Result, при вызове которого вызывающший поток получает возвращаемое значение задачи, выполненной асинхронно;
- Получение исключения, возникшего в асинхронной операции, для его обработки;
- Просмотр состояния асинхронной операции;
- Поддержка отмены выполнения;
- Продолжения задач;
- Планирование выполнения асинхронной операции.

**Task (задача)** - это конструкция, которая реализует модель параллельной обработки, основанной на обещаниях (Promise).

Задача обещает, что работа будет выполнена позже, позволяя взщаимодействовать с помощью "обещания" с чистым API.

Существует стратегия параллельных вычислений - **Features & Promises**.
- **Feature** - это представление переменной, доступной только для чтения, или ссылка на ещё не вычисленное значение. Другими словами, объект для результата, который ещё не существует. Это возвращаемое значение асинхронной функции **Promise**;
- **Promise** - это функция, которая присваивает значение **Feature**. Это некий контейнер, "завершающий будущее". Он может завершить операцию как с успешным результатом, так и с исключением, указывающим на неудачу.

## **01 Класс Task**
---
Для работы с задачами в **.NET** испольузется класс **Task**.

**Конструкторы класса Task**:
- **Task(Action/Func)** - принимает экземпляр делегата Action или Func;
- **Task(Action/Func, CancellationToken)** - принимает экземпляр делегата Action или Func, токен отмены;
- **Task(Action/Func, TaskCreationOptions)** - принимает экземпляр делегата Action или Func, опции создания задачи;
- **Task(Action/Func, object)** - принимает экземпляр делегата Action или Func, аргумент для задачи;
- **Task(Action/Func, CancellationToken, TaskCreationOptions)** - комбинация предыдущих параметров;
- **Task(Action/Func, object, CancellationToken)** - комбинация предыдущих параметров;
- **Task(Action/Func, object, TaskCreationOptions)** - комбинация предыдущих параметров;
- **Task(Action/Func, object, CancellationToken, TaskCreationOptions)** - комбинация предыдущих параметров.

**Статические свойства класса Task**:
- **int? TaskId** - возвращает целочисленный идентификатор задачи. Может быть Null, если вызвано в коде вне контекста какой-либо задачи;
- **TaskFactory Factory** - возвращает экземпляр самой простой фабрики задач;
- **Completedtask** - возвращает экземпляр завершённой задачи.

**Экземплярные свойства класса Task**:
- **TaskCreationOptions CreationOptions** - настройки задачи, которые были заданы при её создании;
- **bool IsCompleted** - значения: **true** - задача завершена, **false** - задача не завершена;
- **bool IsCanceled** - значения: **true** - задача отменена, **false** - задача не отменена;
- **TaskStatus Status** - возвращает значение, указывающее на текущее состояние жизненного цикла задачи;
- **AggregateAxception Exception** - возвращает исключение, которое в задаче во время её выполнения;
- **int Id** - возвращает целочисленный идентификатор задачи;
- **object AsyncState** - возвращает аргумент для задачи, переданный в конструктор при создании задачи;
- **bool IsFaulted - значения: **true** - задача была провалена, **false** - задача не была провалена.

Задачи также содержат набор методов, которые будут рассмотрены далее.

Задачи разделяются на 2 разновидности: **"холодные"** и **"горячие"**:
- **Холодные задачи** - это задачи, созданные с помощью конструктора класса Task, что означает, что задача ещё не запущена и ждёт отдельной команды на запуск;
- **Горячие задачи** - это задачи, которые помимо создания также сразу же запускаются. Обычно создаются с помощью специальных методов, таких, как фабричный метод **StartNew()** класса **TaskFactory** или статический метод **Run()** класса **Task**.

Примеры создания холодныхъ и горячих задач:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task printIterationsTask1 = new(PrintIterations, "ColdAsyncTask");
        Task printIterationsTask2 = new(PrintIterations, "ColdSyncTask");

        printIterationsTask1.Start();
        printIterationsTask2.RunSynchronously();

        Task.Factory.StartNew(PrintIterations, "HotFactoryAsyncTask");
        Task.Run(() => PrintIterations("HotRunMethodAsyncTask"));

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
            Thread.Sleep(100);
        }
    }
}
```

Создание задачи через фабричный метод **StartNew()** класса **TaskFactory** считается более устаревшим, сейчас более предпочтительно создавать задачи через статический метод **Run()** класса **Task**.

## **02 Класс Task<TResult>**
---
Для создания задач с возвращаемыми значениями в **.NET** используется класс **Task<TResult>**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<int> coldAsyncTask = new(PrintIterations, "ColdAsyncTask");
        Task<int> coldSyncTask = new(PrintIterations, "ColdSyncTask");

        coldAsyncTask.Start();

        Console.WriteLine($"ColdAsyncTask completed with result of {coldAsyncTask.Result}");

        coldSyncTask.RunSynchronously();

        Console.WriteLine($"ColdAsyncTask completed with result of {coldSyncTask.Result}");

        Task<int> hotFactoryAsyncTask = Task.Factory.StartNew(PrintIterations, "HotFactoryAsyncTask");
        Task<int> hotRunMethodAsyncTask = Task.Run(() => PrintIterations("HotRunMethodAsyncTask"));

        Console.WriteLine($"HotFactoryAsyncTask completed with result of {hotFactoryAsyncTask.Result}");
        Console.WriteLine($"HotRunMethodAsyncTask completed with result of {hotRunMethodAsyncTask.Result}");

        Console.ReadKey();
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

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }
}
```

## **03 Перечисление TaskCreationOptions**
---
**Перечисление TaskCreationOptions** позволяет настраивать создаваемые задачи.

Оно содержит несколько констант, которые позволяют очень легко повлиять на работу задачи.

Перечисление **TaskCreationOptions**:
- **TaskCreationOptions.None** - указывает, что должно использоваться поведение по умолчанию;
- **TaskCreationOptions.PreferFairness** - указание для планироващика задач, что те задачи, которые были запланированы ранее, выполнялись ранее, а те, которые были запланированы позже, выполнялись позже;
- **TaskCreationOptions.LongRunning** - указывает, что задача будет долговыполняемая, что подсказывает планировщику задач, что для этой задачи может потребоваться отдельный выделенный поток, а не поток из пула потоков, так как занимать поток из пула потоков на слишком долгое время не выгодно с точки зрения производительности, так как может возникнуть очередь из задач, которые будут ожидать освобождения потока из пула, пока поток из пула будет занят долговыполняющейся задачей. В этом случае создани и уничтожение отдельного потока не из пула потоков оправдано;
- **TaskCreationOptions.AttachedToParent** - указывает, что вложенную задачу необходимо присоединить к родительской задаче;
- **TaskCreationOptions.DenyChildAttach** - указывает, что к задаче запрещено присоединять дочерние задачи:
- **TaskCreationOptions.HideScheduler** - заставляет использовать дочерние задачи планировщик по умолчанию, а не родительский;
- **TaskCreationOptions.RunContinuationsAsynchronously** - заставляет выполнять продолжение задачи асинхронно по отношению к самой задаче.

Пример создания долговыполняемой задачи, для которой будет создан отдельный поток вне пула потоков:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<int> taskAsync = new(PrintIterations, "AsyncTask", TaskCreationOptions.LongRunning);
        taskAsync.Start();

        Thread.Sleep(100);

        PrintIterations("Regular method call");

        taskAsync.Wait();
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(100);
        }

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }
}
```

Асинхронная задача выполнялась в отдельном потоке и имела Id задачи, а обычный вызов метода инициировал выполнение в вызывающем потоке, при этом, Id задачи имел значение "null", так как выполнялся не в контексте задачи.

## **04 Перечисление TaskStatus**
---
**Перечисление TaskStatus** - это вспомогательное перечисление, которое помогает просматривать состояние жизненного цикла задачи.

Перечисление **TaskStatus**:
- **Created** - задача инициализирована, но ещё не запланирована;
- **WaitingForActivation** - задача ожидает активации и планирования внутри инфраструктуры .NET;
- **WaitingToRun** -  задача была запланирована для выполнения, но еще не начала выполняться;
- **Running** - задача выполняется, но еще не завершена;
- **WaitingForChildrenToComplete** - задача завершила выполнение и неявно ожидает завершения прикрепленных дочерних задач;
- **RanToCompletion** - задача успешно завершила выполнение;
- **Canceled** - задача подтвердила отмену, создав исключение OperationCanceledException со своим собственным CancellationToken, когда токен находился в сигнальном состоянии, или же CancellationToken задачи уже получил сигнал до того, как задача начала выполняться;
- **Faulted** - задача завершена из-за необработанного исключения.

Пример просмотра статуса задачи на разных этапах её жизненного цикла:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<int> task = new(PrintIterations, "AsyncTask");

        PrintTaskStatus(task);

        task.Start();

        PrintTaskStatus(task);

        Thread.Sleep(100);

        PrintTaskStatus(task);

        task.Wait();

        PrintTaskStatus(task);
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 5)
        {
            iterationNumber++;

            Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(200);
        }

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }

    private static void PrintTaskStatus(Task task) => Console.WriteLine($"[AsyncTask Status: {task.Status}]");
}
```

## **05 Методы Wait(), WaitAll() и WaitAny() класса Task**
---
Для ожидания задач класс имеет следующие методы:
- **экземплярный Wait()** - при вызове блокирует вызывающий поток до завершения задачи, на которой был вызван;
- **статический WaitAll()** - при вызове блокирует вызывающий поток до завершения всех задач, содержащихся в массиве, ссылка на который была передана в качестве аргумента метода;
- **статический WaitAny()** - при вызове блокирует вызывающий поток до того, пока не завершится хотя бы одна задача из всех задач задач, содержащихся в массиве, ссылка на который была передана в качестве аргумента метода.

> TODO: рассмотреть перегрузки

Пример применения методов **Wait()**, **WaitAll()** и **WaitAny()** класса **Task** для ожидания задач:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        // singleTask.Wait();
        Console.WriteLine("Creating 1 Single Task.");

        Task<int> singleTask = new(PrintIterations, "SingleAsyncTask");
        singleTask.Start();

        Console.WriteLine("Waiting for 1 Single Task.");

        singleTask.Wait();

        Console.WriteLine($"SingleAsyncTask has finished. Task's Result: {singleTask.Result}.{Environment.NewLine}");

        // Task.WaitAll();
        Console.WriteLine("Creating 2 Important Tasks.");

        Task<int>[] allTheImportantTasks = Enumerable.Range(1, 2)
            .Select(i => Task.Factory.StartNew(PrintIterations, $"ImportantAsyncTask{i}"))
            .ToArray();

        Console.WriteLine("Waiting for All 2 The Important Tasks.");

        Task.WaitAll(allTheImportantTasks);

        Console.WriteLine($"All 2 The Important Tasks have finished.");

        Array.ForEach(allTheImportantTasks, importantTask => Console.WriteLine($"Task's Result: {importantTask.Result}"));
        Console.WriteLine();

        // Task.WaitAny();
        Console.WriteLine("Creating Some 2 Tasks.");

        Task<int>[] someTasks = Enumerable.Range(1, 2)
            .Select(i => Task.Factory.StartNew(PrintIterations, $"SomeAsyncTask{i}"))
            .ToArray();

        Console.WriteLine("Waiting for Any 1 of 2 Some Tasks.");

        int taskIndex = Task.WaitAny(someTasks);

        Console.WriteLine($"AsyncTask with index [{taskIndex}] has finished first. Task's Result: {someTasks[taskIndex].Result}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 5)
        {
            iterationNumber++;

            Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(100);
        }

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }
}
```

Методы **Wait()**, **WaitAll()** и **WaitAny()** не рекомендуются к повсеместному использованию, потому что могут вызывать блокировки.

## **06 Передача аргументов в задачи через замыкание (Closure)**
---
В случае необходимости передачи в задачу нескольких аргументов и невозможности сделать составной объект, содержащий несколько параметров в качестве полей, можно использовать **замыкание**.

**Замыкание** - это передача в метод предопределённых переменных из внешней зоны видимости кода не через список параметров, а через использование этих переменных в теле метода, вложенного в другой метод, в котором и определены эти переменные.

Также **невозможно передать аргумент в задачу классическим способом** при создании и запуске задачи через статический метод **Run** класса **Task**, что побуждает к использованию замыкания при передаче аргументов:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        string taskName = "AsyncTask";
        int iterationNumber = 10;
        int iterationDelayMilliseconds = 100;

        Task<int> task = Task.Run(() => PrintIterations(taskName, iterationNumber, iterationDelayMilliseconds));

        task.Wait();

        Console.WriteLine($"AsyncTask has finished. Task's Result: {task.Result}");
    }

    private static int PrintIterations(string taskName, int iterationNumber, int iterationDelayMilliseconds)
    {
        int iterationIndex = 0;

        while (iterationIndex < iterationNumber)
        {
            iterationIndex++;

            Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(iterationDelayMilliseconds);
        }

        int calculationResult = iterationIndex * 1000;

        return calculationResult;
    }
}
```

## **07 Особенности передачи аргументов в задачи через замыкание (Closure)**
---
При использовании техники замыкания следует учитывать некоторые особенности, сопустствующие её использованию.

При использовании замыкания значения переменных должны быть переданы в другой метод в качестве аргументов в обход прямой передачи через параметры метода.

Такая необходимость создаёт сложность при передаче локальных данных одного метода в другой, особенно, если второй метод выполняется в качестве асинхронной задачи в отдельном потоке и имеет отдельный от вызывающего метода стек, что делает невозможным передачу параметров внутри стека через спецификатор **ref**, **in** или **out**.

Единственным возможным решением становится передача значений через объект на куче, который будет являться глобальными данными, доступными всем методам и потокам, имеющим на него ссылку.

Исходя из такого заключения, в компилятор C# был внедрён механизм, который преобразует "замыкания" таким образом, чтобы создавался класс-обёртка с набором открытых полей, которые своими именами и типами соответствуют переданным в метод через замыкание переменным, а также с помещённым в тело этого класса закрытым методом, соотстветствующим методу, в который передавались переменные с использованием техники замыкания.

В вызывающем коде создаётся экземпляр этого класса, его поля заполняются значениями из соответствующих им переменных и в нужном месте уже без применения техники замыкания вызывается метод, к которому в исходном коде она применялась, ведь теперь он будет обращаться по ссылке к полям экземпляра класса, размещённого на куче, а не к локальным переменным другого метода, с которым даже может выполняться в другом потоке.

Передача через использование техники замыкания аргументов в метод, вызванный внутри лямбда-выражения, переданного в качестве аргумента в метод **Run** класса **Task**, после преобразования компилятора C# имеет недопустимые и недоступные в языке программирования C# символы и конструкции, поэтому наиболее близкой демонстрацией преобразования замыкания может быть продемонстрирован через создание задачи через конструктор класса **Task** и запуск через метод **Run**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        string taskName = "AsyncTask";
        int iterationNumber = 10;
        int iterationDelayMilliseconds = 100;

        DisplayClass displayClass = new()
        {
            _taskName = taskName,
            _iterationNumber = iterationNumber,
            _iterationDelayMilliseconds = iterationDelayMilliseconds
        };

        Task<int> task = new(state => ((DisplayClass)state).Main(), displayClass);
        task.Start();

        task.Wait();

        Console.WriteLine($"AsyncTask has finished. Task's Result: {task.Result}");
    }

    private static int PrintIterations(string taskName, int iterationNumber, int iterationDelayMilliseconds)
    {
        int iterationIndex = 0;

        while (iterationIndex < iterationNumber)
        {
            iterationIndex++;

            Console.WriteLine($"{taskName} - Task#{Task.CurrentId?.ToString() ?? "null"}- Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(iterationDelayMilliseconds);
        }

        int calculationResult = iterationIndex * 1000;

        return calculationResult;
    }

    private sealed class DisplayClass
    {
        public string _taskName;
        public int _iterationNumber;
        public int _iterationDelayMilliseconds;

        internal int Main()
        {
            return PrintIterations(_taskName, _iterationNumber, _iterationDelayMilliseconds);
        }
    }
}
```
Замыкания в языке программирования C# нежелательно использовать из-за усложнения выходного кода, создания лишних экземпляров на куче при каждом вызове метода с последующей необходимостью сборщику мусора их отслеживать и удалять.

Однако, их использование ускоряет разработку, а при классической передаче составных параметров они всё равно проходят запаковку и распаковку, поэтому выбор остаётся за разработчиком.

## **08 Продолжение задачи**
---
**Продолжение задачи** - это асинхронная задача, вызываемая другой задачей при своём завершении. Это одна из форм метода обратного вызова (Callback Method).

Обычно задачу продолжения называют просто **продолжением**.

Задачу, которая выполнялась до задачи продолжения, называют **предшествующей**.

**Задача продолжения** - это задача, корторая вызывается другой задачей, предшествующей, при завершении этой предшествующей задачи.

Это означает, что можно зарегистрировать при завершении одной задачи (A) запуск выполнения другой задачи (B).

Например, вторая задача (B) обработает результат первой задачи (A).

При помощи продолжений возможно выстраивать действительно асинхронные и параллельные выполнения целых участков кода, потому что теперь у асинхронных операций будет иметься логическое продолжение и не нужно будет возвращать ожидать их выполнения в вызывающем потоке, чтобы как-то обработать результат, так как теперь этим займётся **продолжение**, из которых возможно выстраивать длинные **цепочки продолжений**.

Метод **ContinueWith()** возвращает новый экземпляр **Task**, что позволяет выстраивать цепочки продолжений.

В **TPL** добавления продолжения используется метод **ContinueWith()**.

Задача продолжения при создании и до завершения предшествующего метода находится в состоянии **WaitingForActiovation**. Она активируется автоматически при завершении предшествующей задачи.

> TODO: перегрузки метода **ContinueWith()**

Пример создания продолжения:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<int> task = new(PrintIterations, "AsyncTask");
        Task continuationTask = task.ContinueWith(PrintIterationsContinuation);

        task.Start();

        continuationTask.Wait();
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(100);
        }

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }

    private static void PrintIterationsContinuation(Task previousTask)
    {
        Task<int> castedPreviousTask = (Task<int>)previousTask;

        Console.WriteLine($"ContinuationAction - Thread#{Environment.CurrentManagedThreadId} - PreviousTask has Result of {castedPreviousTask.Result}");
    }
}
```

Запуск задачи и цепочки продолжений можно выполнить только на экземпляре исходной задачи, поэтому не получится "прикрепить" продолжения по слабой ссылке к экземпляру исходной задачи, и вызвать всю цепочку по сильной ссылке на экземпляре, возвращённым последним методом ContinueWith().

Поэтому необходимо создать экземпляр исходной задачи по сильной ссылке, прикрепить к ней продолжения, после чего запустить исходную задачу по сильной ссылке. 

Ожидать возможно как исходную задачу, так и все продолжения по отдельности, обычно ожидается последняя задача продолжения в цепочке продолжений.

## **09 Множественное продолжение задачи**
---
Метод **ContinueWith()** возвращает ссылку на экземпляр задачи продолжения, добавленной к предшествующей задаче. Это позволяет создавать цепочки продолжения сообщая с экземпляпами делегатов как ссылки на именованные методы, так и на анонимные методы, лямбда-операторы и лямбда-выражения:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<IEnumerable<string>> task = new(MakeIterations, "AsyncTask");

        Task continuationTasks = task
            .ContinueWith(PrintIterationsSymbolsNumber)
            .ContinueWith(PrintIterationsReports)
            .ContinueWith(previousTask =>
            {
                Console.WriteLine($"Lambda operator has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");

                Console.WriteLine("Final continuation action is processing.");

                Console.WriteLine($"Lambda operator has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");
            });

        task.Start();

        continuationTasks.Wait();
    }

    private static IEnumerable<string> MakeIterations(object state)
    {
        string taskName = state.ToString();
        Console.WriteLine($"{nameof(MakeIterations)} method has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");
        List<string> iterationReports = new(10);

        for (int i = 0; i < iterationReports.Count; i++)
        {
            Console.WriteLine($"{taskName} - Task#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{i}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"{nameof(MakeIterations)} method has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");

        return iterationReports;
    }

    private static IEnumerable<string> PrintIterationsSymbolsNumber(Task previousTask)
    {
        Console.WriteLine($"{nameof(PrintIterationsSymbolsNumber)} method has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");

        Task<IEnumerable<string>> castedPreviousTask = (Task<IEnumerable<string>>)previousTask;

        IEnumerable<string> iterationReports = castedPreviousTask.Result;

        int calculationResult = iterationReports.Select(x => x.Length).Sum();

        Console.WriteLine($"ContinuationAction {nameof(PrintIterationsSymbolsNumber)} - Task#{Task.CurrentId}- Thread#{Environment.CurrentManagedThreadId} - PreviousTask has Result of [{calculationResult}] symbols in all iterations.");

        Console.WriteLine($"{nameof(PrintIterationsSymbolsNumber)} method has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");

        return iterationReports;
    }

    private static void PrintIterationsReports(Task previousTask)
    {
        Console.WriteLine($"{nameof(PrintIterationsReports)} method has started in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.");

        Task<IEnumerable<string>> castedPreviousTask = (Task<IEnumerable<string>>)previousTask;

        IEnumerable<string> iterationReports = castedPreviousTask.Result;

        foreach (string iterationReport in iterationReports)
        {
            Console.WriteLine(iterationReport);
        }

        Console.WriteLine($"{nameof(PrintIterationsReports)} method has finished in Task#{Task.CurrentId} and Thread#{Environment.CurrentManagedThreadId}.{Environment.NewLine}");
    }
}
```

## **10 Перечисление TaskContinuationOptions**

**Перечисление TaskContinuationOptions** позволяет настраивать создаваемые продолжения задач.

Оно содержит несколько констант, которые позволяют очень легко повлиять на работу продолжения задачи.

Значения перечисления **TaskContinuationOptions**:

- **None** - псли параметры продолжения не указаны, указывает, что при выполнении продолжения следует использовать поведение по умолчанию. Продолжение выполняется асинхронно после завершения предшествующей задачи, независимо от конечного значения свойства System.Threading.Tasks.Task.Status предшествующей задачи. Если продолжение является дочерней задачей, оно создается как отдельная вложенная задача;
- **PreferFairness** - подсказка для System.Threading.Tasks.TaskScheduler для планирования задач в том порядке, в котором они были запланированы, чтобы задачи, запланированные раньше, с большей вероятностью выполнялись раньше, а задачи, запланированные позже, с большей вероятностью выполнялись позже;
- **LongRunning** - указывает, что продолжение будет длительной, детализированной операцией. Он дает подсказку System.Threading.Tasks.TaskScheduler о том, что может быть оправдано превышение лимита подписки;
- **AttachedToParent** - указывает, что продолжение, если оно является дочерней задачей, присоединено к родительской задаче в иерархии задач. Продолжение может быть дочерней задачей только в том случае, если ее предшествующая задача также является дочерней задачей. По умолчанию дочерняя задача (то есть внутренняя задача, созданная внешней задачей) выполняется независимо от своего родителя. Вы можете использовать параметр System.Threading.Tasks.TaskContinuationOptions.AttachedToParent, чтобы синхронизировать родительские и дочерние задачи. Обратите внимание, что если родительская задача настроена с параметром System.Threading.Tasks.TaskCreationOptions.DenyChildAttach, параметр System.Threading.Tasks.TaskCreationOptions.AttachedToParent в дочерней задаче не действует, и дочерняя задача будет выполняться как отсоединенная дочерняя задача;
- **DenyChildAttach** - указывает, что любая дочерняя задача (то есть любая вложенная внутренняя задача, созданная этим продолжением), созданная с параметром System.Threading.Tasks.TaskCreationOptions.AttachedToParent и пытающаяся выполниться как присоединенная дочерняя задача, не сможет присоединиться к родительской задаче и вместо этого будет выполняться как отдельная дочерняя задача;
- **HideScheduler** - указывает, что задачи, созданные продолжением путем вызова таких методов, как System.Threading.Tasks.Task.Run(System.Action) или System.Threading.Tasks.Task.ContinueWith(System.Action{System.Threading.Tasks.Task}) видят планировщик по умолчанию (System.Threading.Tasks.TaskScheduler.Default), а не планировщик, на котором выполняется это продолжение, в качестве текущего планировщика;
- **LazyCancellation** - в случае отмены продолжения предотвращает завершение продолжения до тех пор, пока не завершится предшествующая задача;
- **RunContinuationsAsynchronously** - указывает, что задача продолжения должна выполняться асинхронно. Этот параметр имеет более высокий приоритет над System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously;
- **NotOnRanToCompletion** - указывает, что задача продолжения не должна планироваться, если ее предшествующая задача завершена. Предшествующая задача выполняется до завершения, если его свойство System.Threading.Tasks.Task.Status после завершения имеет значение System.Threading.Tasks.TaskStatus.RanToCompletion. Этот параметр недействителен для многозадачных продолжений;
- **NotOnFaulted** - указывает, что задача продолжения не должна планироваться, если ее предшествующая задача вызвала необработанное исключение. Предшествующая задача создает необработанное исключение, если его свойство System.Threading.Tasks.Task.Status после завершения имеет значение System.Threading.Tasks.TaskStatus.Faulted. Этот параметр недействителен для многозадачных продолжений;
- **OnlyOnCanceled** - указывает, что продолжение следует планировать, только если оно отменено. Антецедент отменяется, если его свойство System.Threading.Tasks.Task.Status после завершения имеет значение System.Threading.Tasks.TaskStatus.Canceled. Этот параметр недействителен для многозадачных продолжений;
- **NotOnCanceled** - указывает, что задача продолжения не должна планироваться, если ее предшествующая задача была отменена. Предшествующая задача отменяется, если его свойство System.Threading.Tasks.Task.Status после завершения имеет значение System.Threading.Tasks.TaskStatus.Canceled. Этот параметр недействителен для многозадачных продолжений;
- **OnlyOnFaulted** - указывает, что задачу продолжения следует планировать только в том случае, если ее предшествующая задача вызвала необработанное исключение. Предшествующая задача создает необработанное исключение, если его свойство System.Threading.Tasks.Task.Status после завершения имеет значение System.Threading.Tasks.TaskStatus.Faulted. Параметр System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted гарантирует, что свойство System.Threading.Tasks.Task.Exception в предшественнике не равно null. Вы можете использовать это свойство, чтобы перехватить исключение и посмотреть, какое исключение вызвало ошибку задачи. Если вы не обращаетесь к свойству System.Threading.Tasks.Task.Exception, исключение не обрабатывается. Кроме того, если вы попытаетесь получить доступ к свойству System.Threading.Tasks.Task`1.Result задачи, которая была отменена или возникла ошибка, создается новое исключение. Этот параметр недействителен для многозадачных продолжений;
- **OnlyOnRanToCompletion** - указывает, что продолжение следует планировать только в том случае, если его предшественник выполнен до завершения. Предшественник выполняется до завершения, если его свойство System.Threading.Tasks.Task.Status после завершения имеет значение System.Threading.Tasks.TaskStatus.RanToCompletion. Этот параметр недействителен для многозадачных продолжений;
- **ExecuteSynchronously** - Указывает, что задача продолжения должна выполняться синхронно. Если указан этот параметр, продолжение выполняется в том же потоке, который вызывает переход предшествующей задачи в ее конечное состояние. Если предшествующая задача уже завершена, когда создается продолжение, продолжение будет выполняться в потоке, создавшем продолжение. Если предшественник System.Threading.CancellationTokenSource расположен в блоке finally, продолжение с этим параметром будет выполняться в этом блоке finally. Синхронно должны выполняться только очень короткие продолжения. Поскольку задача выполняется синхронно, нет необходимости вызывать такой метод, как System.Threading.Tasks.Task.Wait, чтобы убедиться, что вызывающий поток ожидает завершения задачи.

Пример создания продолжений задачи с опциями продолжения:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task = new(PrintIterations, -1);

        Task continuationTasks = task
            .ContinueWith(HandleExceptionContinuation, System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted);

        task.Start();

        continuationTasks.Wait();
    }

    private static void PrintIterations(object state)
    {
        Console.WriteLine($"{nameof(PrintIterations)} - Thread#{Environment.CurrentManagedThreadId} - Task has started.");

        int iterationsNumber = (int)state;

        if (iterationsNumber < 0) throw new ArgumentOutOfRangeException(nameof(iterationsNumber), $"Incorrect Value: {iterationsNumber}.");

        int iterationIndex = 0;

        while (iterationIndex < iterationsNumber)
        {
            iterationIndex++;

            Console.WriteLine($"{nameof(PrintIterations)} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(100);
        }
    }

    private static void HandleExceptionContinuation(Task previousTask)
    {
        Console.WriteLine($"{nameof(HandleExceptionContinuation)} - Thread#{Environment.CurrentManagedThreadId} - PreviousTask has finished with an exception.");

        Exception taskInnerException = previousTask.Exception.InnerException;

        Console.WriteLine($"Exception Type: {taskInnerException.GetType()}{Environment.NewLine}Exception Message: {taskInnerException.Message}");
    }
}
```
