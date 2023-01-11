# **Синхронизация потоков (ThreadsSynchronisation)**

## **Синхронизация потоков при помощи объектов ядра ОС Windows**

Существуют **объекты синхронизации уровня CLR**: конструкция **lock**, классы **Monitor** и **Interlocked**.

**Ядро операционной системы** - это программа, которая как содержит другие небольшие программы, так и управляет пользовательскими приложениями.

В .NET существуют специальные удобные объекты-обёртки над объектами синхронизации ОС, дающие возможность работать с объектами синхронизации ОС без обращения к ним напрямую через API ОС.

## **Ключевое слово Volatile**
---
В процессоре существуют микросхемы, которые называются "Буфферы предсказания переходов и ветвлений". В ряде случаев эти микросхемы, нацеленные на оптимизацию, могут нарушить ход выполнения программы, задуманный разработчиком. Особенно это выражено при работе с многопоточностью и разделяемыми ресурсами.



Для устранения логических ошибок, связанных с ненужными оптимизациями компилятора в многопоточной среде при работе с полями, можно использовать **ключевое слово volatile**.

Поля, помеченные ключевым словом **volatile**, не проходят оптимизацию компилятором.

Ключевое слово Volatile можно применять только к полям класса или структуры, поля (значения) перечислений, а также локальные переменные не могут быть объявлены как **volatile**.

Ключевое слово Volatile можно применять к полям **следующих типов**:
- Ссылочные типы;
- Простейшие типы **sbyte, byte, short, ushort, int, uint, char, float, bool**;
- Тип перечисления с одним из следующих базовых типов: **byte, sbyte, short, ushort, int, uint**;
- Параметрам универсальных типов, являющихся ссылочными типами.

Пример использования ключевого слова **volatile** для исключения ненужных оптимизаций компилятора, связанных с полем:

```cs
internal class Program
{
    private static volatile bool _secondaryThreadsEnable = true; // Without JIT optimization. Works only in Release mode.
    //private static bool _secondaryThreadsEnable = false; // With JIT optimization.

    private static void Main(string[] args)
    {
        Thread thread = new(CalculateIterations);

        thread.Start("\tSecondary");

        Thread.Sleep(1000);

        _secondaryThreadsEnable = false;

        thread.Join();
    }

    private static void CalculateIterations(object arg)
    {
        Console.WriteLine($"Thread {arg} has started.");

        long iterationNumber = 1;

        while (_secondaryThreadsEnable)
        {
            iterationNumber++;

            Console.WriteLine($"Thread \"{arg}\" is executing iteration #{iterationNumber}.");
            Thread.Sleep(100);
        }

        Console.WriteLine($"Thread \"{arg}\" has stopped after {iterationNumber} iterations.");
    }
}
```

Запуская в параллельном потоке метод, в котором есть цикл, проверяющийся значением переменной извне и не меняющий её значение в своём теле, оптимизируется JIT компилятором так, что проверка условия входа в цикл и значения этой переменной происходит только при первой итерации, что исключает возможность прервать этот цикл, изменив значение переменной из другого потока.

Код оптимизированного цикла в режиме **Release** без ключевого слова **volatile**:

```cs
if (_secondaryThreadsEnable != false)
{
    Label:
    iterationNumber++;
    Console.WriteLine($"Thread \"{arg}\" is executing iteration #{iterationNumber}.");
    Thread.Sleep(100);
    goto Label;
}
```

Без ключевого слова **volatile** оптимизации, не позволяющие остановиться циклу во вторичном потоке, будут внесены компилятором только в режиме **Release**. В режиме **Debug** изменений не будет, так как в этом режиме оптимизации компилятора не применяются.

## **Методы Thread.VolatileWrite() и Thread.VolatileRead()**
---
Вместо использования ключевого слова **volatile** можно использовать методы класса **Thread** **VolatileWrite** и **VolatileRead**, однако у них нет перегрузки для типа данных **bool**:

```cs
internal class Program
{
    private static int _secondaryThreadsEnable = 1;

    private static void Main(string[] args)
    {
        Thread thread = new(CalculateIterations);

        thread.Start("\tSecondary");

        Thread.Sleep(1000);

        Thread.VolatileWrite(ref _secondaryThreadsEnable, 0);

        thread.Join();
    }

    private static void CalculateIterations(object arg)
    {
        Console.WriteLine($"Thread {arg} has started.");

        long iterationNumber = 1;

        while (Thread.VolatileRead(ref _secondaryThreadsEnable) != 0)
        {
            iterationNumber++;

            Console.WriteLine($"Thread \"{arg}\" is executing iteration #{iterationNumber}.");
            Thread.Sleep(100);
        }

        Console.WriteLine($"Thread \"{arg}\" has stopped after {iterationNumber} iterations.");
    }
}
```

## **Пул потоков (ThreadPool)**
---
**Пул потоков (ThreadPool)** - это коллекция потоков, которые могут использоваться для выполнения нескольких задач в фоновом режиме.

В .NET пул потоков представлен классом **ThreadPool**, который является коллекцией элементов типа **Thread**.

Потоки, попадающие в коллекцию, называются **задачами**.

При запуске большего количества потоков, чем допустимо к размещению в коллекции, не вошедшие потоки выстраиваются в **очередь задач**, ожидающих выполнения.

**Рабочие потоки** – потоки, которые запускаются пользователем.
**Потоки ввода-вывода** – потоки, работающие с портами ввода-вывода (отправка информации на диск или любое другое устройство).

Метод **GetAvailableThreads(out int, out int)** возвращает количество рабочих потоков и потоков ввода-вывода, доступных (свободных) в данный момент.

Метод **GetMaxThreads(out int, out int)** возвращает максимально возможное количество рабочих потоков и потоков ввода-вывода.

**Асинхронные методы** принято называть **Task**.

Метод **QueueUserWorkItem()** добавляет в свою очередь задачу типа делегата **WaitCallback**, переданную в качестве аргумента.

Пример использования пула потоков с отображением состояния пула:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Program start.");
        Report();

        ThreadPool.QueueUserWorkItem(new WaitCallback(Task1));
        Report();

        ThreadPool.QueueUserWorkItem(Task2);
        Report();

        Thread.Sleep(3000);
        Console.WriteLine("Program end.");
        Report();
    }

    private static void Task1(object state)
    {
        Thread.CurrentThread.Name = "1";
        Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
        Thread.Sleep(2000);
        Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
    }

    private static void Task2(object state)
    {
        Thread.CurrentThread.Name = "2";
        Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
        Thread.Sleep(500);
        Console.WriteLine("Thread {0} started\n", Thread.CurrentThread.Name);
    }

    private static void Report()
    {
        Thread.Sleep(200);
        int availableWorkerThreads, availableIoThreads, maxWorkerThreads, maxIoThreads;
        ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableIoThreads);
        ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIoThreads);

        Console.WriteLine("Available worker threads in the ThreadPool: {0} of {1}", availableWorkerThreads, maxWorkerThreads);
        Console.WriteLine("Available IO threads in the ThreadPool:     {0} of {1}\n", availableIoThreads, maxIoThreads);
    }
}
```

## **Мьютекс (Mutex)**
---
**Mutex (Mutual Exclusion, взаимное исключение)** – примитив синхронизации, который также может использоваться в межпроцессорной синхронизации. Функционирует аналогично AutoResetEvent, но снабжён дополнительной логикой: 

- Запоминает, какой поток им владеет. ReleaseMutex не может вызвать поток, который не владеет мьютексом;
- Управляет рекурсивным счётчиком, указывающим, сколько раз поток-владелец уже владел объектом.

Мьютекс позволяет выполнять защищённую секцию кода **только одному потоку одновременно**.

Методы **WaitOne()** и **ReleaseMutex()** экземпляра класса **Mutex служат** «турникетами» для потоков на вход и на выход.

При передаче имени мьютекса в параметре конструктора создаётся **межпроцессный** мьютекс, без передачи имени – **внутрипроцессный**:

```cs
internal class Program
{
    private static Mutex _mutex = new(false, typeof(Program).FullName);

