# Producer-Consumer Pattern

**Шаблон Producer-Consumer** подходит для ситуаций, когда скорость получения (генерации) данных/задач отличается от скорости обработки данных/задач.  

Применяется в тех местах, где необходимо выполнять несколько процессов одновременно, которые отличаются по скорости выполнения.

Например, получение запросов из удалённого источника, и их обработка, где сетевой запрос и локальные вычисления, различающиеся по скорости выполнения, разделены.

Шаблон состоит из 3-х частей:
1. **Структура данных** (напримре, коллекция), где могут быть расположены данные и задачи для их хранения и последующего извлечения. Может быть одна в рамках одной реализации шаблона;
2. **Producer** – это изготовитель (поставщик) данных (задач), который создает или поставляет данные (задачи) в структуру данных. В роли поставщика может высступать поток выполнения, удалённый сервис или другая система (приложение). Может быть несколько в рамках одной реализации шаблона;
3. **Consumer** – это потребитель, который берет данные (задачи) из структуры данных и выполняет над ними манипуляции (обрабатывает, выполняет, отправляет результаты). В роли потребителя может высступать поток выполнения, удалённый сервис или другая система (приложение). Может быть несколько в рамках одной реализации шаблона. 

> TODO: изображение со схемой участников шаблона **Producer-Consumer**

В некоторых случаях подразумевается блокирование потребителей, если структура данных пустая до момента появления в них новых данных для обработки.

Также в некоторых случаях поставщики могут указать, что данных больше не будет и потребители могут быть разблокированы для финализации своей работы.

В некоторых случаях поставщики также могут быть заблокированы, так как в структуре данных может быть указан максимальный или фиксированный размер, и, чтобы её не переполнять, может быть установлена блокировка на поставщиках.

### **Интерфейс ```IProducerConsumerCollection<T>```**

**Интерфейс ```IProducerConsumerCollection<T>```** - реализация шаблона Producer-Consumer в **.NET** с помощью потокобезопасных коллекций.

