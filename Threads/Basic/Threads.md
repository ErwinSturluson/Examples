# **Потоки (Threads)**

**Псевдо-многозадачность** - выполнение нескольких задач одновременно на одном физическом ядре, фактически выполняя их поочерёдно небольшими частями.

**Истинная многозадачность** - выполнение нескольких задач одновременно параллельно на нескольких физических ядрах.

## **Метод Thread.Join()**
---
Метод **Join** позволяет заблокировать вызывающий поток до момента завершения работы потока, на котором был вызван этот метод:

```cs
private static void Main(string[] args)
{
    Thread thread = new(PrintIterations);

    thread.Start("\tSecondary");

    Thread.Sleep(100);

    thread.Join();

    PrintIterations("Primary");

    Console.ReadKey();
}

private static void PrintIterations(object arg)
{
    int iterationNumber = 0;

    while (iterationNumber < 10)
    {
        iterationNumber++;

        Console.WriteLine($"{arg} - {iterationNumber}");
        Thread.Sleep(100);
    }
}
```
Метод Join инициирует блокировку текущего потока через запрос к **планировщику потоков**.

Возможно составлять различные комбинации ожидания, например, ожидать родительский поток в дочернем потоке, либо ожидать потоки, которые, в свою очередь, ожидают другие потоки:

```cs
internal class Program
{
    private static List<string> _list = new();

    private static void Main(string[] args)
    {
        Thread thread = new(WriteIterations);

        thread.Start("\tSecondary");

        Thread.Sleep(100);

        thread.Join();

        WriteIterations("Primary");

        PrintIterations();

        Console.ReadKey();
    }

    private static void WriteIterations(object arg)
    {
        Thread thread = new(Write);
        thread.Start($"\t{arg} - Secondary");
        thread.Join();

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            _list.Add($"{arg} - {iterationNumber}");
        }
    }

    private static void Write(object arg)
    {
        _list.Add($"{arg}");
    }

    private static void PrintIterations()
    {
        foreach (string item in _list)
        {
            Console.WriteLine(item);
        }
    }
}
```

При использовании ожидания выполнения потока необходимо следить за другими возможными блокировками ожидания, чтобы в программе не возникла взаимная блокировка, приводящая к зависанию задач, когда они будут бесконечно ожидать выполнения друг дурга.

## **Атрибут [ThreadStatic]**
---
По умолчанию **статические поля классов** являются общими для всех потоков.
Чтобы сделать статическое поле индивидуальным для каждого потока, необходимо декорировать его атрибутом **ThreadStatic**:

```cs
internal class Program
{
    [ThreadStatic]
    private static int _iterationNumber = 0;

    private static void Main(string[] args)
    {
        Thread thread = new(PrintIterations);

        thread.Start("\tSecondary");

        Thread.Sleep(100);

        thread.Join();

        PrintIterations("Primary");

        Console.ReadKey();
    }

    private static void PrintIterations(object arg)
    {
        for (int i = 0; i < 10; i++)
        {
            _iterationNumber++;

            Console.WriteLine($"{arg} - {_iterationNumber}");
            Thread.Sleep(100);
        }
    }
}
```
В таком случае, статическая переменная будет храниться в **контексте потока**, или **локальном хранилище потока (Thread Local Storage - TLS)**.

## **Метод Thread.Abort()\***
---
\* Доступен только в .NET Framework на платформе Windows.

Метод **Abort** позволяет прервать выполнение потока, на котором был вызван этот метод:

```cs
private static void Main(string[] args)
{
    Thread thread = new(PrintIterations);

    thread.Start("\tSecondary");

    Thread.Sleep(500);

    thread.Abort();

    PrintIterations("Primary");

    Console.ReadKey();
}

private static void PrintIterations(object arg)
{
    int iterationNumber = 0;

    while (iterationNumber < 10)
    {
        iterationNumber++;

        Console.WriteLine($"{arg} - {iterationNumber}");
        Thread.Sleep(100);
    }
}
```

Для того, чтобы обработать прерывыание выполнения потока, необходимо поместить код метода потока в блок
try, и в блоке catch отлавливать исключение **ThreadAbortException**.

## **Свойство Priority**
---
**Свойство Priority** позволяет указать приоритет потока. Его приоритет влияет на то, насколько часто планировщик потоков будет передавать его на выполнение процессору, а также на количество выделяемых на его выполнение квантов времени.

Приоритеты представлены перечислением **ThreadPriority**, содержащем следующие значения:
- Lowest
- BelowNormal
- Normal
- AboveNormal
- Highest

нативные потоки, например, в платформе Windows, содержат гораздо больше приоритетов, но в .NET представлены 5.

По умолчанию все потоки имеют приоритет **Normal**.

При выдаче потокам приоритетов, их выполнение всё равно будет сменяться на выполнение других потоков даже при установке им приоритета **Highest**, но они будут выполняться на процессоре чаще и дольше, чем другие потоки. Поэтому, для демонстрации приоритетов лучше всего продемонстрировать количество работы, выполненное потоками с разными приоритетами за одинаковое количество времени, а не время их завершения:

```cs
internal class Program
{
    private static bool _secondaryThreadsEnable = false;

    private static void Main(string[] args)
    {
        int processorNumber = Environment.ProcessorCount;

        if (processorNumber > 1)
        {
            Thread[] threads = new Thread[processorNumber - 1];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(CalculateIterations);
                threads[i].Priority = ThreadPriority.Highest;
            }

            threads[0].Priority = ThreadPriority.Lowest;

            _secondaryThreadsEnable = true;

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start($"Secondary - {threads[i].Priority}");
            }

            Thread.Sleep(3000);

            Console.WriteLine("Thread \"Primary\" has stopped \"Secondary\" threads.");

            _secondaryThreadsEnable = false;
        }
        else
        {
            throw new NotSupportedException($"{nameof(processorNumber)}:{processorNumber}");
        }

        Console.ReadKey();
    }

    private static void CalculateIterations(object arg)
    {
        Console.WriteLine($"Thread {arg} has started.");

        long iterationNumber = 1;

        while (_secondaryThreadsEnable)
        {
            iterationNumber++;
        }

        decimal result = Convert.ToDecimal(iterationNumber) / 1_000_000;

        Console.WriteLine($"Thread \"{arg}\" has stopped with result of \"{decimal.Round(result).ToString("#,#")}\".");
    }
}
```

Несмотря на такую лёгкодоступность управления приоритетами потоков, не рекомендуется менять их приоритеты, так как на конечной машине пользователя могут иметься другие приложения, работа которых может нарушиться. Кроме этого, нарушиться может работа даже собственного приложения разработчика, который изменил приоритеты потоков без необходимости.

## **Класс Interlocked**
---
Класс **Interlocked** позволяет из разных потоков безопасно и синхронно получать доступ к управляемым разделяемым ресурсам, например, к числовым переменным для инкрементации и декрементации.

Класс **Interlocked** содержит ряд полезных статических потокобезопасных методов:

- Add - добавляет к одному числу другое, результат помещает на место первого числа, выполняя все операции как одну атомарную операцию;
- And - побитово складывает 2 числа, результат помещает на место первого числа, выполняя все операции как одну атомарную операцию;
- CompareExchange - принимает 3 аргумента, сравнивает первый и третий аргументы, и, если они равны, замещает значение первого аргумента значением второго аргумента и возвращает оригинальное значение первого аргумента;
- Decrement - уменьшает переданное число на 1, выполняя все операции как одну атомарную операцию;
- Exchange - замещает значение первого аргумента значением второго аргумента и возвращает оригинальное значение первого аргумента;
- Increment - увеличивает переданное число на 1, выполняя все операции как одну атомарную операцию;
- MemoryBarrier - TODO;
- MemoryBarrierProcessWide - TODO;
- Or - побитово умножает 2 числа, результат помещает на место первого числа, выполняя все операции как одну атомарную операцию;
- Read - выполняет чтение значения переданной переменной как атомарую операцию.

Например, при инкременте общего целочисленного поля класса из разных потоков итоговое значение этого поля может быть не равно сумме всех инкрементаций всех потоков. Это происходит потому, что при переключении потоков в процессоре возможно прерывание выполнения потока, не завершившего инкремент, а после совершения инкремента другими потоками, ранее выгруженный поток снова загружается в процессор и инкрементирует переменную к тому значению, к которому инкрементировал до выгрузки и до того, как другие потоки совершили свои инкрементации.

TODO: подробный и простой пример с картинками;

Например, поле, которое инкрементируют 10 потоков по 1_000_000 раз, может в итоге иметь значение не 10*1_000_000=10_000_000, а всего лишь ~200_000-250_000:

```cs
internal class Program
{
    private static int _sharedField = 0;

    private static void Main(string[] args)
    {
        Thread[] threads = new Thread[10];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(CalculateIterations);
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start($"Secondary - {threads[i].Priority}");
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }

        Console.WriteLine($"Expected result of Shared Field: 10,000,000, Actual result: {_sharedField.ToString("#,#")}");

        Console.ReadKey();
    }

    private static void CalculateIterations(object arg)
    {
        Console.WriteLine($"Thread {arg} has started.");

        for (int i = 0; i < 1_000_000; i++)
        {
            _sharedField++;
        }

        Console.WriteLine($"Thread \"{arg}\" has finished with result of \"1,000,000\" iterations.");
    }
}
```

Для исправления ошибки можно применить конструкцию **Lock**:

```cs
internal class Program
{
    private static object _sharedFieldLock = new();
    private static int _sharedField = 0;

    private static void Main(string[] args)
    {
        Thread[] threads = new Thread[10];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(CalculateIterations);
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start($"Secondary - {threads[i].Priority}");
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }

        Console.WriteLine($"Expected result of Shared Field: 10,000,000, Actual result: {_sharedField.ToString("#,#")}");

        Console.ReadKey();
    }

    private static void CalculateIterations(object arg)
    {
        Console.WriteLine($"Thread {arg} has started.");

        for (int i = 0; i < 1_000_000; i++)
        {
            lock (_sharedFieldLock)
            {
                _sharedField++;
            }
        }

        Console.WriteLine($"Thread \"{arg}\" has finished with result of \"1,000,000\" iterations.");
    }
}
```

Но лучше всего такие задачи решать при помощи класса **Interlocked** и его статического метода **Increment**:

```cs
internal class Program
{
    private static object _sharedFieldLock = new();
    private static int _sharedField = 0;

    private static void Main(string[] args)
    {
        Thread[] threads = new Thread[10];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(CalculateIterations);
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start($"Secondary#{i}");
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }

        Console.WriteLine($"Expected result of Shared Field: 10,000,000, Actual result: {_sharedField.ToString("#,#")}");

        Console.ReadKey();
    }

    private static void CalculateIterations(object arg)
    {
        Console.WriteLine($"Thread {arg} has started.");

        for (int i = 0; i < 1_000_000; i++)
        {
            Interlocked.Increment(ref _sharedField);
        }

        Console.WriteLine($"Thread \"{arg}\" has finished with result of \"1,000,000\" iterations.");
    }
}
```
