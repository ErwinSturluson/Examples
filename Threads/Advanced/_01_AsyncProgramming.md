# **Async Programming**

**Синхронность** - в программировании это последовательное выполнение чатей программ в контексте одного потока.

**Асинхронность** - в программировании это параллельное выполнение частей программ в контексте вторичного потока относительно родительского потока.

Или по отношению к .NET:

**Синхронность** - вызов методв в контексте текущего потока:

> TODO: изображение или GIF  синхронного выполнения

Синхронные методы выполняют свою работу перед возвратом управления вызывающему коду через оператор **return**.

**Асинхронность** - выполнение метода в контексте вторичного потока:

> TODO: изображение или GIF асинхронного выполнения

Асинххронные методы выполняют большую часть работы, либо всю работу уже после возврата управления вызывающему коду.

В системе с одним ядром асинхронная задача будет выполняться в контексте вторичного потока, логически параллельно родителькому потоку, однако физически обе задачи будут выполняться по частям, и в один момент времени на единственном доступном ядре будет выполняться только одна часть одной из задач.

В системе с более один ядром в предполагаемых идеальных условиях синхронная задача будет выполняться в контексте вторичного потока, параллельно родителькому потоку, как логически, так и физически.

**Поток выполнения** - наименьшая единица обработки для параллеьного выполнения отдельных частей одной программы.

**Многопоточные приложения** могут решать более одной задачи одновременно.

**Многозадачность** - возможность выполнять более одной задачи одновременно.

**Многозадачность на основе потоков** означает параллельное выполнение отдельных частей приложения.

Физически задачи могут выполняться **псевдопараллельно**, ведь количество параллельно запущенных потоков во всех процессах может быть больше, чем количество процессоров (или ядер в одном процессоре) машины, на которой происходит выполнение, из-за чего задачи могут выполняются частями поочерёдно как на одном физическом ядре, так и на разных, в зависимости от решения **планировщика потоков**.

**Асинхронное программирование** - подход к написанию кода, который позволяет выполнять второстепенные и долговыполняемые задачи, не блокируя основной поток выполнения. Благодаря этому увеличивается производительность и снижается нагрузка в приложениях. Иначе говоря, одни задачи не блокируются для ожидания других задач.

**Параллельное программирование** - физическое выполнение нескольких операций одновременно. Достигается путём аппаратных возможностей вычислительной техники, а именно - благодаря наличию нескольких ядер.

**Случаи применения асинхронности**:
- Наличие у приложения пользовательского интерфейса, чтобы выполняемые задачи не блокировали выполнение пользовательского интерфейса;
- Второстепенные задачи;
- Одновременная обработка нескольких клиентских запросов;
- Запросы в базу данных;
- Работа с файловой системой;
- Сетевые запросы.

## **01 Класс Thread**
---
Для работы с потоками в **.NET** представлен класс **Thread**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Thread thread = new(PrintIterations);

        thread.Start("\tSecondary");

        Thread.Sleep(100);

        PrintIterations("Primary");

        thread.Join();

        Console.ReadKey();
    }

    private static void PrintIterations(object arg)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"{arg} thread - {iterationNumber}");
            Thread.Sleep(100);
        }
    }
}
```
Более подробно класс **Thread** был разобран в предыдущих разделах.


## **02 Класс ThreadPool**
---
**Пул потоков** - это коллекция потоков, которые могут использоваться для выполнения методов в фоновом режиме.

Создание потоков через класс **Thread** считается слишком накладным действием в приложении, которое требует значительных затрат времени и ресурсов. 

Также возможно создать множество потоков, которые просто будут простаивать, или, если их будет слишком много, операционной системе придётся затрачивать значительные ресурсы для планированиия их выполнения и осуществления их переключения для выполнения.

Уничтожение потока - также очень трудоёмкий процесс. Пул потоков не уничтожает потоки, а возвращает их в пул после окончания работы.

**Пул потоков** был создан для упрощения второстепенной логики обработки задач, а также снижения затрат ресурсов компьютера за счёт **переиспользования** потоков из пула.

**Пул потоков** в **.NET** представлен классом **ThreadPool**.

**Пул потоков** создаётся CLR отдельно для каждого приложения.

Для выполнения метода асинхронно с помощью **ThreadPool**, необходимо поместить его в очередь на выполнение **ThreadPool**, а сам **ThreadPool** уже будет извлекать методы из очереди и отправлять на выполнение согласно своей логике.

**ThreadPool** выделит столько потоков для выполнения параллельных задач, сколько необходимо для скорейшего выполнения всех помещённых в очередь задач, однако имеет лимит одновременно выделяемых потоков - по умолчанию это 1024 потока и это число может меняться в зависимости от платформы и машины, на которых выполняется приложение.

> TODO: изображение или GIF работы пула потоков

Пример использования **ThreadPool**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Program has started.");
        Report();

        ThreadPool.QueueUserWorkItem(new WaitCallback(Task1));
        Report();

        ThreadPool.QueueUserWorkItem(Task2);
        Report();

        Thread.Sleep(3000);
        Console.WriteLine("Program has finished.");
        Report();
    }

    private static void Task1(object state)
    {
        Thread.CurrentThread.Name = "1";
        Console.WriteLine("Thread {0} has started\n", Thread.CurrentThread.Name);
        Thread.Sleep(2000);
        Console.WriteLine("Thread {0} has finished\n", Thread.CurrentThread.Name);
    }

    private static void Task2(object state)
    {
        Thread.CurrentThread.Name = "2";
        Console.WriteLine("Thread {0} has started\n", Thread.CurrentThread.Name);
        Thread.Sleep(500);
        Console.WriteLine("Thread {0} has finished\n", Thread.CurrentThread.Name);
    }

    private static void Report()
    {
        Thread.Sleep(200);
        ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableIoThreads);
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxIoThreads);

        Console.WriteLine("Available worker threads in the ThreadPool: {0} of {1}", availableWorkerThreads, maxWorkerThreads);
        Console.WriteLine("Available IO threads in the ThreadPool:     {0} of {1}\n", availableIoThreads, maxIoThreads);
    }
}
```

