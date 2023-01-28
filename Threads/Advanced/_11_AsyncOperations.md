# **Асинхронные операции**

### **Случаи применения асинхронного программирования**

**Асинхронность** может применятся в следующих случаях:
- **Операции CPU**:
    - Параллельное/неблокирующее/фоновое выполнение;
    - Распараллеливание операции.
- **Операции ввода-вывода**:
    - Работа с файловой системой;
    - Работа с сетью (с удаленной базой данных, веб-сервисами);

### **Асинхронные CPU операции**

**CPU операции** – это операции, которые выполняются ресурсами центрального процессора. Эти операции представлены обыкновенными синхронными методами, которые иногда необходимо вызывать асинхронно.

Причины вызова таких методов асинхронно, в контексте вторичного потока:
- Блокирование основного потока на время своего выполнения;
- Фоновое выполнение;
- Параллельное выполнение - логическое разделение операции на несколько потоков и последующие соединение их результатов.

Для запуска синхронного метода асинхронно необходимо воспользоваться статическим методом ```Task.Run()```.

Асинхронные CPU операции **используют многопоточность**, чтобы в контексте вторичных потоков выполнять необходимые операции, не блокируя основной/вызывающий поток. Такие операции зависят от ресурсов центрального процессора.

**Асинхронный вызов метода (CPU 1 kernel):**
> TODO: изображение со схемой выполнения двух операций одновременно процессором с 1 ядром

**Асинхронный вызов метода (CPU 2 kernel):**
> TODO: изображение со схемой выполнения двух операций одновременно процессором с 2 ядрами

## **01 Асинхронные операции CPU**
---
Для демонстрации асинхронных операций CPU, а также преимущества параллельных вычислений, создадим проект по шаблону **WPF Application** с названием **AsyncOperations._01_CPU** и внесём следующие изменения в файлы:

- В файле **MainWindow.xaml** добавляем в пользовательский интерфейс элементы:
    - TextBlock для отображения статичного текста;
    - 2x TextBox для отображения вывода результатов операций;
    - 4x Button для запуска синхронной, асинхронной, синхронной распараллеленной и асинхронной распараллеленной операций.

> MainWindow.xaml
```html
<Window x:Class="AsyncOperations._01_CPU.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        mc:Ignorable="d"
        Title="MainWindow" Height="520" Width="800">
    <Grid Margin="0,0,0,5">
        <TextBox x:Name="TextBox1"  HorizontalAlignment="Left" Margin="65,63,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="280" Height="280" />
        <TextBox x:Name="TextBox2"  HorizontalAlignment="Left" Margin="455,63,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="280" Height="280" />
        <Button Content="SyncButton" HorizontalAlignment="Left" Margin="105,362,0,0" VerticalAlignment="Top" Height="40" Width="200" Click="SyncButton_Click" />
        <Button Content="AsyncButton" HorizontalAlignment="Left" Margin="495,362,0,0" VerticalAlignment="Top" Height="40" Width="200" Click="AsyncButton_Click" />
        <ProgressBar x:Name="ProgressBar" HorizontalAlignment="Center" Height="15" Margin="0,31,0,0" VerticalAlignment="Top" Width="758" />
        <TextBlock HorizontalAlignment="Center" Margin="0,10,0,0" TextWrapping="Wrap" Text="User Interface Responsiveness:" VerticalAlignment="Top" />
        <Button Content="SyncParallelButton" HorizontalAlignment="Left" Margin="105,422,0,0" VerticalAlignment="Top" Height="40" Width="200" Click="SyncParallelButton_Click" />
        <Button Content="AsyncParallelButton" HorizontalAlignment="Left" Margin="495,422,0,0" VerticalAlignment="Top" Height="40" Width="200" Click="AsyncParallelButton_Click" />
    </Grid>
</Window>
```

В файле **MainWindow.xaml.cs** создаём необходимые методы логики и методы-обработчики событий кликов по кнопкам:

```cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncOperations._01_CPU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            Stopwatch sw = Stopwatch.StartNew();

            IEnumerable<string> iterationList = PrintIterations("Synchronous");

            sw.Stop();

            TextBox1.Text = ConcatenateStringEnumerable(iterationList, Environment.NewLine) + MakeElapsedTimeString(sw.Elapsed);
        }

        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            IEnumerable<string> iterationList = await PrintIterationsAsync("Asynchronous");

            sw.Stop();

            TextBox2.Text = ConcatenateStringEnumerable(iterationList, Environment.NewLine) + MakeElapsedTimeString(sw.Elapsed);
        }

        private void SyncParallelButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            string[] iterationList = new string[10];

            string callName = "SynchronousParallel";

            Parallel.For(1, 10, (i) =>
            {
                iterationList[i] = PrintCallReport(callName, i);
            });

            sw.Stop();

            TextBox1.Text = ConcatenateStringEnumerable(iterationList, Environment.NewLine) + MakeElapsedTimeString(sw.Elapsed);
        }

        private async void AsyncParallelButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            string[] iterationList = new string[10];

            string callName = "SynchronousParallel";

            await Task.Run(() => Parallel.For(1, 10, (i) =>
            {
                iterationList[i] = PrintCallReport(callName, i);
            }));

            sw.Stop();

            TextBox2.Text = ConcatenateStringEnumerable(iterationList, Environment.NewLine) + MakeElapsedTimeString(sw.Elapsed);
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

                iterationList.Add(PrintCallReport(callName, iterationIndex));
            }

            return iterationList;
        }

        private static string PrintCallReport(string callName, int iterationIndex)
        {
            Thread.Sleep(200);

            string callReport = $"{callName,-12}- Task#{Task.CurrentId,-1} - Thread#{Environment.CurrentManagedThreadId,-1} - [{iterationIndex}]";

            return callReport;
        }

        private void TimerCallbackAction(object _) => Dispatcher.BeginInvoke(() => this.ProgressBar.Value = this.ProgressBar.Value != 100 ? ++this.ProgressBar.Value : 0);

        private static string MakeElapsedTimeString(TimeSpan elapsed) => $"{Environment.NewLine}ElapsedMilliseconds:{elapsed.TotalMilliseconds}";

        private static string ConcatenateStringEnumerable(IEnumerable<string> stringEnumerable, string separator) => stringEnumerable.Aggregate((x, y) => $"{x}{separator}{y}");
    }
}
```

Результаты работы операций, которые могут быть получены:

|Operation|Lock UI|Elapsed Time|
|---|---|---|
|Synchronous|yes|2002 ms|
|Asynchronous|no|215 ms|
|Synchronous Parallel|yes|2002 ms|
|Asynchronous Parallel|no|201 ms|

Из приведённых результатов можно сделать выводы, что **для улучшения пользовательского опыта использования приложения** можно не только избежать блокировки UI за счёт **использования асинхронных операций**, но и значительно увеличить производительность и ускорить получение пользователем результатов выполнения операции за счёт её **грамотного распараллеливания**.

### **Операции ввода-вывода**
---
**Операции ввода-вывода** – это операции передачи/получения сигнала (данных) между нашим приложением (выполняемым процессором и размещённом в ОЗУ) и аппаратным обеспечением. Таким образом происходит обмен информацией с аппаратными частями компьютера (жесткий диск, сетевой адаптер и т.п.).

**В низкоуровневом программировнаии** операции ввода-вывода между процессором, ОЗУ и другими аппаратными устройствами реализованы с помощью **портов ввода-вывода**. На их основе работают **драйверы устройств**.

**В высокоуровневом программировании** для работы с операциями ввода-вывода существуют **высокоуровневые абстрактные концепции**, такие, как **API операционной системы** или даже **специальные функции, классы, объекты и методы** в самих языках программирования, предоставляющие возможность правильно и в простой манере работать с операциями ввода-вывода, даже не выходя за рамки конкретного языка программирования.

