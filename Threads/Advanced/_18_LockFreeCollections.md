# Потокобезопасные (Lock-Free) коллекции

---
1) Потокобезопасные коллекции
2) Разновидности потокобезопасных коллекций
3) ConcurrentQueue<T>
4) ConcurrentStack<T>
5) ConcurrentBag<T>
6) Шаблон Producer-Consumer. IProducerConsumerCollection<T>
7) Класс BlockingCollection<T>
8) Параллельная обработка. Класс Parallel
9) Параллельные циклы (For, ForEach)
---

**Коллекция** – это объект, который содержит набор сгруппированных данных с поддержкой и перебора, изменения, добавления и удаления. Одним словом – удобный интерфейс для взаимодействия с набором данных.

**Потокобезопасная коллекция** – та же коллекция, но работа с ее элементами из нескольких потоков безопасна, в отличие от обычной коллекции. То есть разные потоки могут читать и писать в коллекцию без опасности потери или поврежденияданных.

При обращении к коллекции из разных потоков могут возникнуть проблемы, когда один поток изменяет коллекцию, когда другой поток пытается её прочитать, тем самым можно получить либо логические ошибки в виде испорченных данных, либо технические ошибки уровня выполнения, которые полностью сломают работу приложения.

Потокобезопасные коллекции не всегда используют монопольное блокировние потоков, а позволяют работать с собой нескольким потокам параллельно в большинстве случаев, что способствует снижению нагрузки на поток.

Монопольное блокирование - блокирование, при котором к определённому участку кода имеет доступ только один поток, а остальные ожидают снятия блокировки.

### **Техники синхронизации потокобезопасных коллекций**

Потокобезопасные коллекции используют несколько техник синхронизации доступа, таких, как:

- Конструкции синхронизации (```lock```, ```Monitor```, ```SpinWait```, ```WaitHandle```);
- Атомарные инструкции (```Interlocked```, ```volatile```);
- Неблокирующие алгоритмы.

```cs
lock(syncRoot)
{
    list.Remove(item);
}
```

Использование нескольких техник синхронизации доступа в потокобезопасных коллекциях повышет их эффективность.

В случае работы несколкьих потоков с коллекцией, потокобезопасная коллекция - это идеальное решение в плане безопасности, удобаства использования и производительности.

В .NET имеется набор готовых потокобезопасных коллекций, таких, как:

- ```CuncurrentDictionary<TKey, TValue>``` - коллекция, представляющая обычный словарь, аналог класса ```Dictionary<TKey, TValue>```, с которым безопасно работать из нескольких потоков одновременно;
- ```CuncurrentStack<T>``` -  коллекция, представляющая обычный стек, работающая по принципу **LIFO**, аналог класса ```Stack<Е>```, с которым безопасно работать из нескольких потоков одновременно;
- ```CuncurrentQueue<T>``` -  коллекция, представляющая обычную очередь, работающая по принципу **FIFO**, аналог класса ```Queue<T>```, с которым безопасно работать из нескольких потоков одновременно;
- ```CuncurrentBag<T>``` - коллекция, котоаря хранит свои элементы в хаотичном порядке и с которой безопасно работать из нескольких потоков одновременно, не имеет обычных аналогов.

Обычные коллекции в .NET, не имеющие потокобезопасного аналога:
- ```T[]``` - массив;
- ```List<T>``` - список;
- ```LinkedList<T>``` - связанный список;
- ```HashSet<T>``` - хэш-набор;
- ```SortedSet<T>``` - сортированный набор;
- ```ObservableCollection<T>``` - обозреваемая коллекция.

## **01 Потокобезопасная очередь ```ConcurrentQueue<T>```**
---
```ConcurrentQueue<T>``` - потокобезопасная коллекция, работающая по
принципу **FIFO (First In First Out)**.

Коллекцию, работающую по принципу **FIFO** называют - очередью.

Принципы работы очереди:
1. При добавлении элемента, он будет добавлен в конец очереди;
2. При извлечении элемента, вы будете извлекать их из начала очереди, при этом удаляя его из очереди.

Открытые API для работы с элементами:
- ```void Enqueue(T item)``` – добавляет элемент в конец очереди;
- ```bool TryDequeue(out T item)``` – извлекает элемент из начала очереди, удаляя его;
- ```bool TryPeek (out T item)``` – извлекает элемент из начала очереди для просмотра.

Методы, подобные ```bool TryXXX``` возвращают **true** – если метод выполнился успешно, **false** – если у метода не получилось выполниться. Элемент они возвращают в ```out``` параметре.