    private static void Main(string[] args)
    {
        Thread[] threads = new Thread[3];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(PrintIterations);
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start($"Secondary#{i}");
        }
    }

    private static void PrintIterations(object arg)
    {
        _mutex.WaitOne();

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"{arg} - {iterationNumber}");
            Thread.Sleep(100);
        }

        _mutex.ReleaseMutex();
    }
}
```

## **Рекурсивная блокировка (Recursive Locking)**
---
**Рекурсивная блокировка** - это блокировка, которая при блокировке сначала определяет, удерживается ли она уже, и если да, то просто позволяет коду получить ее рекурсивно.

Существует 2 реализации рекурсивной блокировки:
- При рекурсивном вызове метода, где этот вызов происходит после удержания мьютекса, но до его освобождения, и внутри рекурсивного вызова мьютекс каждый раз удерживается заново и выполнение метода не блокируется, так как вызов производится из одного и того же потока;
- При обычном вызове метода внутри другого метода, имеющего удержание мьютекса, где этот вызов происходит после удержания мьютекса, но до его освобождения, мьютекс удерживается заново и выполнение метода не блокируется, так как вызов производится из одного и того же потока.

Пример второго варианта рекурсивной блокировки:

```cs
internal class Program
{
    private static Mutex _mutex = new();

    private static void Main(string[] args)
    {
        Console.WriteLine($"{nameof(Main)} in Thread#{Environment.CurrentManagedThreadId} has started.");

        Thread thread1 = new(Method1);
        Thread thread2 = new(Method2);

        thread1.Start();
        Thread.Sleep(100);
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine($"{nameof(Main)} in Thread#{Environment.CurrentManagedThreadId} has finished.");
    }

    private static void Method1()
    {
        _mutex.WaitOne();
        Thread.Sleep(100);
        Console.WriteLine($"{nameof(Method1)} in Thread#{Environment.CurrentManagedThreadId} has started.");
        Method2();
        _mutex.ReleaseMutex();
        Console.WriteLine($"{nameof(Method1)} in Thread#{Environment.CurrentManagedThreadId} has finished.");
    }