Например, абстракция, используемая для чтения и записи файлов, работы с сокетами и т.п. называется потоком данных, который в C# представлен классом ```Stream``` и его производными классами, находящимися в пространстве имён ```System.IO```.

**ОС Windows запрещает напрямую "общаться" с аппаратными устройствами**, но предоставляет возможность отправлять сигналы через специальные низкоуровневые открытые **API операционной системы**, пропуская эти сигналы через своё ядро.

**В языке программирования C# нет необходимости использовать API операционной системы.** Стандартные библиотеки уже имеют набор типов для удобной работы с операциями ввода-вывода в виде потоков данных, представленных классами из пространства имён ```System.IO```.

При чтении файла поток, выполняющийся процессором, блокируется, так как для продолжения выполнения инструкций, следующих за прочтением файла, необходимо получить ответ от другого устройства, время формирования которого не зависит от процессора, и ему приходится перейти в состояние ожидания сигнала о завершении операции от аппаратного устройства ввода-вывода.

**Аппаратное обеспечение** решает задачи ввода-вывода **без участия потоков процессора**. Это оказывает влияние на **эффективность расходования системных ресурсов**.

**Цель процессора в работе ввода-вывода** – это передача запроса на операцию ввода-вывода соответствующему устройству и получение результатов работы от устройства.

### **Пример синхронной работы с файловой системой**

```cs
var fs = new FileStream("filePath");
int bytes = fs.Read(buffer, offset, count);
```

> TODO: Изображение GIF с примером синхронной работы с файловой системой;

### **Асинхронные операции ввода-вывода**

**Асинхронный ввод-вывод** – это форма неблокирующей обработки ввода-вывода, которая позволяет потоку продолжить свое выполнение, не дожидаясь окончания передачи данных.

При запуске **асинхронной операции ввода-вывода**, поток продолжает обрабатывать другие операции, пока ядро не отправит сигнал потоку, указывая, что асинхронная операция ввода-вывода завершена.

### **Перекрывающий ввод-вывод**

**Перекрывающий ввод-вывод (Overlapped I/O)** – это название асинхронного ввода-вывода на уровне **API операционной системы **Windows**.

Перекрывающий ввод-вывод представляет структура ```OVERLAPPED```. Она означает, что исполнение запросов ввода-вывода перекрывается по времени с исполнением потоком других операций.

Чтобы операция ввода-вывода стала асинхронной, необходимо передать специальную структуру ```OVERLAPPED```.

### **Завершение асинхронной операции ввода-вывода**

Есть несколько способов завершения асинхронной (перекрытой) операции ввода-вывода:

- **Событие Win32** - по завершению операции ввода-вывода драйвер проверяет, установлено ли событие. Если да, то он его вызывает;
- **Очередь APC (Asynchronous Procedure Call)** - передача пользовательской процедуры, которая будет вызвана по завершению асинхронной операции ввода-вывода. Когда процедура асинъронного **IO** будет завершена, процедура обратного вызова будет передана в **очередь APC**, а когда у потока будет время, он будет очищать свою APC очередь, выполняя находящиеся там процедуры, но делать он это может только из состояния ожидания извещения, что составляет самое большое неудобство данного способа;
- **Порты завершения ввода-вывода (IO Completion Ports)** - наилучший способ среди остальных. Классы в .NET при асинхронном вводе-выводе используют **порты завершения**, работа с которыми обеспечивается через пул потоков.

### **IO Completion Ports**

**IO Completion Ports (IOCP)** – это объект, являющийся очередью, который используется для одновременного управления несколькими операциями ввода-вывода. Управление производится с помощью привязки дескрипторов к **IOCP**.

По завершении операции над дескриптором, пакет завершения ввода/вывода помещается в очередь **IOCP**.

Объект **ThreadPool** отвечает за мониторинг **IOCP** и диспетчеризацию задач для потоков портов завершения, отвечающих за обработку завершения операции.

