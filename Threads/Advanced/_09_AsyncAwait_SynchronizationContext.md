# **Контекст синхронизации в Async Await**

## **01 Использование async await в WPF**
---
Использование ключевых слов ```async await``` упрощает асинхронное программирование в приложениях, написанных по технологии WPF.

Сложность асинхронного кода для приложений WPF всегда была в том, чтобы обращаться к элементам управления из потока пользовательского интерфейса.

Приходилось либо постоянно разными способами передавать данные из одного потока в другой, либо блокировать поток пользовательского интерфейса на время выполнения. Трудностей в передаче данных между потоками не было, но код становился грязным, тяжелым к рассмотрению и изменениям.

Использование ключевых слов ```async await``` делает работу с WPF достаточно простой. Асинхронный код выглядит как синхронный.

**Синхронный обработчик** клика по кнопке, приводящий к блокировке UI потока и зависанию пользовательского интерфейса:
```cs
private void Button_Click(object sender, RoutedEventArgs e)
{
    var operationResult = Operation();
    txtResult.Text = operationResult;
}

```

**Acинхронный обработчик** клика по кнопке, не приводящий к блокировке UI потока и зависанию пользовательского интерфейса:
```cs
private async void Button_Click(object sender, RoutedEventArgs e)
{
    var operationResult = await OperationAsync();
    txtResult.Text = operationResult;
}
```

Пример окна приложения **WPF**, содержащего следующие элементы:
- **TextBlock** с надписью-наименованием прогресс бара;
- **ProgressBar** для отображения отзывчивости пользовательского интерфейса;
- **TextBox1** для отображения результатов работы программы;
- **TextBox2** для отображения результатов работы программы;
- **SyncButton** для демонстрации работы синхронного обработчика клика по кнопке, записывающего результат в **TextBox1**;
- **AsyncButton** для демонстрации работы синхронного обработчика клика по кнопке, записывающего результат в **TextBox2**.

Код примера:

> MainWindow.xaml
```html
<Window x:Class="AsyncAwait.SyncContext._01_WPF.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <TextBox x:Name="TextBox1"  HorizontalAlignment="Left" Margin="65,63,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="280" Height="280" />
        <TextBox x:Name="TextBox2"  HorizontalAlignment="Left" Margin="455,63,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="280" Height="280" />
        <Button Content="SyncButton" HorizontalAlignment="Left" Margin="105,362,0,0" VerticalAlignment="Top" Height="40" Width="200" Click="SyncButton_Click" />
        <Button Content="AsyncButton" HorizontalAlignment="Left" Margin="495,362,0,0" VerticalAlignment="Top" Height="40" Width="200" Click="AsyncButton_Click" />
        <ProgressBar x:Name="ProgressBar" HorizontalAlignment="Center" Height="15" Margin="0,31,0,0" VerticalAlignment="Top" Width="758" />
        <TextBlock HorizontalAlignment="Center" Margin="0,10,0,0" TextWrapping="Wrap" Text="User Interface Responsiveness:" VerticalAlignment="Top" />
    </Grid>
</Window>
```

> MainWindow.xaml.cs
```cs
public partial class MainWindow : Window
{
    private readonly Timer _progressBarTimer;

    public MainWindow()
    {
        InitializeComponent();

        _progressBarTimer = new Timer(TimerCallbackAction, null, 50, 50);
    }

    private void SyncButton_Click(object sender, RoutedEventArgs e)
    {
        IEnumerable<string> iterationList = PrintIterations("Synchronous");

        TextBox1.Text = ConcatenateStringEnumerable(iterationList, Environment.NewLine);
    }

    private async void AsyncButton_Click(object sender, RoutedEventArgs e)
    {
        IEnumerable<string> iterationList = await PrintIterationsAsync("Asynchronous");

        TextBox2.Text = ConcatenateStringEnumerable(iterationList, Environment.NewLine);
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

    private static string ConcatenateStringEnumerable(IEnumerable<string> stringEnumerable, string separator) => stringEnumerable.Aggregate((x, y) => $"{x}{separator}{y}");

    private void TimerCallbackAction(object _) => Dispatcher.BeginInvoke(() => this.ProgressBar.Value = this.ProgressBar.Value != 100 ? ++this.ProgressBar.Value : 0);
}
```

Если попытаться обратиться из вторичного потока, не создававшего элемент управления, к этому элементу управления, то в этом месте будет выброшено исключение ```InvalidOperationException```.

