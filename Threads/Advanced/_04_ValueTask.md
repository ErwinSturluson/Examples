# **Значимая задача (Value Task)**

Значимая задача - это обёртка над обыкновенной задачей, представленной классом **Task**.

Значимые задачи представлены в **.NET** структурами **ValueTask** и **ValueTask<TResult>**.

**ValueTask** была создана для уменьшения потребления ресурсов управляемой кучи.

В ряде случаев является сомнительной оптимизацией, а иногда даже снижает производительность.

Главная задача **ValueTask** - использование для возврата из задачи значения, которое уже доступно синхронно, для предотвращения выделения памяти для экземпляра класса **Task** на куче.

Эти структуры полноценно включены в **.NET Core** и **.NET 5+**, а для использования их в **.NET Framework** существует сборка **System.Threading.Tasks.Extensions**, которая представляет собой библиотеку для расширения работы **TPL**.

### **Task vs ValueTask**

**Экземпляр класса Task** необходим для выполнения задач в контексте вторичных потоков. В стеке занимает только 4 или 8 байт (в зависимости от разрядности системы), размещается в стеке, имеет богатый набор свойств и методов для синхронного и асинхронного выполнения задач.

**Экземпляр структуры ValueTask** необходим тогда, когда использование экземпляра класса **Task** избыточно, например, когда метод будет выполняться синхронно, или когда возвращаемое из задачи значение может быть полученно синхронно, и нет смысла выделять память для большого и сложного экзмепляра класса **Task** на куче. При этом, не стоит использовать для задач тип **ValueTask** повсеместно, ведь в отличии от ссычлочного экземпляра класса **Task**, занимающего в стеке лишь переменную со ссылкой на себя, экземпляр структуры **ValueTask** занимает гораздо больше пространства, так как имеет ряд внутренних полей, которые также размещаются в стеке. **ValueTask** не может самостоятельно создавать и запускать задачи без помещённого в себя экземпляра класса **Task**, из чего можно сделать вывод о том, что в случаях, когда необходим запуск задачи, которая может быть создана и запущена только экземпляром класса **Task**, нет смысла дополнительно оборачивать его в экземпляр структуры **ValueTask**, которая попросту займёт дополнительную память и скроет большую часть функционала настоящей задачи.

В случае, когда однозначно неизвестно, какой тип задачи использовать, - ссылочный или значимый, следует использовать ссылочный.

## **01 Оптимизация. Доступный синхронно результат.**
---
В случае, если результат асинхронной операции может быть доступен синхронно, то есть, либо сразу же после вызова асинхронной операции, либо после проверки условий выполнения операции, то нет причин для создания полноценной задачи в виде экземпляра класса **Task**. Вместо этого можно создать экземпляр структуры **ValueTask**, который разместится в стеке и не будет запускать вспомогательные механизмы асинхронных операций **TPL**, и поместить в него доступное синхронно возвращаемое значение задачи, после чего вернуть этот экземпляр, не создавая полноценную задачу в виде экземпляра класса **Task**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        // Incorrect value.
        PrintIterationsAsync(0).GetAwaiter().GetResult();

        // Correct value.
        PrintIterationsAsync(10).GetAwaiter().GetResult();
    }

    private static ValueTask PrintIterationsAsync(int iterationsNumber)
    {
        if (iterationsNumber < 1)
        {
            Console.WriteLine($"Invalid value {nameof(iterationsNumber)}: [{iterationsNumber}] is less than \"1\".{Environment.NewLine}");
            return new ValueTask();
        }
        else
        {
            Console.WriteLine($"Value {nameof(iterationsNumber)}: [{iterationsNumber}] is valid. A Task instance will be created.");
            ValueTask valueTask = new (Task.Factory.StartNew(PrintIterations, iterationsNumber));

            return valueTask;
        }
    }

    private static void PrintIterations(object state)
    {
        int iterationNumber = (int)state;

        int iterationIndex = 0;

        while (iterationIndex < iterationNumber)
        {
            iterationIndex++;

            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(100);
        }
    }
}
```

Для методов, которые возвращают экземпляр класса **Task** и иструктуры **ValueTask**, существует правило именования, что к названию метода добавляется суффикс **-Async**.

Так же, для ожидания задачвместо методов **Wait()**, **WaitAll()** и **WaitAny()** можно использовать цепочку методов **GetAwaiter().GetResult()**, вызванную на экземпляре класса **Task** или структуры **ValueTask**.

В первом случае в асинхронный метод, вычисляющий количество значений на основе итераций цикла, и принимающий в качестве аргумента значение количества итераций, которые следует выполнить, передаётся значение **"0"**, которое является заведомо некорректным, так как ни одна итерация цикла не будет выполнена и не будет совершено никакой полезной работы. 

Поэтому, внутри асинхронного метода **PrintIterationsAsync** производится проверка, является ли аргумент параметра **iterationsNumber** меньше, чем 1. 

И, если он меньше, чем 1, то создаётся экземпляр структуры **ValueTask()**, который не подразумевает создания реальной задачи, и возвращается из метода. 

Таким образом, вызывающий код получает результат работы асинхронного метода сразу же, синхронно и без затрат ресурсов и времени на бессмысленное создание настоящей задачи.

В другом случае, если значение переданного аргумента параметра **iterationsNumber** не меньше 1, то выполнение асинхронной операции будет иметь смысл, так как, как внутренний алгоритм метода **PrintIterations** совершит как минимум 1 итерацию.

Для этого будет создана и запущена настоящая задача, представленная экземпляром класса **Task**, а ссылка не неё будет передана в качестве аргумента в конструктор создаваемого экземпляра структуры **ValueTask**, так возвращаемый тип метода - **ValueTask**.

## **02 Оптимизация. Возвращаемое из ValueTask значение.**

Для того, чтобы получить результат из значимой задачи, необходимо воспользоваться экземпляром структуры **ValueTask<TResult>**, параметризированной местом заполнения типа **TResult**.

Для получения результата значимой задачи и для ожидания её выполнения можно воспользоваться свойством **Result** экземпляра структуры **ValueTask<TResult>**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        // Incorrect value.
        ValueTask<int> valueTask1 = PrintIterationsAsync(0);

        Console.WriteLine($"The \"{nameof(valueTask1)}\" with input of \"{0}\" has finished with the Result of \"{valueTask1.Result}\".{Environment.NewLine}");

        // Correct value.
        ValueTask<int> valueTask2 = PrintIterationsAsync(10);

        Console.WriteLine($"The \"{nameof(valueTask2)}\" with input of \"{10}\" has finished with the Result of \"{valueTask2.Result}\".");
    }

    private static ValueTask<int> PrintIterationsAsync(int iterationsNumber)
    {
        if (iterationsNumber < 1)
        {
            Console.WriteLine($"Invalid value {nameof(iterationsNumber)}: [{iterationsNumber}] is less than \"1\".");
            return new ValueTask<int>(0);
        }
        else
        {
            Console.WriteLine($"Value {nameof(iterationsNumber)}: [{iterationsNumber}] is valid. A Task instance will be created.");
            return new ValueTask<int>(Task.Factory.StartNew(PrintIterations, iterationsNumber));
        }
    }

    private static int PrintIterations(object state)
    {
        int iterationNumber = (int)state;

        int iterationIndex = 0;

        while (iterationIndex < iterationNumber)
        {
            iterationIndex++;

            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }
}
```

