# **Дочерние и вложенные задачи (Child and Nested Tasks)**

**Дочерние задачи (Child Tasks)** - задачи, которые создаются в контексте других задач и прикрепляются к ним. Родительская задача **зависит** от дочерних задач и для своего выполнения **ожидает** выполнения всех дочерних задач.

**Вложенные задачи (Nested Tasks)** - задачи, которые создаются в контексте других задач, но не прикрепляются к ним. одительская задача **не зависит** от вложенных задач и для своего выполнения **не ожидает** выполнения всех дочерних задач.

Задача, в контексте которой создаются дочерние и вложенные задачи, по отношению к ним называется родительской.

Родительская задача может иметь любое количество дочерних и вложенных задач, количество которых ограничивается только системными ресурсами.

Нескоько дочерних задач могут иметь общего родителя. Пока дочерние задачи полностью не выполняться, родитель не вернёт результат.

Для присоединения к родительской задаче дочерней, необходимо передать флаг перечисления **TaskCreationOptions.AttachedToParent** в конструктор вложенной задачи, представленной экземпляром класса **Task**.

Для запрета присоединения дочерней задачи к родительской, необходимо передать флаг перечисления **TaskCreationOptions.DenyChildAttach** в конструктор родительской задачи, представленной экземпляром класса **Task**.

В большинстве случаев рекомендуется использовать вложенные задачи, так как, в таком случае будет меньше зависимостей между задачами.

При простом создании одной задачи в теле другой, она по умолчанию считается вложенной, и она становится дочерней только при передаче флага перечисления **TaskCreationOptions.AttachedToParent** в конструктор задачи, представленной экземпляром класса **Task**.

### **Разница дочерних и вложенных задач:**

|Поведение|Дочерние задачи|Вложенные задачи|
|---|---|---|
|Основная задача ожидает завершения внутренней задачи задачи|Да|Нет|
|Состояние основной задачи зависит от состояния внутренней задачи|Да|Нет|
|Основная задача передаёт исключения внутренней задачи|Да|Нет|

Применение вложенной задачи полностью эквивалентно обычной задаче.

Применение дочерней задачи применяются, например, для разбития родительской задачи на подзадачи, выполняющиеся параллельно, где родительская задача сможет корректно выполнить ожидание дочерних задач, собрать результаты выполнения и исключения всех дочерних задач. 

## **01 Дочерние задачи (Child Tasks)**
---
Для того, чтобы создать дочернюю задачу, необходимо создать её внутри другой задачи, и при создании через экземпляр класса **Task** передать в конструктор аргумент **TaskCreationOptions.AttachedToParent**.