Но с асинхронными методами при обращении к элементам управления из блока кода после оператора ```await```, который становится продолжением асинхронной операции, такого не происходит благодаря контексту синхронизации, который указывает, где и как должно выполниться продолжение асинхронной операции.

В приложениях **WPF** контекст синхронизации представлен классом ```DispatcherSynchronizationContext```.

### **SynchronizationContext**

```SynchronizationContext``` – базовый класс для создания контекстов синхронизации. Контекст синхронизации – абстрактный механизм, который позволяет выполнить код вашего приложения в определенном месте.

Служит для обеспечения единого механизма распространения контекста синхронизации в различных моделях синхронизации.

```SynchronizationContext``` позволяет расширять себя и предоставлять свои собственные реализации методов.

### **Основной функционал базового класса SynchronizationContext**

**Свойства**:
- ```Current``` (static) – отдает контекст синхронизации, прикрепленный к текущему потоку.
Методы:
- ```CreateCopy``` (virtual) – при переопределении в производном классе создает копию контекста синхронизации. Определяет стратегию копирования объекта;
- ```OperationStarted()``` (virtual) – при переопределении в производном классе отвечает за уведомление о начале операции;
- ```OperationCompleted()``` (virtual) – при переопределении в производном классе отвечает за уведомление о завершении операции;
- ```Send()``` (virtual) – при переопределении в производном классе отправляет синхронное сообщение в контекст синхронизации;
- ```Post()``` (virtual) - при переопределении в производном классе отправляет асинхронное сообщение в контекст синхронизации;
- ```SetSynchronizationContext()``` (static) – задает текущий контекст синхронизации.

### **Методы ```Send()``` и ```Post()``` базового класса SynchronizationContext**

Стандартная реализация метода ```Send()```:

```cs
public virtual void Send(SendOrPostCallback callback, object state)
{
    callback(state);
}
```

Стандартная реализация метода ```Post()```:

```cs
public virtual void Post(SendOrPostCallback callback, object state)
{
    ThreadPool.QueueUserWorkItem(new WaitCallback(callback.Invoke, state);
}
```

|```Send()```|```Post()```|
|---|---|
|Производит **синхронную** отправку сообщения|Производит **асинхронную** отправку сообщения|
|**Дожидается** завершения обработки отправленного сообщения|**Не дожидается** завершения обработки отправленного сообщения|

**Рекомендуется** при переопределении методов ```Send()``` и ```Post()``` сохранять критерии, описанные в таблице выше.

### **Использование контекста синхронизации**

Чтобы наделить механизм в приложении собственным контекстом синхронизации, необходимо:
1. Создание своего класса, производного от ```SynchronizationContext```;
2. Создание экземпляра класса ```SynchronizationContext``` или производного от него класса;
3. Регистрация этого контекста синхронизации с помощью метода ```SetSynchronizationContext```.

Когда вы захотите обратится к контексту синхронизации, вы должны запросить его с помощью статического свойства ```Current```. На полученном контексте, с помощью метода ```Post()``` или ```Send()```, вы можете отправить сообщение в контекст синхронизации.

Обычно для определения свобственного контекста синхронизации достаточно только переопределения методов ```Post()``` или ```Send()```, остальные члены переопределяются в зависимости от использования.

Нет чёткой инструкции, руководства, манифеста или правила для определения контекста синхронизации. В разных типах приложений и в разных обстоятельствах определяются свои контексты синхронизации со своей логикой отправки сообщений.

### **Готовые контексты синхронизации**

Существуют готовые контексты синхронизации, которые используются невидимо для разработчика:

- ```WindowsFormsSynchronizationContext``` - используется в приложениях **Windows Forms**. Создан для того, чтобы через него вторичные потоки передавали делегаты на выполнение в поток пользовательского интерфейса. Внутри себя он использует предшественника ```SynchronizationContext``` - интерфейс ```ISynchronizedInvoke``` и его конкретные реализации. Работа ```ISynchronizedInvoke``` заключалась в том, что вторичный поток может поставить делегат в очередь на выполнение в основной поток. При этом, не обязательно даже дожидаться завершения работы этого делегата, что очень напоминает работу метода ```Post()``` в ```SynchronizationContext```. Интерфейс ```ISynchronizedInvoke``` уже считается устаревшим механизмом, так как от него отказались из-за того, что другие типы приложений не могли с ним адекватно взаимодействовать;
- ```DispatcherSynchronizationContext``` - используется в приложениях **WPF**. Создан для того, чтобы через него вторичные потоки передавали делегаты на выполнение в поток пользовательского интерфейса. Делает это с приоритетом ```Default```. Все делегаты, поставленные в очередь контекстом синхронизации, выполняются по одному потоком пользовательского интерфейса в том порядке, в котором они были поставлены в очередь;
- ```AspNetSynchronizationContext``` - используется в приложениях **ASP.NET**. 
- ```SynchronizationContext``` - базовый класс контекста синхронизации. Называется контекстом синхронизации **по умолчанию** или контекстом синхронизации **пула потоков**. 

