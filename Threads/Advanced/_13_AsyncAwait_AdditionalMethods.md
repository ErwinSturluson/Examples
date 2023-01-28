# **Асинхронные методы для работы с async await**

## **01 Task.Delay()**
---
```Task.Delay()``` – статический метод, который создает задачу и делает ее завершенной через указанный интервал времени. Время можно указать в миллисекундах (тип int) или в промежутке (тип TimeSpan).

Используется для задержки выполнения асинхронной задачи.

Является асинхронным вариантом метода Thread.Sleep(), поддерживается оператором await через возврат задачи.

Метод Delay поддерживает отмену через CancellationToken.

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Code before [Task.Delay(2000)]");

        await Task.Delay(2000);

        Console.WriteLine("Code after [Task.Delay(2000)]");
    }
}
```

## **02 Task.WhenAll()**
---
```Task.WhenAll()``` – статический метод, который ожидает завершение всех переданных задач. Метод
возвращает задачу или задачу с массивом результатов выполнившихся задач типа TResult.

Имеет 4 перегруженных варианта:

- Task WhenAll(params Task[] tasks);
- Task WhenAll(IEnumerable<Task> tasks);
- Task<TResult[]> WhenAll(params Task<TResult>[] tasks);
- Task<TResult[]> WhenAll(IEnumerable<Task<TResult>> tasks).

Если переданный массив или коллекция не содержит задач, результирующая задача получит состояние «успешно завершена» (RanToCompletion) еще до того, как она вернется пользователю.

Состояние результирующей задачи:

- Если хоть одна из переданных задач будет завершена в состоянии Failed, то результирующая задача тоже будет провалена. Свойство Exception результирующей задачи будет содержать набор всех исключений из каждой задачи, где оно произошло.
- Если все задачи не были провалены (Failed), но хоть одна из них была отменена, то результирующая задача получит состояние Canceled.
- Если все задачи не были провалены (Failed) и не были отменены (Canceled), то результирующая задача получит состояние RanToCompletion (успешно завершена).

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        await Task.WhenAll(PrintIterationsAsync("AsyncTask1"), PrintIterationsAsync("AsyncTask2"));

        await Console.Out.WriteLineAsync("All The Async Tasks have completed.");
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($"{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");

            Thread.Sleep(100);
        }
    }
}
```

## **03 Task.WhenAny()**
---
```Task.WhenAny()``` – статический метод, который ожидает завершение одной, только первой задачи из переданных в параметрах. По завершении возвращает задачу или задачу с результатом типа TResult первой выполнившейся задачи.

Имеет 4 перегруженных варианта:
- Task WhenAny(params Task[] tasks);
- Task WhenAny(IEnumerable<Task> tasks);
- Task<Task<TResult>> WhenAny(params Task<TResult>[] tasks);
- Task<Task<TResult>> WhenAny(IEnumerable<Task<TResult>> tasks).

Состояние результирующей задачи:
- Результирующая задача будет возвращена после первого завершения любой из переданных задач.
- Результирующая задача всегда будет завершена в состоянии RanToCompletion с установленным результатом, который был получен от первой завершившейся задачи.
- Такое поведение будет даже если первая завершенная задача завершилась с ошибкой (Failed) или отменой (Cancelled).

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        Task<IEnumerable<string>> iterationsCompletedTask = await Task.WhenAny(PrintIterationsAsync("AsyncTask1"), PrintIterationsAsync("AsyncTask2"));

        IEnumerable<string> iterations = iterationsCompletedTask.Result;

        Console.WriteLine($"Tasks with Id#{iterationsCompletedTask.Id} has completed first.");

        foreach (var iteration in iterations)
        {
            Console.WriteLine(iteration);
        }
    }

    private static async Task<IEnumerable<string>> PrintIterationsAsync(string taskName)
    {
        Task<IEnumerable<string>> printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        return await printIterationsTask;
    }

    private static IEnumerable<string> PrintIterations(object state)
    {
        string callName = state.ToString();

        List<string> iterationList = new();

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            iterationList.Add($"{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");

            Thread.Sleep(100);
        }

        return iterationList;
    }
}
```
