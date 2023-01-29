# **Исключения в асинхронном коде**

### **Исключение**

**Исключение** – ситуация, при которой продолжение выполнения кода в соответствии с базовым алгоритмом невозможно или бессмысленно.

Если приложению недостаёт необходимого ресурса, данных, или произошла ошибка алгоритма в вычислениях, то приложение выбрасывает исключение, говоря о том, что не может продолжать своё выполнение и завершает работу, чтобы бессмысленно не занимать системные ресурсы.

Приложение не знает, как можно было бы исправить такую ситуацию, она может сделать только то, но что была запрограммирована.

Когда происходи что-то неожиданное, или то, с чем приложение не может справиться, то это считается для него исключительной ситуацией.

В .NET имеется механизм создания, возбуждения, перехвата и обработки исключений, представленный базовым классом для всех исключений ```Exception```, ключевым словом ```throw``` и конструкцией ```try/catch```.

### **Виды исключений**

- **Синхронные** – возникают в заранее известных, определенных точках программы. Такой вид исключения легко обрабатывается конструкцией ```try/catch```. При таком исключении компилятор точно понимает, в какой точке возникло исключение, что позволяет разработчику выявлять потенциально место возникновения исключения и реализовать в этом месте обработку этого исключения, чтобы конечный пользователь не увидел этой ошибки, а приложение стало более надёжным, устойчивым и работоспособным;

- **Асинхронные** – возникают в любой момент времени во вторичном потоке и не зависят от того, какую конкретно инструкцию выполняет основной поток. Такой вид исключения тяжело обрабатывать, из-за непредсказуемости времени и места возникновения. Единственный способ защиты от таких исключений - это включение всего кода выполняемого во вторичном потоке метода в конструкцию ```try/catch```. Такие исключения может быть выброшено при прямой работе с потоками ```Thread``` или с пулом потоков ```ThreadPool```. Задачи ```Task``` решают такие проблемы совсем иначе, поглащая все возникшие в текущей и её дочерних задачах исключения в исключение-обёртку, прдеставленную экземпляром класса ```AggregateException```.

Если исключение происходит в контексте вторичного потока, то и основной поток, который выполняет приложение, будет аварийно завершён исключенрием.

Следует быть осторожным с передачей методов на выполнение в ```Thread``` и ```ThreadPool```. Если в них присутствует вероятность возникновения исключения, то следует поместить вызов этих методов в конструкцию ```try/catch```, чтобы обработать возникшее исключение и не сломать работу других потоков.

### **Исключения в контексте вторичного потока**
---
Если исключение возникает в контексте потока ```Thread``` – это разрушает работу приложения. Такое исключение можно поймать и обработать только в контексте того потока, в котором оно произошло.

Опасным кодом является использование класса ```Thread``` или ```ThreadPool``` для выполнения операции в контексте вторичного потока. Если код не помещен в конструкцию ```try/catch``` – это разрушает работу приложения.