    private static void Method2()
    {
        _mutex.WaitOne();
        Console.WriteLine($"{nameof(Method2)} in Thread#{Environment.CurrentManagedThreadId} has started.");
        Thread.Sleep(1000);
        _mutex.ReleaseMutex();
        Console.WriteLine($"{nameof(Method2)} in Thread#{Environment.CurrentManagedThreadId} has finished.");
    }
}
```

Рекурсивная блокировка в данном примере может быть в несколько шагов:
1. В разных потоках запускаются **Method1** и **Method2**, оба из которых имеют удержание мьютекса в своих телах;
2. Первый поток с **Method1** запускается раньше, чем **Method2**, и первым удерживает мьютекс;
3. Второй поток с **Method2** запускается позже, чем **Method1**, и первым ожидает освобождения мьютекса;
4. **Method1** первого потока внутри своего тела после удержания мьютекса, но до его освобождения, вызывает **Method2**;
5. Внутри **Method2**, вызванного первым потоком из **Method1**, выполнение не блокируется на попытке удержания мьютекса, который уже был удержан в коде **Method1**, так как удержание было произведено из этого же потока, даже несмотря на то, что второй поток раньше зашёл в **Method2** и раньше начал ожидать освобождения мьютекса в этом месте;
6. Первый поток повторно удерживает мьютекс в **Method2**. При этом, освобождаться мьютекс должен столько же раз, сколько удерживался;
7. Первый поток освобождает мьютекс сначала в **Method2**, затем в **Method1**, когда ему вернулось управление;
8. После того, как первый поток освободил мьютекс столько же раз, сколько удерживал, второй поток в **Method2** разблокируется и сможет удержать мьютекс, после чего выполнится.

В итоге, можно сделать вывод, что, если один поток удерживает мьютекс, то ему разрешено удерживать его повторно необходимое количество раз. При этом, необходимо освободить мьютекс такое же количество раз, сколько раз он удерживался. 

Остальные потоки, требующие удержания этого же мьютекса, будут ожидать, пока поток, который первым заблокировал мьютекс, не освободит его такое же количество раз, сколько раз его удерживал.

## **Семафор (Semaphore)**
---
**Семафор (Semaphore)** используется для управления доступом к пулу ресурсов. Потоки занимают слот семафора, вызывая метод **WaitOne()** и освобождают занятый слот вызовом метода **Release()**.

В .NET семафор представлен классом **Semaphore**.

Конструктор класса Semaphore имеет 3 параметра:
1. Текущее количество потоков, которым разрешён доступ в защищённую секцию;
2. Максимальное количество потоков, которым может быть разрешён доступ в защищённую секцию. Текущее количество потоков, которым разрешён доступ в защищённую секцию может быть изменено перегрузкой метода **Release(int releaseCount)**, но не может превысить значение этого параметра;
3. Имя семафора. Необязательный параметр. При передаче имени семафора создаётся **межпроцессный** семафор, без передачи имени – **внутрипроцессный**.

Пример контроля количества входящих в защищённую секцию потоков с последующим увеличением этого количества до максимального:

```cs
internal class Program
{
    private static Semaphore _semaphore = new(2, 4, typeof(Program).FullName);

    private static void Main(string[] args)
    {
        MakeThreads(4);
        Console.WriteLine();

        _semaphore.Release(2);

        MakeThreads(4);

        Console.WriteLine();
    }

    private static void Calculate(object arg)
    {
        _semaphore.WaitOne();

        Thread.Sleep(100);

        Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} has entered the protected section.");

        Thread.Sleep(1000);

        Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} has left the protected section.");

        Thread.Sleep(100);

        _semaphore.Release();
    }

    private static void MakeThreads(int threadsNumber)
    {
        Thread[] threads = new Thread[threadsNumber];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(Calculate);
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start($"Secondary#{i + 1}");
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }
    }
}
```

## **Легковесный семафор (SemaphoreSlim)**
---
**Легковесный семафор (SemaphoreSlim)** - это семафор в .NET, который не использует средства ОС для синхронизации потоков созданных классом Thread внутри .NET и является более производительным решением в рамках одного .NET приложения, если не требуется синхронизация из разных процессов:

```cs
internal class Program
{
    private static SemaphoreSlim _semaphoreSlim = new(2, 4, );

    private static void Main(string[] args)
    {
        MakeThreads(4);
        Console.WriteLine();

        _semaphoreSlim.Release(2);

        MakeThreads(4);

        Console.WriteLine();
    }

    private static void Calculate(object arg)
    {
        _semaphoreSlim.Wait();

        Thread.Sleep(100);

        Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} has entered the protected section.");

        Thread.Sleep(1000);

        Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} has left the protected section.");

        Thread.Sleep(100);

        _semaphoreSlim.Release();
    }

    private static void MakeThreads(int threadsNumber)
    {
        Thread[] threads = new Thread[threadsNumber];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(Calculate);
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start($"Secondary#{i + 1}");
        }

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }
    }
}
```

## **Автоматически сбрасываемое событие (AutoResetEvent)**
---
**Автоматически сбрасываемое событие (AutoResetEvent)** - это объект, который уведомляет сигнальным состоянием ожидающий поток о том, что произошло событие, после которого можно продолжить выполнение. В параметре конструктора передаётся **bool** значение: **true** – создаётся в сигнальном состоянии, **false** – в не сигнальном состоянии. Автоматически сбрасываемом событием оно называется потому, что сбрасывает сигнальное состояние автоматически после снятия блокировки с первого потока, который ожидал этого сигнала.

Метод **WaitOne()** приостанавливает поток и ожидает установки события в сигнальное состояние, метод **Set()** устанавливает событие в сигнальное состояние для одного потока:

```cs
internal class Program
{
    private static AutoResetEvent _autoResetEvent = new(false);

