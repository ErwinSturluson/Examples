# **Блокировки в асинхронном коде**

**Блокировка (взаимная блокировка)** – ситуация, когда **несколько потоков** ожидают ресурсы, занятые друг другом, что не дает продолжать выполнение. Такая ситуация будет повторяться до тех пор, пока логика работы с разделяемыми ресурсами в многопоточной среде не будет переписана.

Возможна ситуация, когда в блокировке участвует **один поток** – он сам себя ждет и никто не дает ему сигнал для продолжения работы.

## **01 Взаимная блокировка множества потоков**
---
Взаимная блокировка множества потоков происходит тогда, когда два и более потоков блокируют разные разделяемые ресурсы, а после этого, не освободив эти ресурсы, пытаются завладеть уже ранее заблокироваными друг другом ресурсами, из-за чего уже не могут быть разблокированными и продолжить свою работу:

```cs
internal class Program
{
    private static readonly object _sharedResource1Lock = new object();
    private static readonly object _sharedResource2Lock = new object();

    private static async Task Main(string[] args)
    {
        Task task1 = Task.Run(() =>
        {
            Console.WriteLine("Task1 has started.");

            lock (_sharedResource1Lock)
            {
                Console.WriteLine("  Task1 has locked Shared Resource 1.");
                Thread.Sleep(500);

                lock (_sharedResource2Lock)
                {
                    Console.WriteLine("    Task1 has locked Shared Resource 1.");
                    Thread.Sleep(500);
                    Console.WriteLine("    Task1 has unlocked Shared Resource 1.");
                }

                Console.WriteLine("  Task1 has unlocked Shared Resource 1.");
            }

            Console.WriteLine("Task1 has finished.");
        });

        Task task2 = Task.Run(() =>
        {
            Console.WriteLine("Task2 has started.");

            lock (_sharedResource2Lock)
            {
                Console.WriteLine("  Task2 has locked Shared Resource 1.");
                Thread.Sleep(500);

                lock (_sharedResource1Lock)
                {
                    Console.WriteLine("    Task2 has locked Shared Resource 1.");
                    Thread.Sleep(500);
                    Console.WriteLine("    Task2 has unlocked Shared Resource 1.");
                }

                Console.WriteLine("  Task2 has unlocked Shared Resource 1.");
            }

            Console.WriteLine("Task2 has finished.");
        });

        await Task.WhenAll(task1, task2);

        await Console.Out.WriteLineAsync("All the operations were completed.");
    }
}
```

> TODO: Example with a fix of a Deadlock

## **02 Самостоятельная блокировка одного потока**
---
Самостоятельная блокировка одного потока может возникнуть, например, когда для завершения выполнения задачи необходимо выполнить некоторые действия в контексте вызывающего потоке, например, при использовании контекста синхронизации.

Но, так как вызывающий поток ожидает завершения запущенной задачи, он не может перключиться на выполнение операций, необходимых для её завершения, из-за чего уже не будет разблокирован:

> TODO: WPF example with Deadlock

В данном случае такая блокировка может быть исправлена вызовом метода ```ConfigureAwait()``` с аргументом ```false``` для того, чтобы продолжение не пыталось выполниться с захватом контекста синхронизации в контексте вызывающего и ожидающего его потока.

> TODO: Example with a fix of a Deadlock through ConfigureAwait(false)

Также возможно исправить данную проблему, поместив вызов и ожидание данной задачи в ещё одну задачу, чтобы не был захвачен контекст синхронизации.

> TODO: Example with a fix of a Deadlock through Task.Run(() => OperationAsync().Result).Result

Самым лучшим решение данной проблемы является использование оператора ```await``` и модификация метода модификатором ```async```.

> TODO: Example with a fix of a Deadlock through await OperationAsync()
