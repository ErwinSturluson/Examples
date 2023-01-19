# **Планировщики задач (Task Schedulers)**

**Планироващик задач (Task Scheduler)** - это механизм, который позволяет настраивать выполнение задач указанным разработчиком способом.

Для работы с планировщиком в **.NET** используют класс **TaskScheduler** из пространства имён **System.Threading.Tasks**.

**TaskScheduler** является базовым (абстрактным) классом.

Реализация конкретной логики работы планировщика полностью ложится на программиста-пользователя.

В библиотеке **.NET** представлены стандартные варианты планировщиков, например, стандартный планировщик, построенный на пуле потоков **ThreadPoolTaskScheduler** из статического свойства **TaskScheduler.Default** или **SynchronizationContextTaskScheduler**, который по умолчанию используется в приложениях типа **WPF**, имеющихъ графический интерфейс, для синхронизации продолжений, взаимодействтвующих с интерфейсом с **UI** потоком приложения.

### **Члены абстрактного класса TaskScheduler.**

**Абстрактные методы:**
- **QueueTask(Task task)** - помещает переданную задачу в очередь выполнения;
- **GetScheduledTasks()** - возвращает очередь задач в виде коллекции. **Используется только для отладки**;
- **TryExecuteInline(Task task, bool taskWasPreviouslyQueued)** - запрашивает возможность выполниться синхронно. Значение параметра **taskWasPreviouslyQueued** указывает, ставилась ли уже задача в очередь: **true** - задача уже было поставлена в очередь, **false** - задача должна выполниться немедленно без поставновки в очередь. Возвращает **true**, если задачу удалось выполнить, **false** - если нет. Метод необходим для того, чтобы выполнять задачу синхронно, если вызывающий поток вызвал метод**RunSynchronously()**, а также методы **Wait()**, **WaitAll()**, **WaitAny()** или свойство **Result**. При вызове методов и свойств ожидания могут возникать блокировки, поэтому планировщик отвечает за то, чтобы убедиться, что поток подходит для выполнения задачи. 

**Другие методы:**
- **TryExecuteTask(Task task)** - попытка выполнить задачу. Возвращает **true**, если задачу удалось выполнить, **false** - если нет. Выполняет всю логику выполнения задачи, включая работу с планировщиком задач, обработкой исключений, управлением состоянием и жизненным циклом задачи. Для запуска задачи из планировщика необходимо использовать именно этот метод, логику его работы изменить нельзя. Имеет модификатор доступа **protected**, поэтому вызвать из производных классов его нельзя;
- **TryDequeue(Task task)** - попытка удалить задачу из очереди выполнения. Возвращает **true**, если задачу удалось удалить, **false** - если нет. Метод является вирутальным, и для удаления задач его необходимо переопределять. Метод выполняется при отмене задачи;
- **FromCurrentSynchronizationContext()** - создаёт планировщик, связанный с текущим элементом SynchronizationContext. Внутри себя создаёт планировщик задач **SynchronizationContextTaskScheduler**

**Свойства:**
- **Default** - возвращает стандартный планировщик **ThreadPoolTaskScheduler**, построенный на пуле потоков;
- **Current** - возвращает текущий планировщик. Если планировщик не будет найден, то возвращает стандартный планировщик;
- **MaximumConcurrencyLevel** - максимальный уровень параллелизма. По умолчанию возвращает значение **int.MaxValue**;
- **Id** - идентификатор планировщика.

Собственная реализация конкретного планировщика, унаследованного от абстрактного базового класса **TaskScheduler** - это **единственный способ** для взаимодействия с открытым API для **управления выполнением задач**.

## **01 Реализация планировщика задач**
---
При реализации собственного планировщика задач, необходимо его унаследовать от базового абастрактного класса **TaskScheduler** и реализовать 3 абстрактных метода:
- **GetScheduledTasks()** - метод используется только для отладки;
- **QueueTask()** - метод вызывается методом **Start()** класса **Task**. Это очень важный метод, в котором определяется, как и где будет выполняться задача;
- **TryExecuteTaskInline()** - метод используется для выполнения задачи синхронно, например, когда при вызове методов **RunSynchronously()**, а также **Wait()** или **Result** на задаче, представленной экземпляром класса **Task**, она ещё не была фактически запущена, или когда при вызове методов **WaitAll()** и **WaitAny()** также имеются ещё незапущенные задачи, которые всё равно придётся ожидать, и выделять для таких задач дополнительные потоки не имело бы смысла, поэтому задача, запущенная методом **TryExecuteTaskInline()**, будет выполнена в контексте вызывающего потока.