    private static void Main(string[] args)
    {
        Thread thread = new(PrintIterations);

        thread.Start("Secondary");

        Thread.Sleep(100);

        for (int i = 0; i < 3; i++)
        {
            Console.Write("Press any key to set the event: ");
            Console.ReadKey();
            _autoResetEvent.Set();
            Thread.Sleep(100);
        }
    }

    private static void PrintIterations(object arg)
    {
        int iterationNumber = 0;

        while (iterationNumber < 3)
        {
            _autoResetEvent.WaitOne();
            iterationNumber++;
            Console.WriteLine($"{Environment.NewLine}{arg} - {iterationNumber}");
        }

        Console.WriteLine();
    }
}
```

Также возможно ожидания одного события несколькими потоками. В таком случае, блокировка снимается с того потока, который первым обнаружил сигнальное состояние события, и перевёл его обратно в несигнальное состояние, поэтому другие потоки будут дожидаться повторных установок события в сигнальное состояние:

```cs
internal class Program
{
    private static AutoResetEvent _autoResetEvent = new(false);

    private static void Main(string[] args)
    {
        Thread thread1 = new(PrintIterations);
        Thread thread2 = new(PrintIterations);

        thread1.Start("Secondary");
        thread2.Start("Secondary");
        Thread.Sleep(100);

        Console.Write("Press any key to set the event for the first thread: ");
        Console.ReadKey();
        Console.WriteLine();
        _autoResetEvent.Set();
        Thread.Sleep(1000);

        Console.Write("Press any key to set the event for the second thread: ");
        Console.ReadKey();
        Console.WriteLine();
        _autoResetEvent.Set();
    }

    private static void PrintIterations(object arg)
    {
        _autoResetEvent.WaitOne();

        int iterationNumber = 0;

        while (iterationNumber < 3)
        {
            iterationNumber++;
            Console.WriteLine($"{arg} - {iterationNumber}");
            Thread.Sleep(200);
        }

        Console.WriteLine();
    }
}
```

## **События, сбрасываемые вручную (ManualResetEvent)**
---
**События, сбрасываемые вручную (ManualResetEvent)** - это объект, который уведомляет сигнальным состоянием ожидающий поток о том, что произошло событие, после которого можно продолжить выполнение. В параметре конструктора передаётся **bool** значение: **true** – создаётся в сигнальном состоянии, **false** – в не сигнальном состоянии. Событием,сбрасываемом вручную оно называется потому, что сбрасывает сигнальное состояние только после вызова метода **Reset()**, а до этого момента все потоки, ожидавшие сигнального состояния события, будут разблокированы для выполнения.

Метод **WaitOne()** приостанавливает поток и ожидает установки события в сигнальное состояние, метод **Set()** устанавливает событие в сигнальное состояние для всех потоков, метод **Reset()** устанавливает событие в несигнальное состояние:

```cs
internal class Program
{
    private static ManualResetEvent _manualResetEvent = new(false);

    private static void Main(string[] args)
    {
        Thread thread1 = new(PrintIterations);
        Thread thread2 = new(PrintIterations);

        thread1.Start("Secondary 1");
        thread2.Start("Secondary 2");
        Thread.Sleep(100);

        Console.Write("Press any key to set the event for all threads: ");
        Console.ReadKey();
        Console.WriteLine();
        _manualResetEvent.Set();
        Thread.Sleep(1000);

        Console.Write("Press any key to reset the event: ");
        Console.ReadKey();
        Console.WriteLine();
        _manualResetEvent.Reset();

        Thread thread3 = new(PrintIterations);
        Thread thread4 = new(PrintIterations);

        thread3.Start("Secondary 3");
        thread4.Start("Secondary 4");
        Thread.Sleep(100);

        Console.Write("Press any key to set the event for all remained threads: ");
        Console.ReadKey();
        Console.WriteLine();
        _manualResetEvent.Set();
        Thread.Sleep(1000);
    }

