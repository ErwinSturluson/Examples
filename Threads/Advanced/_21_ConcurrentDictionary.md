# **Concurrent Dictionary**

```ConcurrentDictionary<TKey, TValue>```- потокобезопасная коллекция, работающая по принципу **"ключ-значение"**, доступ к которой могут одновременно получать несколько потоков.

Словари используются тогда, когда необходимо созранить элемент за определённым признаком, чтобы в дальнеёшем найти этот элемент с помощью этого признака.

```ConcurrentDictionary<TKey, TValue>```, в отличии от многих других потокобезопасных коллекций, не является реализацией шаблона Producer-Consumer, он не наследует интерфейс ```IProducerConsumerCollection<T>```.

```ConcurrentDictionary<TKey, TValue>``` является очень удобной коллекцией из-за большого разнообразия методов по работе с коллекцией.

## **01 Обычный словарь, представленный классом ```Dictionary<TKey, TValue>```**
---
Для ознакомления со сложной коллекцией-словарём ```ConcurrentDictionary<TKey, TValue>``` необходимо сперва всопмнить работу с обычным, непотокобезопасным словарём.

Добавление элементов в словарь может производиться следующими способами:
- В **блоке инициализатора**;
- Методом ```void Add(TKey, TValue)```, который пытается добавить элемент в словарь, а в случае неудачи возбуждает исключение;
- Методом ```bool TryAdd(TKey, TValue)```, который пытается добавить элемент в словарь, в случае успеха возвращает значение **true**, а в случае неудачи **false**.
- Индексатором ```[TKey]```, который пытается присвоить новое значение элементу словаря по ключу, и, в случае отсутствия элемента с указанным в индексаторе ключём, создаёт такой элемент.

Чтение элементов из словаря может производиться следующими способами:
- Методом ```bool TryGetValue(TKey, out TValue)```, который пытается добавить элемент в словарь, в случае успеха возвращает значение **true** и помещает значение в ```out``` параметр, а в случае неудачи возвращает **false**;
- Методом ```bool TryGetValueOrDefault(TKey, out TValue)```, который пытается добавить элемент в словарь, в случае успеха возвращает значение **true** и помещает значение в ```out``` параметр, а в случае неудачи помещает значение по умолчанию для типа значения в ```out``` параметр и возвращает **false**;
- Индексатором ```[TKey]```, который пытается найти и вернуть значение по указанному ключу, а в случае неудачи возбуждает исключение;
- Циклом ```foreach``` с использованием унаследованных и реализованных интерфейсов ```IEnumerator``` и ```IEnumerable```.

Изменение элементов в словаре может производиться следующими способами:
- Индексатором ```[TKey]```, который пытается присвоить новое значение элементу словаря по ключу, но, в случае отсутствия элемента с указанным в индексаторе ключём, создаёт такой элемент.

Удаление элементов из словаря может производиться следующими способами:
- Методом ```bool Remove(TKey)```, который пытается удалить элемент из словаря, и, в случае успеха возвращает значение **true**, а в случае неудачи возвращает **false**;
- Методом ```bool Remove(TKeym out TValue)```, который пытается удалить элемент из словаря, и, в случае успеха возвращает значение **true** и помещает значение в ```out``` параметр, а в случае неудачи возвращает **false**;
- Методом ```Clear()```, который удаляет все элементы из словаря.

Также есть методы, позволяющие проверить наличие ключа или значения в словаре:
- ```bool ContainsKey(TKey)```, который пытается найти элемент в словаре по ключу и, в случае успеха возвращает значение **true**, а в случае неудачи возвращает **false**;
- ```bool ContainsValue(TValue)```, который пытается найти элемент в словаре по значению и, в случае успеха возвращает значение **true**, а в случае неудачи возвращает **false**.

