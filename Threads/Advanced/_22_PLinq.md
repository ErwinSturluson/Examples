# PLINQ

### **Параллельное программирование**
**.NET** предлагает вам несколько стандартных решений для распараллеливания кода:

- **Типы** ```System.Threading.Tasks.Task``` и ```System.Threading.Tasks.Task<TResult>```;
- **Тип** ```System.Threading.Tasks.Parallel```;
- **Параллельный LINQ (PLINQ)** в виде типов ```System.Linq.ParallelEnumerable``` и
```System.Linq.ParallelQuery<T>```.

**Parallel LINQ (PLINQ)** – это параллельная реализация **LINQ to Objects**. Он имеет полный набор стандартных операторов запроса **LINQ** в виде методов расширений, а также дополнительные операторы для параллельных операций. Операции PLINQ представляет класс ```ParallelEnumerable```.

**PLINQ** прост в использовании как обычный **LINQ**, при этом дополняясь мощью параллельного программирования. Он может значительно увеличить скорость запросов **LINQ to Objects** за счет более эффективного использования всех доступных ядер на компьютере.

**PLINQ можно использовать:**
- **В синтаксическом варианте**;
- **Расширяющими методами**;
- **Статическими методами**.

Все расширяющие методы **PLINQ** находятся в классе ```ParallelEnumerable```. Большая часть расширяющих методов вам будет знакома в виду их наличия в статическом классе ```Enumerable```, который представляет операции **LINQ to Objects**.

Чтобы вы могли использовать методы **PLINQ**, ваш источник должен быть типа ```ParallelQuery<TSource>```. Поэтому практически все расширяющие методы класса ```ParallelEnumerable``` возвращают именно этот тип.

### **Операторы PLINQ**

- ```AsParallel()``` – метод вызывается на последовательности IEnumerable<T>, чтобы превратить LINQ в PLINQ. Он возвращает ParallelQuery<T>, что позволяет использовать операторы ParallelEnumerable. Чтобы начать работать с PLINQ необходимо вызвать этот метод. Список операторов, которые присутствуют только в PLINQ:
- ```AsOrdered()``` – указывает, что PLINQ должен сохранять порядок элементов исходной последовательности для остальной части запроса.
- ```AsUnordered()``` – указывает, что PLINQ для остальной части запроса не должен сохранять исходный порядок элементов.
- ```AsSequential()``` – указывает, что вся последующая часть запроса должна вновь выполняться последовательно. Превращает ParallelQuery<T> в IEnumerable<T>.
- ```ForAll()``` – многопоточный метод, который позволяет параллельно обработать результаты запроса указанным вами делегатом, без слияния элементов. Метод ничего не возвращает.

### **Как работает PLINQ**

Работа каждого оператора разделяется на несколько частей, количество которых зависит от указания максимального уровня параллелизма. Каждая часть представляет собой **PartitionerStream**, который будет вытягивать данные из перебираемой последовательности. Все **PartitionerStream**-ы будут запущены в контексте задач (```Task```), что и дает параллельное выполнение.

Во время построения запроса каждый оператор запоминает, кто будет выполняться после него. Таким образом, после завершения параллельной работы одного оператора – работа переходит к другому, который повторяет те же самые действия.

### **Операторы настройки PLINQ**

Для удобства расширяющие методы **LINQ** можно называть **операторами**.

Список операторов для корректировки работы **PLINQ**:
- ```WithCancellation``` – позволяет передать токен отмены (```CancellationToken```), чтобы у вас была возможность отменить выполнение параллельной обработки запроса.
- ```WithDegreeOfParallelism``` – позволяет указать максимальное число процессоров, которое **PLINQ** может использовать для параллельной обработки. Но, **PLINQ** сам выбирает, сколько ему использовать процессоров для обработки. Вы можете указать только граничное значение.
- ```WithExecutionMode``` – позволяет заставить **PLINQ** выполняться параллельно в 100% случаев, в отличие от поведения по умолчанию, где **PLINQ** пытается понять, стоит ему выполнять запрос параллельно или последовательно.
- ```WithMergeOptions``` – указывает, как **PLINQ** должен объединить результаты из различных разделов обратно в одну последовательность.

### **ParallelExecutionMode**

Метод ```WithExecutionMode(ParallelExecutionMode executionMode)``` позволяет вам указать режим выполнения запроса для **PLINQ**.

Перечисление ```ParallelExecutionMode``` имеет следующие константы:

- ```Default``` (Значение по умолчанию) – **PLINQ** проверит структуру запроса и выберет распараллеливание только в том случае, если ему покажется, что это приведет к ускорению обработки;
- ```ForceParallelism``` – явное указание, чтобы **PLINQ** распараллелил запрос. Необходимо указывать этот флаг, если вы уверены, что параллельное выполнение приведет к ускорению работы.