    private static void PrintIterations(object arg)
    {
        _manualResetEvent.WaitOne();

        int iterationNumber = 0;

        while (iterationNumber < 3)
        {
            iterationNumber++;
            Console.WriteLine($"{arg} - {iterationNumber}");
            Thread.Sleep(200);
        }

        Console.WriteLine();
    }
}
```

## **Легковесные события, сбрасываемые вручную (ManualResetEvent)**
---
**Легковесные события, сбрасываемые вручную (ManualResetEvent)** - это события, сбрасываемые вручную в .NET, которые не используют средства ОС для синхронизации потоков созданных классом Thread внутри .NET и являются более производительным решением в рамках одного .NET приложения, если не требуется синхронизация из разных процессов:

```cs
internal class Program
{
    private static ManualResetEventSlim _manualResetEventSlim = new(false);

    private static void Main(string[] args)
    {
        Thread thread1 = new(PrintIterations);
        Thread thread2 = new(PrintIterations);

        thread1.Start("Secondary 1");
        thread2.Start("Secondary 2");
        Thread.Sleep(100);

        Console.Write("Press any key to set the event for all threads: ");
        Console.ReadKey();
        Console.WriteLine();
        _manualResetEventSlim.Set();
        Thread.Sleep(1000);

        Console.Write("Press any key to reset the event: ");
        Console.ReadKey();
        Console.WriteLine();
        _manualResetEventSlim.Reset();

        Thread thread3 = new(PrintIterations);
        Thread thread4 = new(PrintIterations);

        thread3.Start("Secondary 3");
        thread4.Start("Secondary 4");
        Thread.Sleep(100);

        Console.Write("Press any key to set the event for all remained threads: ");
        Console.ReadKey();
        Console.WriteLine();
        _manualResetEventSlim.Set();
        Thread.Sleep(1000);
    }

    private static void PrintIterations(object arg)
    {
        _manualResetEventSlim.Wait();

        int iterationNumber = 0;

        while (iterationNumber < 3)
        {
            iterationNumber++;
            Console.WriteLine($"{arg} - {iterationNumber}");
            Thread.Sleep(200);
        }

        Console.WriteLine();
    }
}
```

## **RegisteredWaitHandle**
---
**RegisteredWaitHandle** - это класс в .NET, который предназначен для асинхронного интервального вызова метода.

Делегат **WaitOrTimerCallback** – делегат, экземпляр которого передаваётся в пул потоков через метод **RegisterWaitForSingleObject()**. С этим делегатом сообщается будущий метод обратного вызова - метод, который будет вызываться автоматически при возникновении какого-либо события.

Метод **RegisterWaitForSingleObject()** класса **ThreadPool** предназначен для запуска методов с определённым интервалом, возвращая RegisterWaitHandle, сразу запуская работу метода.

Параметры метода **RegisterWaitForSingleObject()**:
1. **WaitHandle waitObject** – объект синхронизации, от которого нужно ждать сигнал;
2. **WaitOrTimerCallback callBack** – экземпляр делегата, с которым сообщён метод, который необходимо выполнять;
3. **object state** – первый аргумент метода, который необходимо выполнить;
4. **int millisecondsTimeOutInterval** – интервал выполнения в миллисекундах;
5. **bool executeOnlyOnce** – **true** - вызвать метод один раз, **false** - вызывать множество раз с установленным интервалом.

Метод **Set()** экземпляра типа **WaitHandle** дополнительно устанавливает событие в сигнальное состояние, что способствует вызову метода до истечения интервального времени.

Метод **Unregister(WaitHandle)** экземпляра класса **RegisterWaitHandle** снимает регистрацию с объекта синхронизации, завершая работу:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        AutoResetEvent autoResetEvent = new(false);
        WaitOrTimerCallback callbackMethodDelegate = new(PrintIterations);

        RegisteredWaitHandle registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(autoResetEvent, callbackMethodDelegate, "Secondary", 2000, false);

        char command;

        while (true)
        {
            Console.Write("Press any key to set the event for associated RegisteredWaitHandle instances, 'x' to exit: ");

            command = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (command == 'x')
            {
                Console.WriteLine("Associated RegisteredWaitHandle instances was unregistered.");
                registeredWaitHandle.Unregister(autoResetEvent);
                break;
            }
            else
            {
                autoResetEvent.Set();
            }
        }
    }

    private static void PrintIterations(object state, bool timedOut)
    {
        int iterationNumber = 0;

        Console.WriteLine();

        while (iterationNumber < 3)
        {
            iterationNumber++;
            Console.WriteLine($"{state} - {iterationNumber}");
            Thread.Sleep(200);
        }
    }
}
```