## **03 Wrapper over ThreadPool**
---

В будущем будут расмотрены готовые многофункциональные инструменты, которых достаточно, чтобы создавать любую логику обработки асинхронных задач, однако, могут возникнуть специфичые требования, для которых понадобится обращаться к пулу потоков.

Для удобства работы с пулом потоков можно создать класс-обёртку, который позволит ожидать выполняемые задачи, получать возвращаемые значения и обрабатывать возникшие исплючения.

Код класса-обёртки над **ThreadPool**:

```cs
namespace AsyncProgramming._03_ThreadPoolWrapper
{
    internal class ThreadPoolWrapper<TArg, TResult>
    {
        private readonly ManualResetEvent _lock = new(false);
        private readonly Func<TArg, TResult> _func;

        private Exception _exception;
        private TResult _result;
        private bool _completed;
        private bool _completedSuccessfully;

        public ThreadPoolWrapper(Func<TArg, TResult> func)
        {
            _func = func;
        }

        public bool Completed => _completed;

        public TResult Result
        {
            get
            {
                Wait();
                return _result;
            }
        }

        public bool CompletedSuccessfully => _completedSuccessfully;

        public void Start(TArg arg)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(MethodWrapper), arg);
        }

        public void Wait()
        {
            _lock.WaitOne();

            if (!_completedSuccessfully)
            {
                throw _exception;
            }
        }

        private void MethodWrapper(object state)
        {
            try
            {
                TArg arg = (TArg)state;

                _result = _func.Invoke(arg);

                _completedSuccessfully = true;
            }
            catch (Exception ex)
            {
                _completedSuccessfully = false;
                _exception = ex;
            }
            finally
            {
                _completed = true;
                _lock.Set();
            }
        }
    }
}
```

Пример использования класса-обёртки над **ThreadPool**:

```cs
namespace AsyncProgramming._03_ThreadPoolWrapper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ThreadPoolWrapper<int, int> task1 = new(PrintIterations);
            task1.Start(10);

            int syncTask1Resut = PrintIterations(5);
            Console.WriteLine($"The result of sync task:{syncTask1Resut}.");

            int asyncTaskResut = task1.Result;
            Console.WriteLine($"The result of async task 1:{asyncTaskResut}.");

            try
            {
                ThreadPoolWrapper<int, int> task = new(PrintIterations);
                task.Start(-1);

                int asyncTask2Resut = task.Result;
                Console.WriteLine($"The result of async task 2:{asyncTaskResut}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception was occured in task 2: {ex.GetType()}{Environment.NewLine}{ex.Message}");
            }
        }

        private static int PrintIterations(int iterationsNumber)
        {
            if (iterationsNumber < 0) throw new ArgumentOutOfRangeException(nameof(iterationsNumber), $"Incorrect value: {iterationsNumber}.");

            int iterationIndex = 0;

            while (iterationIndex < iterationsNumber)
            {
                iterationIndex++;

                Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            int calculatedResult = iterationIndex * 100;

            return calculatedResult;
        }
    }
}
```