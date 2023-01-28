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
