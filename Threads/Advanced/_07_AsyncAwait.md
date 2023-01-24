# **Async Await**

**async await** - ключевые слова, которые позволяются написать асинхронный код как синхронный, упрощая разработку.

Ключевые слова **async await** были введены для создания очень сильной абстракции, их основная цель - создать иллюзию для написания асинхронного кода, как будто он является обычным синхронным кодом, где следующая операция не начнётся без полноценного окончания выполнения предыдущей. Это делает такой код намного более чистым и понятным программисту, а также даёт преимущества при рефакторинге и отладке.

Ключевые слова **async await** полностью поддерживают работу асинхронного шаблона программирования **TAP**.

Наличие ключевых слов **async await** не означает, что их нужно применять всегда, так как есть особенности выполнения программы при использовании этих ключевых слов, которые могут как дать преимущества, так и породить недостатки.

## Ключевое слово **async**
---

**Ключевое слово async** является модификатором для методов. Указывает, что метод является асинхронным. **Модификатор async** позволяет использовать в асинхронном методе **ключевое слово await** и указывает компилятору на необходимость создания **конечного автомата** для обеспечения работы асинхронного метода.

Функционал модификатора ```async```:

- Указывает компилятору, что необходимо создать конечный автомат для обеспечения работы асинхронного метода. Основная задача конечного автомата - приостановка и затем асинхронное возобновление работы в точках ожидания;
- Позволяет использовать в теле асинхронного метода ключевое слово await;
- Позволяет записать возвращаемое значение (если метод возвращает Task<TResult>) или необработанное исключение в результирующую задачу.

## Ключевое слово **await**
---
**Ключевое слово await** является унарным оператором, операнд которого располагается справа от самого оператора. Применение **оператора await** означает, что необходимо дождаться завершения выполнения асинхронной операции. При этом, если ожидание будет произведено, то вызывающий поток будет освобожден для своих дальнейших действий, а код, находящейся после **оператора await**, по завершению асинхронной операции будет выполнен в виде продолжения.

Тип, к которому применяется оператор ```await```, должен иметь доступный метод ```GetAwaiter()```, возвращаемый объект которого должен иметь доступное свойство ```bool IsCompleted``` и доступный метод ```GetResult()```, возвращаемый тип которого зависит от возвращаемого типа асинхронного метода, а также эт от объект должен реализовать один из интерфейсов ```INotifyCompletion``` или ```ICriticalNotifyCompletion```. Класс ```Task``` подходит под заданные критерии. К реализации рекомендуется интерфейс ```ICriticalNotifyCompletion```, наследующий интерфейс ```INotifyCompletion```.

Функционал оператора ```await```:
- Если асинхронная операция завершена:
    - Дальнейшее выполнение метода продолжается синхронно в том же вызывающем потоке.
- Если асинхронная операция не завершена:
    - Компилятор с помощью специальных типов инициирует ожидание завершения асинхронной операции
    - Инициируется захват контекста выполнения.
    - Происходит «регистрация кода», который находился после оператора await в виде продолжения (Continuation), которое выполнится по завершению асинхронной операции
    - Если есть возможность, продолжение будет отправлено на выполнение в вызывающий поток
    - Выполняется освобождение вызывающего потока.
    - Экземпляр класса-делегата, являющегося продолжением, повторно запускает асинхронный метод. Данный метод
продолжит выполняться с точки, на которой оператор await инициировал ожидание.

---

**Работу ключевых слов async и await обеспечивает компилятор**, поэтому без его поддержки будет
потеряна вся "магия" ключевых слов.

**Асинхронные методы** – методы, которые используют ключевые слова async/await и имеют специальный
тип возвращаемого значения. В имени метода имеют суффикс Async или TaskAsync для быстрой
узнаваемости.

С помощью ключевых слов **async await** создают **асинхронные методы**.

## **Асинхронные методы**

**Асинхронные методы** – методы, которые используют ключевые слова **async/await** и имеют специальный тип возвращаемого значения. В имени метода имеют суффикс **Async** или **TaskAsync** (если в коде уже имеется аналогичный метод с суффиксом **Async**, построенный по старым шаблонам) для быстрой узнаваемости.

Наличие ключевого слова **async** не означает, что метод будет выполняться во вторичном/фоновом потоке, а лишь означает, что будет построен конечный автомат, и метод сможет содержать операторы **await**, и часть операций метода сможет выполняться асинхронно.

## **01 Использование ключевых слов async await**
---
Пример использования ключевых слов **async await**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **Типы возвращаемых значений асинхронных методов**

**Асинхронные методы** могут иметь следующие типы **возвращаемых значений**:
- Тип void – используется только для обработчиков событий;
- Тип Task – для асинхронной операции, которая не возвращает значение;
- Тип Task<TResult> - для асинхронной операции, которая возвращает знаение;
- Тип ValueTask – для асинхронной операции, которая не возвращает значения. Используется только тогда, когда действительно может привести к приросту производительности;
- Тип ValueTask<TResult> - для асинхронной операции, которая возвращает значение. Используется только тогда, когда действительно может привести к приросту производительности.

## **Применение оператора await**
Для применения оператора await к типу, данный тип должен bметь доступный метод **GetAwaiter()**, возвращаемый объект которого должен иметь:
- Реализацию интерфейсов **ICriticalNotifyCompletion** и **INotifyCompletion**.
- Свойство **bool IsCompleted { get;}**.
- Метод **GetResult()**. Тип возвращаемого значения метода должен зависеть от того, должна ли вернуть асинхронная операция результат. Если да, то тип должен совпадать с **типом результата операции**. Если нет - тип возвращаемого значение должен быть **void**.

> TODO: добавить изображение со схемой работы оператора **await**

Алгоритм работы оператора **await**:
1. Вызов оператора await;
2. Проверка завершённости асинхронной операции;
3. Если асинхронная операция завершена, то получение результата асинхронной операции и продолжение выполнения метода. Если асинхронная операция ничего не возвращает, то подразумевается полное окончание её работы;
4. Если асинхронная операция не завершена, то установка продолжения для асинхронной операции и возвращение управления вызывающему потоку. По завершению работы асинхронной операции для возобновления работы метода будет вызвано установленное ранее продолжение.

При возможности, продолжение попытается выполниться в вызывающем потоке, который инициировал ожидание. В случае неудачи, оно будет выполнено в контексте синхронизации, если его удалось захватить, а если нет, то в контексте планироващика задач, а если и его не будет, то продолжение будет выполнено в контексте одного из потоков из пула потоков.

Оператор **await** не может передать просто так в качестве продолжения случайный класс-делегат, для этого ему необходимы специальные механизмы и контекст синхронизации. Консольные приложения по умолчанию не обладают такими механизмами, поэтому, если продолжение не удастся выполнить в контексте вызывающего потока, он это сделает в контексте потока из пула потоков. 

Выводы: 
- Оператор **await** при своём срабатывании не блокирует работу вызывающего потока, возвращая ему управление;
- Часть метода, расположенная после оператора **await** может стать продолжением асинхронной задачи и быть выполнена в другом потоке, ол=ичном от потока, в котором выполнялась часть метода до оператора **await**;
- Оператор **await** имеет преимущество над свойством **Result** и методом **Wait()**, так как не блокирует вызывающий поток;
- Ключевые слова **async** и **await** делают очень элегантным и простым асинхронное программирование в C#, на манер синхронного программирования, где последующий за асинхронной операцией, зависящий от её завершения код автоматически переносится в продолжение асинхронной операции, и нет необходимости вызывать метод **ContinueWith()**;
- Ключевые слова **async** и **await** делают код более красивым и читабельным, его гораздо проще поддерживатьи  отлаживать;
- Если не ожидать выполнения асинхронных методов, то по умолчанию они будут выполняться параллельно вызывающему потоку, но это зависит от планировщика задач.

