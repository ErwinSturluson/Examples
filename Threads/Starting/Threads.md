# **Потоки (Threads)**

**Многозадачность (Multicasting)** – свойство операционной системы или среды программирования обеспечить возможность параллельной (или псевдопараллельной) обработки нескольких процессов.

Многозадачность может быть основана на **процессах** и на **потоках**.

**Многозадачность на основе процессов** позволяет выполнять одновременно более одной программы в контексте Операционной Системы. При использовании многозадачности на сонове процессов, программа является наименьшей единицей кода, выполнение которой может контролировать планировщик задач.

**Многозадачность на основе потоков** означает параллельное выполнение отдельных частей программы, когда у каждого процесса может быть один или более потоков. Это означает, что процесс может решать более одной задачи одновременно. 

Многозадачность на основе потоков называется **многопоточностью**.

**Многопоточность** - это свойство платформы (ОС, ВМ), позволяющее выполнять две и более задачи параллельно друг другу.

**Задача** - набор инструкций, которые необходимо выполнить. Это может быть как программа целиком, так поток или, даже, отдельный метод.

Технически задачи могут выполняться небольшими частями поочерёдно, в частности на одноядерных процессорах, при этом переключение выполнения частей задач происходит достаточно быстро, чтобы у пользователя создалось ощущение параллельного выполнения.

В многоядерных процессорах задачи могут выполняться на разных ядрах по-настоящему параллельно.

В многоядерьных процессорах имеется **2 режима работы ядер**:
- **Режим, когда каждое ядро выполняет свою задачу независимо** (при этом, одно ядро в многоядерном процессоре может также, как и в одноядерном процессоре, выполнять несколько задач по-частям поочерёдно);
- **Режим двойного функционирования:** когда одно ядро выполняет задачу, а второе ядро с небольшим отставанием дублирует её выполнение. Это делается для того, чтобы, в случае технической ошибки процессора, выполнение задачи не перезапускалось, а продолжилось дублирующим ядром, что повышает производительность программ. При этом дублирующее ядро становится лидирующим, а лидировавшее ранее становится дублирующим.

Переключение выполняемых задач в процессоре осуществляется **Планировщиком**. 

**Планировщик** – программа, которая занимается работой по управлению контроллера прерываний. Является частью ОС.

**Планировщик** выделяет ограниченное время на выполнение задачи, и, по его завершении, обращается к **Программируемому контроллеру прерываний (ПКП\*)** с запросом на прерывание выполнения задачи, которая выполняется процессором в данный момент, чтобы сохранить её состояние в памяти и передать процессору на выполнение следующую задачу. **ПКП** связывается с процессором через его ножку **inta** (сокращение от interrupt).

\* ПКП также называется **микросхемой 8259A**

Процессор имеет специальный **указатель инструкций (регистр Instruction Pointer)** и он может читать любую из программ (процессов).

Планировщик так же имеет **записи о наличии потоков**, в том числе равноправных, которыми он управляет.

Время на выполнение задачи процессором называется **квантами процессорного времени**.

Планировщик выдает каждой задаче определенное количество квантов процессорного времени.

Каждую задачу процессор выполняет столько квантов, сколько на её выполнение их выделяет **Планировщик**. 

Этапы одного цикла работы планировщика:
1. Планировщик указывает процессору, какую задачу необходимо выполнить;
2. Устанавливает в ПКП таймер со врменем, выделенным на выполнение процессором текущей задачи;
3. Планировщик теряет управление, выполнение процессором переходит к задаче;
4. По истечении выделенного на выполнение процессором времени, ПКП отправляет процессору запрос на прерывание выполняемой задачи через ножку "inta";
5. В памяти сохраняется состояние выполняемой задачи;
6. Процессор передаёт управление планировщику для получения новой задачи на выполнение.

В английском языке потоки (Threads) называют **нитями (прямой перевод Threads)**, по аналогии с ткацкими станками, когда параллельно идёт множество нитей. А потоками называют **стримы (Streams)** – разновидность потоков, не относящаяся к параллельному выполнению, например: **FileStream**.