Переопределение виртуальных, создание новых открытых, защищённых или закрытых членов класса является определением собственной логики работы планировщика задач:

> ConcreteTaskScheduler.cs
```cs
namespace TaskSchedulers._01_Implementation
{
    internal class ConcreteTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasksList = new();

        internal int ExecuteTasksDelayMilliseconds { get; set; } = 0;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            Console.WriteLine($"Task List was requested in Thread#{Environment.CurrentManagedThreadId}.");
            return _tasksList;
        }

        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"Task with Id#{task.Id} was queued in Thread#{Environment.CurrentManagedThreadId}.");

            lock (_tasksList)
            {
                _tasksList.AddLast(task);
            }

            ThreadPool.QueueUserWorkItem(ExecuteTasks, null);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"Task with Id#{task.Id} was tried execute inline in Thread#{Environment.CurrentManagedThreadId}.");

            lock (_tasksList)
            {
                _tasksList.Remove(task);
            }

            bool executedInline = TryExecuteTask(task);

            if (executedInline)
            {
                Console.WriteLine($"Task with Id#{task.Id} was successfully executed inline in Thread#{Environment.CurrentManagedThreadId}.");
            }
            else
            {
                Console.WriteLine($"Task with Id#{task.Id} was failed to execute inline in Thread#{Environment.CurrentManagedThreadId}.");
            }

            return executedInline;
        }

        protected override bool TryDequeue(Task task)
        {
            Console.WriteLine($"Task with Id#{task.Id} was tried to dequeue in Thread#{Environment.CurrentManagedThreadId}.");

            bool taskDequeued = false;

            lock (_tasksList)
            {
                taskDequeued = _tasksList.Remove(task);
            }

            if (taskDequeued)
            {
                Console.WriteLine($"Task with Id#{task.Id} was dequeued successfully in Thread#{Environment.CurrentManagedThreadId}.");
            }
            else
            {
                Console.WriteLine($"Task with Id#{task.Id} was failed to dequeue in Thread#{Environment.CurrentManagedThreadId}.");
            }

            return taskDequeued;
        }

        private void ExecuteTasks(object _)
        {
            while (true)
            {
                Thread.Sleep(ExecuteTasksDelayMilliseconds);
                Task task = null;

                lock (_tasksList)
                {
                    if (_tasksList.Count == 0)
                    {
                        break;
                    }

                    task = _tasksList.First.Value;
                    _tasksList.RemoveFirst();
                }

                if (task == null)
                {
                    break;
                }

                TryExecuteTask(task);
            }
        }
    }
}
```

> Program.cs
```cs
namespace TaskSchedulers._01_Implementation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Testing_QueueTask();

            Testing_TryExecuteTaskInline();

            Testing_TryDequeue();
        }

        private static void Testing_QueueTask()
        {
            Console.WriteLine($"Testing of QueueTask method:");

            ConcreteTaskScheduler taskScheduler = new();

            Task<int>[] tasks = Enumerable.Range(1, 3)
                .Select(i => new Task<int>(PrintIterations, 0))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task.WaitAll(tasks);

            Console.WriteLine();
        }

        private static void Testing_TryExecuteTaskInline()
        {
            Console.WriteLine($"Testing of TryExecuteTaskInline method:");

            ConcreteTaskScheduler taskScheduler = new();
            taskScheduler.ExecuteTasksDelayMilliseconds = 2000;

            Task<int>[] tasks = Enumerable.Range(1, 3)
                .Select(i => new Task<int>(PrintIterations, 100))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Array.ForEach(tasks, t => t.Wait());

            Console.WriteLine();
        }

        private static void Testing_TryDequeue()
        {
            Console.WriteLine($"Testing of TryDequeue method:");

            ConcreteTaskScheduler taskScheduler = new();

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            cts.CancelAfter(200);

            Task<int>[] tasks = Enumerable.Range(1, 3)
               .Select(i => new Task<int>(PrintIterations, 100, ct))
               .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            try
            {
                Task.WaitAll(tasks);
            }
            catch (Exception)
            {
                Console.WriteLine($"Some tasks were canceled.");
            }

            Console.WriteLine();
        }

        private static int PrintIterations(object state)
        {
            int iterationDelay = (int)state;

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"Task with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(iterationDelay);
            }

            int calculationResult = iterationIndex * iterationDelay;

            return calculationResult;
        }
    }
}
```