Методы интерфейса ```IProducerConsumerCollection<T>``:
- ```void CopyTo(T[] array, int index)``` – скопировать содержимое коллекции в массив;
- ```T[] ToArray()``` – превратить коллекцию в массив;
- ```bool TryAdd(T item)``` – попытаться добавить элемент в коллекцию;
- ```bool TryTake(out T item)``` – попытаться получить и удалить элемент из коллекции.

Данный интерфейс наследуют и реализуют следующие потокобезопасные коллекции в **.NET**:
- ```ConcurrentQueue<T>```
- ```ConcurrentStack<T>```
- ```ConcurrentBag<T>```

Реализация этого интерфейса в упомянутых коллекциях устроена достаточно простым способом: методы ```TryAdd()``` и ```TryTake()``` перевызывают аналогичные методы в конкретных коллекциях, например, методы ```Enqueue()``` и ```Dequeue()``` в коллекции ```ConqurrentQueue```.

## **01 Интерфейс ```IProducerConsumerCollection<T>``` и ```ConcurrentQueue<T>```**
---
При приведении экземпляра класса ```ConcurrentQueue<T>``` к интерфейсу ```IProducerConsumerCollection<T>``` и вызову его методов ```TryAdd()``` и ```TryTake()``` вместо методов ```Enqueue()``` и ```Dequeue()```, поведение коллекции не изменится, как если бы работа производилась с типом ```ConcurrentQueue<T>``` напрямую.

Пример работы интерфейса ```IProducerConsumerCollection<T>```, к которому приведён ```ConcurrentQueue<T>```, где создаются 2 экзмепляра класса ```Producer```, представляющие поставщиков данных и 3 экземпляра класса ```Consumer```, представляющие потребителей данных и одновременно из разных потоков добавляют данные в структуру данных и забирают данные их неё:

```cs
namespace _01_IProducerConsumerCollection.ConcurrentQueue
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IProducerConsumerCollection<int> dataStructure = new ConcurrentQueue<int>();

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(100);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(producer1Task, producer2Task, consumer1Task, consumer2Task, consumer3Task);

            await Console.Out.WriteLineAsync($"All the Tasks were completed successfully.{Environment.NewLine}");
        }
    }

    internal class Producer
    {
        private readonly IProducerConsumerCollection<int> _dataStructure;

        public Producer(IProducerConsumerCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(IProducerConsumerCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(IProducerConsumerCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly IProducerConsumerCollection<int> _dataStructure;

        public Consumer(IProducerConsumerCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (_dataStructure.TryTake(out int result))
            {
                Console.WriteLine($"-Element [{result}] was removed from [{nameof(IProducerConsumerCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(200);
            }
        }
    }
}
```

Как видно, что, несмотря на то, что над структурой данных одновременно из разных потоков производились манипуляции по добавлению и забору данных различным количеством поставщиков и потребителей, всё отработало корректно, все данные были добавлены и забраны.

## **02 Интерфейс ```IProducerConsumerCollection<T>``` и ```ConcurrentStack<T>```**
---
При приведении экземпляра класса ```ConcurrentStack<T>``` к интерфейсу ```IProducerConsumerCollection<T>``` и вызову его методов ```TryAdd()``` и ```TryTake()``` вместо методов ```Push()``` и ```TryPop()```, поведение коллекции не изменится, как если бы работа производилась с типом ```ConcurrentStack<T>``` напрямую.

Пример работы интерфейса ```IProducerConsumerCollection<T>```, к которому приведён ```ConcurrentStack<T>```, где создаются 2 экзмепляра класса ```Producer```, представляющие поставщиков данных и 3 экземпляра класса ```Consumer```, представляющие потребителей данных и одновременно из разных потоков добавляют данные в структуру данных и забирают данные их неё:

```
> The same code as in Example#01 but ConcurrentQueue is changed to ConcurrentStack
```

Как видно, что, несмотря на то, что над структурой данных одновременно из разных потоков производились манипуляции по добавлению и забору данных различным количеством поставщиков и потребителей, всё отработало корректно, все данные были добавлены и забраны.

## **03 Интерфейс ```IProducerConsumerCollection<T>``` и ```ConcurrentBag<T>```**
---
При приведении экземпляра класса ```ConcurrentBag<T>``` к интерфейсу ```IProducerConsumerCollection<T>``` и вызову его методов ```TryAdd()``` и ```TryTake()``` вместо методов ```Add()``` и ```ConcurrentBag<T>.TryTake()```, поведение коллекции не изменится, как если бы работа производилась с типом ```ConcurrentBag<T>``` напрямую:

Пример работы интерфейса ```IProducerConsumerCollection<T>```, к которому приведён ```ConcurrentBag<T>```, где создаются 2 экзмепляра класса ```Producer```, представляющие поставщиков данных и 3 экземпляра класса ```Consumer```, представляющие потребителей данных и одновременно из разных потоков добавляют данные в структуру данных и забирают данные их неё:

```
> The same code as in Example#01 but ConcurrentQueue is changed to ConcurrentBag
```

Как видно, что, несмотря на то, что над структурой данных одновременно из разных потоков производились манипуляции по добавлению и забору данных различным количеством поставщиков и потребителей, всё отработало корректно, все данные были добавлены и забраны.

# **Класс ```BlockingCollection<T>```**
---
Мотивация для использования интерфейса ```IProducerConsumerCollection<T>```, когда можно использовать потокобезопасные коллекции напрямую, заключается в том, что интерфейс ```IProducerConsumerCollection<T>``` просто показывает, какие коллекции могут быть использованы в роли хранилища данных для **шаблона Producer-Consumer** и является ключём для использования класса ```BlockingCollection<T>```, упрощающего работу с шаблоном ```Producer-Consumer<T>``` с помощью вспомогательных методов и свойств.

**Класс ```BlockingCollection<T>```** - является не коллекцией, а объектом, который является **декоратором** оболочкой для потокобезопасных коллекций, наследующих интерфейс ```IProducerConsumerCollection<T>```, и не может функционировать самостоятельно, без использования коллекций.

Для упращения можно называть этот объект коллекцией, так как его внешнее поведение подобно поведению коллекции.

**```BlockingCollection<T>``` позволяет**:

- Одновременное добавление и удаление элементов с помощью методов ```Add()/TryAdd()``` и ```Take()/TryTake()``` из нескольких потоков;
- Поддержка ограничения и блокировки. Блокирование операции Add или Take, когда коллекция заполнена или пустая;
- Возможность отмены выполнения методов ```Add()/TryAdd()``` и ```Take()/TryTake()``` с помощью ```CancellationToken``` или таймаута.

**Коллекция ```BlockingCollection<T>```** реализует интерфейс ```IDisposable```. Когда вы закончили работать с коллекцией, вы должны избавиться от нее, вызвав метод ```Dispose()``` руками или через конструкцию ```using```.

Метод ```Dispose()``` не является потокобезопасным! Все остальные открытые и защищенные члены ```BlockingCollection<T>``` могут использоваться одновременно из нескольких потоков.

При создании коллекции ```BlockingCollection<T>``` имеется возможность её конфигурации необходимым в данном случае образом.

При создании объекта ```BlockingCollection<T>``` возможно задать несколько настроек для коллекции с
помощью конструктора:
1. Указать какой тип коллекции будет использован внутри. Этот параметр принимает объект, реализующий интерфейс IProducerConsumerCollection<T>. По умолчанию при создании типа используется ConcurrentQueue<T>;
2. Установка максимальной вместимости коллекции. Это ограничение важно, потому что оно позволяет контролировать максимальный размер коллекции в памяти.

### **Конструкторы:**
- ```public BlockingCollection()```;
- ```public BlockingCollection(int boundedCapacity)```;
- ```public BlockingCollection(IProducerConsumerCollection<T> collection)```;
- ```public BlockingCollection(IProducerConsumerCollection<T> collection, int boundedCapacity)```.

### **Свойства:**
- ```bool IsAddingCompleted { get; }``` – показывает, помечена ли коллекция как завершенная для добавления;
- ```bool IsCompleted { get; }``` – показывает, помечена ли коллекция для добавления и является ли она пустой;
- ```int BoundedCapacity { get; }``` – отдает установленную ограниченную емкость;
- ```int Count { get; }``` – отдает количество элементов.

### **Блокирующие методы для добавления и удаления элементов коллекции:**
- ```void Add (T item)``` – добавляет элемент в коллекцию. Если при инициализации коллекции была указана ограниченная емкость, выполнение метода Add может заблокировано, пока не освободится место для добавления элемента.
- ```void Add (T item, CancellationToken cancellationToken)``` – делает то же, что и метод Add выше, но, при этом еще и поддерживает отмену выполнения.
- ```T Take()``` – возвращает и удаляет элемент из коллекции. Если в коллекции нет элементов, метод Take заблокирует поток, пока он не получит элемент.
- ```T Take(CancellationToken cancellationToken)``` – делает то же, что и метод Take выше, но при этом еще и поддерживает отмену выполнения.

### **Не блокирующие методы для добавления и удаления элементов коллекции:**

- ```bool TryAdd()``` – пытается добавить элемент в коллекцию. Если коллекция является ограниченной и она в данный момент заполнена до предела, то этот метод немедленно возвращает значение **false**, не добавляя элемент. Если элемент был успешно добавлен - возвращает значение true. У метода ```TryAdd()``` есть различные перегрузки, которые поддерживают отмену или таймаут на выполнение:
    - ```bool TryAdd(T item)```;
    - ```bool TryAdd(T item, int millisecondsTimeout)```;
    - ```bool TryAdd(T item, TimeSpan timeout)```;
    - ```bool TryAdd(T item, int millisecondsTimeout), CancellationToken cancellationToken)```;
- ```bool TryTake()``` – пытается вернуть и удалить элемент из коллекции. Если коллекция пустая – метод немедленно возвращает значение **false**. Если метод TryTake смог найти элемент, поместить его значение в out параметр и удалить из коллекции, то метод возвращает значение true. У метода ```TryTake()``` есть различные перегрузки, которые поддерживают отмену или таймаут на выполнение:
    - ```bool TryTake(out T item)```;
    - ```bool TryTake(out T item, int millisecondsTimeout)```;
    - ```bool TryTake(out T item, TimeSpan timeout)```;
    - ```bool TryTake(out T item, int millisecondsTimeout, CancellationToken cancellationToken)```.


## **04 Класс ```BlockingCollection<T>``` и ```ConcurrentQueue<T>```**
---
Пример работы класса ```BlockingCollection<T>```, в который обёрнут ```ConcurrentQueue<T>```, где создаются 2 экзмепляра класса ```Producer```, представляющие поставщиков данных и 3 экземпляра класса ```Consumer```, представляющие потребителей данных и одновременно из разных потоков добавляют данные в структуру данных и забирают данные их неё: 

```
> The same code as in Example#01 but IProducerConsumerCollection is changed to BlockingCollection
```

Как видно, что, несмотря на то, что над структурой данных одновременно из разных потоков производились манипуляции по добавлению и забору данных различным количеством поставщиков и потребителей, всё отработало корректно, все данные были добавлены и забраны.

## **05 Класс ```BlockingCollection<T>``` и ```ConcurrentStack<T>```**
---
Пример работы класса ```BlockingCollection<T>```, в который обёрнут ```ConcurrentStack<T>```, где создаются 2 экзмепляра класса ```Producer```, представляющие поставщиков данных и 3 экземпляра класса ```Consumer```, представляющие потребителей данных и одновременно из разных потоков добавляют данные в структуру данных и забирают данные их неё: 

```
> The same code as in Example#02 but IProducerConsumerCollection is changed to BlockingCollection
```

Как видно, что, несмотря на то, что над структурой данных одновременно из разных потоков производились манипуляции по добавлению и забору данных различным количеством поставщиков и потребителей, всё отработало корректно, все данные были добавлены и забраны.

## **06 Класс ```BlockingCollection<T>``` и ```ConcurrentBag<T>```**
---
Пример работы класса ```BlockingCollection<T>```, в который обёрнут ```ConcurrentBag<T>```, где создаются 2 экзмепляра класса ```Producer```, представляющие поставщиков данных и 3 экземпляра класса ```Consumer```, представляющие потребителей данных и одновременно из разных потоков добавляют данные в структуру данных и забирают данные их неё: 

```
> The same code as in Example#03 but IProducerConsumerCollection is changed to BlockingCollection
```

Как видно, что, несмотря на то, что над структурой данных одновременно из разных потоков производились манипуляции по добавлению и забору данных различным количеством поставщиков и потребителей, всё отработало корректно, все данные были добавлены и забраны.

### **Поддержка ```BlockingCollection<T>``` ограничения и блокировки**
```BlockingCollection<T>``` поддерживает ограничение и блокировку. Под ограничением имеется в виду то, что вы можете установить максимальную вместимость коллекции.

- Несколько потоков-поставщиков могут одновременно добавлять элементы в коллекцию. Если коллекция будет наполнена до максимально указанной емкости, то потоки-поставщики будут заблокированы на операции добавления, пока элемент не будет удален.
- Несколько потоков-потребителей могут одновременно удалять элементы из коллекции. Если коллекция станет пустой, потоки-потребители будут заблокированы, пока потоки-поставщики не добавят новые элементы.
- Если добавление элементов окончено, то для того чтобы разблокировать потоки-потребители, которые ждут новых элементов, необходимо вызвать метод ```CompleteAdding()```. Он указывает, что больше новых элементов не будет добавлено и будит потоки исключением ```InvalidOperationException```;
- Потоки-потребители могут узнать, что коллекция пуста и элементов больше не будет добавлено с помощью свойства ```IsCompleted```.


## **07 Свойство ```BoundedCapacity``` класса ```BlockingCollection<T>```**
---
При установке в конструкторе класса ```BlockingCollection<T>``` значения в параметр ```boundedCapacity```, будет установлен лимит на максимальное количество элементов, которые возможно добавить в коллекцию, которое также будет отражено в открытом свойстве ```BoundedCapacity```.

В случае, если какой-либо поток произведёт попытку добавить данные в коллекцию после её заполнения максимально возможным количеством элементов, он будет заблокирован до тех пор, пока какой-нибудь другой поток не извлечёт хотя бы один элемент из коллекции, тем самым, освободив место для добавления:

```cs
namespace _07_BlockingCollection.Capacity
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentBag<int>(), 5);

            await Console.Out.WriteLineAsync(
                $"[{nameof(BlockingCollection<int>)}.{nameof(BlockingCollection<int>.BoundedCapacity)}] is " +
                $"[{dataStructure.BoundedCapacity}] elements.{Environment.NewLine}");

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(100);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(producer1Task, producer2Task, consumer1Task, consumer2Task, consumer3Task);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Tasks were completed successfully.{Environment.NewLine}");
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (_dataStructure.TryTake(out int result))
            {
                Console.WriteLine($"-Element [{result}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(200);
            }
        }
    }
}
```

Из примера видно, что, после заполнения коллекции максимальным количеством элементов, при попытке добавить дополнительные элементы, добавляющие поток был заблокированы, пока другие потоки на извлекли из коллекции элементы и не освободили тем самым место для добавления элементов заблокированными ранее потоками, которые блокировались каждый раз, когда коллекция заполнялась полностью.

## **08 Метод ```CompleteAdding()``` класса ```BlockingCollection<T>```**
---
После вызова метода ```CompleteAdding()``` на экземпляре класса ```BlockingCollection<T>``` в него больше невозможно будет добавить новые элементы и при попытке добавления нового элемента будет возбуждено исключение ```InvalidOperationException```:

```cs
namespace _08_BlockingCollection.CompleteAdding
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentQueue<int>());

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(200);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            dataStructure.CompleteAdding();

            try
            {
                await Task.WhenAll(producer1Task, producer2Task, consumer1Task, consumer2Task, consumer3Task);
                await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Tasks were completed successfully.{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(
                    $"{Environment.NewLine}An error was occurred while the Tasks is processing." +
                    $"{Environment.NewLine}Exception Type: {ex.GetType().Name}" +
                    $"{Environment.NewLine}Exception Message: {ex.Message}");
            }
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (_dataStructure.TryTake(out int result))
            {
                Console.WriteLine($"-Element [{result}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(200);
            }
        }
    }
}
```

## **09 Свойство ```IsAddingCompleted``` класса ```BlockingCollection<T>```**
---
Для того, чтобы избежать исключения при добавлении элементов в коллекцию ```BlockingCollection<T>```, на которой был вызван метод ```CompleteAdding```, можно при добавлении воспользоваться свойством ```IsAddingCompleted``` коллекции  ```BlockingCollection<T>``` и обработать ситуацию, когда добавление новых элементов в коллекцию более невозможно:

```cs
namespace _09_BlockingCollection.IsAddingComplete
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentQueue<int>());

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(300);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            dataStructure.CompleteAdding();

            await Task.WhenAll(producer1Task, producer2Task, consumer1Task, consumer2Task, consumer3Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Tasks were completed successfully.{Environment.NewLine}");
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.IsAddingCompleted)
                {
                    Console.WriteLine(
                        $">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.IsAddingCompleted)}] property is [{_dataStructure.IsAddingCompleted}].");
                }
                else if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (_dataStructure.TryTake(out int result))
            {
                Console.WriteLine($"-Element [{result}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(200);
            }
        }
    }
}
```

## **10 Метод ```Take()``` класса ```BlockingCollection<T>``` после вызова метода ```CompleteAdding()```**
---
В случае, если на экземпляре коллекции ```BlockingCollection<T>``` был вызван метод ```CompleteAdding()```, означающий, что более в коллекцию не будут добавляться новые элементы, то, после взятия всех элементов из коллекции, очередной вызов метода ```Take()```, изымающего элемент из коллекции, выбросит исключение ```InvalidOperationException()```:

```cs
namespace _10_BlockingCollection.TakeFomEmptyCollection
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentQueue<int>());

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(200);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            dataStructure.CompleteAdding();

            try
            {
                await Task.WhenAll(consumer1Task, consumer2Task, consumer3Task);
                await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Consumer)}'s Tasks were completed successfully.{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(
                    $"{Environment.NewLine}An error was occurred while the Tasks is processing." +
                    $"{Environment.NewLine}Exception Type: {ex.GetType().Name}" +
                    $"{Environment.NewLine}Exception Message: {ex.Message}");
            }
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (true)
            {
                Console.WriteLine($"-Element [{_dataStructure.Take()}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(200);
            }
        }
    }
}
```

## **11 Метод ```TryTake()``` класса ```BlockingCollection<T>``` после вызова метода ```CompleteAdding()```**
---
В случае, если на экземпляре коллекции ```BlockingCollection<T>``` был вызван метод ```CompleteAdding()```, означающий, что более в коллекцию не будут добавляться новые элементы, то, после взятия всех элементов из коллекции, очередной вызов метода ```TryTake()```, изымающего элемент из коллекции, вернёт значение **false**, и, при дополнительной проверке свойства ```BlockingCollection<T>.IsAddingCompleted``` можно будет сделать вывод о том, что дальнейшие попытки изъятия из коллекции элементов бессмысленны и завершить работу потребителя:

```cs
namespace _11_BlockingCollection.CompleteAdding.TryTake
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentQueue<int>());

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(200);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            dataStructure.CompleteAdding();

            await Task.WhenAll(consumer1Task, consumer2Task, consumer3Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Consumer)}'s Tasks were completed successfully.{Environment.NewLine}");
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (true)
            {
                bool itemTaken = _dataStructure.TryTake(out int item);

                if (itemTaken)
                {
                    Console.WriteLine($"-Element [{item}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                }
                else if (!itemTaken && _dataStructure.IsAddingCompleted)
                {
                    Console.WriteLine(Environment.NewLine +
                        $">Any element can not be removed anymore from [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.IsAddingCompleted)}] property is [{_dataStructure.IsAddingCompleted}] " +
                        $"and [{nameof(BlockingCollection<int>.Count)}] property is [{_dataStructure.Count}].");

                    break;
                }
                else
                {
                    Console.WriteLine(
                        $">Any element was not removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.Count)}] property is [{_dataStructure.Count}].");
                }

                await Task.Delay(200);
            }
        }
    }
}
```

### **Интерфейсы ```IEnumerable<T>``` и ```IEnumerator<T>``` с классом ```BlockingCollection<T>```**

Класс ```BlockingCollection<T>``` **реализует** интерфейс ```IEnumerable<T>```.

В BlockingCollection<T> есть две версии метода для получения перечислителя:

- Метод ```GetEnumerator()``` реализован явно **(explicitly)**. Он возвращает перечислителя со **"снимком"** коллекции элементов. Под снимком имеется в виду получение элементов на момент вызова метода;
- Метод ```GetConsumingEnumerable()``` возвращает перечислитель, который будет отдавать (удалять из коллекции) элементы (если они есть в коллекции) до тех пор, пока значение свойства ```IsCompleted``` не станет равным **true**. Если элементов в коллекции нет и значение свойства ```IsCompleted``` равно **false** - цикл блокируется до тех пор, пока не появится доступный элемент или до отмены ```CancellationToken```.

Метод ```GetConsumingEnumerable()``` имеет две перегрузки. Первая - без параметров, вторая - принимает один параметр типа ```CancellationToken```. Это показывает, что отмена является необязательной.

## **12 Цикл ```foreach``` и метод ```GetEnumerator()``` класса ```BlockingCollection<T>```**
---
Пример использования класса ```BlockingCollection<T>``` с циклом ```foreach``` с использованием реализации метода ```GetEnumerator()``` в классе ```BlockingCollection<T>```:

```cs
namespace _12_BlockingCollection.GetEnumerator
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentQueue<int>());

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            await Task.Delay(200);

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            foreach (var item in dataStructure)
            {
                Console.WriteLine($"=Element [{item}] is read from [{nameof(BlockingCollection<int>)}].");

                await Task.Delay(100);
            }

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(BlockingCollection<int>)}'s elements are read.{Environment.NewLine}");

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(consumer1Task, consumer2Task, consumer3Task);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Consumer)}'s Tasks were completed successfully.{Environment.NewLine}");
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (_dataStructure.TryTake(out int result))
            {
                Console.WriteLine($"-Element [{result}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                await Task.Delay(100);
            }
        }
    }
}
```

## **13 Метод ```GetConsumingEnumerable()``` класса ```BlockingCollection<T>```**
---
Пример использования класса ```BlockingCollection<T>``` с циклом ```foreach``` с использованием объекта типа ```IEnumerable<T>```, полученного из метода ```GetConsumingEnumerable()``` класса ```BlockingCollection<T>```:

```cs
namespace _13_BlockingCollection.GetConsumingEnumerable
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure = new(new ConcurrentQueue<int>());

            Producer producer1 = new(dataStructure);
            Producer producer2 = new(dataStructure);

            Consumer consumer1 = new(dataStructure);
            Consumer consumer2 = new(dataStructure);

            Task producer1Task = producer1.AddDataAsync(0, 5);
            Task producer2Task = producer2.AddDataAsync(5, 5);

            CancellationTokenSource cts = new();

            IEnumerable<int> consumingEnumerable = dataStructure.GetConsumingEnumerable(cts.Token);

            Task consumingEnumerableTask = Task.Run(async () =>
            {
                foreach (int item in consumingEnumerable)
                {
                    Console.WriteLine($"=Element [{item}] is read from [{consumingEnumerable.GetType().Name}].");
                    await Task.Delay(100);
                }
            });

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.");

            cts.Cancel();

            await Console.Out.WriteLineAsync($"{Environment.NewLine}Consumer Enumerable Task was canceled.");
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Producer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                if (_dataStructure.TryAdd(i))
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int> _dataStructure;

        public Consumer(BlockingCollection<int> dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public async Task TakeDataAsync()
        {
            while (true)
            {
                bool itemTaken = _dataStructure.TryTake(out int item);

                if (itemTaken)
                {
                    Console.WriteLine($"-Element [{item}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                }
                else if (!itemTaken && _dataStructure.IsAddingCompleted)
                {
                    Console.WriteLine(Environment.NewLine +
                        $">Any element can not be removed anymore from [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.IsAddingCompleted)}] property is [{_dataStructure.IsAddingCompleted}] " +
                        $"and [{nameof(BlockingCollection<int>.Count)}] property is [{_dataStructure.Count}].");

                    break;
                }
                else
                {
                    Console.WriteLine(
                        $">Any element was not removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.Count)}] property is [{_dataStructure.Count}].");
                }

                await Task.Delay(200);
            }
        }
    }
}
```

В цикле ```foreach``` элементы обрабатываются и изымаются из коллекции по мере их поступления в них, если коллекция пуста, то цикл ```foreach``` блокируется. Для корректного завершения работы цикла можно воспользоваться маркером отмены ```CancellationToken```.

## **14 Метод ```TryAddToAny()``` класса ```BlockingCollection<T>```**
---
Статический метод ```TryAddToAny()``` класса ```BlockingCollection<T>``` используется для добавления элемента в одну из коллекций, которая может принять этот элемент.

Метод ```TryAddToAny()``` принимает 2 аргумента:
1. ```BlockingCollection<T>[] collections``` - массив коллекций, в которые будет произведена попытка добавления элемента;
2. ```T item``` - элемент для попытки добавления.

Также метод ```TryAddToAny()``` возвращает значение типа ```int```, соответствующее индексу коллекции из массива, в которую был помещён элемент. Если элемент не удалось поместить ни в одну из коллекций, то будет возвращено значение **-1**.

Пример использования метода ```BlockingCollection<T>.TryAddToAny()```:

```cs
namespace _14_BlockingCollection.TryAddToAny
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure1 = new(new ConcurrentQueue<int>(), 5);
            using BlockingCollection<int> dataStructure2 = new(new ConcurrentQueue<int>(), 5);
            using BlockingCollection<int> dataStructure3 = new(new ConcurrentQueue<int>(), 5);

            BlockingCollection<int>[] collections = new[] { dataStructure1, dataStructure2, dataStructure3 };

            Producer producer1 = new(collections);
            Producer producer2 = new(collections);

            CancellationTokenSource cts = new();
            Task producer1Task = producer1.AddDataAsync(0, 10);
            Task producer2Task = producer2.AddDataAsync(10, 5);

            await Task.WhenAll(producer1Task, producer2Task);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            Task enumerateTask1 = EnumerateCollection(dataStructure1, nameof(dataStructure1), cts.Token);
            Task enumerateTask2 = EnumerateCollection(dataStructure2, nameof(dataStructure2), cts.Token);
            Task enumerateTask3 = EnumerateCollection(dataStructure3, nameof(dataStructure3), cts.Token);

            cts.CancelAfter(1000);

            await Task.Delay(1000);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}Consumer Enumerable Task was canceled.");
        }

        private static async Task EnumerateCollection(BlockingCollection<int> dataStructure, string dataStructureName, CancellationToken ct)
        {
            IEnumerable<int> consumingEnumerable = dataStructure.GetConsumingEnumerable(ct);

            await Task.Run(async () =>
            {
                foreach (int item in consumingEnumerable)
                {
                    Console.WriteLine($"=Element [{item}] is read from [{consumingEnumerable.GetType().Name}] with a name [{dataStructureName}].");
                    await Task.Delay(100);
                }
            });
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int>[] _collections;

        public Producer(params BlockingCollection<int>[] collections)
        {
            _collections = collections;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                int collectionIndex = BlockingCollection<int>.TryAddToAny(_collections, i);

                if (collectionIndex != -1)
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] with Index [{collectionIndex}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }
}
```

## **15 Метод ```TryTakeFromAny()``` класса ```BlockingCollection<T>```**
---
Статический метод ```TryTakeFromAny()``` класса ```BlockingCollection<T>``` используется для добавления элемента в одну из коллекций, которая может принять этот элемент.

Метод ```TryTakeFromAny()``` принимает 1 аргумент типа ```BlockingCollection<T>[] collections```, представляющий массив коллекций, из которых будут извлекаться элементы.

Также метод ```TryTakeFromAny()``` возвращает значение типа ```int```, соответствующее индексу коллекции из массива, из которой был извлечён элемент. Если ни из одной коллекции не удалось извлечь элемент, то будет возвращено значение **-1**.

Пример использования метода ```BlockingCollection<T>.TryTakeFromAny()```:

```cs
namespace _15_BlockingCollection.TryTakeFromAny
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using BlockingCollection<int> dataStructure1 = new(new ConcurrentQueue<int>(), 5);
            using BlockingCollection<int> dataStructure2 = new(new ConcurrentQueue<int>(), 5);
            using BlockingCollection<int> dataStructure3 = new(new ConcurrentQueue<int>(), 5);

            BlockingCollection<int>[] collections = new[] { dataStructure1, dataStructure2, dataStructure3 };

            Producer producer1 = new(collections);
            Producer producer2 = new(collections);

            Consumer consumer1 = new(collections);
            Consumer consumer2 = new(collections);

            Task producer1Task = producer1.AddDataAsync(0, 10);
            Task producer2Task = producer2.AddDataAsync(10, 10);

            await Task.Delay(500);

            Task consumer1Task = consumer1.TakeDataAsync();
            Task consumer2Task = consumer2.TakeDataAsync();
            Task consumer3Task = consumer2.TakeDataAsync();

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            dataStructure1.CompleteAdding();
            dataStructure2.CompleteAdding();
            dataStructure3.CompleteAdding();

            await Task.WhenAll(producer1Task, producer2Task);
            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Producer)}'s Tasks were completed successfully.{Environment.NewLine}");

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(BlockingCollection<int>)}'s elements are read.{Environment.NewLine}");

            await Task.WhenAll(consumer1Task, consumer2Task, consumer3Task);

            await Console.Out.WriteLineAsync($"{Environment.NewLine}All the {nameof(Consumer)}'s Tasks were completed successfully.{Environment.NewLine}");
        }
    }

    internal class Producer
    {
        private readonly BlockingCollection<int>[] _collections;

        public Producer(params BlockingCollection<int>[] collections)
        {
            _collections = collections;
        }

        public async Task AddDataAsync(int startIndex, int elementsNumber)
        {
            for (int i = startIndex; i < startIndex + elementsNumber; i++)
            {
                int collectionIndex = BlockingCollection<int>.TryAddToAny(_collections, i);

                if (collectionIndex != -1)
                {
                    Console.WriteLine($"+Element [{i}] was added to [{nameof(BlockingCollection<int>)}] with Index [{collectionIndex}] by a [{nameof(Producer)}].");
                }
                else
                {
                    Console.WriteLine($">Element [{i}] was not added to [{nameof(BlockingCollection<int>)}] by a [{nameof(Producer)}].");
                }

                await Task.Delay(100);
            }
        }
    }

    internal class Consumer
    {
        private readonly BlockingCollection<int>[] _collections;

        public Consumer(params BlockingCollection<int>[] collections)
        {
            _collections = collections;
        }

        public async Task TakeDataAsync()
        {
            while (true)
            {
                int collectionIndex = BlockingCollection<int>.TryTakeFromAny(_collections, out int item);

                if (collectionIndex != -1)
                {
                    Console.WriteLine($"-Element [{item}] was removed from [{nameof(BlockingCollection<int>)}] by a [{nameof(Consumer)}].");
                }
                else if (collectionIndex == -1 && _collections.All(x => x.IsAddingCompleted))
                {
                    string report = Environment.NewLine;

                    for (int i = 0; i < _collections.Length; i++)
                    {
                        report += $"{Environment.NewLine}>Any element can not be removed anymore from [{nameof(BlockingCollection<int>)}] with Index [{i}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.IsAddingCompleted)}] property is [{_collections[i].IsAddingCompleted}] " +
                        $"and [{nameof(BlockingCollection<int>.Count)}] property is [{_collections[i].Count}].";
                    }

                    await Console.Out.WriteLineAsync(report);

                    break;
                }
                else
                {
                    string report = Environment.NewLine;

                    for (int i = 0; i < _collections.Length; i++)
                    {
                        report += $"{Environment.NewLine}>Any element was not removed from [{nameof(BlockingCollection<int>)}] with Index [{i}] by a [{nameof(Producer)}] " +
                        $"due to [{nameof(BlockingCollection<int>.Count)}] property is [{_collections[i].Count}].";
                    }

                    await Console.Out.WriteLineAsync(report);
                }

                await Task.Delay(200);
            }
        }
    }
}
```