В .NET для работы с потоками предоставлены пространство имён **System.Threading**, и, в частности, расположенный в нём класс **Thread**.

**System.Threading** – пространство имен для работы с потоками, содержит классы для управления потоками, такие как:
**Thread, ThreadStart, ParameterizedThreadStart, Monitor**

**Первичный поток** - это поток, в котором начинает и заканчивает свою работу программа.

**Вторичный поток** - это поток, который может быть создан и запущен как первичным, так и другим вторичным потоком.

**Синхронный вызов** – вызов метода в контексте текущего потока (последовательный, поочерёдный, выполняемый синхронно).

**Асинхронный вызов** – вызов метода в контексте нового потока (параллельный, выполняемый асинхронно).

Количество потоков в программе должно быть продумано – не мало, не много, а оптимально.

Для запуска метода в контексте вторичного потока с помощью класса **Thread**, сначала необходимо сообщить его с делегатами **ThreadStart** или **ParametrizedThreadStart**, и затем передать экземпляр этого делегата в класс Thread.

Делегат **ThreadStart** используется для запуска в контексте вторичного потока методов, не принимающих входных параметров.

Делегат **ParametrizedThreadStart** используется для запуска в контексте вторичного потока методов, принимающих один входной параметр типа **object**.

Для блокировки выполнения текущего потока можно использовать метод **Thread.Sleep(int millisecondsTimeout);**

Пример выполнения метода без входных параметров в контексте вторичного потока:

```cs
private static void Main(string[] args)
{
    ThreadStart writeSecond = new(PrintSecondary);

    Thread thread = new(writeSecond);

    thread.Start();

    while (true)
    {
        Console.WriteLine("Primary");
    }
}

private static void PrintSecondary()
{
    while (true)
    {
        Console.WriteLine("\tSecondary");
    }
}
```

Пример выполнения метода с одним входным параметром типа "object" в контексте вторичного потока:

```cs
private static void Main(string[] args)
{
    ParameterizedThreadStart writeSecond = new(PrintSecondary);

    Thread thread = new(writeSecond);

    thread.Start("Test Argument");

    while (true)
    {
        Console.WriteLine("Primary");
        Thread.Sleep(100);
    }
}

private static void PrintSecondary(object arg)
{
    while (true)
    {
        Console.WriteLine($"\tSecondary, arg = \"{arg}\"");
        Thread.Sleep(100);
    }
}
```

Для запуска метода в контексте вторичного потока с помощью класса **Thread** возможно опустить явное создание экземпляров делегатов **ThreadStart** и **ParametrizedThreadStart**, используя технику "предположения делегата":

```cs
private static void Main(string[] args)
{
    Thread thread = new(PrintSecondary);

    thread.Start();

    while (true)
    {
        Console.WriteLine("Primary");
    }
}

private static void PrintSecondary()
{
    while (true)
    {
        Console.WriteLine("\tSecondary");
    }
}
```

При запуске метода в контексте вторичного потока, для него создаётся отдельный стек в памяти, имеющий свои собственные копии всех локальных переменных:

```cs
private static void Main(string[] args)
{
    Thread thread = new(PrintIterations);

    thread.Start("\tSecondary");

    PrintIterations("Primary");

    Console.ReadKey();
}

private static void PrintIterations(object? arg)
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

Для работы каждого нового потока выделяется дополнительная память для стека (дополнительный 1мб для каждого из потоков).

Класс **Thread** имеет перегрузки конструкторов, принимающие параметр **int maxStackSize**, позволяющий задать максимальный размер выделяемого потоку стека:

```cs
private static void Main(string[] args)
{
    Thread thread = new(PrintSecondary, 10_000_000);

    Thread threadParametrized = new(PrintSecondaryParametrized, 10_000);
}

private static void PrintSecondary()
{
    while (true)
    {
        Console.WriteLine("\tSecondary");
    }
}