Для тестирования и демонтстрации работы планироващика задач ConcreteTaskScheduler были созданы 3 метода:

- **Testing_QueueTask()** - метод для тестирования стандартного запуска нескольких задач в контексте тестового планировщика и последующего ожидания всех задач через вызов статического метода **WaitAll()** класса **Task**. Некоторые из задач могут быть выполнены синхронно;
- **Testing_QueueTask()** - метод для тестирования запуска задач синхронно за счёт вызовов метода **Wait()** на экземплярах класса **Task** до того, как они успели запуститься асинхронно;
- **Testing_TryDequeue()** - метод для тестирования удаления задач из очереди на выполнение за счёт инициации отмены через токен отмены, если эти задачи ещё не успели запуститься асинхронно.

## **02 IndependentThreadTaskScheduler**
---
В случае необходимости логики, в которой задачи будут выполняться в контексте независимых потоков, не занимая потоки из **ThreadPool**, может быть определён планировщик задач назвисимых потоков.

Так как задачи не будут ставиться в очередь, а будут сразу же выполняться в контексте независимых потоков, то метод **GetScheduledTasks()** должен возвращать пустую коллекцию. Он не должен возвращать **null** или выбрасывать исключение **NotImplementedException()**, потому что может использоваться как в собственном, так и во стороннем программном коде, задействованном в системе.

Для выполнения задачи через метод **QueueTask()** также используется унаследованный защищённый метод **TryExecuteTask()**. Для того, чтобы сохранить поведение задачи, как фонового асинхронного процесса, не блокирующего завершение основного потока, свойству **IsBackground** созданного потока необходимо присвоить значение **true**.

Так как никакая особая логика, и даже очереди ожидающих задач в планировщике не присутствуеют, то, для встроенного выполнения метода задачи через метод **TryExecuteTaskInline()** достаточно просто вызвать унаследованный защищённый метод **TryExecuteTask()**.

Пример реализации и использования **IndependentThreadTaskScheduler**:

> IndependentThreadTaskScheduler.cs
```cs
namespace TaskSchedulers._02_IndependentThreadTaskScheduler
{
    internal class IndependentThreadTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task)
        {
            Thread taskThread = new(() => TryExecuteTask(task));

            taskThread.IsBackground = true;

            taskThread.Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);
    }
}
```

> Program.cs
```cs
namespace TaskSchedulers._02_IndependentThreadTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IndependentThreadTaskScheduler taskScheduler = new();

            Task<int>[] tasks = Enumerable.Range(1, 3)
                .Select(i => new Task<int>(PrintIterations, 100))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task.WaitAll(tasks);
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine(
                $"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} " +
                $"has started in Thread#{Environment.CurrentManagedThreadId}. " +
                $"Is this Thread from the ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}.");

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"Task with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationIndex * 1000;

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

            return calculationResult;
        }
    }
}
```

## **03 LimitedConcurrencyTaskScheduler**
---
Для выполнения задач с ограниченным уровнем параллелизма можно создать планировщик задач **LimitedConcurrencyTaskScheduler**. В конструкторе класса **LimitedConcurrencyTaskScheduler** будет передаваться аргумент, значение которого будет соответствовать максимальному уровню параллелизма для задач, выполнение которых планируется этим планировщиком задач.

Для определения, обрабатывает ли текущий поток в данный момент задачу, или нет, необходимо создать поле **_currentThreadIsProcessingItems**. Оно должно быть статическим, чтобы его значение было синхронизировано между всеми экземплярами такого планировщика, но также должно быть декорировано атрибутом **[ThreadStatic]**, чтобы являться собственным для каждого потока отдельно.

Для непосредственного запуска задач используется метод **NotifyThreadPoolOfPendingWork()**, внутри которого задачи запускаются при помощи **ThreadPool**.

При помещении задач в очередь с помощью метода **QueueTask()** задача сначала помещается в список задач, и, если количество выполняющихся задач меньше, чем максимально допустимое значение параллелизма, то счётчик выполняющихся задач увеличивается, и вызывается метод **NotifyThreadPoolOfPendingWork()**:

> LimitedConcurrencyTaskScheduler.cs
```cs
namespace TaskSchedulers._03_LimitedConcurrencyTaskScheduler
{
    internal class LimitedConcurrencyTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasksList = new();
        private readonly int _concurrencyLevel;

        private int _runningTasks = 0;

        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        public LimitedConcurrencyTaskScheduler()
            : this(1)
        {
        }

        public LimitedConcurrencyTaskScheduler(int concurrencyLevel)
        {
            if (concurrencyLevel < 1) throw new ArgumentOutOfRangeException(nameof(concurrencyLevel), $"Incorrect Value: {concurrencyLevel}");

            _concurrencyLevel = concurrencyLevel;
        }

        public override int MaximumConcurrencyLevel => _concurrencyLevel;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_tasksList)
            {
                return _tasksList;
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (_tasksList)
            {
                _tasksList.AddLast(task);

                if (_runningTasks < _concurrencyLevel)
                {
                    _runningTasks++;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (_currentThreadIsProcessingItems == false)
            {
                return false;
            }

            if (taskWasPreviouslyQueued == true)
            {
                TryDequeue(task);
            }

            return TryExecuteTask(task);
        }

        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                _currentThreadIsProcessingItems = true;

                try
                {
                    while (true)
                    {
                        Task task;

                        lock (_tasksList)
                        {
                            if (_tasksList.Count == 0)
                            {
                                _runningTasks--;
                                break;
                            }

                            task = _tasksList.First.Value;
                            _tasksList.RemoveFirst();
                        }

                        TryExecuteTask(task);
                    }
                }
                finally
                {
                    _currentThreadIsProcessingItems = false;
                }
            }, null);
        }

        protected override sealed bool TryDequeue(Task task)
        {
            lock (_tasksList)
            {
                return _tasksList.Remove(task);
            }
        }
    }
}
```

> Program.cs
```cs
namespace TaskSchedulers._03_LimitedConcurrencyTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LimitedConcurrencyTaskScheduler taskScheduler = new(2);

            Task<int>[] tasks = Enumerable.Range(1, 4)
                .Select(i => new Task<int>(PrintIterations, $"AsyncTask{i}"))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task.WaitAll(tasks);
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"Task with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int calculationResult = iterationIndex * 1000;

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

            return calculationResult;
        }
    }
}
```

## **04 ExecutionPriorityTaskScheduler**
---
В случае необходимости управления порядком выполнения, или приоритетом задач, можно создать **ExecutionPriorityTaskScheduler**. Такой планировщик позволит повышать и понижать приоритет задач во время планирования их выполнения.

При попытке повышения или понижения приоритета поставленной в очередь на выполнение задачи, производится поиск этой задачи в списке запланированных задач, и, в случае её нахождения, она извлекается из списка, после чего помещается в его начало, либо конец.

Остальные методы работают стандартно, запуская задачи в контексте потоков из пула потоков **ThreadPool**.

В случае, если задача уже была запущена и удалена из очереди на выполнение, то новая задача с повышенным приоритетом уже не сможет быть запущена ранее, чем уже запущенная задача.

В данном планировщике приоритет определяется порядком в очереди на выполнение, поэтому, если после понижения приоритета одной из задач, запланированных для выполнения, будет запущена новая задача, то она будет помещена в очереди за задачей, приоритет которой был понижен. То есть, в данной реализации приоритет играет роль в контексте запланированных в данный момент задач, но не работает для новых запускаемых задач:

> ExecutionPriorityTaskScheduler.cs
```cs
namespace TaskSchedulers._04_ExecutionPriorityTaskScheduler
{
    internal class ExecutionPriorityTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasksList = new LinkedList<Task>();

        public bool TryIncreasePriority(Task task)
        {
            lock (_tasksList)
            {
                var node = _tasksList.Find(task);

                if (node != null)
                {
                    _tasksList.Remove(node);
                    _tasksList.AddFirst(node);

                    return true;
                }
            }

            return false;
        }

        public bool TryDecreasePriority(Task task)
        {
            lock (_tasksList)
            {
                var node = _tasksList.Find(task);

                if (node != null)
                {
                    _tasksList.Remove(node);
                    _tasksList.AddLast(node);

                    return true;
                }
            }

            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_tasksList)
            {
                return _tasksList;
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (_tasksList)
            {
                _tasksList.AddLast(task);
            }

            ThreadPool.QueueUserWorkItem(ProcessNextQueuedItem, null);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);

        protected override bool TryDequeue(Task task)
        {
            lock (_tasksList)
            {
                return _tasksList.Remove(task);
            }
        }

        private void ProcessNextQueuedItem(object _)
        {
            Task task;

            lock (_tasksList)
            {
                if (_tasksList.Count > 0)
                {
                    task = _tasksList.First.Value;
                    _tasksList.RemoveFirst();
                }
                else
                {
                    return;
                }
            }

            TryExecuteTask(task);
        }
    }
}
```