Пример использования основной части этих методов:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        Dictionary<int, string> dictionary = new()
        {
            { 0, $"Text value of key <{0}>." },
            { 1, $"Text value of key <{1}>." }
        };

        FillDictionary(dictionary, 2, 5);

        dictionary[5] = $"Text value of key <{5}>.";

        dictionary[0] = $"Text value of key <{0}> (CHANGED).";
        dictionary[3] = $"Text value of key <{3}> (CHANGED).";

        Console.WriteLine($"Filling operation was completed successfully.{Environment.NewLine}");

        for (int i = 0; i < dictionary.Count; i++)
        {
            Console.WriteLine($"+Element [{i}][{dictionary[i]}] was read from [{nameof(Dictionary<int, string>)}.[int]] indexer.");
            Thread.Sleep(100);
        }

        Console.WriteLine($"Reading operation by [{nameof(Dictionary<int, string>)}.[int]] indexer was completed successfully.{Environment.NewLine}");

        ReadDictionary(dictionary);

        Console.WriteLine($"Reading operation by {nameof(IEnumerator)} was completed successfully.{Environment.NewLine}");

        for (int i = 0; i < dictionary.Count; i++)
        {
            CleanupDictionaryElement(dictionary, i);
        }

        Console.WriteLine("Cleanup operation was completed successfully.");
    }

    private static void FillDictionary(Dictionary<int, string> dictionary, int startIndex, int elementsNumber)
    {
        for (int i = startIndex; i < startIndex + elementsNumber; i++)
        {
            int key = i;
            string value = $"Text value of key <{i}>.";

            if (dictionary.TryAdd(key, value))
            {
                Console.WriteLine($"+Element [{key}][{value}] was added to [{nameof(Dictionary<int, string>)}].");
            }
            else if (dictionary.ContainsKey(key))
            {
                Console.WriteLine($"+Element with Key [{key}] cannot be added in [{nameof(Dictionary<int, string>)}] due to some element with the same Key already exists.");
            }
            else
            {
                Console.WriteLine($"+Element with Key [{key}] cannot be added in [{nameof(Dictionary<int, string>)}] for some reason.");
            }

            Thread.Sleep(100);
        }
    }

    private static void ReadDictionary(Dictionary<int, string> dictionary)
    {
        foreach (var item in dictionary)
        {
            Console.WriteLine($"+Element [{item.Key}][{item.Value}] was read from [{nameof(Dictionary<int, string>)}].");
            Thread.Sleep(100);
        }
    }

    private static void CleanupDictionaryElement(Dictionary<int, string> dictionary, int key)
    {
        if (dictionary.TryGetValue(key, out string value))
        {
            dictionary.Remove(key);
            Console.WriteLine($"+Element [{key}][{value}] was removed from [{nameof(Dictionary<int, string>)}].");
        }
        else
        {
            Console.WriteLine($"+Element with Key [{key}] does not exist in [{nameof(Dictionary<int, string>)}].");
        }
        Thread.Sleep(100);
    }
}
```

### **Работа с элементами в ```ConcurrentDictionary<TKey, TValue>```**

Для работы с данными в **ConcurrentDictionary** есть две группы методов:

1. Методы шаблона **TryXXX**:
    - ```TryAdd()``` – добавление элемента;
    - ```TryGetValue()``` – получение значения элемента;
    - ```TryRemove()``` – удаление элемента;
    - ```TryUpdate()``` – обновление значения элемента;
2. Методы **"Всегда-выполняемые"**:
    - ```AddOrUpdate()``` – изменение значения элемента или его добавление;
    - ```GetOrAdd()``` – получение значения элемента или его добавление.

## **02 Методы шаблона TryXXX класса ```ConcurrentDictionary<TKey, TValue>```**
---
Методы шаблона **TryXXX** – выполняются без генерации исключений. Для отображения корректности своего выполнения, методы шаблона **TryXXX** возвращают значение **true**, если у них получилось выполниться, **false** – если результат выполнения неудача:

- ```bool TryAdd(TKey key, TValue value)``` – метод пытается добавить элемент в коллекцию по ключу.
- ```bool TryGetValue(TKey key, out TValue value)``` – метод пытается получить значение через out параметр.
- ```bool TryRemove(TKey key, out TValue value)``` – метод пытается удалить элемент по ключу, значение удаленного элемента будет помещено в out параметр.
- ```bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)``` - метод пытается обновить значение элемента по ключу (первый параметр). Второй параметр – это новое значение. Третий параметр – значение для сравнения с элементом, который находится внутри словаря.

Также для класса ```ConcurrentDictionary<TKey, TValue>``` недоступна инициализация через блок инициализатора.

Пример, демонстрирующий работу всех методов **TryXXX**:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        ConcurrentDictionary<int, string> dictionary = new();

        Task fillingTask1 = FillConcurrentDictionaryAsync(dictionary, 0, 5);
        Task fillingTask2 = FillConcurrentDictionaryAsync(dictionary, 5, 5);

        dictionary[10] = $"Text value of key <{5}>.";

        dictionary[0] = $"Text value of key <{0}> (CHANGED).";
        dictionary[5] = $"Text value of key <{5}> (CHANGED).";

        await Task.WhenAll(fillingTask1, fillingTask2);
        await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

        Task removingTask = Task.Run(() =>
        {
            for (int i = 0; i < dictionary.Count; i++)
            {
                RemoveConcurrentDictionaryElement(dictionary, i);
            }
        });

        await Task.Delay(300);

        Task readingTask1 = ReadEnumeratorConcurrentDictionaryAsync(dictionary);
        Task readingTask2 = Task.Run(() =>
        {
            int dictionaryLength = dictionary.Count;

            for (int i = 0; i < dictionaryLength; i++)
            {
                if (dictionary.TryGetValue(i, out string value))
                {
                    Console.WriteLine($">Element [{i}][{value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
                }
                else
                {
                    Console.WriteLine($"!Element with Key [{i}] cannot be read from [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
                }

                if (dictionary.TryUpdate(i, $"{value} (UPDATED)", value))
                {
                    Console.WriteLine($"^Element [{i}][{value}] was updated in [{nameof(ConcurrentDictionary<int, string>)}].");
                }
                else
                {
                    Console.WriteLine($"!Element with Key [{i}] cannot be updated in [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
                }

                if (i > 3)
                {
                    Thread.Sleep(100);
                }
            }
        });

        await Task.WhenAll(removingTask, readingTask1, readingTask2);
        await Console.Out.WriteLineAsync("All the Reading and Removing Tasks were completed successfully.{Environment.NewLine}");

        dictionary.Clear();
        await Console.Out.WriteLineAsync($"Cleanup Operation was completed successfully. Current Dictionary size is [{dictionary.Count}].{Environment.NewLine}");
    }

    private static Task FillConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
    {
        return Task.Run(() => FillConcurrentDictionary(dictionary, startIndex, elementsNumber));
    }

    private static void FillConcurrentDictionary(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
    {
        for (int i = startIndex; i < startIndex + elementsNumber; i++)
        {
            int key = i;
            string value = $"Text value of key <{i}>.";

            if (dictionary.TryAdd(key, value))
            {
                Console.WriteLine($"+Element [{key}][{value}] was added to [{nameof(ConcurrentDictionary<int, string>)}].");
            }
            else
            {
                Console.WriteLine($"!Element with Key [{key}] cannot be added to [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
            }

            Thread.Sleep(100);
        }
    }

    private static Task ReadEnumeratorConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary)
    {
        return Task.Run(() =>
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($"+Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
                Thread.Sleep(200);
            }
        });
    }

    private static void ReadEnumeratorConcurrentDictionary(ConcurrentDictionary<int, string> dictionary)
    {
        foreach (var item in dictionary)
        {
            Console.WriteLine($">Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
            Thread.Sleep(200);
        }
    }

    private static void RemoveConcurrentDictionaryElement(ConcurrentDictionary<int, string> dictionary, int key)
    {
        if (dictionary.TryRemove(key, out string value))
        {
            Console.WriteLine($"-Element [{key}][{value}] was removed from [{nameof(ConcurrentDictionary<int, string>)}].");
        }
        else
        {
            Console.WriteLine($"!Element with Key [{key}] cannot be removed from [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
        }

        Thread.Sleep(100);
    }
}
```