private static void PrintSecondaryParametrized(object arg)
{
    while (true)
    {
        Console.WriteLine($"\tSecondary, arg = \"{arg}\"");
        Thread.Sleep(100);
    }
}
```

Однако, выделено больше памяти для стека потока не будет из-за структуры адресации памяти в защищённом режиме работы процессора.

ЦП имеет 5 режимов работы:
-	Режим х86-го (запускаются только базовая архитектура: базовые регистры, сегментная адресация);
-	Защищенный режим (имеется страничная адресация памяти);
-	Режим линейной адресации (позволяет адресовать всю память линейно, но это небезопасно);
-	Режим виртуального х86-го (позволяющий выполнять программы, написанные под DOS из защищённого режима);
-	Режим пониженного энергопотребления (спящий режим).

Существуют процессоры, которые позволяют выделять больший размер стека, например, некоторые процессоры, используемые в мобильных телефонах.

**Замыкание (Closure)** – возможность обращения из вложенного (внутреннего) метода к локальным переменным внешнего метода. 

```cs
internal class Program
{
    private static int _fieldIterationNumber = 0;

    private static void Main(string[] args)
    {
        int variableIterationNumber = 0;

        Thread thread = new(PrintIterations);

        thread.Start();

        Console.ReadKey();

        void PrintIterations()
        {
            while (_fieldIterationNumber < 10 && variableIterationNumber < 10)
            {
                _fieldIterationNumber++;
                variableIterationNumber++;

                Console.WriteLine($"{nameof(_fieldIterationNumber)}:{_fieldIterationNumber}|{nameof(variableIterationNumber)}:{variableIterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}
```

Если это возможно, то лучше использовать передачу аргументов "напрямую" через "object" параметр метода, сообщённого с делегатом "ParametrizedThreadStart", а не через замыкания, потому что замыкания заставляют компилятор генерировать классы-обёртки в результирующем коде, а также их экземпляры на куче. 

Такие классы-обёртки получают поля, соответствующие локальным переменным, передаваемым в класс-обёртку, а также метод, в который эти переменные должны быть переданы, но обращение к ним будет осуществляться уже как к полям класса, а не как к локальным переменным.

У потока имеется ряд свойств, основные из них:

- Thread.CurrentThread - статическое свойство, возвращающее ссылку на экземпляр текущего потока;
- ManagedThreadId - идентификатор управляемого потока в CLR. Отличается от Id потока в ОС.
- Name - имя потока;


```cs
private static void Main(string[] args)
{
    Thread thread = new(PrintIterations);

    thread.Start("Secondary");

    PrintIterations("Primary");

    Console.ReadKey();
}

private static void PrintIterations(object arg)
{
    Thread currentThread = Thread.CurrentThread;

    int id = currentThread.ManagedThreadId;

    currentThread.Name = arg.ToString();

    string name = currentThread.Name;

    int iterationNumber = 0;

    while (iterationNumber < 10)
    {
        iterationNumber++;

        Console.WriteLine($"{nameof(id)}:{id}|{nameof(name)}:{name}|{nameof(iterationNumber)}:{iterationNumber}");
        Thread.Sleep(100);
    }
}
```

С потоками также возможно сообщать анонимные методы, лямбда-выражения и лямбда-операторы при помощи техники предположения делегата:

```cs
private static void Main(string[] args)
{
    Thread threadAnonimousMethod = new(delegate ()
    {
        while (true)
        {
            Console.WriteLine("\tSecondary Anonimous Method");
        }
    });

    Thread threadLambdaOperator = new(delegate ()
    {
        while (true)
        {
            Console.WriteLine("\tSecondary Lambda Operator");
        }
    });

    threadAnonimousMethod.Start();
    threadLambdaOperator.Start();

    Console.ReadKey();
}
```

Класс "Thread" имеет свойство **IsBackground**, которое указывает, является ли поток фоновым. Отличие фоновых потоков от не фоновых в том, что основной поток при выполнении всех своих инструкций будет дожидаться завершения не фоновых потоков для завершения работы программы, а не фоновых - нет.

По умолчанию все потоки не являются фоновыми, что значит, что основной поток будет дожидаться их завершения для завершения работы программы.

Для того, чтобы сделать поток фоновым, необходимо присвоить свойству "IsBackground" значение **true**:

```cs
private static void Main(string[] args)
{
    Thread thread = new(PrintIterations);

    thread.IsBackground = true;

    thread.Start("Secondary");
}

private static void PrintIterations(object arg)
{
    int iterationNumber = 0;

    while (iterationNumber < 10)
    {
        iterationNumber++;

        Console.WriteLine($"{arg}|{nameof(iterationNumber)}:{iterationNumber}");
        Thread.Sleep(100);
    }
}
```

**Синхронизация доступа** к разделяемому ресурсу из нескольких потоков представляет собой **координирование действий** этих потоков.

**Объект синхронизации доступа** к разделяемому ресурсу называется также **объектом блокировки**.

**lock** – ключевое слово, которое не позволит одному потоку войти в важный раздел кода в тот момент, когда в нем находится другой поток. **Конструкция lock** принимает объект - разделяемый ресурс, который является индикатором того, можно ли выполнять код внутри этой конструкции в текущий момент времени. При попытке входа другого потока в заблокированный код потребуется дождаться снятия блокировки объекта.

lock обрамляет операторными скобками **критическую секцию** – участок кода, в котором происходит попытка доступа к разделяемому ресурсу.

```cs
internal class Program
{
    private static object _lock = new();

    private static void Main(string[] args)
    {
        Thread thread = new(PrintIterations);

        thread.Start("\tSecondary");

        PrintIterations("Primary");

        Console.ReadKey();
    }

    private static void PrintIterations(object arg)
    {
        lock (_lock)
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{arg} - {i}");
                Thread.Sleep(100);
            }
        }
    }
}
```

**Конструкция lock** ялвяется синтаксической обёрткой для вызова методов **Monitor.Enter(object obj)** и **Monitor.Exit(object obj)**:

```cs
internal class Program
{
    private static object _lock = new();

    private static void Main(string[] args)
    {
        Thread thread = new(PrintIterations);

        thread.Start("\tSecondary");

        PrintIterations("Primary");

        Console.ReadKey();
    }

    private static void PrintIterations(object arg)
    {
        Monitor.Enter(_lock);

        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"{arg} - {i}");
            Thread.Sleep(100);
        }

        Monitor.Exit(_lock);
    }
}
```
**lock** – аналог Monitor.Enter() и Monitor.Exit(), но безопаснее.


В реальных программах в качестве объекта блокировки используются разделяемые ресурсы, к которым производится обращение из разных потоков, например, не безопасные при работе с потоками коллекции, такие, как List:

```cs
internal class Program
{
    private static List<string> _list = new();