Для того, чтобы установить запуск обратного метода полностью в ручной режим, можно передать в качестве аргумента в параметр **millisecondsTimeOutInterval** значение **Timeout.Infinite** - бесконечность, которая исключит повторный вызов метода:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        AutoResetEvent autoResetEvent = new(false);

        RegisteredWaitHandle registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(autoResetEvent, PrintIterations, null, Timeout.Infinite, false);

        char command;

        while (true)
        {
            Thread.Sleep(800);
            Console.Write("Press any key to set the event for associated RegisteredWaitHandle instances, 'x' to unregister the event and exit: ");

            command = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (command == 'x')
            {
                Console.WriteLine("Associated RegisteredWaitHandle instances was unregistered.");
                registeredWaitHandle.Unregister(autoResetEvent);
                break;
            }
            else
            {
                autoResetEvent.Set();
            }
        }
    }

    private static void PrintIterations(object state, bool timedOut)
    {
        int iterationNumber = 0;

        Console.WriteLine();

        while (iterationNumber < 3)
        {
            iterationNumber++;
            Console.WriteLine($"{state} - {iterationNumber}");
            Thread.Sleep(200);
        }
    }
}
```

## **EventWaitHandle**
---
Класс **EventWaitHandle** является базовым классом для событийных объектов синхронизации, таких, как **AutoResetEvent** и **ManualResetEvent**.

Параметры конструктора класса **EventWaitHandle**:
1. **bool initialState** - **true** – создаётся в сигнальном состоянии, **false** – в не сигнальном состоянии;
2. **EventResetMode mode** - **ManualReset** - ручной сброс сигнального состояния, **AutoReset** - автоматический сброс сигнального состояния;
3. **string name** - **необязательный параметр** - имя событийного объекта синхронизации для межпроцессной синхронизации.

На основе класса **EventWaitHandle** можно как создавать пользовательские классы событийных объектов синхронизации, так и использовать сам этот класс вместо **AuteResetEvent** и **ManualResetEvent**:

```cs
internal class Program
{
    private static EventWaitHandle _eventWaitHandle = new(false, EventResetMode.ManualReset, typeof(Program).FullName);

    private static void Main(string[] args)
    {
        Thread thread = new(PrintIterations);

        thread.Start("Secondary");

        Thread.Sleep(100);

        Console.Write("Press any key to set the event: ");
        Console.ReadKey();
        _eventWaitHandle.Set();
        Thread.Sleep(1000);
    }

    private static void PrintIterations(object arg)
    {
        _eventWaitHandle.WaitOne();

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;
            Console.WriteLine($"{Environment.NewLine}{arg} - {iterationNumber}");
            Thread.Sleep(100);
        }