## **01 Исключения в Thread**
---
При возникновении исключения в коде, выполняющемся во вторичном потоке, запущенном при помощи класса ```Thread```, работа программы, и, следовательно, первичного потока прекращается в произвольном месте в то время, когда во вторичном потоке возникло исключение:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Thread secondaryThread = new(PrintIterations);

        secondaryThread.Start("SecondaryThreadCall");

        PrintIterations("PrimaryThreadCall");

        Console.ReadKey();
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        bool secondaryThread = callName.StartsWith("Secondary");

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (secondaryThread && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **02 Исключения в Thread, запущенном в блоке try/catch**
---
Помещение вызова вторичного потока при помощи класса ```Thread``` из блока ```try/catch``` не помогает избежать проблемы и работа программы, как и первичного потока прекращается в произвольном месте в то время, когда во вторичном потоке возникло исключение:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Thread secondaryThread = new(PrintIterations);

        try
        {
            secondaryThread.Start("SecondaryThreadCall");
        }
        catch (Exception ex)
        {
            ReportExceptionDetails(ex);
        }

        PrintIterations("PrimaryThreadCall");

        Console.ReadKey();
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        bool secondaryThread = callName.StartsWith("Secondary");

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (secondaryThread && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportExceptionDetails(Exception ex)
    {
        string exceptionDetails = $"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message: {ex.Message}";

        Console.WriteLine(exceptionDetails);
    }
}
```

Здесь можно предположить, что исключение не было перехвачено потому, что на момент его возникновения во вторичном потоке, первичный поток уже покинул блок ```try``` конструкции ```try/catch```.

## **03 Исключения в Thread, запущенном в блоке try/catch и ожидаемом методом Join()**
---
При вызове метода ```Join()``` на экземпляре класса ```Thread```, запущенного и ожидаемого в блоке ```try``` конструкции ```try/catch``` проблема также не решается, так как исключение считается тяжёлым и не позволяет его обработать первичному потоку:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Thread secondaryThread = new(PrintIterations);

        try
        {
            secondaryThread.Start("SecondaryThreadCall");

            PrintIterations("PrimaryThreadCall");

            secondaryThread.Join();
        }
        catch (Exception ex)
        {
            ReportExceptionDetails(ex);
        }

        Console.ReadKey();
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        bool secondaryThread = callName.StartsWith("Secondary");

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (secondaryThread && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportExceptionDetails(Exception ex)
    {
        string exceptionDetails = $"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message: {ex.Message}";

        Console.WriteLine(exceptionDetails);
    }
}
```

## **04 Исключения в Thread с операциями, размещёнными в блоке try/catch**
---
Единственный гарантированный способ перехватить исключение, возникшее в коде, выполняющемся во вторичном потоке, запущенном при помощи экземпляра класса ```Thread``` - это помещение всего выполняющегося во вторичном потоке кода в блок ```try``` конструкции ```try/catch```.

Если же у разработчика нет возможности изменить код метода, вызываемого в контексте вторичного потока, то можно создать метод-обёртку **XxxWrapper**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Thread secondaryThread = new(PrintIterationsWrapper);

        secondaryThread.Start("SecondaryThreadCall");

        PrintIterations("PrimaryThreadCall");

        Console.ReadKey();
    }

    private static void PrintIterationsWrapper(object state)
    {
        try
        {
            PrintIterations(state);
        }
        catch (Exception ex)
        {
            ReportExceptionDetails(ex);
        }
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        bool secondaryThread = callName.StartsWith("Secondary");

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (secondaryThread && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportExceptionDetails(Exception ex)
    {
        string exceptionDetails = $"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message: {ex.Message}";

        Console.WriteLine(exceptionDetails);
    }
}
```

Как видно из результатов выполнения, такой способ позволил перехватить и обработать возникшее во вторичном потоке исключение и не дать приложению непредвиденно завершиться.

## **05 Исключения в потоке ThreadPool**
---
При возникновении исключения в коде, выполняющемся во вторичном потоке, запущенном при помощи класса ```ThreadPool```, работа программы, и, следовательно, первичного потока прекращается в произвольном месте в то время, когда во вторичном потоке возникло исключение:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        ThreadPool.QueueUserWorkItem(PrintIterations, "ThreadPoolCall");

        PrintIterations("PrimaryThreadCall");

        Console.ReadKey();
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Thread.CurrentThread.IsThreadPoolThread && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **06 Исключения в потоке ThreadPool, запущенном в блоке try/catch**
---
Помещение вызова вторичного потока при помощи класса ```ThreadPool``` из блока ```try/catch``` не помогает избежать проблемы, как и в случае с классом ```Thread```, и работа программы, как и первичного потока прекращается в произвольном месте в то время, когда во вторичном потоке возникло исключение:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            ThreadPool.QueueUserWorkItem(PrintIterations, "ThreadPoolCall");
        }
        catch (Exception ex)
        {
            ReportExceptionDetails(ex);
        }

        PrintIterations("PrimaryThreadCall");

        Console.ReadKey();
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Thread.CurrentThread.IsThreadPoolThread && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportExceptionDetails(Exception ex)
    {
        string exceptionDetails = $"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message: {ex.Message}";

        Console.WriteLine(exceptionDetails);
    }
}
```

Возможности дождаться завершения потока из пула потоков нет, поэтому проверить предположение о том, что на момент возникновения во вторичном потоке исключения, первичный поток уже покинул блок ```try``` конструкции ```try/catch```, не получится. Однако, из примера с вызовом на эксземпляре потока метода ```Join()``` уже понятно, что проблема с этим не связана.

## **07 Исключения в потоке ThreadPool с операциями, размещёнными в блоке try/catch**
---
Единственный гарантированный способ перехватить исключение, возникшее в коде, выполняющемся во вторичном потоке, запущенном при помощи класса ```ThreadPool``` - это помещение всего выполняющегося во вторичном потоке кода в блок ```try``` конструкции ```try/catch```.

Если же у разработчика нет возможности изменить код метода, вызываемого в контексте вторичного потока, то можно создать метод-обёртку **XxxWrapper**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            ThreadPool.QueueUserWorkItem(PrintIterationsWrapper, "ThreadPoolCall");
        }
        catch (Exception ex)
        {
            ReportExceptionDetails(ex);
        }

        PrintIterations("PrimaryThreadCall");

        Console.ReadKey();
    }

    private static void PrintIterationsWrapper(object state)
    {
        try
        {
            PrintIterations(state);
        }
        catch (Exception ex)
        {
            ReportExceptionDetails(ex);
        }
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Thread.CurrentThread.IsThreadPoolThread && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportExceptionDetails(Exception ex)
    {
        string exceptionDetails = $"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message: {ex.Message}";

        Console.WriteLine(exceptionDetails);
    }
}
```

Как видно из результатов выполнения, такой способ позволил перехватить и обработать возникшее во вторичном потоке исключение и не дать приложению непредвиденно завершиться.

### **Исключения в контексте задачи**
---
При выполнении операции в контексте задачи, поведение исключения меняется. Все исключения, возникшие в контексте задачи, будут записаны в свойство ```Exception```. Это свойство типа ```AggregateException```. При этом приложение не будет разрушено.

```AggregateException``` это класс, экземпляры которого могут принять несколько исключений и записать в себя. Он содержит коллекцию всех возникших исключений асинхронной операции и дочерних операций.

## **08 Исключения в Task**
---
При возникновении исключения в асинхронной операции, выполняющейся в контексте задачи ```Task```, выполнение этой задачи немедленно прекращается, однако, это никак не отражается на инициировавшем запуск этой задачи коде, он просто продолжает своё выполнение:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task = Task.Run(() =>
        {
            PrintIterations("AsyncTask");
        });

        PrintIterations("SyncCall");

        Console.ReadKey();
    }

    private static void PrintIterations(string callName)
    {
        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Task.CurrentId != null && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

### **Обработка асинхронных исключений задач (Task)**
---
Обработка исключений из асинхронного кода привычным способом через конструкцию ```try/catch``` не получится.

В режиме **Debug** вы получите исключение, это помогает отладке

В режиме **Release** исключение будет проигнорировано.

Доступные API, выдающие исключения при вызове:

- Свойство ```Exception``` типа ```AggregateException```;
- Методы ожидания – ```Wait()```, ```WaitAll()```, ```WaitAny()```;
- Получение результата операции – свойство ```Result```;
- Оператор ```await```;
- Асинхронные методы ожидания – ```WhenAll()```, ```WhenAny()```.

## **09 Исключения в Task, ожидаемой через свойство Result**
---
При возникновении исключения в асинхронной операции, выполняющейся в контексте задачи ```Task``` и ожидаемой через вызов свойства ```Result```, выполнение этой задачи немедленно прекращается, и исключение будет проброшено в ожидающий поток, но не сразу же, а только в месте обращения к свойству ```Result```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<int> task = Task.Run(() => PrintIterations("AsyncTask"));

        PrintIterations("SyncCall");

        int taskResult = task.Result;

        Console.ReadKey();
    }

    private static int PrintIterations(string callName)
    {
        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Task.CurrentId != null && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        int result = iterationIndex * 1000;

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

        return result;
    }
}
```