Если вызвать асинхронный метод с возвращаемым значением, отличным от **void**, то компилятор выдаст предупреждение **CS4014**, что происходит вызов асинхронного метода без его ожидания, что может привести к тому, что вызывающий метод может завершить своё выполнение раньше, чем асинхронный метод.

## **(awaitable) ожидаемые методы**

Ожидаемые методы – это асинхронные методы, завершение которых можно подождать, если необходим результат их работы в данный момент. По сути, являются обычными асинхронными методами. Если не ожидать окончания его работы, то можно не получить его рещультат в нужный момент.

Ожидать завершения асинхронных методов и задач необходимо с помощью оператора await.

Другие способы ожидания:
- Свойство **Result**;
- Методы ожидания **Wait(), WaitAll(), WaitAny()**;
- Метод **GetResult()**, вызванный на экземпляре структуры **TaskAwaiter**, которая получена через вызов метода **GetAwaiter()**.

Не рекомендуется прибегать к вызову **GetAwaiter()**. Пользуйтесь ключевым словом await. Тип **TaskAwaiter** и его члены предназначены для внутреннего использования компилятором. Он помогает оператору **await** подписаться на оповещение о звершении работы задачи.

Методы сами вам подскажут, что они ожидаемые – с помощью подсветки синтаксиса. Среда **VisualStudio** подсказывает, что метод является ожидаемым (**awaitable**). Данным методам соответствует зеленая линия подчеркивания, надпись  **(awaitable)** и предупреждение от компилятора **CS4014**. **IntelliSense** также подскажет способ взаимодействия с ожидаемыми методами.

> TODO: картинки с надписью (awaitable) и подсказками об использовании асинхронных методов из VisualStudio.

Если ваш метод будет без модификатора **async**, но с возвращаемым значением **Task/ValueTask** или их универсальными вариантами, то его вызов в обычных методах будет без предупреждения, а только с подсказкой awaitable при наведении.

Но вызов такого метода в асинхронном методе с модификатором **async** уже будет с предупреждением, что метод можно подождать.

> TODO: картинки с предупреждениями в **VisualStudio**.

Если ваш метод будет с модификатором **async**, то его вызов как из обычных, так и из асинхронных методов будет происходить с предупреждением. Если необходимо его запустить просто для параллельного выполнения, то предупреждение можно проигнорировать или скрыть атрибутом или директивой, но такой подход является не совсем правильным, поэтому можно сделать расширяющий метод, скрывающий это предупреждение.

## **02 Методы GetAwaiter() и GetResult()**
---
Пример вызова методов **GetAwaiter()** и **GetResult()**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask").GetAwaiter().GetResult();

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **03 Подавление предупреждения ожидаемых методов без await**
---
Пример создания статического класса с методом расширения, подавляющим предупреждения об отсутствии оператора **await** перед вызовом асинхронных методов: 

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask").SuppressAwaitableWarning();

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}

internal static class AsyncAwaitExtensions
{
    internal static void SuppressAwaitableWarning(this Task _)
    {
    }
}
```

## **04 Отложенное ожидание выполнения асинхронного метода**
---
Ожидание задачи не обязательно сразу же при её вызове. Её можно запустить, получить ссылку на экземпляр задачи, выполнить необходимые в данный момент операции и уже после этого инициировать ожидание:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        Task task = PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        task.GetAwaiter().GetResult();

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **05 Синхронное продолжение**
---
Если оператор **await** к моменту своего выполнения обнаружит, что асинхронная операция уже завершена, то просто выполнит продолжение синхронно, в контексте вызывающего потока.

Пример, который демонстрирует это, где асинхронная задача выполняется 1000мс, а её ожидание перед вызовом оператора **await** составляет 1200мс:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask").SuppressAwaitableWarning();

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        Thread.Sleep(1200);

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}

internal static class AsyncAwaitExtensions
{
    internal static void SuppressAwaitableWarning(this Task _)
    {
    }
}
```

В коде производственных приложений подобный код может давать различные результаты, так как многие дейтсвия и время их выполнения зависит от внешних факторов, таких, как время ответа внешних сетевых сервисов или баз данных.

## **Конечный автомат (Finite-State Machine)** 

**Конечный автомат (Finite-State Machine)** - это модель вычислений, которая позволяет объекту изменить свое поведение в зависимости от своего внутреннего состояния. Поведение объекта изменяется настолько, что создается впечатление, что изменился класс объекта.

В один момент времени может быть активно только одно состояние. По завершению выполнения действия, конечный автомат меняет свое внутреннее состояние.

> TODO: изображение со схемой работы конечного автомата оператора **await**

Конечный автомат для ```async await``` – объект, способный представить состояние асинхронного метода, которое можно сохранить при достижении оператора await и восстановить позже, для дальнейшего продолжения выполнения асинхронного метода.

Конечный автомат выступает в роли типа, который сохраняет состояние и локальные переменные метода в виде полей.

При сохранении объекта такого типа, будет сохранено состояние и локальные переменные асинхронного метода. Это позволяет полноценно сохранить состояние асинхронного метода в любой точке, для дальнейшего возобновления его работы позже.

Конечный автомат для повышения производительности описывается структурой, ведь при синхронном завершении асинхронного метода не придется выделять память для объекта кучи.

### **Внутренняя реализация async await**

Работу ключевых слов ```async await``` обслуживает конечный автомат. Компилятор, с помощью интерфейса ```IAsyncStateMachine``` и специальных строителей, создает конечный автомат, который обслуживает асинхронный метод "под капотом".

Сам асинхронный метод превращается в **метод-заглушку**, который будет использовать созданный конечный автомат.

> TODO: изображение с типами построителей асинхронных методов и интерфейсом асинхронной машины состояний

### **Трансформация асинхронного метода**

Асинхронный метод перестраивается компилятором в метод-заглушку. Тело асинхронного метода перемещается в метод ```MoveNext()``` конечного автомата, с некоторыми дополнениями и оптимизациями.

Метод-заглушка создает конечный автомат, который обслуживает работу асинхронного метода. В нем происходит инициализация открытых полей структуры конечного автомата.

Здесь же и происходит первый запуск конечного автомата, с помощью вызова метода ```Start()```.

Атрибут ```AsyncStateMachine``` помогает идентифицировать конечный автомат для асинхронного метода. Благодаря этому атрибуту с помощью рефлексии можно найти в коде все асинхронные методы с модификатором ```async```. Нельзя самостоятельно декорировать методы этим атрибутом, так как им пользуется только компилятор.

Асинхронный метод:
```cs
private static async Task PrintIterationsAsync(string taskName)
{
    Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

    Task printIterationsTask = new(PrintIterations, taskName);

    printIterationsTask.Start();

    await printIterationsTask;

    Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
}
```

Метод-заглушка:
```cs
[AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
[DebuggerStepThrough]
private static Task PrintIterationsAsync(string taskName)
{
    PrintIterationsAsyncStateMachine stateMachine = new();
    stateMachine._builder = AsyncTaskMethodBuilder.Create();
    stateMachine._taskName = taskName;
    stateMachine._state = -1;
    stateMachine._builder.Start(ref stateMachine);

    return stateMachine._builder.Task;
}
```

## **06 Конечный автомат async await после декомпиляции в режиме Debug**
---
Так как работа ключевых слов **async await** полностью поддерживается компилятором, то следует рассмотреть, в какие инструкции и структуры данных преобразует компилятор асинхронные методы, к которым применяются это ключевые слова.

В рассмотренных далее примерах приводится упрощённая и улучшенная для читаемости версия преобразованного компилятором кода, в которой удалены не относящиеся к теме конструкции, которые могли отвлечь внимание от самого **async await**. 

Компилятор после преобразования метода с ключевыми словавми **async await** использует технические названия переменных, полей, типов и методов, например **s__8**, **num1**, **num2**, и другие, которые мало говорят своими названиями об их назначении, а также использует недопустимые в идентификаторах (названгиях) в исходном коде C# символы, такие, как апострофы **(`)**.