### **Продолжения оператора await**

Оператор ```await``` невидимо для разработчика занимается планированием, установкой и запуском продолжений.

Оператор ```await``` имеет установленные правила для выполнения своих продолжений. Он выполняет продолжение в
контексте одного из заранее определенных механизмов. **Всегда выбирается только один из них.**

Поиск механизма, который может выполнить продолжение оператора ```await```, идет в следующем порядке (по умолчанию):
- **Контекст синхронизации.** Оператор ```await``` вначале пытается захватить контекст синхронизации. Если он захватил его, то продолжение будет отправлено на выполнение в контекст синхронизации;
- **Планировщик задач.** Оператор ```await``` пытается получить текущий планировщик задач. Если он его получает, то продолжение будет отправлено на выполнение через планировщик задач;
- Если предыдущие варианты не сработали, или если было рекомендовано отказаться от окружения
вызывающего потока, тогда система будет выбирать где его выполнить. Обычно это означает одно из двух:
    - Выполнение продолжения синхронно там, где была завершена ожидаемая задача;
    - Выполнение продолжения в контексте пула потоков (```ThreadPool```).

## **02 Использование async await в Console**
---
Использовать контекст синхронизации с ```async await``` можно не только в настолькых приложениях **WindowsForms**, **WPF** и сетевых приложениях **ASP.NET**, но и в консольных приложениях, если подготовитьб всю необходимую инфраструктуру:

> ConsoleMessage.cs - файл с классом, который будет содержать экземпляр делегата ```SendOrPostCallback``` и его аргумент для выполнения в контексте синхронизации.
```cs
namespace AsyncAwait.SyncContext._02_Console
{
    internal class ConsoleMessage
    {
        public SendOrPostCallback Callback { get; set; }

        public object State { get; set; }

        public ConsoleMessage()
        { }

        public ConsoleMessage(SendOrPostCallback callback, object state)
        {
            Callback = callback;
            State = state;
        }
    }
}
```

> ConsoleMessageListenter.cs - файл с классом, который будет содержать очередь сообщений для запуска в контексте синхронизации, методы для добавления, выполнени и ожидания сообщений.
```cs
namespace AsyncAwait.SyncContext._02_Console
{
    internal class ConsoleMessageListenter
    {
        private static readonly LinkedList<ConsoleMessage> messagesList = new();

        public static void AddMessage(ConsoleMessage message)
        {
            messagesList.AddLast(message);
        }

        public void Listen()
        {
            while (true)
            {
                if (messagesList.Count > 0)
                {
                    ConsoleMessage message = messagesList.First.Value;

                    if (message != null)
                    {
                        messagesList.Remove(message);
                        DispatchMessage(message);
                    }
                }
            }
        }

        private void DispatchMessage(ConsoleMessage message)
        {
            SendOrPostCallback callback = message.Callback;
            object state = message.State;

            try
            {
                callback.Invoke(state);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An Exception was occured.");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.WriteLine($"Exception Message: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
```

> ConsoleSynchronizationContext.cs - файл с классом самого контекста синхронизации в консольном приложении. В нём переопределены методы ```OperationStarted()```, ```OperationCompleted()```, ```Post()``` и ```Send()```.
```cs
namespace AsyncAwait.SyncContext._02_Console
{
    internal class ConsoleSynchronizationContext : SynchronizationContext
    {
        private static readonly object _consoleLock = new();

        public override void OperationStarted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(ConsoleSynchronizationContext.OperationStarted)}");
            Console.ResetColor();
        }

        public override void OperationCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(ConsoleSynchronizationContext.OperationCompleted)}");
            Console.ResetColor();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (_consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{nameof(Post)} method has been executed.");
                    Console.ResetColor();
                }

                ConsoleMessage message = new(d, state);
                ConsoleMessageListenter.AddMessage(message);
                
            }, null);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{nameof(Send)} method has been executed.");
                Console.ResetColor();
            }

            ConsoleMessage message = new(d, state);
            ConsoleMessageListenter.AddMessage(message);
            
        }
    }
}
```

