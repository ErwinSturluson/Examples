using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncAwait.SyncContext._01_WPF
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
}