В связи с этим недопустимые симвлоы были удалены, а технические названия замены на названия концептуальные.

Также преобразованные компилятором конструкции, такие, как конструкции, связанные с обработкой и форматированием строк, были заменены обратно на исходные, чтобы не отвлекать внимание от рассматриваемой темы - преобразований, вызванных ключевыми словами **async await**.

Пример декомпилированного кода, скомпилированного ранее в режиме **Debug** из кода с методом, к которому были применены ключевые слова **async await**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
    [DebuggerStepThrough]
    private static Task PrintIterationsAsync(string taskName)
    {
        PrintIterationsAsyncStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._taskName = taskName;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);

        return stateMachine._builder.Task;
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    [CompilerGenerated]
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string _taskName;
        private Task _printIterationsTask;
        private TaskAwaiter _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            try
            {
                TaskAwaiter awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                    _printIterationsTask = new Task(PrintIterations, _taskName);
                    _printIterationsTask.Start();

                    awaiter = _printIterationsTask.GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        _state = 0;
                        _awaiter = awaiter;
                        PrintIterationsAsyncStateMachine stateMachine = this;

                        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                        return;
                    }
                }
                else
                {
                    awaiter = _awaiter;
                    _awaiter = new TaskAwaiter();
                    _state = -1;
                }

                awaiter.GetResult();

                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _printIterationsTask = null;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _printIterationsTask = null;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

Обзор преобразования:

1. Метод **Main()** остался таким же, каким и был;
2. Метод **PrintIterations()** остался таким же, каким и был;
3. Метод **PrintIterationsAsync()** был притерпел значительные изменения:
    - Прежними остались: **идентификатор (название)**, **набор параметров**, **тип возвращаемого значения**, **модификатор доступа** и **модификатор static**;
    - Модификатор **async был удалён**, так как после компиляции компилятором C# он уже не нужен, ведь в исходном коде он требуется как раз для того, чтобы сказать компилятору о необходимости проведения данного преобразования кода, после чего надобность в нём отпадает; 
    - Метод был декорирован атрибутами 
        ```[AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]```, который, по сути, является заменой для модификатора **async** и принимающий в качестве параметра тип конченого автомата, обеспечивающего работу асинхронного метода и атрибутом для отладки ```[DebuggerStepThrough]```;
    - В классе ```Program```, имеющем метод ```PrintIterationsAsync```, был сгенерирован класс ```PrintIterationsAsyncStateMachine```, унаследованный от интерфейса ```IAsyncStateMachine```;
    - В классе ```PrintIterationsAsyncStateMachine``` было реализовано 2 метода интерфейса ```IAsyncStateMachine``` и ряд полей:
        - Метод ```void IAsyncStateMachine.MoveNext()```
        - Метод ```void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)```, декорированный атрибутом ```[DebuggerHidden]```, при компиляции в режиме **Debug** имеющий пустое тело, так как необходим только для упаковки конечного автомата и размещения его на куче и сохранения его для последующих вызовов после смены состояния конечного автомата, когда экземпляр самого конечного автомата создаётся в виде структурного типа в режиме **Release**;
        - Поле ```public int _state``` - Cостояние (в данном случае можно сказать, что это стадия) конечного автомата;
        - Поле ```public AsyncTaskMethodBuilder _builder``` - Тип ```AsyncTaskMethodBuilder``` представляет строителя асинхронных методов. Он необходим для обеспечения инфраструктуры работы конечного автомата;
        - Поле ```public string _taskName``` - Параметр **taskName** метода ```PrintIterationsAsync()``, которая будет передана в конечный автомат асинхронной операции. Начиная с этого параметра и далее, до поля **_awaiter**, будут созданы поля, соответствующие параметрам и переменным из исходного метода, которые задействованы в асинхронной операции. Открытые поля будут соответствовать параметрам метода и могут быть инициализированы извне, закрытые поля будут соответствовать локальным переменным метода и могут быть инициализированы только изнутри;
        - Поле ```private Task _printIterationsTask``` - Переменная **_printIterationsTask** метода ```PrintIterationsAsync()``, которая будет инициализирована и использована в процессе выполнения конечного автомата;
        - Поле ```private TaskAwaiter _awaiter``` - 
    - Внутри метода ```PrintIterationsAsync()``` генерируются следующие операции:
        - Создаётся класса экземпляр конечного автомата ```PrintIterationsAsyncStateMachine```;
        - Заполняются поля созданного экземпляра класса ```PrintIterationsAsyncStateMachine```:
            - **_builder** - С помощью статического фабричного метода ```Create()``` структуры ```AsyncTaskMethodBuilder``` мы получаем экземпляр этой структуры и присваиваем текущему полю;
            - **_taskName** - Инициализируется значением параметра **taskName** 
            - **_state** - Инициализируется значением **-1**. Значение **-1** означает, что конечный автомат создан и готов к выполнению. 
        - На экземпляре ```AsyncTaskMethodBuilder``` вызывается обобщённый метод ```Start<T>(ref T stateMachine)```, закрытый типом ```PrintIterationsAsyncStateMachine```, в параметр которого передаётся только что созданный и проиниализированный экземпляр класса ```PrintIterationsAsyncStateMachine```;
        - С помощью оператора ```return``` из метода возвращается задача, представленная экземпляром класса ```Task```, размещённая в свойстве ```stateMachine._builder.Task```. Задача, возвращаемая этим свойством, называется задачей-марионеткой, которая отображает выполнение конечного автомата. Именно поэтому в асинхроных методах с модификатором ```async``` не требуется вызывать оператор ```return``` и возвращать экземпляр класса ```Task```.
    - Все операции метода ```PrintIterationsAsync()``` были перенесены в видоизменённом виде в метод ```MoveNext()``` класса ```PrintIterationsAsyncStateMachine```.

Процесс запуска конечного автомата:
1. Внутри асинхронного метода создаётся экземпляр конечного автомата класса ```PrintIterationsAsyncStateMachine```, инициализируются его открытые поля;
2. На экземпляре ```AsyncTaskMethodBuilder``` вызывается обобщённый метод ```Start<T>(ref T stateMachine)```, закрытый типом ```PrintIterationsAsyncStateMachine```, в параметр которого передаётся только что созданный и проиниализированный экземпляр класса ```PrintIterationsAsyncStateMachine```;
3. Внутри метода ```Start<T>(ref T stateMachine)``` запускается метод ```IAsyncStateMachine.MoveNext()```, реализованный в конечном автомате. Метод ```MoveNext()``` ничего не принимает и не возвращает, так как все необходимые для работы конечного автомата данные уже переданы в конечный автомат при его создании и инициализации.
4. В начале метода ```MoveNext()``` создаётся переменная ```int state``` и инициилизируется из поля ```_state```, для того, чтобы получить текущее состояние конечного автомата, представленное целым числом. В зависимости от этого состояния конечный автомат будет выполнять соответствующие этому состоянию действия;
5. Далее расположен блок ```try/catch```, созданный компилятором. Он необходим для того, чтобы перехватить большинство исключений, кроме самых тяжёлых, и, за счёт этого дать выполниться конечному автомату в любом случае. В тело блока ```try``` и было в изменённом виде перенесено тело асинхронного метода;
6. В начале блока ```try``` создаётся переменная ```awaiter``` типа ```TaskAwaiter```, которая будет нужна для созранения объекта ожидания завершения асинхронной задачи;
7. В первой части ```if``` условной конструкции проверяется значение состояния конечного автомата, записанного в переменную ```state```. При первом запуске метода ```MoveNext()``` в поле ```_state``` было установлено значение **-1**. Это означает, что в первый раз условие удовлетворит истинности и будет выполнен этот блок;
    1. Далее первой части условной конструкции содержатся перенесённые из метода ```PrintIterationsAsync``` располагавшиеся до оператора ```await```;
    2. Вместо операции с оператором ```await``` выполняется вызов метода ```GetAwaiter()``` на экземпляре класса ```Task```, который ранее ожидался оператором ```await``` и возвращаемое значение этого метода присваивается в переменную ```awaiter```;
    3. Далее создаётся условная конструкция, проверяющая на объекте ожидания, записанном в переменную ```awaiter```, не завершена ли уже асинхронная операция. Если она ещё не завершена, то в поле ```_state``` записывается значение **0**, в объект ожидания записывается в поле ```_awaiter```, на объекте строителя асинхронных методов из поля ```_builder``` вызывается метод ```AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)```, закрытый типами ```TaskAwaiter``` и ```PrintIterationsAsyncStateMachine```, в который передаются объект ожидания выполнения асинхронной операции и ссылка на экземпляр конечного автомата, который нужно будет снова запустить по окончанию ожидания объектом ожидания, после чего производится завершение метода через вызов оператора ```return```.