Из примера видно, что все методы отработали без логических ошибок и без технических ошибок времени выполнения, но были выявлениы следующие особенности выполнения:
- Метод ```TryAdd()``` не смог добавить некоторые значения, потому что они уже были добавлены индексаторами раньше, чем задачами на добавление элементов;
- Метод ```TryGetValue()``` не смог прочитать некоторые значения, потому что их не существовало в словаре на момент прочтения, так как они уже были удалены к тому моменту задачей на удаление элементов;
- Метод ```TryGetValue()``` не смог обновить некоторые значения, потому что их не существовало в словаре на момент прочтения, так как они уже были удалены к тому моменту задачей на удаление элементов.

## **03 Метод ```AddOrUpdate()``` класса ```ConcurrentDictionary<TKey, TValue>```**
---
Метод ```AddOrUpdate()``` – используется для добавления или изменения значения в коллекции. Если ключ существует, то значение будет изменено, если нет – будет добавлено по ключу указанное значение. Метод возвращает новое значение, которое теперь находится по указанному ключу.

Для обновления элемента необходимо передать функцию, которая вычислит и вернет новое значение:

```public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)```

Если для добавления нового значения требуются вычисления – есть специальные перегрузки, которые позволяют передать функцию. 

Также, если требуется, можно передать дополнительное значение:

```cs
public TValue AddOrUpdate(TKey key,
                            Func<TKey, TValue> addValueFactory,
                            Func<TKey, TValue, TValue> updateValueFactory)
```

```cs
public TValue AddOrUpdate(TKey key,
                            Func<TKey, TArg, TValue> addValueFactory,
                            Func<TKey, TValue, TArg, TValue> updateValueFactory,
                            TArg factoryArgument)
```

Пример использования метода ```AddOrUpdate()```:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        ConcurrentDictionary<int, string> dictionary = new();

        Task fillingTask1 = FillConcurrentDictionaryAsync(dictionary, 0, 5);
        Task fillingTask2 = FillConcurrentDictionaryAsync(dictionary, 5, 5);

        dictionary[10] = $"Text value of key <{5}>.";

        dictionary[0] = $"Text value of key <{0}> (CHANGED).";
        dictionary[5] = $"Text value of key <{5}> (CHANGED).";

        await Task.WhenAll(fillingTask1, fillingTask2);
        await Console.Out.WriteLineAsync($"All the Filling Tasks were completed successfully.{Environment.NewLine}");

        Task removingTask = Task.Run(() =>
        {
            for (int i = 0; i < dictionary.Count; i++)
            {
                RemoveConcurrentDictionaryElement(dictionary, i);
            }
        });

        await Task.Delay(300);

        Task readingTask1 = ReadEnumeratorConcurrentDictionaryAsync(dictionary);
        Task readingTask2 = Task.Run(() =>
        {
            int dictionaryLength = dictionary.Count;

            for (int i = 0; i < dictionaryLength; i++)
            {
                if (dictionary.TryGetValue(i, out string capturedValue))
                {
                    Console.WriteLine($">Element [{i}][{capturedValue}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
                }
                else
                {
                    Console.WriteLine($"!Element with Key [{i}] cannot be read from [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
                }

                dictionary.AddOrUpdate(i, $"{capturedValue} (RECOVERED).", (_, valueToUpdate) => $"{valueToUpdate} (UPDATED).");

                dictionary.TryGetValue(i, out string addedOrUpdatedValue);

                if (addedOrUpdatedValue.EndsWith(" (RECOVERED)."))
                {
                    Console.WriteLine($"++Element [{i}][{addedOrUpdatedValue}] was added (recovered) in [{nameof(ConcurrentDictionary<int, string>)}].");
                }
                else
                {
                    Console.WriteLine($"^^Element [{i}][{addedOrUpdatedValue}] was updated in [{nameof(ConcurrentDictionary<int, string>)}].");
                }

                if (i > 3)
                {
                    Thread.Sleep(100);
                }
            }
        });

        await Task.WhenAll(removingTask, readingTask1, readingTask2);
        await Console.Out.WriteLineAsync("All the Reading and Removing Tasks were completed successfully.{Environment.NewLine}");

        dictionary.Clear();
        await Console.Out.WriteLineAsync($"Cleanup Operation was completed successfully. Current Dictionary size is [{dictionary.Count}].{Environment.NewLine}");
    }

    private static Task FillConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
    {
        return Task.Run(() => FillConcurrentDictionary(dictionary, startIndex, elementsNumber));
    }

    private static void FillConcurrentDictionary(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
    {
        for (int i = startIndex; i < startIndex + elementsNumber; i++)
        {
            int key = i;
            string value = $"Text value of key <{i}>.";

            if (dictionary.TryAdd(key, value))
            {
                Console.WriteLine($"+Element [{key}][{value}] was added to [{nameof(ConcurrentDictionary<int, string>)}].");
            }
            else
            {
                Console.WriteLine($"!Element with Key [{key}] cannot be added to [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
            }

            Thread.Sleep(100);
        }
    }

    private static Task ReadEnumeratorConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary)
    {
        return Task.Run(() =>
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($"+Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
                Thread.Sleep(200);
            }
        });
    }

    private static void ReadEnumeratorConcurrentDictionary(ConcurrentDictionary<int, string> dictionary)
    {
        foreach (var item in dictionary)
        {
            Console.WriteLine($">Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
            Thread.Sleep(200);
        }
    }

    private static void RemoveConcurrentDictionaryElement(ConcurrentDictionary<int, string> dictionary, int key)
    {
        if (dictionary.TryRemove(key, out string value))
        {
            Console.WriteLine($"-Element [{key}][{value}] was removed from [{nameof(ConcurrentDictionary<int, string>)}].");
        }
        else
        {
            Console.WriteLine($"!Element with Key [{key}] cannot be removed from [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
        }

        Thread.Sleep(100);
    }
}
```
Из примера видно, что, метод ```AddOrUpdate()``` пытался найти в словаре элемент по ключу для обновления его значения, и, если не находил, то добавлял его в словарь с соответствующим ключём.

## **04 Метод ```GetOrAdd()``` класса ```ConcurrentDictionary<TKey, TValue>```**
---
Метод ```GetOrAdd()``` – используется для получения значения из коллекции по ключу. Но если такого ключа там не будет, метод добавит новое указанное значение, вернув его из метода.

```public TValue GetOrAdd(TKey key, TValue value)```

Существуют дополнительные перегрузки, когда для добавления нового значения требуется его вычислять с помощью функции.

```public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)```

Функция может быть достаточно сложной, требуя дополнительного входящего параметра. Для этого существует еще одна дополнительная перегрузка.

```public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)```

Пример использования метода ```GetOrAdd()```:

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        ConcurrentDictionary<int, string> dictionary = new();

        Task fillingTask1 = FillConcurrentDictionaryAsync(dictionary, 0, 5);
        Task fillingTask2 = FillConcurrentDictionaryAsync(dictionary, 5, 5);

        dictionary[10] = $"Text value of key <{5}>.";

        dictionary[0] = $"Text value of key <{0}> (CHANGED).";
        dictionary[5] = $"Text value of key <{5}> (CHANGED).";

        await Task.WhenAll(fillingTask1, fillingTask2);
        await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Filling Tasks were completed successfully.{Environment.NewLine}");

        Task removingTask = Task.Run(() =>
        {
            for (int i = 0; i < dictionary.Count; i++)
            {
                RemoveConcurrentDictionaryElement(dictionary, i);
            }
        });

        await Task.Delay(300);

        Task readingTask1 = ReadEnumeratorConcurrentDictionaryAsync(dictionary);
        Task readingTask2 = Task.Run(() =>
        {
            int dictionaryLength = dictionary.Count;

            for (int i = 0; i < dictionaryLength; i++)
            {
                string gottenOrAddedValue = dictionary.GetOrAdd(i, (_) => $"Text value of key <{i}>. (ADDED).");

                if (gottenOrAddedValue.EndsWith(" (ADDED)."))
                {
                    Console.WriteLine($"++Element [{i}][{gottenOrAddedValue}] was added (recovered) in [{nameof(ConcurrentDictionary<int, string>)}].");
                }
                else
                {
                    Console.WriteLine($">>Element [{i}][{gottenOrAddedValue}] was gotten in [{nameof(ConcurrentDictionary<int, string>)}].");
                }

                if (dictionary.TryUpdate(i, $"{gottenOrAddedValue} (UPDATED).", gottenOrAddedValue))
                {
                    Console.WriteLine($"^Element [{i}][{gottenOrAddedValue}] was updated in [{nameof(ConcurrentDictionary<int, string>)}].");
                }
                else
                {
                    Console.WriteLine($"!Element with Key [{i}] cannot be updated in [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
                }

                if (i > 3)
                {
                    Thread.Sleep(100);
                }
            }
        });

        await Task.WhenAll(removingTask, readingTask1, readingTask2);
        await Console.Out.WriteLineAsync($"{Environment.NewLine}All the Reading and Removing Tasks were completed successfully.");

        dictionary.Clear();
        await Console.Out.WriteLineAsync($"{Environment.NewLine}Cleanup Operation was completed successfully. Current Dictionary size is [{dictionary.Count}].");
    }

    private static Task FillConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
    {
        return Task.Run(() => FillConcurrentDictionary(dictionary, startIndex, elementsNumber));
    }

    private static void FillConcurrentDictionary(ConcurrentDictionary<int, string> dictionary, int startIndex, int elementsNumber)
    {
        for (int i = startIndex; i < startIndex + elementsNumber; i++)
        {
            int key = i;
            string value = $"Text value of key <{i}>.";

            if (dictionary.TryAdd(key, value))
            {
                Console.WriteLine($"+Element [{key}][{value}] was added to [{nameof(ConcurrentDictionary<int, string>)}].");
            }
            else
            {
                Console.WriteLine($"!Element with Key [{key}] cannot be added to [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
            }

            Thread.Sleep(100);
        }
    }

    private static Task ReadEnumeratorConcurrentDictionaryAsync(ConcurrentDictionary<int, string> dictionary)
    {
        return Task.Run(() =>
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($"+Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
                Thread.Sleep(200);
            }
        });
    }

    private static void ReadEnumeratorConcurrentDictionary(ConcurrentDictionary<int, string> dictionary)
    {
        foreach (var item in dictionary)
        {
            Console.WriteLine($">Element [{item.Key}][{item.Value}] was read from [{nameof(ConcurrentDictionary<int, string>)}].");
            Thread.Sleep(200);
        }
    }

    private static void RemoveConcurrentDictionaryElement(ConcurrentDictionary<int, string> dictionary, int key)
    {
        if (dictionary.TryRemove(key, out string value))
        {
            Console.WriteLine($"-Element [{key}][{value}] was removed from [{nameof(ConcurrentDictionary<int, string>)}].");
        }
        else
        {
            Console.WriteLine($"!Element with Key [{key}] cannot be removed from [{nameof(ConcurrentDictionary<int, string>)}] for a some reason.");
        }

        Thread.Sleep(100);
    }
}
```