        Console.WriteLine();
    }
}
```

## **Таймер (Timer)**

**Таймер (Timer)** предоставляет возможность для выполнения метода обратного вызова в заданные интервалы времени.

В .NET имеется несколько классов **Timer** в разных фреймворках.

В данном примере будет рассмотрен класс **Timer** из пространства имён **System.Threading**.

Параметры конструктора класса **Timer**:
1. **TimerCallback callback** - экземпляр делегата, с которым сообщён метод обратного вызова, который необходимо выполнять;
2. **object state** – первый аргумент метода, который необходимо выполнить;
3. **int dueTime** - задержка до первого вызова метода обратного вызова, в миллисекундах;
4. **int Period** - интервал между вызовами метода обратного вызова, в миллисекундах.

Метод **Change(int dueTime, int Period)** позволяет переконфигурировать задержку и интервал, метод **Dispose()** - останавливает и финализирует таймер.

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        TimerCallback callback = new(PrintIterations);

        Timer timer = new(callback, "Secondary", 1000, 500);

        Console.WriteLine("Timer has started." + Environment.NewLine);

        Thread.Sleep(3000);

        timer.Change(500, 1000);

        Thread.Sleep(3000);

        timer.Dispose();

        Console.WriteLine("Timer has finished.");

        Console.ReadKey();
    }

    private static void PrintIterations(object arg)
    {
        int iterationNumber = 0;

        while (iterationNumber < 3)
        {
            iterationNumber++;
            Console.WriteLine($"{arg} - {iterationNumber}");
            Thread.Sleep(100);
        }

        Console.WriteLine();
    }
}
```

## **WaitHandle**
---
Абстрактный класс **WaitHandle** - является базовым абстрактным классом объектов синхронизации в .NET и служит для синхронизации.

Метод **WaitAll(WaitHandle[])** экземпляра класса **WaitHandle** ожидает сигнального состояния всех событий, метод **int WaitAny(WaitHandle[])** экземпляра класса **WaitHandle** ожидает сигнального состояния хотя бы одного из событийЖ

```cs
namespace ThreadsSynchronisation._16_WaitHandle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WaitHandle[] waitHandles = new WaitHandle[2]
            {
                new AutoResetEvent(false),
                new AutoResetEvent(false)
            };

            PrintIterationArgs[] printIterationArgs = new PrintIterationArgs[2]
            {
                new PrintIterationArgs
                {
                    TaskName = "Secondary",
                    WaitHandle = waitHandles[0]
                },
                new PrintIterationArgs
                {
                    TaskName = "Secondary",
                    WaitHandle = waitHandles[1]
                }
            };

            Console.WriteLine($"2x {nameof(PrintIterations)} methods has started.");

            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[0]);
            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[1]);

            WaitHandle.WaitAll(waitHandles);

            Console.WriteLine($"Both {nameof(PrintIterations)} methods has finished.{Environment.NewLine}");

            Console.WriteLine($"2x new {nameof(PrintIterations)} methods has started.");

            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[0]);
            ThreadPool.QueueUserWorkItem(PrintIterations, printIterationArgs[1]);

            int index = WaitHandle.WaitAny(waitHandles);

            Console.WriteLine($"Method {nameof(PrintIterations)} with index {index} has finished first.");

            WaitHandle.WaitAny(waitHandles);

            Console.WriteLine($"Both {nameof(PrintIterations)} methods has finished.");
        }

        private static void PrintIterations(object arg)
        {
            PrintIterationArgs printIterationArgs = arg as PrintIterationArgs ?? throw new ArgumentException(null, nameof(arg));

            AutoResetEvent autoResetEvent = printIterationArgs.WaitHandle as AutoResetEvent ?? throw new ArgumentException(null, nameof(arg));

            int iterationNumber = 0;

            while (iterationNumber < 3)
            {
                iterationNumber++;
                Console.WriteLine($"{printIterationArgs.TaskName} - Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(500);
            }

            autoResetEvent.Set();
        }
    }

    internal class PrintIterationArgs
    {
        public string TaskName { get; set; }

        public WaitHandle WaitHandle { get; set; }
    }
}
```
