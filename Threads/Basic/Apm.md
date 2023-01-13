# **Асинхронная модель программирования (Asynchronous Programming Model, APM)**

Существует 2 шаблона асинхронной разработки, рекомендованных **Microsoft**:

- Шаблон асинхронной разработки, основанный на интерфейсе IAsyncResult
- Шаблон – «Асинхронная модель, основанная на событиях» (Event-based Asynchronous Pattern) – рассмотреть самостоятельно.

**Синхронный вызов** – последовательный вызов в контексте текущего потока.

**Асинхронный вызов** – вызов метода в контексте вторичного потока через использование класса Thread, ThreadPool или другие варианты.

Microsoft **отказываются от простых Thread**, переходя на ThreadPool.

Необходимо всегда **использовать** именно пул потоков **ThreadPool**, или же инструменты, его использующие, а создание потоков "напрямую" через класс **Thread** можно осуществлять **только для специфических задач**.

Реализация шаблона асинхронной обработки позволяет получать возвращаемое значение метода из отдельных потоков.

## **Метод делегатов BeginInvoke\***
---
\*Начиная с .NET Core дизайн асинхронного выполнения с помощью метода BeginInvoke считается концептуально устаревшим и доступен только в .NET Framework на платформе Windows.

**Метод делегатов BeginInvoke** позволяет запустить выполнение сообщённого с делегатом метода асинхронно с использованием пула потоков.

**Метод делегатов BeginInvoke** имеет 2 параметра:
1. **AsyncCallback callback** - принимает экземпляр делегата метода продолжения. Метод продолжения - это метод обратного вызова, который должен выполниться после выполнения основного метода, обычно для обработки результата его работы;
2. **object @object** - принимает аргумент, который будет доступен в методе продолжения.

```cs
private static void Main(string[] args)
{
    Action actionDelegate = new(PrintIterations);

    actionDelegate.BeginInvoke(null, null);

    PrintIterations();

    Console.ReadKey();
}

private static void PrintIterations()
{
    int iterationNumber = 0;

    while (iterationNumber < 10)
    {
        iterationNumber++;

        Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
        Thread.Sleep(100);
    }
}
```

## **Метод делегатов EndInvoke\***
---
\*Также, как и с методом делегатов **BeginInvoke**, начиная с .NET Core дизайн асинхронного выполнения с помощью метода BeginInvoke считается концептуально устаревшим и доступен только в .NET Framework на платформе Windows.

**Метод делегатов EndInvoke** позволяет заблокировать вызывающий поток до окончания асинхронного выполнения метода, сообщённого с делегатом, через передачу **методу делегатов EndInvoke** экземпляра типа **IAsyncResult**, который возвращает **метод делегатов BeginInvoke**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Action actionDelegate = new(PrintIterations);

        IAsyncResult actionResult = actionDelegate.BeginInvoke(null, null);

        actionDelegate.EndInvoke(actionResult);

        PrintIterations();

        Console.ReadKey();
    }

    private static void PrintIterations()
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(100);
        }
    }
}
```
Для ожидания **EndInvoke** использует объект синхронизации **ManualResetEvent**. 

## **Получение возвращаемого значения через метод EndInvoke из метода, вызванного асинхронно через метод BeginInvoke**
---
Если метод, сообщённый с делегатом, имеет возвращаемое значение, то его можно получить из метода **EndInvoke**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Func<int> funcDelegate = new(PrintIterations);

        IAsyncResult funcResult = funcDelegate.BeginInvoke(null, null);

        int funcIterationNumber = funcDelegate.EndInvoke(funcResult);

        Console.WriteLine($"Result of {nameof(funcIterationNumber)} from async call:{funcIterationNumber}");

        int iterationNumber = PrintIterations();

        Console.WriteLine($"Result of {nameof(iterationNumber)} from sync call:{iterationNumber}");

        Console.ReadKey();
    }

    private static int PrintIterations()
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(100);
        }

        return iterationNumber;
    }
}
```