> Program.cs - файл с точкой входа в приложение, установкой собственного контекста синхронизации и запуска ожидания получения сообщений из собственного контекста синхронизации.
```cs
namespace AsyncAwait.SyncContext._02_Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new ConsoleSynchronizationContext());

            ConsoleMessage message1 = new(PrintIterationsWrapperAsync, "AsyncTask1");
            ConsoleMessage message2 = new(PrintIterationsWrapperAsync, "AsyncTask2");
            ConsoleMessageListenter.AddMessage(message1);
            ConsoleMessageListenter.AddMessage(message2);

            ConsoleMessageListenter messageListenter = new();

            messageListenter.Listen();
        }

        private static async void PrintIterationsWrapperAsync(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"+  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsWrapperAsync)}]");

            await PrintIterationsAsync(taskName);

            Console.WriteLine($"-  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsWrapperAsync)}]");
        }

        private static async Task PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;

            Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
}
```

При выполнении можно заметить, что, несмотря на наличие двух операторов ```await``` в каждой цепочке вызовов асинхронных методов ```PrintIterationsWrapperAsync()```, метод ```Post()``` контекста синхронизации выполняется только 1 раз.

Это происходит потому, что при планировании продолжения в асинхронном методе ```PrintIterationsAsync()``` завхватывается контекст синхронизации и продолжение выполняется в нём, а при возвращении управления методу ```PrintIterationsWrapperAsync()```, ожидавшему выполнения метода ```PrintIterationsAsync()```, и планировании его продолжения, был снова захвачен контекст синхронизации и сопоставлен с тем, что был сохранён при создании этого продолжения, где было установлено, что они совпадают, и потому продолжение метода ```PrintIterationsWrapperAsync()``` может выполниться встроенно, синхронно с продолжением метода ```PrintIterationsAsync()``` и не возвращать управление до этого.

Поэтому метод ```Post()``` контекста синхронизации вызывается только 1 раз, при запуске самого первого (и самому низкому по стеку вызовов) продолжения, а остальные (выше по стеку вызовов) продолжения выполняются во встроенном режиме, синхронно вызывающему планирующему их потоку.


## **03 Использование планировщика задач с async await для синхронизации продолжений**
---
sss

> SynchronousTaskScheduler.cs - файл, в котором размещён класс планировщика, выполняющего задачи в независимых потоках
```cs
namespace AsyncAwait.SyncContext._03_.TaskScheduler
{
    internal class IndependentThreadTaskScheduler : System.Threading.Tasks.TaskScheduler
    {
        private static readonly object _consoleLock = new object();

        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{nameof(IndependentThreadTaskScheduler.QueueTask)} was executed for Task#{task.Id} in Thread#{Environment.CurrentManagedThreadId}");
                Console.ResetColor();
            }

            Thread taskThread = new(() => TryExecuteTask(task))
            {
                IsBackground = true
            };

            taskThread.Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{nameof(IndependentThreadTaskScheduler.TryExecuteTaskInline)} was executed for Task#{task.Id} in Thread#{Environment.CurrentManagedThreadId}");
                Console.ResetColor();
            }

            return TryExecuteTask(task);
        }
    }
}
```

> Program.cs - файл с точкой входа в приложение, созданием и запуском задач в контексте планировщика задач ```SynchronousTaskScheduler```.
```cs
namespace AsyncAwait.SyncContext._03_.TaskScheduler
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Task<Task> task1 = new(PrintIterationsAsync, "AsyncTask1");
            Task<Task> task2 = new(PrintIterationsAsync, "AsyncTask2");

            task1.Start(new IndependentThreadTaskScheduler());
            task2.Start(new IndependentThreadTaskScheduler());

            await await task1;
            await await task2;
        }

        private static async Task PrintIterationsAsync(object state)
        {
            string taskName = state.ToString();

            Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;

            Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
}
```

При выполнении можно заметить, что, несмотря на наличие двух операторов ```await``` в цепочке вызовов асинхронных методов ```PrintIterationsAsync()```, для выполнения продолжения асинхронного метода выше по стеку  был вызван метод ```TryExecuteTaskInline()```.