## **10 Исключения в Task, ожидаемой через метод Wait()**
---
При возникновении исключения в асинхронной операции, выполняющейся в контексте задачи ```Task``` и ожидаемой через вызов метода ```Wait()```, как и в случае с ожиданием через свойство ```Result```, выполнение этой задачи немедленно прекращается, и исключение будет проброшено в ожидающий поток, но не сразу же, а только в месте обращения к методу ```Wait()```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task = Task.Run(() => PrintIterations("AsyncTask"));

        PrintIterations("SyncCall");

        task.Wait();

        Console.ReadKey();
    }

    private static void PrintIterations(string callName)
    {
        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Task.CurrentId != null && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **11 Исключения в Task, ожидаемой через свойство Result в блоке try/catch**
---
При возникновении исключения в асинхронной операции, выполняющейся в контексте задачи ```Task``` и ожидаемой через вызов свойства ```Result```, размещнном в блоке ```try``` конструкции ```try/catch```, выполнение этой задачи немедленно прекращается, и исключение будет проброшено в ожидающий поток, но не сразу же, а только в месте обращения к свойству ```Result```. При этом, вызывающий поток сможет перехватить и обработать пойманное исключение, представленное экземпляром класса ```AggregateException```.

Для вывода типа и сообщения исключения также создадим отдельный метод ```ReportExceptionDetails()```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task<int> task = Task.Run(() => PrintIterations("AsyncTask"));

        PrintIterations("SyncCall");

        try
        {
            int taskResult = task.Result;
        }
        catch (AggregateException ex)
        {
            ReportExceptionDetails(ex);
        }

        Console.ReadKey();
    }

    private static int PrintIterations(string callName)
    {
        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Task.CurrentId != null && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        int result = iterationIndex * 1000;

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

        return result;
    }

    private static void ReportExceptionDetails(AggregateException parentException, string indent = null)
    {
        Console.WriteLine(
            $"Exception Type: {parentException.GetType().Name}" +
            $"{Environment.NewLine}" +
            $"Exception Message: {parentException.Message}" +
            $"{Environment.NewLine}" +
            $"{new string('-', 40)}");

        foreach (var innerException in parentException.InnerExceptions)
        {
            if (innerException is AggregateException aggregateException)
            {
                ReportExceptionDetails(aggregateException, indent + "  ");
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}
```

## **12 Исключения в Task, ожидаемой через метод Wait() в блоке try/catch**
---
При возникновении исключения в асинхронной операции, выполняющейся в контексте задачи ```Task``` и ожидаемой через вызов метода ```Wait()```, как и в случае с ожиданием через свойство ```Result```, размещнном в блоке ```try``` конструкции ```try/catch```, выполнение этой задачи немедленно прекращается, и исключение будет проброшено в ожидающий поток, но не сразу же, а только в месте обращения к методу ```Wait()```. При этом, вызывающий поток сможет перехватить и обработать пойманное исключение, представленное экземпляром класса ```AggregateException```.

Для вывода типа и сообщения исключения также создадим отдельный метод ```ReportExceptionDetails()```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task = Task.Run(() => PrintIterations("AsyncTask"));

        PrintIterations("SyncCall");

        try
        {
            task.Wait();
        }
        catch (AggregateException ex)
        {
            ReportExceptionDetails(ex);
        }

        Console.ReadKey();
    }

    private static void PrintIterations(string callName)
    {
        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Task.CurrentId != null && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportExceptionDetails(AggregateException parentException, string indent = null)
    {
        Console.WriteLine(
            $"Exception Type: {parentException.GetType().Name}" +
            $"{Environment.NewLine}" +
            $"Exception Message: {parentException.Message}" +
            $"{Environment.NewLine}" +
            $"{new string('-', 40)}");

        foreach (var innerException in parentException.InnerExceptions)
        {
            if (innerException is AggregateException aggregateException)
            {
                ReportExceptionDetails(aggregateException, indent + "  ");
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}
```

## **13 Исключения в Task и её дочерних Task, ожидаемой через метод Wait() в блоке try/catch**
---
При возникновении исключений в дочерних задачах ```Task``` родительской задачи ```Task```, выполнение этих задач немедленно прекращается, но это не влияет на выполнение родительской задачи, и она просто продолжает своё выполнение до конца.

Экземпляры исключений проваленных дочерних задач ```AggregateException``` записываются в экземпляр исключения ```AggregateException``` родительской задачи.

Если задача ожидается через вызов метода ```Wait()``` или свойства ```Result```, размещнный в блоке ```try``` конструкции ```try/catch```, то исключение, выброшенное родительской задачей, будет перехвачено и обработано.

Исключение с экземплярами исключений дочерних задач будет выброшено родительской задачей даже в том случае, если сама родительская задача завершилась корректно.

Кроме того, если в родительской задаче произошло исключение после запуска дочерних задач, то родительская задача после прекращения выполнения собственных инструкций всё равно ожидает завершения выполнения дочерних задач и только после этого выбрасывает исключение ```AggregateException```.

Для вывода типов и сообщений исключений также модифицируем метод ```ReportExceptionDetails()```, добавив возможность отображения нескольких и вложенных исключений:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task task = new(PrintIterationsCollective, "AsyncTask");

        task.Start();

        PrintIterations("SyncCall");

        try
        {
            task.Wait();
        }
        catch (AggregateException ex)
        {
            ReportExceptionDetails(ex);
        }

        Console.ReadKey();
    }

    private static void PrintIterationsCollective(object state)
    {
        string callName = state.ToString();

        Task task1 = new(() => PrintIterations($"Child{callName}"), TaskCreationOptions.AttachedToParent);
        Task task2 = new(() => PrintIterations($"Child{callName}"), TaskCreationOptions.AttachedToParent);

        task1.Start();
        task2.Start();

        throw new Exception($"[!!!EXCEPTION!!! Parent{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1}]");
    }

    private static void PrintIterations(string callName)
    {
        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Task.CurrentId != null && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void ReportExceptionDetails(AggregateException parentException, string indent = null)
    {
        Console.WriteLine(
            $"Exception Type: {parentException.GetType().Name}" +
            $"{Environment.NewLine}" +
            $"Exception Message: {parentException.Message}" +
            $"{Environment.NewLine}" +
            $"{new string('-', 40)}");

        foreach (var innerException in parentException.InnerExceptions)
        {
            if (innerException is AggregateException aggregateException)
            {
                ReportExceptionDetails(aggregateException, indent + "  ");
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}
```

## **14 Исключения в Task, ожидаемой через оператор await в блоке try/catch**
---
При возникновении исключения в асинхронной операции, выполняющейся в контексте задачи ```Task``` и ожидаемой через оператор ```await```, размещнном в блоке ```try``` конструкции ```try/catch```, выполнение этой задачи немедленно прекращается, и исключение будет проброшено в ожидающий поток, но не сразу же, а только в месте обращения к методу ```await```. 

При этом, в отличии от ожидания через вызов свойства ```Result``` и методов ```Wait()```, ```WaitAll()```, ```WaitAny()```, вызывающий поток сможет перехватить и обработать пойманное исключение, представленное экземпляром того класса исключения, которое было выброшено оператором ```throw```, без его оборачивания в экземпляр класса ```AggregateException```, как если бы исключение возникло и было перехвачено в обычном синхронном коде:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            await PrintIterationsAsync("AsyncTask");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message{ex.Message}");
        }

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string callName)
    {
        Task iterationsTask = new(PrintIterations, callName);

        iterationsTask.Start();

        await iterationsTask;
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);

            if (Task.CurrentId != null && iterationIndex > 5)
            {
                throw new Exception($"[!!!EXCEPTION!!! {callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1}]");
            }
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **15 Исключения в Task, ожидаемой через вызов методов GetAwaiter().GetResult() в блоке try/catch**
---
Поведение кода при возникновении исключения в задаче, ожидаемой через вызов методов ```GetAwaiter().GetResult()``` полностью идентично поведению при ожидании через оператор ```await```, однако стоит помнить, что это методы, созданные для внутреннего пользования компилятора и не предназначенные для регулярного использования в коде приложений:

```cs
internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                PrintIterationsAsync("AsyncTask").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Type: {ex.GetType().Name}{Environment.NewLine}Exception Message{ex.Message}");
            }

            Console.ReadKey();
        }

        private static async Task PrintIterationsAsync(string callName)
        {
            Task iterationsTask = new(PrintIterations, callName);

            iterationsTask.Start();

            await iterationsTask;
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);

                if (Task.CurrentId != null && iterationIndex > 5)
                {
                    throw new Exception($"[!!!EXCEPTION!!! {callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1}]");
                }
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
```

> TODO: 16. получение исключения из свойства ```Exception``` класса ```Task```

> TODO: 17. демонстрация передачи оператором ```await``` только первого исключения и игнорирование остальных

> TODO: 18. демонстрация передачи оператором ```await``` только первого исключения и игнорирование остальных с методом ожидания ```WhenAny()```

> TODO: 19. демонстрация передачи оператором ```await``` только первого исключения и игнорирование остальных с методом ожидания ```WhenAll()```

> TODO: 20. получение нескольких исключений с оператором ```await``` из свойства ```Exception``` класса ```Task```

> TODO: 21. получение нескольких исключений с оператором ```await``` при помощи продолжения ```ContinueWith()``` и метода ```Wait()``` с оператором ```await```

> TODO: 22. получение нескольких исключений с оператором ```await``` при помощи продолжения ```ContinueWith()``` и метода ```Wait()``` внутри этого продолжения