## **Передача параметров в метод, вызванный асинхронно через метод BeginInvoke**
---
Помимо статичных (двух последних, или единственных) параметров метода делегатов **BeginInvoke** он также имеет параметры, объявленные в сигнатуре класса-делегата, либо в местах указателя заполнения типом. При вызове такого метода, в начале списка параметров идут параметры, через которые можно передать аргументы в сообщённый с экземпляром делегата метод:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Func<string, int, int> funcDelegate = new(PrintIterations);

        IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, null, null);

        int funcIterationNumber = funcDelegate.EndInvoke(funcResult);

        Console.WriteLine($"Result of {nameof(funcIterationNumber)} from async call:{funcIterationNumber}");

        int iterationNumber = PrintIterations("SyncTask", 200);

        Console.WriteLine($"Result of {nameof(iterationNumber)} from sync call:{iterationNumber}");

        Console.ReadKey();
    }

    private static int PrintIterations(string taskName, int iterationsDelay)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - {iterationNumber}");
            Thread.Sleep(iterationsDelay);
        }

        return iterationNumber;
    }
}
```

## **Объект синхронизации уровня ядра ОС WaitHandle в поле AsyncWaitHandle в типе IAsyncResult**
---
Для синхронизации метода, вызванного асинхронно через метод делегатов **BeginInvoke**, с потоком, вызвавшим метод делегатов **EndInvoke**, используется **объект синхронизации уровня ядра ОС абстрактного типа WaitHandle** в поле **AsyncWaitHandle** в типе **IAsyncResult**.
В этом поле содержится событийный объект синхронизации уровня ядра ОС **ManualResetEvent**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Func<string, int, int> funcDelegate = new(PrintIterations);

        IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, null, null);

        WaitHandle.WaitAll(new WaitHandle[] { funcResult.AsyncWaitHandle });

        Console.WriteLine($"The type of {nameof(IAsyncResult.AsyncWaitHandle)} is {funcResult.AsyncWaitHandle.GetType()}");

        int funcIterationNumber = funcDelegate.EndInvoke(funcResult);

        Console.WriteLine($"Result of {nameof(funcIterationNumber)} from async call:{funcIterationNumber}");

        int iterationNumber = PrintIterations("SyncTask", 200);

        Console.WriteLine($"Result of {nameof(iterationNumber)} from sync call:{iterationNumber}");

        Console.ReadKey();
    }

    private static int PrintIterations(string taskName, int iterationsDelay)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - {iterationNumber}");
            Thread.Sleep(iterationsDelay);
        }

        return iterationNumber;
    }
}
```
При этом, при вызове метода **EndInvoke** для получения результата блокировка вызывающего потока не произойдёт, так как **ManualResetEvent** уже будет установлен в сигнальное состояние для всех ожидающих его **WaitOne()**.

## **Ожидание путём опроса свойства IsCompleted типа IAsyncResult**
---
Тип **IAsyncResult** имеет свойство **IsCompleted**. Если его значение **true**, то метод, вызванный асинхронно, завершился. Если **false** - то он ещё выполняется. Синхронизацию метода, вызванного асинхронно можно также выполнить через цикличную проверку этого свойства:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Action<string, int> funcDelegate = new(PrintIterations);

        IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, null, null);

        while (!funcResult.IsCompleted)
        {
            Thread.Sleep(100);
        }

        PrintIterations("SyncTask", 200);

        Console.ReadKey();
    }

    private static void PrintIterations(string taskName, int iterationsDelay)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - {iterationNumber}");
            Thread.Sleep(iterationsDelay);
        }
    }
}
```
Хоть такая возможность синхронизации и имеется, но лучше использовать вызов метода **EndInvoke**. Свойство **IsCompleted** предназначено именно для проверки, завершился ли метод, вызванный асинхронно, или нет, а не для синхронизации с ним.

## **Передача в BeginInvoke и выполнение метода продолжения, сообщённого с делегатом AsyncCallback**
---
Предпоследний параметр метода **BeginInvoke** принимает экземпляр делегата **AsyncCallback** и выполняет сообщённый с ним метод сразу же после завершения метода, вызванного асинхронно: 

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Action<string, int> funcDelegate = new(PrintIterations);

        AsyncCallback funcDelegateCallback = new(PrintIterationsCallback);

        IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, funcDelegateCallback, null);

        PrintIterations("SyncTask", 200);

        Console.ReadKey();
    }

    private static void PrintIterations(string taskName, int iterationsDelay)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - ThreadId:{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(iterationsDelay);
        }

        Console.WriteLine($"TaskName:{taskName} completed in Thread#{Environment.CurrentManagedThreadId}");
    }

    private static void PrintIterationsCallback(IAsyncResult ar)
    {
        Console.WriteLine($"Collback method has executed in Thread#{Environment.CurrentManagedThreadId}");
    }
}
```