8. Во второй части ```else``` условной конструкции, которая выполнится при повторном запуске конечного автомата в состоянии **0**, в переменную ```awaiter``` возвращается объект ожидания из поля ```_awaiter```, в само поле ```_awaiter``` записывается новый объект ожидания, который не прикреплён к какой-либо задаче, а в поле ```_state``` присваивается значение **-1**;
9. За всеми блоками условной конструкцией вызывается метод ```GetResult()``` на изначальном объекте ожидания, сохранённом в переменной ```awaiter```, который завершит ожидание асинхронной задачи, а также выбросит исключение, которое могло произойти при выполнении асинхронной задачи, чтобы блок ```catch``` смог его поймать;
10. Далее содержатся операции метода, перенесённые из метода ```PrintIterationsAsync``` располагавшиеся после оператора ```await```;
11. После расположен блок ```catch```, который, в случае срабатывания, устанавливает полю ```_state``` значение **-2**, вызывает метод ```SetException()``` на объекте строителя асинхронных методов из поля ```_builder``` и передаёт ему в качестве аргумента экземпляр перехваченного исключения, после чего завершает работу метода через вызов оператора ```return```;
12. В самом конце полю ```_state``` присваивается значение **-2** на объекте строителя асинхронных методов из поля ```_builder``` вызывается метод ```SetResult()```, который указывает задаче-марионетке, что задача выполнена успешно, после чего метод ```MoveNext()``` и конечный автомат завершаются.

Конечный автомат может содержать несколько разновидностей полей:
- Состояние конечного автомата;
- Строитель асинхронного метода;
- Объекты ожидания;
- Параметры асинхронного метода;
- Локальные переменные асинхронного метода;
- Временные переменные стека;
- Внешний тип.

### **Состояния конечного автомата:**
- **-1** - конечный автомат создан, либо находится в процессе выполнения;
- **-2** - конечный автомат окончил свою работу, без уточнения, завершение произошло с успехом или с ошибкой;
- **0+** - конечный автомат находится в процессе ожидания выполнения операций текущего состояния для перехода к следующему состоянию, обозначенным текущим значением.

### **Интерфейс IAsyncStateMachine**

Интерфейс ```IAsyncStateMachine``` используется для создания конечного автомата, который обеспечивает работу асинхронного метода. Конечный автомат создается для получения объекта, способного представить состояние асинхронного метода, которое можно сохранить при достижения оператора await и для его дальнейшего восстановления. Таким образом происходит запоминание того, в каком месте находится приложение.

### **Метод MoveNext()**

В основе асинхронного конечного автомата лежит метод ```MoveNext()```. Он начинает свою работу через вызов метода ```Start()``` на одном из строителей асинхронных методов. Каждый последующий запуск происходит в виде продолжения, когда ему необходимо возобновить выполнение после приостановки, вызванной оператором ```await```.

В отличие от метода ```SetStateMachine()```, у метода ```MoveNext()``` достаточно много обязанностей:
- Выполнение кода из правильного места;
- Сохранение значений локальных переменных и расположения выполнения кода в виде состояния (при возврате управления из-за инициации ожидания выполнения асинхронной задачи);
- Планирование продолжения, если было инициировано ожидание;
- Получение результатов асинхронных задач от объектов ожидания завершения асинхронных задач;
- Передача исключения. Либо оно будет передано в задачу-марионетку, из-за чего задача будет считаться проваленной, либо в захваченных контекст синхронизации вызывающего потока, или выброшено в контексте пула потоков;
- Передача результата или завершение выполнения асинхронного метода.

Структура метода ```MoveNext()``` для одного логического состояния (не учитывая -1 и -2):

```cs
void IAsyncStateMachine.MoveNext()
{
    int localState = this.state;

    try
    {
        if(localState != 0)
        {
            // Здесь находится часть кода асинхронного метода до первого оператора await.

            if (!awaiter.IsCompleted)
            {
                _state = 0;
                _awaiter = awaiter;
                PrintIterationsAsyncStateMachine stateMachine = this;

                _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                return;
            }
        }
        else
        {
            awaiter = _awaiter;
            _awaiter = new TaskAwaiter();
            _state = -1;
        }

        awaiter.GetResult();

        // Здесь находится часть кода, которая будет выполнена в виде продолжения после возобновления.
    }
    catch (Exception ex)
    {
        _state = -2;
        _builder.SetException(ex);
        return;
    }
    _state = -2;
    _builder.SetResult();
}
```

Структура метода ```MoveNext()``` для множества логических состояний:

```cs
void IAsyncStateMachine.MoveNext()
{
    int localState = this.state;

    try
    {
        switch(localState)
        {
            case 0: goto Label_0
            case 1: goto Label_1
            …. // Количество case и переходов зависит от количества вызовов операторов await.
            default:
            // Здесь находится часть кода асинхронного метода до первого оператора await.
            break;
        }
        Label_0:
        // Здесь находится часть кода, которая будет выполнена в виде продолжения после возобновления.
        Label_1:
        // Здесь находится часть кода, которая будет выполнена в виде продолжения после возобновления.
    }
    catch (Exception ex)
    {
        _state = -2;
        _builder.SetException(ex);
        return;
    }
    _state = -2;
    _builder.SetResult();
}
```

Методы:
- ```void MoveNext()``` – выполняет тело асинхронного метода, перемещает конечный автомат в следующее состояние;
- ```void SetStateMachine(IAsyncStateMachine stateMachine)``` – упаковывает конечный автомат из стека на кучу.

### Метод ```AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)```
Этот метод выполняет следующие операции:
1. Создаёт делегат продолжения в виде вызова метода ```MoveNext()``` конечного автомата;
2. С помощью переданного объекта ожидания ```TaskAwaiter``` устанавливает для ожидаемой задачи продолжение с помощью метода ```UnsafeOnCompleted()```, который является аналогом метода ```ContinueWith()``` класса ```Task```, но немного сложее.