Напрямую с портами завершения сейчас не работают. Программисты используют
предоставляемые типы, которые находятся в библиотеках .NET.

Обычно эти типы используют асинхронный шаблон **APM**, так как их создавали очень давно, но самые необходимые API были переписаны на новый шаблон **TAP**.

### **Пример асинхронной работы с файловой системой**

```cs
var fs = new FileStream("filePath", FileOptions.Asynchronous);
int bytes = await fs.ReadAsync(buffer, offset, count);
```

**Отличия работы синхронного ввода-вывода от асинхронного:**

- При запуске синхронной операции ввода-вывода, поток переходит в состояние ожидания до тех пор, пока не получит сигнал о завершении работы операции ввода-вывода;

- При запуске асинхронной операции ввода-вывода, поток продолжает обрабатывать другие операции, пока ядро не отправит сигнал потоку, указывая, что асинхронная операция ввода-вывода завершена.

## **02 Асинхронные операции IO в .NET Framework 4.7.2**
---
Для демонстрации асинхронных операций IO, реализованных через потоки ввода-вывода портов завершения ```completionPorts``` в пуле потоков ```ThreadPool``` представлен пример консольного приложения, осуществляющий асинхронный запрос удалённого ресурса и обрабатывающий его результат в продолжении. 

В приведённом примере создаётся асинхронный метод ```SendHttpRequestAsync()```, который отправляет GET запрос к сетевому ресурсу по адресу **https://visualstudio.com/**, отправив запрос к сетевому адаптеру, и, не дожидаясь ответа, возвращает управление вызывающему потоку.

Вызывающий поток в методе, который вызвал асинхронный метод, блокируется до получения результата асинхронной операции в целях демонстрации времени её завершения.

После получения ответа от удалённого ресурса, сетевой адаптер инициирует запуск на процессоре асинхронного потока ввода-вывода порта завершения, в котором начинает своё выполнение продолжение асинхронного метода ```SendHttpRequestAsync()```.

Для демонстрации того, что продолжение выполняется именно в асинронном потоке ввода-вывода порта завершения, создаётся и запускается метод ```MakeElapsedTimeString()```, выводящий и подсвечивающий информацию о состоянии пула потоков, если задействованы его рабочие потоки или потоки портов завершения.

Программный код демонстрирующего работу потоков портов завершения:

```cs
internal class Program
{
    private static readonly object _consoleLock = new object();
    private static HttpClient _httpClient;

    private static void Main(string[] args)
    {
        _httpClient = new HttpClient();

        StartThreadPoolMonitoring();

        HttpResponseMessage asyncResult = SendHttpRequestAsync().Result;

        lock (_consoleLock)
        {
            Console.WriteLine("Async Result was received in a calling method.");
            Console.WriteLine("Press any key to finish the program.");
        }

        Console.ReadKey();
    }

    private static async Task<HttpResponseMessage> SendHttpRequestAsync()
    {
        Stopwatch sw = Stopwatch.StartNew();

        HttpResponseMessage asyncResult = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://visualstudio.com/"));

        sw.Stop();

        Thread.Sleep(200);

        lock (_consoleLock)
        {
            Console.WriteLine($"Continuation of Async IO Result of HTTP Request is executing in ThreeadPoolThread: [{Thread.CurrentThread.IsThreadPoolThread}].");
            Console.WriteLine("Async IO Result of HTTP Request was Received after" + MakeElapsedTimeString(sw.Elapsed));
        }

        Thread.Sleep(200);

        return asyncResult;
    }

    private static string MakeElapsedTimeString(TimeSpan elapsed) => $"{Environment.NewLine}ElapsedMilliseconds:{elapsed.TotalMilliseconds}";

    private static void StartThreadPoolMonitoring()
    {
        Thread thredPoolMonitoringThread = new Thread(() =>
        {
            while (true)
            {
                ThreadPool.GetAvailableThreads(out int workerThreads, out int iocpThreads);
                ThreadPool.GetMaxThreads(out int workerMaxThreads, out int iocpMaxThreads);

                bool usedWorkerThreads = workerThreads < workerMaxThreads;
                bool usedIocpThreads = iocpThreads < iocpMaxThreads;

                if (usedWorkerThreads || usedIocpThreads)
                {
                    lock (_consoleLock)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.Write("Worker Threads[");

                        if (usedWorkerThreads)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.Write(workerThreads);
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.Write(" of ");

                        if (usedWorkerThreads)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        }
                        Console.Write(workerMaxThreads);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("] ");

                        Console.Write("IOCP Threads[");

                        if (usedIocpThreads)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.Write(iocpThreads);
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.Write(" of ");

                        if (usedIocpThreads)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        }
                        Console.Write(iocpMaxThreads);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("] ");

                        Console.ResetColor();
                        Console.WriteLine();
                    }

                    Thread.Sleep(100);
                }
            }
        });

        thredPoolMonitoringThread.IsBackground = true;

        thredPoolMonitoringThread.Start();
    }
}
```