Это происходит потому, что при планировании продолжения в асинхронном методе ```PrintIterationsAsync()``` завхватывается планировщик задач ```IndependentThreadTaskScheduler``` и продолжение выполняется в нём, а при возвращении управления методу, сообщённому с задачей ```task```, ожидавшему выполнения метода ```PrintIterationsAsync()```, и планировании его продолжения, был снова захвачен планировщик задач из своего окружения, потому что вложенные и дочерние задачи наследуют в своё окружение планировщик родительской задачи, и потому продолжение метода, сообщённого с задачей ```task```, может выполниться встроенно, синхронно с продолжением метода ```PrintIterationsAsync()``` и не возвращать управление до этого.

Несмотря на то, что планировщик задач был передан при запуске задач из метода ```Main()```, его унаследовали также и вложенные задачи, а для самого метода ```Main()``` был установлен планировщик задач по умолчанию: ```ThreadPoolTaskScheduler```, поэтому продолжение асинхронного метода ```Main()``` было выполнено в контексте потока из пула потоков.

## **04 Использование планировщика задач с async await для синхронизации продолжений. TaskCreationOptions.HideScheduler**
---
Для того, чтобы скрыть переданный родительской задаче планировщик от дочерних и вложенных задач, необходимо при создании задачи передать в параметр ```TaskCreationOptions creationOptions``` аргумент ```TaskCreationOptions.HideScheduler```:

> IndependentThreadTaskScheduler.cs - файл содержит программный код, идентичный коду из примера под номером **03**.

> Program.cs
```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        Task<Task> task = new(PrintIterationsAsync, "AsyncTask", TaskCreationOptions.HideScheduler);

        PrintCurrentTaskSchedulerName(nameof(Main));
        task.Start(new IndependentThreadTaskScheduler());

        await await task;

        PrintCurrentTaskSchedulerName(nameof(Main));
    }

    private static async Task PrintIterationsAsync(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        PrintCurrentTaskSchedulerName(nameof(PrintIterationsAsync));

        printIterationsTask.Start();

        await printIterationsTask;

        PrintCurrentTaskSchedulerName(nameof(PrintIterationsAsync));

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        PrintCurrentTaskSchedulerName(nameof(PrintIterations));

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void PrintCurrentTaskSchedulerName(string methodName) =>
        Console.WriteLine($">>>Method [{methodName}] was executed in [{System.Threading.Tasks.TaskScheduler.Current.GetType().Name}].");
}
```

Тем самым был исключён вариант, когда вложенные и дочерние задачи захватывают планировщик задач родительской задачи, им не нужный.

> TODO: example
Если передать этот параметр в конструктор класса ```Task``` дочерней или вложенной задачи и явно при запуске не передать нужный планировщик, то она уже не захватит планировщик родительской задачи.

Метод ```Task.Run()``` всегда внутри себя использует планировщик по умолчанию ```ThreadPoolTaskScheduler```.


### **Конфигурирование оператора ```await```. Управление ожиданием**

Есть открытый API для управления ожиданием. Для управления ожиданием используется экземплярный метод ```ConfigureAwait(bool continueOnCapturedContext)``` класса ```Task```.

Метод позволяет вам порекомендовать системе, нужно ли выполняться продолжениям в захваченном **контексте синхронизации** или **планировщике задач**.

Переданное значение **true** в параметр ```continueOnCapturedContext``` говорит о том, что нужно разрешить
выполнение продолжения в захваченном контексте синхронизации или планировщике задач.

Переданное значение **false** в параметр ```continueOnCapturedContext``` говорит о том, что нужно запретить
выполнение продолжения в захваченном контексте синхронизации или планировщике задач.

Если этот метод не будет явно вызван с переданным значением **false**, то по умолчанию оператор ```await``` всегда будет пытаться захватить контекст синхронизации и выполнить продолжение в нем. Точно также, как если он будет вызван явно с переданным значением **true**.

Вызов метода ```ConfigureAwait()``` с передачей значения **true** может быть выполнена для того, чтобы переконфигурировать задачу на поведение по-умолчанию.

### **Метод ConfigureAwait()**

Метод ```ConfigureAwait()``` возвращает структуру ```ConfiguredTaskAwaiter``` или ```ConfiguredTaskAwaiter<TResult>```, которая имеет весь необходимый функционал по работе с оператором ```await```.