## **07 Конечный автомат async await после декомпиляции в режиме Release**
---
Пример декомпилированного кода, скомпилированного ранее в режиме **Release** из кода с методом, к которому были применены ключевые слова **async await**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
    [DebuggerStepThrough]
    private static Task PrintIterationsAsync(string taskName)
    {
        PrintIterationsAsyncStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._taskName = taskName;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);

        return stateMachine._builder.Task;
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    [CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string _taskName;
        private TaskAwaiter _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            try
            {
                TaskAwaiter awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                    Task task = new(PrintIterations, _taskName);
                    task.Start();

                    awaiter = task.GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        _state = 0;
                        _awaiter = awaiter;

                        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);

                        return;
                    }
                }
                else
                {
                    awaiter = _awaiter;
                    _awaiter = new TaskAwaiter();
                    _state = -1;
                }

                awaiter.GetResult();

                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
            _builder.SetStateMachine(stateMachine);
        }
    }
}
```

Отличия кода, скомпилированного в режиме **Release** от кода, скомпилированного в режиме **Debug**:
- Вместо класса ```PrintIterationsAsyncStateMachine``` создаётся структура ```PrintIterationsAsyncStateMachine```, декорированная атрибутами ```[CompilerGenerated]``` и ```[StructLayout(LayoutKind.Auto)]```;
- Локальные переменные по возможности оптимизированы, и, если их значения используются в рамках одного состояния конечного автомата, то поля для них не создаются;
- Метод ```IAsyncStateMachine.SetStateMachine()``` теперь имеет реализацию - вызов метода ```SetStateMachine()``` на экземпляре структуры ```AsyncTaskMethodBuilder```, хранящемся в поле ```_builder``` и передача этому методу в качестве аргумента экземпляра типа ```IAsyncStateMachine```, полученного через параметр ```stateMachine``` рассматриваемого метода.

### **Схема работы оператора ```await``` в конечном автомате:**

> TODO: изображение со схемой работы оператора ```await``` в конечном автомате

> TODO: define is it necessary to describe an ```await``` operator work in a **finite-state machine**

## **Задача-марионетка (Puppet Task)**
---
**Задача-марионетка** – это обыкновенная задача, жизненным циклом которой управляет конечный автомат. Результат выполнения задачи может быть указан позже, чем момента создания задачи-марионетки.

Результат задачи-марионетки указывает конечный автомат. Конечный автомат может как отдать ей результат (означает успешное выполнение), так и пробросить исключение (означает провальное выполнение).

Тип задачи-марионетки соответствует возвращаемому типу асинхронного метода.

В декомпилированном коде асинхронного метода ```PrintIterationsAsync``` возвращается задача, представленная экземпляром класса ```Task``` из свойства ```Task``` экземпдяра структуры ```AsyncTaskMethodBuilder``` из поля ```_builder``` экземпляра класса ```PrintIterationsAsyncStateMachine```. Эта задача представляет конечный автомат, в неё устанавливаются соответствующие состояния, результаты и исключения, полученные в ходе выполнения операций конечного автомата:

```cs
[AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
[DebuggerStepThrough]
private static Task PrintIterationsAsync(string taskName)
{
    PrintIterationsAsyncStateMachine stateMachine = new();
    stateMachine._builder = AsyncTaskMethodBuilder.Create();
    stateMachine._taskName = taskName;
    stateMachine._state = -1;
    stateMachine._builder.Start(ref stateMachine);

    return stateMachine._builder.Task;
}
```

## **08 Синхронное выполнение конечного автомата**
---
Чтобы конечный автомат выполнился синхронно, необходимо после создания ожидаемой задачи ```_printIterationsTask```, но до вызова на ней метода ```GetAwaiter()``` и присвоения его возвращаемого значения в переменную ```awaiter``` заблокировать выполняющийся поток на время, большее, чем время выполнения ожидаемой задачи, например, с помощью метода ```Thread.Sleep(1200)```: 

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
    [DebuggerStepThrough]
    private static Task PrintIterationsAsync(string taskName)
    {
        PrintIterationsAsyncStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._taskName = taskName;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);

        return stateMachine._builder.Task;
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    [CompilerGenerated]
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string _taskName;
        private Task _printIterationsTask;
        private TaskAwaiter _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            try
            {
                TaskAwaiter awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                    _printIterationsTask = new Task(PrintIterations, _taskName);
                    _printIterationsTask.Start();

                    Thread.Sleep(1200);

                    awaiter = _printIterationsTask.GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        _state = 0;
                        _awaiter = awaiter;
                        PrintIterationsAsyncStateMachine stateMachine = this;

                        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                        return;
                    }
                }
                else
                {
                    awaiter = _awaiter;
                    _awaiter = new TaskAwaiter();
                    _state = -1;
                }

                awaiter.GetResult();

                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _printIterationsTask = null;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _printIterationsTask = null;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

### **Строители асинхронных методов**

Для построения задачи, которую можно сделать завершенной позже и для представления выполнения
асинхронного метода используются специальные **строители асинхронных методов**. В зависимости от типа возвращаемого значения асинхронного метода используется свой строитель.

Виды строителей:
- ```AsyncTaskMethodBuilder``` – строитель для асинхронных методов с типом ```Task```;
- ```AsyncTaskMethodBuilder<TResult>``` - строитель для асинхронных методов с типом ```Task<TResult>```;
- ```AsyncVoidMethodBuilder``` – строитель для асинхронных методов с типом ```void```;
- ```AsyncValueTaskMethodBuilder``` – строитель для асинхронных методов с типом ```ValueTask```;
- ```AsyncValueTaskMethodBuilder<TResult>``` – строитель для асинхронных методов с типом ```ValueTask<TResult>```.

Строители асинхронных методов в C# описываются в виде структур для повышения производительности и оптимизированы для работы с асинхронными методами.

У строителей есть весь необходимый функционал для создания **задачи-марионетки**, такой, как методы, с помощью которых можно указать результат (```SetResult```), исключение (```SetException```) или вызвов ожидания с регистрацией продолжения для асинхронной задачи (```AwaitOnCompleted/AwaitUnsafeOnCompleted```).

Строители асинхронных методов лучше не использовать напрямую. Они созданы для использования компилятором. Для создания задач-марионеток пользователями существует открытый API в виде класса ```TaskCompletionSource```.

### **Разбитие асинхронного метода на блоки компилятором**

При применении оператора await, каждый новый блок будет заставлять компилятор разбивать метод на определённое количество блоков: по 1 блоку на каждый оператор await, в котором будет размещён весь код от текущего оператора await, включая операцию ожидания задачи, замещающую сам этот оператор, до следующего оператора await, либо до конца метода, а также первый блок, который включит в себя операции от начала метода и до первого оператора ```await```.

> TODO: изображение со схемой разбиения метода операторами ```await```.

## **09 Множественное использование операторов await - 2 раза**
---
В асинхронных методах возможно использование более одного оператора ```await``` в рамках одного асинхронного метода:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask1 = new(PrintIterations, $"{taskName}[1]");
        printIterationsTask1.Start();
        await printIterationsTask1;

        Task printIterationsTask2 = new(PrintIterations, $"{taskName}[2]");
        printIterationsTask2.Start();
        await printIterationsTask2;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **10 Декомпилированный код множественного использования операторов await - 2 раза**
---
При преобразовании асинхронного метода с двумя операторами ```await``` компилятор создаёт переменные ```TaskAwaiter awaiter[1, 2]``` для каждого состояния конечного автомата, добавляет дополнительное условие проверки состояния конечного автомата, а также метку ```Label_State1```, к которой производится переход при втором состоянии (**1**).

Декомпилированный код множественного использования операторов ```await``` - 2 раза, скомпилированный в режиме **Debug**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
    [DebuggerStepThrough]
    private static Task PrintIterationsAsync(string taskName)
    {
        PrintIterationsAsyncStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._taskName = taskName;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);

        return stateMachine._builder.Task;
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    [CompilerGenerated]
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string _taskName;
        private Task _printIterationsTask1;
        private Task _printIterationsTask2;
        private TaskAwaiter _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;

            try
            {
                TaskAwaiter awaiter1;
                TaskAwaiter awaiter2;

                if (localState != 0)
                {
                    if (localState != 1)
                    {
                        Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                        _printIterationsTask1 = new Task(PrintIterations, $"{_taskName}[1]");
                        _printIterationsTask1.Start();
                        awaiter1 = _printIterationsTask1.GetAwaiter();

                        if (!awaiter1.IsCompleted)
                        {
                            _state = 0;
                            _awaiter = awaiter1;
                            PrintIterationsAsyncStateMachine stateMachine = this;
                            _builder.AwaitUnsafeOnCompleted(ref awaiter1, ref stateMachine);

                            return;
                        }
                    }
                    else
                    {
                        awaiter2 = _awaiter;
                        _awaiter = new TaskAwaiter();
                        _state = -1;

                        goto Label_State1;
                    }
                }
                else
                {
                    awaiter1 = _awaiter;
                    _awaiter = new TaskAwaiter();
                    _state = -1;
                }

                awaiter1.GetResult();

                _printIterationsTask2 = new Task(PrintIterations, $"{_taskName}[2]");
                _printIterationsTask2.Start();
                awaiter2 = _printIterationsTask2.GetAwaiter();

                if (!awaiter2.IsCompleted)
                {
                    _state = 1;
                    _awaiter = awaiter2;
                    PrintIterationsAsyncStateMachine stateMachine = this;
                    _builder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);

                    return;
                }

            Label_State1:
                awaiter2.GetResult();
                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _printIterationsTask1 = null;
                _printIterationsTask2 = null;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _printIterationsTask1 = null;
            _printIterationsTask2 = null;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

При компиляции в режиме **Release** структура метода ```MoveNext()``` не отличается, за исключением оптимизаций, таких же, как и при компиляции асинхронного метода с одним оператором ```await```.

## **11 Множественное использование операторов await - 3 и более раз**
---
Пример использования нескольких операторов ```await``` в конексте одного асинхронного метода: 

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask1 = new(PrintIterations, $"{taskName}[1]");
        printIterationsTask1.Start();
        await printIterationsTask1;

        Task printIterationsTask2 = new(PrintIterations, $"{taskName}[2]");
        printIterationsTask2.Start();
        await printIterationsTask2;

        Task printIterationsTask3 = new(PrintIterations, $"{taskName}[3]");
        printIterationsTask3.Start();
        await printIterationsTask3;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **12 Декомпилированный код множественного использования операторов await - 3 и более раз**
---
При преобразовании асинхронного метода с тремя и более операторами ```await``` компилятор также создаёт переменные ```TaskAwaiter awaiter[1, 2, 3, ...]``` для каждого состояния конечного автомата, но вместо дополнительного условия уже создаёт переключатель ```switch-case``` для каждого из пользовательских состояний (исключая технические состояния **-1** и **-2**), а код продолжений под метками ```Label_State[1, 2, 3, ...]```:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
    [DebuggerStepThrough]
    private static Task PrintIterationsAsync(string taskName)
    {
        PrintIterationsAsyncStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._taskName = taskName;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);

        return stateMachine._builder.Task;
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    [CompilerGenerated]
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string _taskName;
        private Task _printIterationsTask1;
        private Task _printIterationsTask2;
        private Task _printIterationsTask3;
        private TaskAwaiter _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;

            try
            {
                TaskAwaiter awaiter1;
                TaskAwaiter awaiter2;
                TaskAwaiter awaiter3;
                switch (localState)
                {
                    case 0:
                        awaiter1 = _awaiter;
                        _awaiter = new TaskAwaiter();
                        _state = -1;
                        break;

                    case 1:
                        awaiter2 = _awaiter;
                        _awaiter = new TaskAwaiter();
                        _state = -1;
                        goto Label_State1;

                    case 2:
                        awaiter3 = _awaiter;
                        _awaiter = new TaskAwaiter();
                        _state = -1;
                        goto Label_State2;

                    default:
                        Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                        _printIterationsTask1 = new Task(PrintIterations, $"{_taskName}[1]");
                        _printIterationsTask1.Start();
                        awaiter1 = _printIterationsTask1.GetAwaiter();

                        if (!awaiter1.IsCompleted)
                        {
                            _state = 0;
                            _awaiter = awaiter1;
                            PrintIterationsAsyncStateMachine stateMachine = this;
                            _builder.AwaitUnsafeOnCompleted(ref awaiter1, ref stateMachine);

                            return;
                        }

                        break;
                }

                awaiter1.GetResult();

                _printIterationsTask2 = new Task(PrintIterations, $"{_taskName}[2]");
                _printIterationsTask2.Start();
                awaiter2 = _printIterationsTask2.GetAwaiter();

                if (!awaiter2.IsCompleted)
                {
                    _state = 1;
                    _awaiter = awaiter2;
                    PrintIterationsAsyncStateMachine stateMachine = this;
                    _builder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);
                    return;
                }
            Label_State1:
                awaiter2.GetResult();
                _printIterationsTask3 = new Task(PrintIterations, $"{_taskName}[3]");
                _printIterationsTask3.Start();
                awaiter3 = _printIterationsTask2.GetAwaiter();

                if (!awaiter3.IsCompleted)
                {
                    _state = 2;
                    _awaiter = awaiter3;
                    PrintIterationsAsyncStateMachine stateMachine = this;
                    _builder.AwaitUnsafeOnCompleted(ref awaiter3, ref stateMachine);

                    return;
                }
            Label_State2:
                awaiter3.GetResult();
                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _printIterationsTask1 = null;
                _printIterationsTask2 = null;
                _printIterationsTask3 = null;
                _builder.SetException(ex);

                return;
            }
            _state = -2;
            _printIterationsTask1 = null;
            _printIterationsTask2 = null;
            _printIterationsTask3 = null;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

При компиляции в режиме **Release** структура метода ```MoveNext()``` не отличается, за исключением оптимизаций, таких же, как и при компиляции асинхронного метода с одним оператором ```await```.

Каждый блок кода, выполняющийся следом за соответствующим ему оператором ```await``` получает свой номер объекта конечного автомата в порядке возрастания, начиная с номера **0**.

В конструкции ```switch-case``` каждый блок ```case``` соответствует одному из объектов конечного автомата и имеет соответствующее значение для переключения, начиная со значения **0**.

В самом начале, когда конечный автомат имеет значение состояния **-1**, выполнится блок ```Default```, содержащий код, размещавшийся в асинхронном методе до первого оператора ```await```, после чего будет осуществлена проверка на завершённость ожидаемой задачи. 

Если ожидаемая задача уже завершена, выполнение продолжится и выйдет за конструкцию ```switch-case```. Если нет, то остальная часть операций конечного автомата будут выполняться в качестве продолжения ожидаемой задачи.

На каждую ожидаемую задачу есть своя метка и соответствующие операции, где каждая задача проверяется на завершённость, и каждый раз следующий за запуском задачи код может как выполниться синхронно относительно запустившего задачу потока, так и может быть передан в качестве продолжения запущенной задачи, в зависимости от того, была ли она выполнена к моменту проверки её завершённости.

Каждый раз при повторном запуске метода ```MoveNext()``` будет выбираться нужный блок в конструкции ```switch-case```, инициализирующий выполнение конечного авомата в следующем состоянии и переводящий его с помощью оператора ```goto```  к выполнению кода с того места, на котором оно было прекращено в последний раз, пока конечный автомат не завершит выполнения кода всех состояний и не завершится.

### **Объект ожидания TaskAwaiter/TaskAwaiter<TResult>**

Структура ```TaskAwaiter/TaskAwaiter<TResult>``` представляет объект ожидания завершения асинхронной задачи. Объект ожидания поддерживает полноценный функционал для работы оператора ```await```.

Определение ```TaskAwaiter```:
```cs
public struct TaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
{
    public bool IsCompleted { get; }
    public void GetResult();
    public void OnCompleted(Action continuation);
    public void UnsafeOnCompleted(Action continuation);
}
```

Определение ```TaskAwaiter<TResult>```:
```cs
public struct TaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
{
    public bool IsCompleted { get; }
    public TResult GetResult();
    public void OnCompleted(Action continuation);
    public void UnsafeOnCompleted(Action continuation);
}
```

Члены структур ```TaskAwaiter/TaskAwaiter<TResult>```:
- ```IsCompleted``` - возвращает **true**, если задача завершена, иначе - **false**;
- ```GetResult()``` - если у асинхронной задачи имеется возвращаемые результат, то используется для его ожидания и получения, если не имеется, то только для ожидания; 
- ```INotifyCompletion.OnCompleted()``` - устанавливает продолжение по окончанию выполнения асинхронной задачи, а также захватывает контекст выполнения;
- ```ICriticalNotifyCompletion.UnsafeOnCompleted()``` - устанавливает продолжение по окончанию выполнения асинхронной задачи и не захватывает контекст выполнения. Благодаря этому методу получается избежать повторного захвата контекста выполнения. Захват контекста выполнения - очень ресурсоёмкая операция, и избежание её повторного запуска позволяет сэкономить ресурсы. Компилятор, видя на объекте ожидания оба интерфейса, выбирает второй. Однако, этими типами и членами нужно пользоваться только при собственной реализации задач и объектов ожидания.

Класс ```ExecutionContext``` представляет контейнер для хранения информации потока выполнения, хранящий в сете другие контексты, такие, как ```SecurityContext```, ```SynchronizationContext``` и другие. С помощью него можно завхватить состояние одного потока и восстановить его в другом.

### **Метод AwaitUnsafeOnCompleted()**

Метод ```AwaitUnsafeOnCompleted()``` занимается планированием конечного автомата, чтобы перейти к следующему действию по завершению заданного объекта типа ```awaiter```.

```cs
public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
where TAwaiter : ICriticalNotifyCompletion
where TStateMachine : IAsyncStateMachine
{
    try
    {
        AsyncMethodBuilderCore.MoveNextRunner runnerToInitialize = null;
        // Создание делегата продолжения. Повторный вызов метода MoveNext.
        Action completionAction = this.m_coreState.GetCompletionAction(AsyncCausalityTracer.LogginOn ? Task : null, ref runnerToInitialize);
        // Если машина состояний еще не была упакована, то в теле блока if произойдет упаковка. Вызов метода SetStateMachine.
        if (this.m_coreState.m_stateMachine == null)
            {
            Task<TResult> task = this.Task;
            this.m_coreState.PostBoxInitialization((IAsyncStateMachine)stateMachine, runnerToInitialize, task);
        }
        // Установка продолжения, которое должно быть выполнено по завершению работы асинхронной задачи.
        awaiter.UnsafeOnCompleted(completionAction);
    }
    catch (Exception ex)
    {
        // Регистрация и проброс исключения.
        AsyncTaskMethodBuilderCore.ThrowAsync(ex, (SynchronizationContext)null);
    }
}
```

**Сигнатура метода ```AwaitUnsafeOnCompleted()```:**
- Возвращаемое значение метода: ```void```;
- Параметры места заполнения типами:
    - ```TAwaiter``` - тип объекта ожимдания завершения асинхронной задачи, имеет ограничением необходимости наследования интерфейса ```ICriticalNotifyCompletion```;
    - ```TStateMachine``` - тип объекта конечного автомата, имеет ограничением необходимости наследования интерфейса ```IAsyncStateMachine```;
- Параметры метода: 
    - ```ref TAwaiter awaiter``` - объект ожимдания завершения асинхронной задачи;
    - ```ref TStateMachine satteMachine``` - объект конечного автомата.

Всё тело метода помещено в блок ```try/catch```.

Структура ```AsyncMethodBuilderCore``` - это внутреннее ядро строителей асинхронных методов, которое удерживает их состояние. Приэтом, в этой структуре содержится методы по созданию продолжений, упаковки конечного автомата и проброса исключений.

Класс ```AsyncMethodBuilderCore.MoveNextRunner``` - вложенный класс структуры ```AsyncMethodBuilderCore```, он занимается вызовом метода ```MoveNext()``` на конечном автомате в переданном ему контексте выполнения.

Вызов метода ```this.m_coreState.GetCompletionAction()``` порождает продолжение ожидаемой в конечном автомате задачи. Именно там происходит захват контекста выполнения вызывающего потока и его передача вместе с конечным автоматов в класс ```MoveNextRunner```, метод ```Run()``` которого выполнит метод конечного автомата ```MoveNext()``` в конетексте выполнения, если он был захвачен.

В условии ```if (this.m_coreState.m_stateMachine == null)``` вроизводится проверка, упакован ли конечный автомат.

Если не упакован, и поле ```m_stateMachine``` имеет значенеие null, то с помощью вызова метода ```this.m_coreState.PostBoxInitialization()``` произойдёт упаковка конечного автомата, внутри этого метода будет вызван метод ```SetStateMachine()``` конечного автомата.

После этого на объекте ожидания асинхронной задачи ```awaiter``` будет вызван метод  ```UnsafeOnCompleted(completionAction)```, которому передаётся созданный делегат продолженияю. Этот метод установит переданный делегат ```Action``` в качестве продолжения асинхронной задачи.

\* В случае, если объект ожидания унаследовал только интерфейс ```INotifyCompletion``` без ```ICriticalNotifyCompletion```, то компилятор выбрал бы метод ```AwaitOnCompleted()``` вместо ```AwaitUnsafeOnCompleted()```, где всё практически такое же, за исключением того, что на полученном объекте ожидания для установки продолжения был бы вызван метод ```OnCompleted()``` вместо ```UnsafeOnCompleted()```. Но, стандартно, и в большиснтве случаев объекты ожидания имеют реализацию интерфейса ```ICriticalNotifyCompletion```, что даже лучше.

Методы ```AwaitUnsafeOnCompleted``` и ```AwaitOnCompleted``` реализованы внутри фреймворка разработчиками **Microsoft**, настоящий код несколько отличается от представленного в примере для упрощения объяснения сути и принципов его работы, например, за счёт удаления из кода **контрактов**.

## **13 Асинхронный метод Main()**
---
Метод ```Main()``` является точкой входа в программу. 

В ```C#``` доспускается делать метод ```Main()``` асинхронным и помечать его ключевыми словами ```async await```.

Это было сделано, чтобы упростить возможность ожидания результата работы асинхронного метода.\

Асинхронные методы ```Main()``` могут иметь 2 типа возвращаемых значений, ```Task```, если метод не возвращает код завершения программы, и ```Task<int>```, если метод возвращает код завершения прогрмамы.

Пример определения асинхронного метода ```Main()```:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        await PrintIterationsAsync("  AsyncTask");

        PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **14 Декомпилированный асинхронный метод Main() после компиляции в режиме Debug**
---
После обработки компилятором, асинхронный метод ```Main()``` трансформируется в обычный синхронный метод, так как, если метод ```Main()``` - первый выполняющийся метод в программе, то его уже некому будет ожидать. Модификатор ```async``` удаляется, возвращаемый тип ```Task``` заменяется на ```void```, а ```Task<int>``` на ```int```.

Сам метод ```Main()``` декорируется атрибутом ```[SpecialName]```, который указывает, что среда выполнения или инструменты обрабатывают тип или член особым образом.

Создаётся ещё один метод ```Main()```, являющийся асинхронным методом-заглушкой, в котором расположены стандартные создание, инициализация и запуск конечного автомата с операциями, которые содержались до компиляции в оригинальном методе ```Main()```.

Внутри главного метода ```Main()``` вызывается его асинхронная перегрузка и ожидается с блокировкой вызывающего потока через вызов методов ```GetAwaiter().GetResult()```.

Ниже приведёт пример декомпилированного кода, скомпилированного из исходного в режиме **Debug**:
```cs
internal class Program
{
    [SpecialName]
    private static void Main(string[] args)
    {
        Main_Async(args).GetAwaiter().GetResult();
    }

    [AsyncStateMachine(typeof(MainStateMachine))]
    [DebuggerStepThrough]
    private static Task Main_Async(string[] args)
    {
        MainStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._args = args;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);
        return stateMachine._builder.Task;
    }

    [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
    [DebuggerStepThrough]
    private static Task PrintIterationsAsync(string taskName)
    {
        PrintIterationsAsyncStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._taskName = taskName;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);

        return stateMachine._builder.Task;
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
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    [CompilerGenerated]
    private sealed class MainStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string[] _args;
        private TaskAwaiter _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            try
            {
                TaskAwaiter awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

                    awaiter = PrintIterationsAsync("  AsyncTask").GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        _state = 0;
                        _awaiter = awaiter;
                        MainStateMachine stateMachine = this;

                        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                        return;
                    }
                }
                else
                {
                    awaiter = _awaiter;
                    _awaiter = new TaskAwaiter();
                    _state = -1;
                }

                awaiter.GetResult();

                PrintIterations("   SyncCall");

                Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }

    [CompilerGenerated]
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string _taskName;
        private Task _printIterationsTask;
        private TaskAwaiter _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            try
            {
                TaskAwaiter awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                    _printIterationsTask = new Task(PrintIterations, _taskName);
                    _printIterationsTask.Start();

                    awaiter = _printIterationsTask.GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        _state = 0;
                        _awaiter = awaiter;
                        PrintIterationsAsyncStateMachine stateMachine = this;

                        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                        return;
                    }
                }
                else
                {
                    awaiter = _awaiter;
                    _awaiter = new TaskAwaiter();
                    _state = -1;
                }

                awaiter.GetResult();

                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _printIterationsTask = null;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _printIterationsTask = null;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```

\* Асинхронный метод ```Main()``` после компиляции также имеет имя **Main**, и отличается от основного метода ```Main()``` только перегрузкой возвращаемого значения. Но, так как в C# нельзя создавать перегрузки методов по возвращаемым значениям, и такой код не скомпилировался бы при переносе его обратно в C#, то имя асинхронного метода ```Main()``` было заменено на **Main_Async**.

### **Сводка по ключевым словам async await**
|```async```|```await```|
|---|---|
|Применяется как модификатор метода|Оператор ```await``` применяется к экземплярам типов, которые имеют метод ```GetAwaiter()```|
|Разрешает методу в своём теле использовать оператор ```await```|Освобождает вызывающий поток|
|Указывает компилятору, что нужно создать асинхронный конечный автомат|Указывает компилятору, где в конечном автомате нужно создать продолжение и сгенерировать возврат управления|
|Бесполезно без использования ключевого слова ```await``` (может создать конечный автомат без надобности|Бесполезно без использования ключевого слова ```async``` (Ошибка компиляции)|

Сами по себе ключевые слова async await не имеют никакой силы. Для их работоспособности нужны либо задачи ```Task```, Либо специальные методы, помеченные, как ```(awaitable)```.

## **15 Получение результата из асинхронного метода**
---
Асинхронные операции способны возвращать результат своей работы.

При работе с задачами есть несколько способов для извлечения результата из асинхронной задачи:

- Свойство Result, вызванное на экземпляре класса Task<TResult>;
- Метод GetResult(), вызванный на экземпляре структуры TaskAwaiter<TResult>;
- Оператор await.

Оператор ```return```, который будет содержаться в теле асинхронного метода возвращающего значеие, должен возвращать значение типа ```TResult```.

Сейчас для получения результата асинхронной задачи, необходимо по возможности использовать только оператор ```await```. Он делает это максимально эффективно и безопасно.

Если метод помечен модификатором ```async```, то значение ```TResult``` будет записано в задачу-марионетку автоматически. Поэтому, необходимо возвращать тип ```TResult``` вместо ```Task<TResult>```:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

        int asyncTaskResult = await PrintIterationsAsync("  AsyncTask");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Result of [{nameof(asyncTaskResult)}] is [{asyncTaskResult}]");

        int syncCallResult = PrintIterations("   SyncCall");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Result of [{nameof(syncCallResult)}] is [{syncCallResult}]");

        Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");

        Console.ReadKey();
    }

    private static async Task<int> PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task<int> printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        int taskResult = await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");

        return taskResult;
    }

    private static int PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        int result = iterationIndex * 1000;

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

        return result;
    }
}
```

## **15 Декомпилированный код олучения результата из асинхронного метода, скомпилированного в Debug**
---
После преобразования кода компилятором, в случае возвращения значения из асинхронного метода, компилятор применяет обощённые типы ```Task<TResult>```, ```AsyncTaskMethodBuilder<TResult>``` и ```TaskAwaiter<TResult>``` вместо их обычных версий, а также передаёт полученное из метода ```GetResult()``` ожидаемой задачи значение в качестве аргумента метода ```SetResult()``` объекта ожидания:

```cs
internal class Program
{
    [SpecialName]
    private static void Main(string[] args)
    {
        Main_Async(args).GetAwaiter().GetResult();
    }