Пример работы с классом ```ConcurrentQueue<T>``` из нескольких потоков одновременно:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        ConcurrentQueue<int> queue = new();

        Task fillingTask1 = Task.Run(() => FillQueue(queue, 0, 5));
        Task fillingTask2 = Task.Run(() => FillQueue(queue, 5, 5));

        await Task.WhenAll(fillingTask1, fillingTask2);
        await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

        Task cleanupTask = Task.Run(() => CleanupQueue(queue));
        await Task.Delay(500);
        Task readingTask = Task.Run(() => ReadQueue(queue));

        await Task.WhenAll(cleanupTask, readingTask);
        await Console.Out.WriteLineAsync("All the Reading and Clearing Tasks were completed successfully.");
    }

    private static void FillQueue(ConcurrentQueue<int> queue, int startIndex, int elementsNumber)
    {
        for (int i = startIndex; i < startIndex + elementsNumber; i++)
        {
            queue.Enqueue(i);
            Console.WriteLine($"+Element [{i}] was added to [{nameof(ConcurrentQueue<int>)}].");
            Thread.Sleep(100);
        }
    }

    private static void ReadQueue(ConcurrentQueue<int> queue)
    {
        foreach (var item in queue)
        {
            Console.WriteLine($"+Element [{item}] was read from [{nameof(ConcurrentQueue<int>)}].");
            Thread.Sleep(200);
        }
    }

    private static void CleanupQueue(ConcurrentQueue<int> queue)
    {
        while (queue.TryDequeue(out int result))
        {
            Console.WriteLine($"+Element [{result}] was removed from [{nameof(ConcurrentQueue<int>)}].");
            Thread.Sleep(100);
        }
    }
}
```

Как видно из примера, операции записи, чтения и изменения коллекции, производимые над ней из разных потоков одновременно, не привели к возникновению ошибок.

При этом, можно было заметить интересную особенность одновременного чтения коллекции через ```foreach``` и удаления из неё элементов - те элементы, которые было удалены до запуска метода чтения и цикла перебора коллекции ```foreach```, как и ожидалось, не были прочитаны, однако, несмотря на то, что все элементы были удалены раньше, чем метод чтения завершил свою работу и цикл ```foreach``` закончил перебор коллекции, цикл всё равно получил доступ ко всем элементам, которые находились в коллекции тот момент, когда он начал её перебор.

## **02 Потокобезопасный стек ```ConcurrentStack<T>```**
---
```ConcurrentStack<T>``` - потокобезопасная коллекция, работающая по принципу **LIFO (Last In First Out)**.

Коллекцию, работающую по принципу **LIFO** называют - стеком.

Принципы работы стека:
1. При добавлении элемента, он будет добавлен в начало (на вершину) стека;
2. При извлечении элемента, вы будете извлекать последний добавленный элемент, при этом удаляя его из стека.

Открытые API для работы с элементами:

- ```void Push(T item)``` – добавляет элемент в начало (на вершину) стека;
- ```void PushRange(T[] items)``` – добавляет элементы массива в начало (на вершину) стека;
- ```void PushRange(T[] items, int startIndex, int count)``` – добавляет указанное (count) количество элементов массива, начиная с указанного индекса (startIndex) в начало (на вершину) стека;
- ```bool TryPeek (out T item)``` – извлекает элемент из начала (вершины) стека для просмотра;
- ```bool TryPop(out T item)``` – извлекает элемент из начала (вершины) стека, удаляя его;
- ```int TryPopRange(T[] items)``` – извлекает элементы из начала (вершины) стека, добавляя их в массив. При этом элементы стека, добавленные в массив, из стека удаляются. Метод пытается извлечь количество элементов, совпадающих с размером переданного массива. TryPopRange возвращает количество извлеченных элементов;
- ```int TryPopRange(T[] items, int startIndex, int count)``` – извлекает указанное (count) количество элементов из начала (вершины) стека, добавляя их в массив, начиная с указанного индекса (startIndex). При этом элементы стека, добавленные в массив, из стека удаляются. Метод пытается извлечь количество элементов, совпадающих с размером переданного массива. TryPopRange возвращает количество извлеченных элементов.

Методы ```bool TryXXX``` возвращают **true** – если метод выполнился успешно, **false** – если у метода не получилось выполниться. Элемент они возвращают в ```out``` параметре.

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        ConcurrentStack<int> stack = new();

        Task fillingTask1 = Task.Run(() => FillStack(stack, 0, 5));
        Task fillingTask2 = Task.Run(() => FillStack(stack, 5, 5));

        await Task.WhenAll(fillingTask1, fillingTask2);
        await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

        Task cleanupTask = Task.Run(() => CleanupStack(stack));
        await Task.Delay(500);
        Task readingTask = Task.Run(() => ReadStack(stack));

        await Task.WhenAll(cleanupTask, readingTask);
        await Console.Out.WriteLineAsync("All the Reading and Cleanup Tasks were completed successfully.");
    }

    private static void FillStack(ConcurrentStack<int> stack, int startIndex, int elementsNumber)
    {
        for (int i = startIndex; i < startIndex + elementsNumber; i++)
        {
            stack.Push(i);
            Console.WriteLine($"+Element [{i}] was added to [{nameof(ConcurrentStack<int>)}].");
            Thread.Sleep(100);
        }
    }

    private static void ReadStack(ConcurrentStack<int> stack)
    {
        foreach (var item in stack)
        {
            Console.WriteLine($"+Element [{item}] was read from [{nameof(ConcurrentStack<int>)}].");
            Thread.Sleep(200);
        }
    }

    private static void CleanupStack(ConcurrentStack<int> stack)
    {
        while (stack.TryPop(out int result))
        {
            Console.WriteLine($"+Element [{result}] was removed from [{nameof(ConcurrentStack<int>)}].");
            Thread.Sleep(100);
        }
    }
}
```