Структура занимается конфигурированиемоператора ```await``` на выполнение продолжения указанным вами способом.

```ConfiguredTaskAwaiter```:
```cs
public struct ConfiguredTaskAwaitable
{
    public ConfiguredTaskAwaiter GetAwaiter();

    public struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted { get; }
        public void GetResult();
        public void OnCompleted(Action continuation);
        public void UnsafeOnCompleted(Action continuation);
    }
}
```

```ConfiguredTaskAwaiter<TResult>```:
```cs
public struct ConfiguredTaskAwaitable<TResult>
{
    public ConfiguredTaskAwaiter GetAwaiter();

    public struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted { get; }
        public TResult GetResult();
        public void OnCompleted(Action continuation);
        public void UnsafeOnCompleted(Action continuation);
    }
}
```

Если на момент применения оператора ```await``` асинхронная задача уже завершена, то код продолжит выполняться в том же потоке синхронно. В таком случае вызов метода ```ConfigureAwait(false)``` будет бесполезен. Поэтому, метод ```ConfigureAwait()``` с указанием значения **false** не гарантирует, что код после него не будет выполнен в оригинальном контексте синхронизации или планировщике задач.

### **Продолжения оператора ```await```**

По завершению ожидаемой задачи, среда выполнения будет запускать продолжение. Но перед этим она может проверить текущий контекст в возобновляющем потоке для определения возможности синхронного запуска продолжения. Если будет получен отказ, то продолжение будет выполнено асинхронно запланированным способом при его создании, как и подразумевалось.

Эта проверка может выполниться независимо от указаний метода ```ConfigureAwait()```.

## **05 Метод ConfigureAwait() и контекст синхронизации**
---
Пример использования метода ```ConfigureAwait()``` для прекращения распространения контекста синхронизации на вложенные и дочерние задачи.

Ниже приведён модифицированный код примера под номером **02**:

> ConsoleMessage.cs - файл остался без изменений.
 
> ConsoleMessageListenter.cs - файл остался без изменений.

> ConsoleSynchronizationContext.cs - файл остался без изменений.

> Program.cs - был добавлен вызов метода ```ConfigureAwait()``` на ожидаемой задаче в методе ```PrintIterationsAsync()```.
```cs
internal class Program
{
    private static void Main(string[] args)
    {
        SynchronizationContext.SetSynchronizationContext(new ConsoleSynchronizationContext());

        ConsoleMessage message1 = new(PrintIterationsWrapperAsync, "AsyncTask1");
        ConsoleMessage message2 = new(PrintIterationsWrapperAsync, "AsyncTask2");
        ConsoleMessageListenter.AddMessage(message1);
        ConsoleMessageListenter.AddMessage(message2);

        ConsoleMessageListenter messageListenter = new();

        messageListenter.Listen();
    }

    private static async void PrintIterationsWrapperAsync(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsWrapperAsync)}]");

        await PrintIterationsAsync(taskName).ConfigureAwait(false);

        Console.WriteLine($"-  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsWrapperAsync)}]");
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

## **06 Метод ConfigureAwait() и планироващик задач**
---
Пример использования метода ```ConfigureAwait()``` для прекращения распространения планировщика потоков на вложенные и дочерние задачи.

Ниже приведён модифицированный код примера под номером **03**:

> IndependentThreadTaskScheduler.cs - файл содержит программный код, идентичный коду из примера под номером **03**.

> Program.cs

```cs
internal class Program
{
    private static async Task Main(string[] args)
    {
        Task<Task> task = new(PrintIterationsAsync, "AsyncTask");

        PrintCurrentTaskSchedulerName(nameof(Main));
        task.Start(new IndependentThreadTaskScheduler());

        await await task;

        PrintCurrentTaskSchedulerName(nameof(Main));
    }

    private static async Task PrintIterationsAsync(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        PrintCurrentTaskSchedulerName(nameof(PrintIterationsAsync));

        printIterationsTask.Start();

        await printIterationsTask.ConfigureAwait(false);

        PrintCurrentTaskSchedulerName(nameof(PrintIterationsAsync));

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        PrintCurrentTaskSchedulerName(nameof(PrintIterations));

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }

    private static void PrintCurrentTaskSchedulerName(string methodName) =>
        Console.WriteLine($">>>Method [{methodName}] was executed in [{System.Threading.Tasks.TaskScheduler.Current.GetType().Name}].");
}
```

Таким образом, изменить поведение выполнения продолжения можно как с помощью установки или восстановления контекста синхронизации или планировщика задач, так и с помощью вызова метода ```ConfigureAwait()``` на объекте ожидания для сброса установленных ранее настроек и использования синхронизации по умолчанию.

### **ExecutionContext**

```ExecutionContext``` (контекст выполнения) – это объект, который представляет собой контейнер для хранения информации потока выполнения. В **.NET** он хранит в себе другие контексты, к примеру, ```SecurityContext```, ```SynchronizationContext```, ```HostExecutionContext``` и другие.

С помощью ```ExecutionContext``` можно захватить состояние одного потока и восстановить его в другом.

Контекст выполнения в ```async await``` захватывается строителями асинхронных методов, если он не был подавлен до их работы.

## **07 Модификатор async для асинхронных методов void**
---
Для асинхронных методов с возвращаемым значением ```void``` существует потенциальное взаимодействие с контекстом синхронизации. Взаимодействие описано внутри строителя асинхронных методов ```AsyncVoidMethodBuilder```.

Если контекст синхронизации будет захвачен, произойдет следующее:
- При создании строителя ```AsyncVoidMethodBuilder``` будет захвачен контекст синхронизации и если он не будет **null**, то среда вызовет метод ```OperationStarted``` на захваченном контексте;
- Если конечный автомат завершает работу с необработанным исключением (ловит его методом ```SetException```), то исключение будет проброшено в захваченный контекст синхронизации;
- По завершению работы конечного автомата (успешном или провальном) будет вызван метод ```OperationCompleted``` на захваченном контексте синхронизации;

**Если контекст синхронизации не будет захвачен**, то вызовов методов ```OperationStarted``` и ```OperationCompleted``` не будет. Возникшее необработанное исключение, в свою очередь, будет выброшено через ```ThreadPool```, что будет означать завершение приложения.

Асинхронные методы с возвращаемым типом ```void``` не являются ожидаемыми ```(awaitable)```.

Методы ```OperationStarted()``` и ```OperationCompleted()``` выполняются потому, что возвращаемый тип асинхронного метода - ```void```:


> VoidSynchronizationContext.cs:
```cs
namespace AsyncAwait.SyncContext._07_Void
{
    internal class VoidSynchronizationContext : SynchronizationContext
    {
        private static readonly object _consoleLock = new();

        public override void OperationStarted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(VoidSynchronizationContext.OperationStarted)}");
            Console.ResetColor();
        }

        public override void OperationCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(VoidSynchronizationContext.OperationCompleted)}");
            Console.ResetColor();
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (_consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{nameof(Post)} method has been executed.");
                    Console.ResetColor();
                }

                callback(state);
            }, null);
        }
    }
}
```

> Program.cs
```cs
namespace AsyncAwait.SyncContext._07_Void
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new VoidSynchronizationContext());

            PrintIterationsAsync("AsyncTask");

            Console.ReadKey();
        }

        private static async void PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;

            Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
}
```

## **08 Исключения в асинхронных методах void**
---
Так как у асинхронных методов с возвращаемым типом ```void``` нет задачи-марионетки, в которому можно установить полученное в конечном автомате исключение, то исключение либо выбрасывается в пуле потоков, либо, если удалось захватить контекст синхронизации, в контексте синхронизации, что позволяет сохранить работоспособность приложения даже при возбуждении исключения в асинхронном методе ```void```:

> VoidException.cs - файл с классом пользовательского исключения.
```cs
namespace AsyncAwait.SyncContext._08_Void.Exception
{
    internal class VoidException : System.Exception
    {
        public VoidException(string message) : base(message)
        {
        }
    }
}
```

> VoidSynchronizationContext.cs - файл с классом контекста синхронизации ```VoidSynchronizationContext```, который при запуске делегата в методе ```Post()``` в контексте пула потоков также обрабатывает выбрасываемые делегатом исключения.
```cs
namespace AsyncAwait.SyncContext._08_Void.Exception
{
    internal class VoidSynchronizationContext : SynchronizationContext
    {
        private static readonly object _consoleLock = new();

