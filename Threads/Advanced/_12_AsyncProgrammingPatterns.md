# **Асинхронные шаблоны программирования**

В платформе **.NET** существует 3 асинхронных шаблона программирования:

1. APM – Asynchronous Programming Model (**устарел**, не рекомендуется к применению);
2. EAP – Event-based Asynchronous Pattern (**устарел**, не рекомендуется к применению);
3. TAP – Task-based Asynchronous Pattern (используется, рекомендуется к применению).

## **01 Шаблон APM**
---
**Asynchronous Programming Model** – шаблон асинхронного программирования, основанный на интерфейсе
```IAsyncResult``` и методах ```BeginXXX()``` и ```EndXXX()```.

Для запуска асинхронной операции используется метод BeginXXX. После его вызова приложение может продолжить выполнение инструкций в вызывающем потоке, в это же время асинхронная операция выполняется в другом потоке.

Для завершения асинхронной операции используется метод EndXXX. Он отдает результаты операций. Шаблон APM поддерживает методы обратного вызова для обработки результатов, не возвращаясь в основной поток. Поэтому, каждый BeginXXX метод всегда принимает два дополнительных параметра:

- Делегат, который представляет метод обратного вызова;
- Состояние для делегата.

Все делегаты поддерживают асинхронный шаблон **APM**. Это доступно с помощью методов ```BeginInvoke()``` и
```EndInvoke()```.

Пример выполнения асинхронного кода с использованием асинхронного шаблона APM:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Func<string, IEnumerable<string>> asyncOperation_Apm = PrintIterations;

        IAsyncResult asyncResult = asyncOperation_Apm.BeginInvoke("AsyncOperation_APM", null, null);

        IEnumerable<string> iterations = asyncOperation_Apm.EndInvoke(asyncResult);

        foreach (string iteration in iterations)
        {
            Console.WriteLine(iteration);
        }

        Console.ReadKey();
    }

    private static IEnumerable<string> PrintIterations(string callName)
    {
        List<string> iterationList = new List<string>();

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            iterationList.Add($"{callName,-12} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");

            Thread.Sleep(100);
        }

        return iterationList;
    }
}
```

## **02 Шаблон EAP**
---
**Event-based Asynchronous Pattern** – шаблон асинхронного программирования, основанный на событиях.

Для обеспечения шаблона **EAP** тип должен иметь метод ```XXXAsync()``` и соответствующее событие
```XXXCompleted```.

Потребитель должен создать обработчик события и подписать его на событие ```XXXCompleted```. После, потребитель вызывает асинхронный метод ```XXXAsync()```. По завершении его работы он вызовет событие ```XXXCompleted```, которое вызовет все обработчики события, подписанные на него.

Для передачи асинхронных результатов, исключений, состояния, используются классы, производные от ```EventArgs```. При завершении асинхронной операции в обработчик события будет отдан этот экземпляр в качестве параметра.

Необязательно: могут поддерживать отмену, отчет о прогрессе или дополнительные результаты для каждого асинхронного метода.

Пример выполнения асинхронного кода с использованием асинхронного шаблона EAP:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        EapExecuter<IEnumerable<string>> eapExecuter = new();

        eapExecuter.OnExecutionCompleted += EapExecuter_OnExecutionCompleted;

        eapExecuter.ExecuteAsync(() => PrintIterations("AsyncOperation_EAP"));

        Console.ReadKey();
    }

    private static void EapExecuter_OnExecutionCompleted(IEnumerable<string> iterations)
    {
        foreach (string iteration in iterations)
        {
            Console.WriteLine(iteration);
        }
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

internal class EapExecuter<TResult>
{
    public event Action<TResult> OnExecutionCompleted;

    public void ExecuteAsync(Func<TResult> operation)
    {
        ThreadPool.QueueUserWorkItem(new WaitCallback(ExecutionWaitCallback), operation);
    }

    private void ExecutionWaitCallback(object state)
    {
        Func<TResult> operation = (Func<TResult>)state;

        TResult result = operation.Invoke();

        OnExecutionCompleted?.Invoke(result);
    }
}
```

## **03 Шаблон TAP**
---
**Task-based Asynchronous Pattern** – шаблон асинхронного программирования, основанный на задачах.

Он основан на типах из библиотеки **TPL (Task Parallel Library)** ```Task``` и ```Task<TResult>```, и ключевых словах ```async await```.

Для работы с шаблоном TAP создают асинхронные методы, которые возвращают задачу. Асинхронные методы имеют суффикс **Async** или **TaskAsync** в названии. Могут использовать ключевые слова ```async await``` для повышения абстракции и упрощения асинхронного программирования.

Преимущества использования шаблона **TAP**:

- Простая инициализация и завершение асинхронной операции;
- Удобный способ получения возвращаемого значения асинхронной операции;
- Получение исключения, возникшего в асинхронной операции для его обработки;
- Просмотр состояния асинхронной операции;
- Поддержка отмены выполнения (Необязательно);
- Продолжения задач (Task Continuations/async await);
- Планирование выполнения асинхронной операции;
- Поддержка прогресса операции (Необязательно).