Из примера видно, что, метод ```GetOrAdd()``` пытался найти в словаре элемент по ключу для возвращения его значения, и, если не находил, то добавлял его в словарь с соответствующим ключём.

### **Рекомендации по использованию "всегда-выполняемых" методов**

Методы ```AddOrUpdate()/GetOrAdd()``` имеют в качестве параметров делегаты:

- Когда вы передаете метод или лямбда выражение в параметр на делегат, то тело метода или лямбда выражения должно быть как можно проще и короче. Ведь вы должны понимать, что там может произойти блокировка и чем дольше будет выполняться ваш делегат, тем дольше могут ждать другие потоки.
- Ни в коем случае методы или лямбда выражения, передаваемые в параметр делегата, не должны выбрасывать исключения. Потому что «всегда-выполняемые» методы выбросят исключение вам в точку их вызова.

### Внутреннее устройство ```ConcurrentDictionary<TKey, TValue>```###

Хранилище ```ConcurrentDictionary``` состоит из так называемых ведер **(buckets)**. Каждый bucket в ```ConcurrentDictionary``` представлен экземпляром класса Node.

Класс ```Node``` представляет собой однонаправленный связный список. Он хранит в себе: **ключ**, **значение**, **эш-код ключа** и **ссылку на следующий экземпляр класса ```Node```**.