### **ParallelMergeOptions**

Метод WithMergeOptions(MergeOptions mergeOptions) – устанавливает параметры слияния параллельных
вычислений запроса.

Перечисление ```MergeOptions``` имеет следующие константы:

- ```NotBuffered``` – требует немедленно возвращать каждый обработанный элемент из каждого потока сразу же после его создания;
- ```AutoBuffered``` – запрос собирает элементы в буфер, после чего периодически выдает все содержимое буфера потоку-потребителю. При выборе этого флага может понадобиться больше времени, чем при использовании NotBuffered, чтобы передать первый элемент в поток-потребитель;
- ```FullyBuffered``` – требует собирать все выходные данные от запроса в единый буфер, после чего возвращать его элементы. При выборе этого флага может понадобиться больше всего времени до передачи первого элемента в поток-потребитель, но при этом полные результаты могут быть получены быстрее. 

Следует замерять работу после указания флага, чтобы понять действительно ли он ускоряет вам выборку в конкретном запросе.

### **Исключения в PLINQ**

Исключение в одном из параллельных потоков обработчиков приводит к завершению выполнения запроса **PLINQ**. Все исключения из параллельных обработчиков собираются и помещаются в исключение AggregateException. Оно будет выброшено через участок кода, который инициирует выполнение запроса **PLINQ** (Например: цикл ```foreach```, ```ForAll()```, ```ToList()```, ```ToArray()```, ```ToDictionary()```, ```ToLookup()```)

Для обработки исключений **PLINQ** необходимо помещать вызов инициатора запроса в тело блока ```try``` конструкции ```try-catch```.

### **Блокирование вызывающего потока PLINQ**

Если блокирование вызывающего потока при использовании запросов **PLINQ** для вас неудобно, то вы можете использовать класс ```Task``` для решения.

Поместите вызов запроса **PLINQ** в задачу (например: через метод ```Task.Run()```) и воспользуйтесь ключевыми словами ```async await``` для неблокирующего ожидания завершения выполнения задачи.

```cs
public async Task OperationAsync<T>(IEnumerable<int> sequence)
{
    await Task.Run(() =>
    {
        var query = from n in sequence
                                .AsParallel()
                    where n % 2 == 0
                    select n;

        var list = query.ToList();
    });
}
```

## **01 Метод ```AsParallel()``` класса ```ParallelQuery<T>```**
---
Пример использования метода ```AsParallel()``` класса ```ParallelQuery<T>``` с демонстрацией разницы с последовательным запросом:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        IEnumerable<int> dataSet = CreateData();

        Stopwatch stopwatch = new();
        Console.WriteLine($"Query has started sequentially...");
        stopwatch.Start();

        var sequentialQuery =
            from number in dataSet
            where QueryPredicate(number)
            select number;

        int sequentialResultsNumber = sequentialQuery.Count();

        Console.WriteLine($"Query has finished sequentially after [{stopwatch.ElapsedMilliseconds}] milliseconds. Number of results is [{sequentialResultsNumber}].{Environment.NewLine}");
        stopwatch.Restart();
        Console.WriteLine($"Query has started parallel...");

        var parallelQuery =
            from number in dataSet
            .AsParallel()
            where QueryPredicate(number)
            select number;

        int parallelResultsNumber = parallelQuery.Count();

        Console.WriteLine($"Query has finished parallel after [{stopwatch.ElapsedMilliseconds}] milliseconds. Number of results is [{parallelResultsNumber}].");
        stopwatch.Stop();
    }

    private static bool QueryPredicate(int number)
    {
        Thread.Sleep(100);
        return number < 1;
    }

    private static IEnumerable<int> CreateData()
    {
        int[] dataSet = new int[10];

        Parallel.For(0, dataSet.Count(), (i, _) => { dataSet[i] = i; });

        for (int i = 0; i < dataSet.Length; i += 2)
        {
            dataSet[i] = dataSet[i] * -1;
        }

        return dataSet;
    }
}
```

## **02 Метод ```AsOrdered()``` класса ```ParallelQuery<int>```**
---
Пример использования метода ```AsOrdered()``` класса ```ParallelQuery<T>``` с демонстрацией разницы с неупорядоченным запросом:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        IEnumerable<int> dataSet = CreateData();

        var query =
            from number in dataSet
            .AsParallel()
            where QueryPredicate(number)
            select number;

        Console.WriteLine("Selected ordered Results:");

        foreach (int number in query)
        {
            Console.WriteLine($"Selected value: [{number}].");
        }

        var orderedQuery =
            from number in dataSet
            .AsParallel()
            .AsOrdered()
            where QueryPredicate(number)
            select number;

        Console.WriteLine(Environment.NewLine + "Selected ordered Results:");

        foreach (int number in orderedQuery)
        {
            Console.WriteLine($"Selected value: [{number}].");
        }
    }

    private static bool QueryPredicate(int number)
    {
        Thread.Sleep(100);
        return number < 1;
    }

    private static IEnumerable<int> CreateData()
    {
        int[] dataSet = new int[10];

        Parallel.For(0, dataSet.Count(), (i, _) => { dataSet[i] = i; });

        for (int i = 0; i < dataSet.Length; i += 2)
        {
            dataSet[i] = dataSet[i] * -1;
        }

        return dataSet;
    }
}
```