Как видно из примера, операции записи, чтения и изменения коллекции, производимые над ней из разных потоков одновременно, не привели к возникновению ошибок.

При этом, можно было заметить интересную особенность одновременного чтения коллекции через ```foreach``` и удаления из неё элементов - те элементы, которые было удалены до запуска метода чтения и цикла перебора коллекции ```foreach```, как и ожидалось, не были прочитаны, однако, несмотря на то, что все элементы были удалены раньше, чем метод чтения завершил свою работу и цикл ```foreach``` закончил перебор коллекции, цикл всё равно получил доступ ко всем элементам, которые находились в коллекции тот момент, когда он начал её перебор.

## **03 Потокобезопасная "сумка" ```ConcurrentBag<T>```**
---
```ConcurrentBag<T>``` - неупорядоченная потокобезопасная коллекция.

```ConcurrentBag<T>``` подходит в случаях, когда порядок элементов не имеет значения. Поэтому, изымая элемент вы можете получить совсем не то, что ожидали.

Открытые API для работы с элементами:

- ```void Add(T item)``` – добавляет элемент в коллекци;
- ```bool TryTake(out T item)``` – извлекает элемент из коллекции, удаляя его;
- ```bool TryPeek (out T item)``` – извлекает элемент из коллекции для просмотра.

Методы ```bool TryXXX``` возвращают **true** – если метод выполнился успешно, **false** – если у метода не получилось выполниться. Элемент они возвращают в ```out``` параметре.

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        ConcurrentBag<int> bag = new();

        Task fillingTask1 = Task.Run(() => FillBag(bag, 0, 5));
        Task fillingTask2 = Task.Run(() => FillBag(bag, 5, 5));

        await Task.WhenAll(fillingTask1, fillingTask2);
        await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

        Task cleanupTask = Task.Run(() => CleanupBag(bag));
        await Task.Delay(500);
        Task readingTask = Task.Run(() => ReadBag(bag));

        await Task.WhenAll(cleanupTask, readingTask);
        await Console.Out.WriteLineAsync("All the Reading and Cleanup Tasks were completed successfully.");
    }

    private static void FillBag(ConcurrentBag<int> bag, int startIndex, int elementsNumber)
    {
        for (int i = startIndex; i < startIndex + elementsNumber; i++)
        {
            bag.Add(i);
            Console.WriteLine($"+Element [{i}] was added to [{nameof(ConcurrentBag<int>)}].");
            Thread.Sleep(100);
        }
    }

    private static void ReadBag(ConcurrentBag<int> bag)
    {
        foreach (var item in bag)
        {
            Console.WriteLine($"+Element [{item}] was read from [{nameof(ConcurrentBag<int>)}].");
            Thread.Sleep(200);
        }
    }

    private static void CleanupBag(ConcurrentBag<int> bag)
    {
        while (bag.TryTake(out int result))
        {
            Console.WriteLine($"+Element [{result}] was removed from [{nameof(ConcurrentBag<int>)}].");
            Thread.Sleep(100);
        }
    }
}
```

Как видно из примера, операции записи, чтения и изменения коллекции, производимые над ней из разных потоков одновременно, не привели к возникновению ошибок.

При этом, можно было заметить интересную особенность одновременного чтения коллекции через ```foreach``` и удаления из неё элементов - те элементы, которые было удалены до запуска метода чтения и цикла перебора коллекции ```foreach```, как и ожидалось, не были прочитаны, однако, несмотря на то, что все элементы были удалены раньше, чем метод чтения завершил свою работу и цикл ```foreach``` закончил перебор коллекции, цикл всё равно получил доступ ко всем элементам, которые находились в коллекции тот момент, когда он начал её перебор.