Как видно из примера, в самом начале механизм асинхронного выполнения кода задействует 1 рабочий поток из пула потоков, но после получения ответа от удалённого ресурса, **продолжение** асинхронного метода, расположенного в блоке кода после оператора ```await```, **запускается в потоке порта завершения**, забирая его из пула.

## **03 Асинхронные операции IO в .NET 7**
---
> Программный код данного примера полностью идентичен прораммному коду из примера **02 Асинхронные операции IO в .NET Framework 4.7.2**, поэтому не приводится с целью избежания бессмысленного дублирования.

Выполнение данного примера в контексте платформы **.NET 7** в корне отличается от выполнения его же в контексте платформы **.NET Framework 4.7.2**.

В **.NET 7** уже не задействуются потоки портов завершения, продолжение выполняется рабочим потоком ```workerThread``` пула потоков, так как код для этой платформы может быть запущен на устройствах и операционных системах, не имеющих портов завершения **Completion Ports**, таких, например, как **OS Linux**.

## **04 Асинхронные операции IO в .NET Core 3.1**
---
> Программный код данного примера полностью идентичен прораммному коду из примера **02 Асинхронные операции IO в .NET Framework 4.7.2**, поэтому не приводится с целью избежания бессмысленного дублирования.

Выполнение данного примера в контексте платформы **.NET Core 3.1** схоже с выполнением его же в контексте платформы **.NET Framework 4.7.2**.

Несмотря на то, что код для платформы **.NET Core 3.1** может быть запущен на различных платформах, таких, как **OS Windows**, **OS Linux** и **OS MacOS**, в отличии от **.NET 7** он имеет возможность использовать потоки портов завершения, если такая возможность имеется.

### **Асинхронность vs Многопоточность**
---

После появления задач и ключевых слов async await использование асинхронности стало очень простым. Поэтому, программисты начали широко использовать асинхронность. Но, зачастую, люди считают, что асинхронность – это обязательно участие потока (```Thread```).

Асинхронность не означает, что вы используете поток для выполнения операции. Асинхронное выполнение может происходить без участия потока. Это асинхронный ввод-вывод.

Асинхронность в большинстве случаев как раз и означает асинхронные операции ввода-вывода, именно для этого она и была введена - чтобы использовать неблокирующие операции без участия потоков. Но, при этом, вам никто не запрещает использовать механизмы ```async await``` для CPU операций, пользуясь потоками.

# **Определение "Асинхронность"**
---

Зная о разнице между работой операций для потоков и для ввода-вывода, мы можем сформировать определение **асинхронности**, которое действительно ей соответствует.

**Асинхронность** – это неблокирующее выполнение кода.

**Асинхронность** может быть представлена как операцией, выполняющейся параллельно её запустившей операции в контексте CPU и RAM, так и операцией, выполняющейся параллельно её запустившей операции в контексте других устройств компьютера, таких, как устройства ввода-вывода (сетевые адаптеры, накопители информации).
