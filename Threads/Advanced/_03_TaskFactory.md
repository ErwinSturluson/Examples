# **Фабрика задач (TaskFactory)**

**Фабрика задач** - механизм, позволяющий настроить набор сгруппированных задач, которые находятся в ожном состоянии.

Классы для работы с фабрикой задач в **.NET** - **TaskFactory** и **TaskFactory<TResult>**.

До появляения метода **Run()** класса **Task** фабрика задач была самым популярным способом создания задач с помощью её метода **StartNew()**.

В некотороых случаях требуется получать наборы экземпляров класса **Task**, находящихся в одном состоянии. Чтобы постоянно не передавать одни и те же параметры в конструктор каждого экземпляра класса **Task**, можно создать фабрику задач с нужными настройками для выпускаемых ей задач.

## **01 Настройка фабрики задач задач**
---
В фабрике задач возможно настраивать такие опции создания задач, как, например, **TaskCreationOptions** и **TaskContinuationOptions**.

Для того, чтобы получить фабрику задач по умолчанию, можно воспользоваться статическим свойством **factory** класса **Task**.

Для создания собственного экземпляра фабрики задач класс **TaskFactory** предоставляет открытый конструктор.

> TODO: рассмотреть члены класса **TaskFactory** и их перегрузки.

Также фабрика задач позволяет удобно настраивать **групповые продолжения задач** и обрабатывать результаты нескольких задач как результаты одной задачи:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        TaskFactory taskFactory = new();

        Task<int> task1 = taskFactory.StartNew(PrintIterations, "AsyncTask1");
        Task<int> task2 = taskFactory.StartNew(PrintIterations, "AsyncTask2");

        Task continuationTask = taskFactory.ContinueWhenAll(new Task<int>[] { task1, task2 }, completedTasks =>
        {
            int completedTasksResutSum = completedTasks.Select(t => t.Result).Sum();

            Console.WriteLine($"All the Tasks completed with sum of results of [{completedTasksResutSum}].");
        });

        continuationTask.Wait();
    }

    private static int PrintIterations(object state)
    {
        string taskName = state.ToString();

        int iterationNumber = 0;

        while (iterationNumber < 5)
        {
            iterationNumber++;

            Console.WriteLine($"{taskName} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
            Thread.Sleep(100);
        }

        int calculationResult = iterationNumber * 1000;

        return calculationResult;
    }
}
```

> TODO: Рассмотреть взаимодействие фабрики задач с другим асинхронным шаблоном - **APM (Asynchronous Programming Model)** с помощью её метода **FromAsync()**.