## **Передача в BeginInvoke аргумента, получаемого в методе продолжения из свойства AsyncState в типе IAsyncResult**
---
Последний параметр метода **BeginInvoke** принимает аргумент типа **object** и записывает его в свойство **AsyncState** экземпляра типа **IAsyncResult**, который в дальнейшем будет передан в метод продолжения:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Action<string, int> funcDelegate = new(PrintIterations);

        AsyncCallback funcDelegateCallback = new(PrintIterationsCallback);

        IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, funcDelegateCallback, "Callback of AsyncTask");

        PrintIterations("SyncTask", 200);

        Console.ReadKey();
    }

    private static void PrintIterations(string taskName, int iterationsDelay)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - ThreadId:{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(iterationsDelay);
        }

        Console.WriteLine($"TaskName:{taskName} completed in Thread#{Environment.CurrentManagedThreadId}");
    }

    private static void PrintIterationsCallback(IAsyncResult ar)
    {
        Console.WriteLine($"{ar.AsyncState} has executed in Thread#{Environment.CurrentManagedThreadId}");
    }
}
```

## **Обработка в методе продолжения результата метода, вызванного асинхронно**
---
Если необходимо обработать результат метода, вызванного асинхронно, в методе продолжения, то можно привести экземпляр типа **IAsyncResult**, переданный в метод продолжения, к типу **AsyncResult** и из его свойства **AsyncDelegate** получить экземпляр делегата, с которым соощён метод, выполнившийся асинхронно.

Свойство **AsyncDelegate** имеет тип **object**, поэтому полученный экземпляр необходимо привести к исходному типу класса-делегата.

На экземпляре делегата, приведённом к исходному типу класса-делегата, можно вызвать метод **EndInvoke**, снова передав ему в качестве аргумента экземпляр типа **IAsyncResult**, и получить возвращаемое значение из метода, выполнившегося асинхронно. При этом, блокировка вызывающего потока уже не произойдёт, так как к моменту выполнения метода продолжения, метод, выполнившийся асинхронно, уже завершён:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Func<string, int, int> funcDelegate = new(PrintIterations);

        IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, PrintIterationsCallback, "Callback of AsyncTask");

        PrintIterations("SyncTask", 200);

        Console.ReadKey();
    }

    private static int PrintIterations(string taskName, int iterationsDelay)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - ThreadId:{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(iterationsDelay);
        }

        Console.WriteLine($"TaskName:{taskName} completed in Thread#{Environment.CurrentManagedThreadId}");

        return iterationNumber;
    }

    private static void PrintIterationsCallback(IAsyncResult ar)
    {
        AsyncResult asyncResult = ar as AsyncResult;

        Func<int, int, int> caller = asyncResult.AsyncDelegate as Func<int, int, int>;

        int callerResult = caller.Result;

        Console.WriteLine($"{ar.AsyncState} has executed in Thread#{Environment.CurrentManagedThreadId}, {nameof(callerResult)}:{callerResult}");
    }
}
```

Второй способ обработки результата метода, вызванного асинхронно, в методе продолжения заключается в передаче экземпляра делегата, с которым сообщён метод, выполняемый асинхронно, в качестве аргумента в последний параметр метода **BeginInvoke** и последующим его извлечением из свойства **AsyncState** экземпляра типа **IAsyncResult**, получаемого в параметре метода продолжения

Свойство **AsyncState** имеет тип **object**, поэтому полученный экземпляр необходимо привести к исходному типу класса-делегата.