## **03 Метод ```AsUnordered()``` класса ```ParallelQuery<int>```**
---
Пример использования метода ```AsUnordered()``` класса ```ParallelQuery<T>``` с демонстрацией разницы с упорядоченным запросом:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        IEnumerable<int> dataSet = CreateData();

        var orderedQuery =
            from number in dataSet
            .AsParallel()
            .AsOrdered()
            where QueryPredicate1(number)
            select number;

        Console.WriteLine("Selected ordered Results:");

        foreach (int number in orderedQuery)
        {
            Console.WriteLine($"Selected value: [{number}].");
        }

        var unorderedQuery =
            from number in orderedQuery
            .AsUnordered()
            where QueryPredicate2(number)
            select number;

        Console.WriteLine(Environment.NewLine + "Selected ordered Results:");

        foreach (int number in unorderedQuery)
        {
            Console.WriteLine($"Selected value: [{number}].");
        }
    }

    private static bool QueryPredicate1(int number)
    {
        Thread.Sleep(100);
        return number < 1;
    }

    private static bool QueryPredicate2(int number)
    {
        Thread.Sleep(100);
        return number > -5;
    }

    private static IEnumerable<int> CreateData()
    {
        int[] dataSet = new int[10];

        Parallel.For(0, dataSet.Count(), (i, _) => { dataSet[i] = i; });

        for (int i = 0; i < dataSet.Length; i += 2)
        {
            dataSet[i] = dataSet[i] * -1;
        }

        return dataSet;
    }
}
```

## **04 Метод ```AsSequential()``` класса ```ParallelQuery<int>```**
---
Пример использования метода ```AsSequential()``` класса ```ParallelQuery<T>``` с демонстрацией выполнения запроса частично параллельно и частично последовательно:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        IEnumerable<int> dataSet = CreateData();

        var parallelQuery =
            from number in dataSet
            .AsParallel()
            where QueryPredicate1(number)
            select number;

        Console.WriteLine("Selected ordered Results:");

        foreach (int number in parallelQuery)
        {
            Console.WriteLine($"Selected value: [{number}].");
        }

        var sequentialQuery =
            from number in parallelQuery
            .AsSequential()
            orderby number ascending
            select number;

        Console.WriteLine(Environment.NewLine + "Selected ordered Results:");

        foreach (int number in sequentialQuery)
        {
            Console.WriteLine($"Selected value: [{number}].");
        }
    }

    private static bool QueryPredicate1(int number)
    {
        Thread.Sleep(100);
        return number < 1;
    }

    private static IEnumerable<int> CreateData()
    {
        int[] dataSet = new int[10];

        Parallel.For(0, dataSet.Count(), (i, _) => { dataSet[i] = i; });

        for (int i = 0; i < dataSet.Length; i += 2)
        {
            dataSet[i] = dataSet[i] * -1;
        }

        return dataSet;
    }
}
```

## **05 Метод ```ForAll()``` класса ```ParallelQuery<int>```**
---
Пример использования метода ```ForAll()``` класса ```ParallelQuery<T>``` с демонстрацией обработки результатов запроса:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        IEnumerable<int> dataSet = CreateData();

        var parallelQuery =
            from number in dataSet
            .AsParallel()
            where QueryPredicate1(number)
            select number;

        Console.WriteLine("Selected ordered Results:");

        parallelQuery.ForAll(x => Console.WriteLine($"Selected value: [{x}]."));
    }

    private static bool QueryPredicate1(int number)
    {
        Thread.Sleep(100);
        return number < 1;
    }

    private static IEnumerable<int> CreateData()
    {
        int[] dataSet = new int[10];

        Parallel.For(0, dataSet.Count(), (i, _) => { dataSet[i] = i; });

        for (int i = 0; i < dataSet.Length; i += 2)
        {
            dataSet[i] = dataSet[i] * -1;
        }

        return dataSet;
    }
}
```

> TODO: additional Examples of parallel query setup