> Program.cs
```cs
namespace TaskSchedulers._04_ExecutionPriorityTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ExecutionPriorityTaskScheduler taskScheduler = new();

            Task<int>[] tasks = Enumerable.Range(1, 10)
                .Select(i => new Task<int>(PrintIterations, $"AsyncTask{i}"))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task<int> highPriorityTask = new(PrintIterations, "High-Priority Task 31");
            highPriorityTask.Start(taskScheduler);
            taskScheduler.TryIncreasePriority(highPriorityTask);

            Task<int> lowPriorityTask = new(PrintIterations, "Low-Priority Task 32");
            lowPriorityTask.Start(taskScheduler);
            taskScheduler.TryDecreasePriority(lowPriorityTask);

            Task.WaitAll(tasks);
            Task.WaitAll(highPriorityTask, lowPriorityTask);
        }

        private static int PrintIterations(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Thread.Sleep(100);
            }

            int calculationResult = iterationIndex * 1000;

            return calculationResult;
        }
    }
}
```

## **05 SynchronizationContextTaskScheduler**
---
Планировщик задач **SynchronizationContextTaskScheduler** является стандартным планировщиком задач, основанным на **контексте синхронизации**, уже представленным и реализованным в **.NET** в пространстве имён **System.Threading.Tasks**.

**SynchronizationContext** - это специальный базовый класс, который представляет способ размещения выполнения участков кода в очереди контекста. То есть, можно передават код, который необходимо выполнить, из одного контекста в другой.

**Контекст синхронизации** позволяет решать ряд очень серьёзных проблем, например, проблему обращения к элементам управления пользовательского интерфейса из потока, который его не создавал.

**SynchronizationContextTaskScheduler** используется для запуска задач в контексте синхронизации приложения:

> TODO: custom synchronization context

> SynchronizationContextTaskScheduler.cs
```cs
namespace TaskSchedulers._05_SynchronizationContextTaskScheduler
{
    internal class SynchronizationContextTaskScheduler : TaskScheduler
    {
        private readonly SynchronizationContext _synchronizationContext;

        public SynchronizationContextTaskScheduler()
            : this(SynchronizationContext.Current)
        {
        }

        public SynchronizationContextTaskScheduler(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
        }

        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task) => _synchronizationContext.Post(_ => TryExecuteTask(task), null);

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            bool contextsEqual = _synchronizationContext == SynchronizationContext.Current;

            return contextsEqual && TryExecuteTask(task);
        }
    }
}
```

> Program.cs
```cs
namespace TaskSchedulers._05_SynchronizationContextTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
        }
    }
}
```

## **06 SynchronousTaskScheduler**
---
Для демонстрации **SynchronousTaskScheduler** возможности создавать планироващики задач с разнообразным и нестандартным поведением, показан планировщик, выполняющий задачи синхронно, последовательно друг за другом, даже если они были созданы в разных потоках:

> SynchronousTaskScheduler.cs
```cs
namespace TaskSchedulers._06_SynchronousTaskScheduler
{
    internal class SynchronousTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasksList = new();

        internal int ExecuteTasksDelayMilliseconds { get; set; } = 0;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            Console.WriteLine($"Task List was requested in Thread#{Environment.CurrentManagedThreadId}.");
            return _tasksList;
        }

        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"Task with Id#{task.Id} was queued in Thread#{Environment.CurrentManagedThreadId}.");

            lock (_tasksList)
            {
                _tasksList.AddLast(task);
            }

            ExecuteTasks();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"Task with Id#{task.Id} was tried execute inline in Thread#{Environment.CurrentManagedThreadId}.");

            lock (_tasksList)
            {
                _tasksList.Remove(task);
            }

            bool executedInline = TryExecuteTask(task);

            if (executedInline)
            {
                Console.WriteLine($"Task with Id#{task.Id} was successfully executed inline in Thread#{Environment.CurrentManagedThreadId}.");
            }
            else
            {
                Console.WriteLine($"Task with Id#{task.Id} was failed to execute inline in Thread#{Environment.CurrentManagedThreadId}.");
            }

            return executedInline;
        }

        protected override bool TryDequeue(Task task)
        {
            Console.WriteLine($"Task with Id#{task.Id} was tried to dequeue in Thread#{Environment.CurrentManagedThreadId}.");

            bool taskDequeued = false;

            lock (_tasksList)
            {
                taskDequeued = _tasksList.Remove(task);
            }

            if (taskDequeued)
            {
                Console.WriteLine($"Task with Id#{task.Id} was dequeued successfully in Thread#{Environment.CurrentManagedThreadId}.");
            }
            else
            {
                Console.WriteLine($"Task with Id#{task.Id} was failed to dequeue in Thread#{Environment.CurrentManagedThreadId}.");
            }

            return taskDequeued;
        }

        private void ExecuteTasks()
        {
            while (true)
            {
                Thread.Sleep(ExecuteTasksDelayMilliseconds);
                Task task = null;

                lock (_tasksList)
                {
                    if (_tasksList.Count == 0)
                    {
                        break;
                    }

                    task = _tasksList.First.Value;
                    _tasksList.RemoveFirst();
                }

                if (task == null)
                {
                    break;
                }

                TryExecuteTask(task);
            }
        }
    }
}
```

> Program.cs
```cs
namespace TaskSchedulers._06_SynchronousTaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SynchronousTaskScheduler taskScheduler = new();

            Task<int>[] tasks = Enumerable.Range(1, 3)
                .Select(i => new Task<int>(PrintIterations, 100))
                .ToArray();

            Array.ForEach(tasks, t => t.Start(taskScheduler));

            Task.WaitAll(tasks);
        }

        private static int PrintIterations(object state)
        {
            int iterationDelay = (int)state;

            int iterationIndex = 0;

            while (iterationIndex < 5)
            {
                iterationIndex++;

                Console.WriteLine($"Task with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(iterationDelay);
            }

            int calculationResult = iterationIndex * iterationDelay;

            return calculationResult;
        }
    }
}
```

## **07 Default ThreadPoolTaskScheduler**
---
**ThreadPoolTaskScheduler** - является планировщиком задач по умолчанию и текущим планировщиком задач, если явно не указан другой. Выполняет задачи в контексте потоков из пула потоков **.NET**, представленного классом **ThreadPool**. Также может выполнять задачи синхронно, если вызваны методы **Wait()**, **WaitAny()**, **WaitAll()**, свойство **Result** или метод **RunSynchronously()**. При запуске задачи без дополнительной настройки планироващика задач её планирование производится планироващиком задач **ThreadPoolTaskScheduler**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        TaskScheduler defaultTaskScheduler = TaskScheduler.Default;
        Console.WriteLine($"Default TaskScheduler is {defaultTaskScheduler.GetType()}.");

        TaskScheduler currentTaskScheduler = TaskScheduler.Default;
        Console.WriteLine($"Current TaskScheduler is {currentTaskScheduler.GetType()}.{Environment.NewLine}");

        ReportThreadPoolState();

        Task<int> task1 = Task.Factory.StartNew(PrintIterations, "AsyncTask1");

        Thread.Sleep(100);
        ReportThreadPoolState();

        Task<int> task2 = new(PrintIterations, "AsyncTask2");
        task2.Start();

        Thread.Sleep(100);
        ReportThreadPoolState();

        Task<int> task3 = Task.Factory.StartNew(PrintIterations, "AsyncTask3");

        Thread.Sleep(100);
        ReportThreadPoolState();

        task1.Wait();
        ReportThreadPoolState();

        task2.Wait();
        ReportThreadPoolState();

        task3.Wait();
        ReportThreadPoolState();
    }

    private static void ReportThreadPoolState()
    {
        ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableIoThreads);
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxIoThreads);

        Console.WriteLine("Available worker threads in the ThreadPool: {0} of {1}", availableWorkerThreads, maxWorkerThreads);
        Console.WriteLine("Available IO threads in the ThreadPool:     {0} of {1}\n", availableIoThreads, maxIoThreads);
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine(
            $"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} " +
            $"has started in Thread#{Environment.CurrentManagedThreadId}. " +
            $"Is this Thread from the ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"{taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }
}
```