Тогда родительская задача для своего завершения будет ожидать завершения дочерних задач, даже если выполнила все свои собственные операции:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task parentTask = new(PrintAllIterations);
        parentTask.Start();

        Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
        parentTask.Wait();
        Console.WriteLine($"- Parent Task and Child Tasks have finished.");
    }

    private static void PrintAllIterations()
    {
        Console.WriteLine($"+ ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task.Factory.StartNew(PrintIterations, "ChildTask1", TaskCreationOptions.AttachedToParent);
        Task.Factory.StartNew(PrintIterations, "ChildTask2", TaskCreationOptions.AttachedToParent);

        Thread.Sleep(100);

        Console.WriteLine($"> ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }
}
```

## **02 Вложенные задачи (Nested Tasks)**
---
Для того, чтобы создать вложенную задачу, необходимо просто создать её внутри другой задачи.

Тогда основная задача для своего завершения не будет ожидать завершения дочерних задач, а завершится тогда, когда выполнит все свои собственные операции. При этом, вложенные задачи продолжат свою работу, пока работает приложение, даже если основная задача уже завершилась:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task mainTask = new(PrintAllIterations);
        mainTask.Start();

        Console.WriteLine($"Main Thread is waiting for MainTask finishes.");
        mainTask.Wait();
        Console.WriteLine($"- MainTask Task has finished.");

        Console.ReadKey();
    }

    private static void PrintAllIterations()
    {
        Console.WriteLine($"+ MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task.Factory.StartNew(PrintIterations, "NestedTask1");
        Task.Factory.StartNew(PrintIterations, "NestedTask2");

        Thread.Sleep(100);

        Console.WriteLine($"> MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }
}
```

## **03 Запрет создания дочерних задач (DenyChildAttach)**
---
Для того, чтобы запретить добавлять к основной задаче дочерние задачи, необходимо при её создании через экземпляр класса **Task** передать в конструктор аргумент **TaskCreationOptions.DenyChildAttach**.

В таком случае, все задачи, созданные внутри основной задачи, останутся просто вложенными задачами, а основная задача не будет ожидать завершения дочерних задач, а завершится тогда, когда выполнит все свои собственные операции. При этом, вложенные задачи продолжат свою работу, пока работает приложение, даже если основная задача уже завершилась:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task nonParentTask = new(PrintAllIterations, TaskCreationOptions.DenyChildAttach);
        nonParentTask.Start();

        Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
        nonParentTask.Wait();
        Console.WriteLine($"- Parent Task has finished.");

        Console.ReadKey();
    }

    private static void PrintAllIterations()
    {
        Console.WriteLine($"+ NonParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task.Factory.StartNew(PrintIterations, "NonChildTask1", TaskCreationOptions.AttachedToParent);
        Task.Factory.StartNew(PrintIterations, "NonChildTask2", TaskCreationOptions.AttachedToParent);

        Thread.Sleep(100);

        Console.WriteLine($"> NonParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }
}
```

## **04 Вложенные задачи, ожидаемые основной задачей**
---
Вложенные задачи, ожидаемые основной задачей через вызов метода ожидания, не являются дочерними задачами, а просто вложенными задачами, которые ожидает основная задача.

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task mainTask = new(PrintAllIterations);
        mainTask.Start();

        Console.WriteLine($"Main Thread is waiting for MainTask finishes.");
        mainTask.Wait();
        Console.WriteLine($"- MainTask Task has finished.");

        Console.ReadKey();
    }

    private static void PrintAllIterations()
    {
        Console.WriteLine($"+ MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task nestedTask1 = Task.Factory.StartNew(PrintIterations, "NestedTask1");
        Task nestedTask2 = Task.Factory.StartNew(PrintIterations, "NestedTask2");

        Console.WriteLine($"> MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} is waiting for nested tasks completion in Thread#{Environment.CurrentManagedThreadId}.");

        Task.WaitAll(nestedTask1, nestedTask2);

        Console.WriteLine($"> MainTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }
}
```

## **05 Продолжения дочерних задач**
---
Продолжения дочерних задач, выполняются после завершения всех дочерних задач в родительской задаче, и, как следствие, после завершения родительской задачи:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task parentTask = new(PrintAllIterations);
        parentTask.Start();

        Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
        parentTask.Wait();
        Console.WriteLine($"- Parent Task and Child Tasks have finished.");

        Console.ReadKey();
    }

    private static void PrintAllIterations()
    {
        Console.WriteLine($"+ ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task<int> childTask1 = Task.Factory.StartNew(PrintIterations, "ChildTask1", TaskCreationOptions.AttachedToParent);
        Task<int> childTask2 = Task.Factory.StartNew(PrintIterations, "ChildTask2", TaskCreationOptions.AttachedToParent);

        childTask1.ContinueWith(PrintIterationsContinuation);
        childTask2.ContinueWith(PrintIterationsContinuation);

        Thread.Sleep(100);

        Console.WriteLine($"> ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }

    private static void PrintIterationsContinuation(Task task)
    {
        Console.WriteLine($"++ Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task<int> castedTask = (Task<int>)task;
        Thread.Sleep(200);

        Console.WriteLine($">> The Result of task with Id#{task.Id} is [{castedTask.Result}].");

        Console.WriteLine($"-- Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");
    }
}
```

## **06 Продолжения дочерних задач, созданные, как дочерние задачи**
---
Для того, чтобы сделать дочерними сами продолжения вложенных задач, необходимо в метод **ContinueWith()** класса **Task** передать значение **TaskContinuationOptions.AttachedToParent** в параметр **TaskContinuationOptions continuationOptions**. В таком случае, дочерними задачами будут являться продолжения вложенных задач, и родительская задача для своего завершения будет дожидаться уже продолжений вложенных задач, а не самих вложенных задач: 

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task parentTask = new(PrintAllIterations);
        parentTask.Start();

        Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
        parentTask.Wait();
        Console.WriteLine($"- Parent Task and Child Tasks have finished.");

        Console.ReadKey();
    }

    private static void PrintAllIterations()
    {
        Console.WriteLine($"+ ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task<int> childTask1 = Task.Factory.StartNew(PrintIterations, "ChildTask1");
        Task<int> childTask2 = Task.Factory.StartNew(PrintIterations, "ChildTask2");

        childTask1.ContinueWith(PrintIterationsContinuation, TaskContinuationOptions.AttachedToParent);
        childTask2.ContinueWith(PrintIterationsContinuation, TaskContinuationOptions.AttachedToParent);

        Thread.Sleep(100);

        Console.WriteLine($"> ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }

    private static int PrintIterationsContinuation(Task<int> task)
    {
        Console.WriteLine($"++ Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Thread.Sleep(200);

        Console.WriteLine($">> The Result of task with Id#{task.Id} is [{task.Result}].");

        Console.WriteLine($"-- Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int newResult = task.Result * 10;

        return newResult;
    }
}
```

## **07 Продолжения дочерних задач, созданные, как дочерние задачи**
---
В случае, если дочерними являются как внутренние задачи, так и их продолжения, выполняющиеся после завершения самих дочерних задач, то логика работы будет такая же, как и в случае, когда дочерними задачами являются только продолжения вложенных задач, ведь родительская задача в любом случае завершится только после завершения всех дочерних задач:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task parentTask = new(PrintAllIterations);
        parentTask.Start();

        Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
        parentTask.Wait();
        Console.WriteLine($"- Parent Task and Child Tasks have finished.");

        Console.ReadKey();
    }

    private static void PrintAllIterations()
    {
        Console.WriteLine($"+ ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Task<int> childTask1 = Task.Factory.StartNew(PrintIterations, "ChildTask1", TaskCreationOptions.AttachedToParent);
        Task<int> childTask2 = Task.Factory.StartNew(PrintIterations, "ChildTask2", TaskCreationOptions.AttachedToParent);

        childTask1.ContinueWith(PrintIterationsContinuation, TaskContinuationOptions.AttachedToParent);
        childTask2.ContinueWith(PrintIterationsContinuation, TaskContinuationOptions.AttachedToParent);

        Thread.Sleep(100);

        Console.WriteLine($"> ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }

    private static int PrintIterationsContinuation(Task<int> task)
    {
        Console.WriteLine($"++ Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");

        Thread.Sleep(200);

        Console.WriteLine($">> The Result of task with Id#{task.Id} is [{task.Result}].");

        Console.WriteLine($"-- Continuation Task of Task with Id#{task.Id} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int newResult = task.Result * 10;

        return newResult;
    }
}
```

## **08 Метод Run и дочерние задачи**
---
Для корректной работы дочерних задач лучше пользоваться конструктором класса **Task** для создания родительской и дочерней задач вместо статического метода **Run()** класса **Task**, так как при создании и запуске задачи через этот метод, логика связывания родительской задачи с дочерними может быть нарушена. Это происходит из-за того, что во время создания задачи, статический метод **Run()** класса **Task** по умолчанию устанавливает ей флаг **TaskCreationOptions.DenyChildAttach**, что блокирует присоединение дочерних задач к родительской:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Task parentTask = Task.Run(() =>
        {
            Console.WriteLine($"+ ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

            Task.Factory.StartNew(PrintIterations, "ChildTask1", TaskCreationOptions.AttachedToParent);
            Task.Factory.StartNew(PrintIterations, "ChildTask2", TaskCreationOptions.AttachedToParent);

            Thread.Sleep(100);

            Console.WriteLine($"> ParentTask with Id#{Task.CurrentId?.ToString() ?? "null"} has finished all the own operations in Thread#{Environment.CurrentManagedThreadId}.");
        });

        Console.WriteLine($"Main Thread is waiting for Parent Task and Child Tasks finish.");
        parentTask.Wait();
        Console.WriteLine($"- Parent Task and Child Tasks have finished - might be incorrect statement.");
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+ {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has started in Thread#{Environment.CurrentManagedThreadId}.");

        int iterationIndex = 0;

        while (iterationIndex < 5)
        {
            iterationIndex++;

            Console.WriteLine($"> {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} - Thread#{Environment.CurrentManagedThreadId} - [{iterationIndex}].");
            Thread.Sleep(100);
        }

        int calculationResult = iterationIndex * 1000;

        Console.WriteLine($"- {taskName} with Id#{Task.CurrentId?.ToString() ?? "null"} has finished in Thread#{Environment.CurrentManagedThreadId}.");

        return calculationResult;
    }
}
```