## **03 Оптимизация. Приведение экземппляра типа ValueTask к Task.**
---
Типы **ValueTask** и **ValueTask<TResult>** очень ограничены по сравнению с типами **Task** и **Task<TResult>**, поэтому, в ряде случаев может потребоваться приведение значимой задачи к ссылочной.

Например, может потребоваться создать продолжение асинхронно выполняющейся задачи. 

Приведение типов **ValueTask** и **ValueTask<TResult>** к типам **Task** и **Task<TResult>** выполняется через вызов метода **AsTask()**:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        // Incorrect value.
        ValueTask<int> valueTask1 = PrintIterationsAsync(0);

        Console.WriteLine($"The \"{nameof(valueTask1)}\" with input of \"{0}\" has finished with the Result of \"{valueTask1.Result}\".{Environment.NewLine}");

        if (!valueTask1.IsCompleted)
        {
            Task continuationTask = valueTask1.AsTask()
                .ContinueWith((task, state) => Console.WriteLine($"The Task {state} with Id#{task.Id} took too long."), nameof(valueTask1));
            continuationTask.Wait();
        }

        // Correct value.
        ValueTask<int> valueTask2 = PrintIterationsAsync(10);

        if (!valueTask2.IsCompleted)
        {
            Task continuationTask = valueTask2.AsTask()
                .ContinueWith((task, state) => Console.WriteLine($"The Task {state} with Id#{task.Id} took too long."), nameof(valueTask2));
            continuationTask.Wait();
        }

        Console.WriteLine($"The \"{nameof(valueTask2)}\" with input of \"{10}\" has finished with the Result of \"{valueTask2.Result}\".");
    }

    private static ValueTask<int> PrintIterationsAsync(int iterationsNumber)
    {
        if (iterationsNumber < 1)
        {
            Console.WriteLine($"Invalid value {nameof(iterationsNumber)}: [{iterationsNumber}] is less than \"1\".");
            return new ValueTask<int>(0);
        }
        else
        {
            Console.WriteLine($"Value {nameof(iterationsNumber)}: [{iterationsNumber}] is valid. A Task instance will be created.");
            return new ValueTask<int>(Task.Factory.StartNew(PrintIterations, iterationsNumber));
        }
    }

    private static int PrintIterations(object state)
    {
        int iterationNumber = (int)state;

        int iterationIndex = 0;

        while (iterationIndex < iterationNumber)
        {
            iterationIndex++;

            Console.WriteLine($"TaskId#{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }
}
```