    private static void Main(string[] args)
    {
        Thread thread = new(PrintIterations);

        thread.Start("\tSecondary");

        PrintIterations("Primary");

        Console.ReadKey();
    }

    private static void PrintIterations(object arg)
    {
        lock (_list)
        {
            for (int i = 1; i <= 10; i++)
            {
                _list.Add($"{arg} - {i}");
            }

            foreach (string value in _list)
            {
                Console.WriteLine(value);
                Thread.Sleep(100);
            }

            Console.WriteLine($"End of {arg}");
            Console.WriteLine(new string('=', 20));
        }
    }
}
```

Объекты блокировки также могут быть испольбзованы для корректного обращения к разделяемым неуправляемым ресурсам, таким, как порты ввода-вывода или консоль:

```cs
internal class Program
{
    private static object _lock = new();

    private static void Main(string[] args)
    {
        Thread thread = new(SafePrintRedText);

        thread.Start("\tSecondary");

        SafePrintGreenText("Primary");

        Console.ReadKey();
    }

    private static void SafePrintRedText(object arg)
    {
        for (int i = 0; i < 10; i++)
        {
            lock (_lock)
            {
                PrintColoredText(arg.ToString(), ConsoleColor.Red);
            }
            Thread.Sleep(100);
        }
    }

    private static void SafePrintGreenText(object arg)
    {
        for (int i = 0; i < 10; i++)
        {
            lock (_lock)
            {
                PrintColoredText(arg.ToString(), ConsoleColor.Green);
            }
            Thread.Sleep(100);
        }
    }

    private static void PrintColoredText(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
```