        public override void OperationStarted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(VoidSynchronizationContext.OperationStarted)}");
            Console.ResetColor();
        }

        public override void OperationCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{nameof(VoidSynchronizationContext.OperationCompleted)}");
            Console.ResetColor();
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (_consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{nameof(Post)} method has been executed.");
                    Console.ResetColor();
                }

                try
                {
                    callback(state);
                }
                catch (System.Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An Exception was occured.");
                    Console.WriteLine($"Exception type: {ex.GetType().Name}");
                    Console.WriteLine($"Exception Message: {ex.Message}");
                    Console.ResetColor();
                }
            }, null);
        }
    }
}
```

> Program.cs
```cs
namespace AsyncAwait.SyncContext._08_Void.Exception
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new VoidSynchronizationContext());

            PrintIterationsAsync("AsyncTask");

            Console.ReadKey();
        }

        private static async void PrintIterationsAsync(string taskName)
        {
            Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

            Task printIterationsTask = new(PrintIterations, taskName);

            printIterationsTask.Start();

            await printIterationsTask;

            Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
        }

        private static void PrintIterations(object state)
        {
            string callName = state.ToString();

            Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

            int iterationIndex = 0;

            while (iterationIndex < 10)
            {
                iterationIndex++;

                Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
                Thread.Sleep(100);
            }

            throw new VoidException("An error was occured while executing an operation.");

            //Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
        }
    }
}
```

## **09 Асинхронные Лямбда выражения**
---
Кроме асинхронных методов существуют также и асинхронные **анонимные методы**, **лямба-выражения** и **лямбда-операторы**. На них накладываются все правила и ограничения асинхронных методов.

Чтобы сделать лямбда выражение асинхронным, необходимо добавить модификатор async перед указанием формальных параметров лямбда выражения:
```cs
async() => await TaskAsync();
```

Если асинхронное лямбда-выражение создано в контексте асинхронного метода, то у этого асинхронного лямбда-выражения должен быть свой модификатор ```async```, иначе оно не сможет использовать оператор ```await``` в своём теле:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        PrintIterationsWrapperAsync("AsyncTask");

        Console.ReadKey();
    }

    private static async Task PrintIterationsWrapperAsync(object state)
    {
        string taskName = state.ToString();

        Console.WriteLine($"+  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsWrapperAsync)}]");

        Func<Task> asyncLambda = async () =>
        {
            Console.WriteLine($"!  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(asyncLambda)}]");

            await PrintIterationsAsync(taskName);

            Console.WriteLine($"!  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(asyncLambda)}]");
        };

        Console.WriteLine($"-  {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsWrapperAsync)}]");
    }

    private static async Task PrintIterationsAsync(string taskName)
    {
        Console.WriteLine($"++ {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterationsAsync)}]");

        Task printIterationsTask = new(PrintIterations, taskName);

        printIterationsTask.Start();

        await printIterationsTask;

        Console.WriteLine($"-- {taskName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterationsAsync)}]");
    }

    private static void PrintIterations(object state)
    {
        string callName = state.ToString();

        Console.WriteLine($"+++{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Started:[{nameof(PrintIterations)}]");

        int iterationIndex = 0;

        while (iterationIndex < 10)
        {
            iterationIndex++;

            Console.WriteLine($">>>{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]");
            Thread.Sleep(100);
        }

        Console.WriteLine($"---{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - Finished:[{nameof(PrintIterations)}]");
    }
}
```

Неправильный пример создания асинхронного лямбда-выражения в методе ```PrintIterationsWrapperAsync()``` с отсутствующим модификатором ```async``` в самом лямбда-выражении:
```cs
private static async Task PrintIterationsWrapperAsync(object state)
    {
        string taskName = state.ToString();

        Func<Task> asyncLambda = () =>
        {
            await PrintIterationsAsync(taskName);
        };
    }
```

### **Делегаты с возвращаемыми значениями void и асинхронные лямбда-выражения**

Необходимо быть аккуратными при работе с асинхронными лямбда-выражениями. Вы можете не заметив использовать асинхронное лямбда-выражение с типом возвращаемого значения ```void```.

Из-за этого, вы можете потерять преимущества использования задач (ожидание, результат, статус, отлов исключения) и получить непредсказуемый проброс исключения, способный привести к непредвиденному завершению программы:

```cs
Action action = async () =>
{
    await Task.Run();
}

private async void LambdaVoid()
{
    await Task.Run();
}
```

```cs
Action action = async () =>
{
    await Task.Run();
}

private async Task LambdaTask()
{
    await Task.Run();
}
```

> TODO: Code example with an async void lambda