Пример выполнения асинхронного кода с использованием асинхронного шаблона TAP:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        await PrintIterationsAsync("AsyncTask_TAP");

        Console.ReadKey();
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        IEnumerable<string> iterations = await Task.Run(() => PrintIterations(taskName));

        foreach (string iteration in iterations)
        {
            Console.WriteLine(iteration);
        }
    }

    private static IEnumerable<string> PrintIterations(string callName)
    {
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

### **Задача – Task (Promise)**

Задача (Task/Task<TResult>) – единица параллельной обработки, написанная по шаблону **"Promise"**.

Она используется одновременно и как высокоуровневая обертка над работой с потоками, и как обертка над работой с асинхронными операциями ввода-вывода.

Это возможно благодаря продолжениям (Continuations) или ключевым словам ```async await```.

### **Превращение APM и EAP в TAP**

Если у вас будут проблемы с использованием старых API, которые работают на шаблоне APM или EAP, вы можете с помощью специального типа переписать их в TAP.

Для этого используют класс ```TaskCompletionSource```. Он позволяет создавать задачи-марионетки.

Асинхронный шаблон **APM** имеет настолько много **API**, что под него были созданы готовые методы для быстрого преобразования в асинхронный шаблон **TAP**. Это методы FromAsync из фабрики задач.

## **04 TaskCompletionSource<TResult> APM -> TAP**
---
Пример преобразования асинхронного кода с использованием асинхронного шаблона APM в асинхронный код с использованием асинхронного шаблона TAP двумя способами, через экземпляр класса ```TaskCompletionSource``` **(TCS)** и через метод ```TaskFactory.FromAsync()```:

```cs
namespace AsyncProgrammingPatterns._04_TaskCompletionSource_APM_TAP
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IEnumerable<string> iterationsTcs = await PrintIterationsAsync_APM_TAP_TCS("AsyncTask_APM_TAP_TCS");

            foreach (string iteration in iterationsTcs)
            {
                Console.WriteLine(iteration);
            }

            await Console.Out.WriteLineAsync();

            IEnumerable<string> iterationsTf = await PrintIterationsAsync_APM_TAP_TF("AsyncTask_APM_TAP_TF");

            foreach (string iteration in iterationsTf)
            {
                Console.WriteLine(iteration);
            }

            Console.ReadKey();
        }

        private static Task<IEnumerable<string>> PrintIterationsAsync_APM_TAP_TCS(string taskName)
        {
            TaskCompletionSource<IEnumerable<string>> tcs = new TaskCompletionSource<IEnumerable<string>>();

            Action<string> asyncOperation_Apm = (callName) =>
            {
                try
                {
                    IEnumerable<string> iterations = PrintIterations(callName);

                    tcs.TrySetResult(iterations);
                }
                catch (OperationCanceledException ex)
                {
                    tcs.TrySetCanceled(ex.CancellationToken);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };

            asyncOperation_Apm.BeginInvoke(taskName, null, null);

            return tcs.Task;
        }

        private static Task<IEnumerable<string>> PrintIterationsAsync_APM_TAP_TF(string taskName)
        {
            TaskFactory taskFactory = new TaskFactory();

            Func<string, IEnumerable<string>> asyncOperation_Apm = PrintIterations;

            return taskFactory.FromAsync(asyncOperation_Apm.BeginInvoke(taskName, null, null), (asyncResult) =>
            {
                IEnumerable<string> iterations = asyncOperation_Apm.EndInvoke(asyncResult);

                return iterations;
            });
        }

        private static IEnumerable<string> PrintIterations(string callName)
        {
            List<string> iterationList = new List<string>();

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
}
```

## **05 TaskCompletionSource<TResult> EAP -> TAP**
---
Пример преобразования асинхронного кода с использованием асинхронного шаблона EAP в асинхронный код с использованием асинхронного шаблона TAP:

```cs
namespace AsyncProgrammingPatterns._05_TaskCompletionSource_EAP_TAP
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IEnumerable<string> iterations = await PrintIterationsAsync_EAP_TAP("AsyncTask_EAP_TAP");

            foreach (string iteration in iterations)
            {
                Console.WriteLine(iteration);
            }

            Console.ReadKey();
        }

        private static Task<IEnumerable<string>> PrintIterationsAsync_EAP_TAP(string taskName)
        {
            TaskCompletionSource<IEnumerable<string>> tcs = new();

            EapExecuter<IEnumerable<string>> eapExecuter = new();

            eapExecuter.OnExecutionCompleted += (iterations) =>
            {
                try
                {
                    tcs.TrySetResult(iterations);
                }
                catch (OperationCanceledException ex)
                {
                    tcs.TrySetCanceled(ex.CancellationToken);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };

            eapExecuter.ExecuteAsync(() => PrintIterations(taskName));

            return tcs.Task;
        }

        private static void EapExecuter_OnExecutionCompleted(IEnumerable<string> iterations)
        {
            foreach (string iteration in iterations)
            {
                Console.WriteLine(iteration);
            }
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

    internal class EapExecuter<TResult>
    {
        public event Action<TResult> OnExecutionCompleted;

        public void ExecuteAsync(Func<TResult> operation)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ExecutionWaitCallback), operation);
        }

        private void ExecutionWaitCallback(object state)
        {
            Func<TResult> operation = (Func<TResult>)state;

            TResult result = operation.Invoke();

            OnExecutionCompleted?.Invoke(result);
        }
    }
}
```