    [AsyncStateMachine(typeof(MainStateMachine))]
    [DebuggerStepThrough]
    private static Task Main_Async(string[] args)
    {
        MainStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder.Create();
        stateMachine._args = args;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);
        return stateMachine._builder.Task;
    }

    [AsyncStateMachine(typeof(PrintIterationsAsyncStateMachine))]
    [DebuggerStepThrough]
    private static Task<int> PrintIterationsAsync(string taskName)
    {
        PrintIterationsAsyncStateMachine stateMachine = new();
        stateMachine._builder = AsyncTaskMethodBuilder<int>.Create();
        stateMachine._taskName = taskName;
        stateMachine._state = -1;
        stateMachine._builder.Start(ref stateMachine);

        return stateMachine._builder.Task;
    }

    private static int PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        int result = iterationIndex * 1000;

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");

        return result;
    }

    [CompilerGenerated]
    private sealed class MainStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder _builder;
        public string[] _args;
        private int _asyncTaskResult;
        private int _syncCallResult;
        private TaskAwaiter<int> _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            try
            {
                TaskAwaiter<int> awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"+    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(Main)}]");

                    awaiter = PrintIterationsAsync("  AsyncTask").GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        _state = 0;
                        _awaiter = awaiter;
                        MainStateMachine stateMachine = this;

                        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                        return;
                    }
                }
                else
                {
                    awaiter = _awaiter;
                    _awaiter = new TaskAwaiter<int>();
                    _state = -1;
                }

                _asyncTaskResult = awaiter.GetResult();

                Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Result of [asyncTaskResult] is [{_asyncTaskResult}]");

                _syncCallResult = PrintIterations("   SyncCall");

                Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Result of [syncCallResult] is [{_syncCallResult}]");

                Console.WriteLine($"-    {nameof(Main),-10}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(Main)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _builder.SetResult();
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }

    [CompilerGenerated]
    private sealed class PrintIterationsAsyncStateMachine : IAsyncStateMachine
    {
        public int _state;
        public AsyncTaskMethodBuilder<int> _builder;
        public string _taskName;
        private Task<int> _printIterationsTask;
        private int _taskResult;
        private TaskAwaiter<int> _awaiter;

        void IAsyncStateMachine.MoveNext()
        {
            int localState = _state;
            try
            {
                TaskAwaiter<int> awaiter;

                if (localState != 0)
                {
                    Console.WriteLine($"++ {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

                    _printIterationsTask = new Task<int>(PrintIterations, _taskName);
                    _printIterationsTask.Start();

                    awaiter = _printIterationsTask.GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        _state = 0;
                        _awaiter = awaiter;
                        PrintIterationsAsyncStateMachine stateMachine = this;

                        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);

                        return;
                    }
                }
                else
                {
                    awaiter = _awaiter;
                    _awaiter = new TaskAwaiter<int>();
                    _state = -1;
                }

                _taskResult = awaiter.GetResult();

                Console.WriteLine($"-- {_taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
            }
            catch (Exception ex)
            {
                _state = -2;
                _printIterationsTask = null;
                _builder.SetException(ex);

                return;
            }

            _state = -2;
            _printIterationsTask = null;
            _builder.SetResult(_taskResult);
        }

        [DebuggerHidden]
        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
```