### **Группировка элементов по bucket-ам и нахождение объекта блокировки**

Элемент добавляется в ```ConcurrentDictionary``` на основе хэш-кода передаваемого ключа. Но все элементы группируются в один bucket по специальному алгоритму метода ```GetBucketAndLockNo```. Этот же метод указывает какой объект блокировки будет использоваться для работы с элементами группы.

```cs
private void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
{
    bucketNo = (hashcode & int.MaxValue) % bucketCount;
    lockNo = bucketNo % lockCount;
}
```

> TODO: изображение GIF с алгоритмом работы ```ConcurrentDictionary<TKey, TValue>```

### **Настройка экземпляра ```ConcurrentDictionary<TKey, TValue>```**

При создании, вы можете сконфигурировать экземпляр ```ConcurrentDictionary``` с помощью выбора необходимой перегрузки конструктора.

Перегрузки:

- ```public ConcurrentDictionary()```;
- ```public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)```;
- ```public ConcurrentDictionary(IEqualityComparer<TKey> comparer)```;
- ```public ConcurrentDictionary(int concurrencyLevel, int capacity)```;
- ```public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)```;
- ```public ConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)```;
- ```public ConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)```;

Параметры:
- ```IEnumerable<KeyValuePair<TKey, TValue>>``` – позволяет создать коллекцию ConcurrentDictionary на основе другого словаря, передав из него элементы;
- ```IEqualityComparer<TKey>``` – позволяет указать свой тип, который сравнивает ключи необходимым вам образом;
- ```int concurrencyLevel``` – позволяет указать уровень параллелизма. Он влияет на начальное количество объектов для блокировки;
- ```int capacity``` – позволяет указать начальное количество элементов словаря. Он влияет на начальное количество создаваемых bucket-ов.

### **Способы получения значения из ```ConcurrentDictionary<TKey, TValue>```**

- **Индексатор** – если есть 100% уверенность, что элемент есть в коллекции. 
    
    ```double value = consumables["Some Key"]```;

- **Метод ```TryGetValue()```** – если нет уверенности, что элемент есть в коллекции, при этом вы не хотите добавлять его при отсутствии.

    ```bool isSuccess = someValue```.```TruGetValue("Some Key", out double value)```;

- **Метод ```GetOrAdd()```** – если нет уверенности, что элемент есть в коллекции, при этом вы хотите добавлять его при отсутствии.

    ```double value = movieRating.GetOrAdd("Some Key" , 0);```