На экземпляре делегата, приведённом к исходному типу класса-делегата, можно вызвать метод **EndInvoke**, снова передав ему в качестве аргумента экземпляр типа **IAsyncResult**, и получить возвращаемое значение из метода, выполнившегося асинхронно. При этом, блокировка вызывающего потока уже не произойдёт, так как к моменту выполнения метода продолжения, метод, выполнившийся асинхронно, уже завершён:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Func<string, int, int> funcDelegate = new(PrintIterations);

        IAsyncResult funcResult = funcDelegate.BeginInvoke("AsyncTask", 100, PrintIterationsCallback, funcDelegate);

        PrintIterations("SyncTask", 200);

        Console.ReadKey();
    }

    private static int PrintIterations(string taskName, int iterationsDelay)
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"TaskName:{taskName} - ThreadId:{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(iterationsDelay);
        }

        Console.WriteLine($"TaskName:{taskName} completed in Thread#{Environment.CurrentManagedThreadId}");

        return iterationNumber;
    }

    private static void PrintIterationsCallback(IAsyncResult ar)
    {
        Func<int, int, int> funcDelegate = ar.AsyncState as Func<int, int, int>;

        int funcDelegateResult = funcDelegate.EndInvoke(ar);

        Console.WriteLine($"{ar.AsyncState} has executed in Thread#{Environment.CurrentManagedThreadId}, {nameof(funcDelegateResult)}:{funcDelegateResult}");
    }
}
```

## **Методы BeginRead и EndRead класса Stream**
---
Модель **BeginXXX/EndXXX**, где на месте **XXX** указывается совершаемое объектом действие, реализована не только в делегатах, но и в ряде классов, например, в классе **Stream** для асинхронного чтения:

> data.txt
```
[====THE CONTENT OF THE FILE====]
```

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Stream stream = new FileStream("data.txt", FileMode.Open, FileAccess.Read);

        byte[] array = new byte[stream.Length];

        IAsyncResult asyncResult = stream.BeginRead(array, 0, array.Length, null, null);

        Console.WriteLine("Reading the file...");

        Console.WriteLine($"{Environment.NewLine}Another work: print iterations while file is reading asynchronously...");

        PrintIterations();

        Console.WriteLine($"Another work has finished.{Environment.NewLine}");

        stream.EndRead(asyncResult);

        stream.Close();

        Console.WriteLine("The file was readed asynchronously. It contains the following text:");

        string fileText = Encoding.GetEncoding("UTF-8").GetString(array);

        Console.WriteLine(fileText);
    }

    private static void PrintIterations()
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(100);
        }
    }
}
```

## **Методы BeginWrite и EndWrite класса Stream**
---
Также класс **Stream** содержит методы **BeginWrite** и **EndWrite** для асинхронной записи. Кроме того, для финализации дейтсвий, совершаемых в асинхронном методе, например, таких, как закрытие потока, можно использовать методы продолжения:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Stream stream = new FileStream("data.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        string appText = "[====THE CONTENT OF THE APP====]";

        byte[] appTextBytes = Encoding.GetEncoding("UTF-8").GetBytes(appText);

        IAsyncResult asyncResult = stream.BeginWrite(appTextBytes, 0, appTextBytes.Length, new AsyncCallback(StreamWriteCallback), stream);

        Console.WriteLine("The app is writing the following text to the file asynchronously:");
        Console.WriteLine(appText);

        Console.WriteLine($"{Environment.NewLine}Another work: print iterations while file is reading asynchronously...");

        PrintIterations();

        Console.WriteLine($"Another work has finished.{Environment.NewLine}");

        stream.EndWrite(asyncResult);

        Console.WriteLine("All the tasks is completed.");
    }

    private static void PrintIterations()
    {
        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
            Thread.Sleep(100);
        }
    }

    private static void StreamWriteCallback(IAsyncResult asyncResult)
    {
        Console.WriteLine("The text was written asynchronously.");

        Stream stream = asyncResult.AsyncState as Stream;

        if (stream != null)
        {
            stream.Close();
        }
    }
}
```

## **Свойство IsBackground класса Thread выполняемого в асинхронном шаблоне потока**
---
По умолчанию, все потоки, выполняющие асинхронные операции в модели **BeginXXX/EndXXX** являются фоновыми и в свойстве **IsBackground** имеют значение **true**, что означает, что главный поток не будет дожидаться выполнения этих потоков. Если это необходимо изменить, то нужно в методе, запускаемом асинхронно, установить в свойство **IsBackground** значение **false**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Action actionDelegate = new(PrintIterations);

        actionDelegate.BeginInvoke(null, null);

        PrintIterations();

        Console.ReadKey();
    }

    private static void PrintIterations()
    {
        System.Threading.Thread.CurrentThread.IsBackground = false;

        int iterationNumber = 0;

        while (iterationNumber < 10)
        {
            iterationNumber++;

            Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
            System.Threading.Thread.Sleep(100);
        }
    }
}
```
