namespace Apm._03_Delegate.EndInvokeReturnValue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Func<int> funcDelegate = new(PrintIterations);

            IAsyncResult funcResult = funcDelegate.BeginInvoke(null, null);

            int funcIterationNumber = funcDelegate.EndInvoke(funcResult);

            Console.WriteLine($"Result of {nameof(funcIterationNumber)} from async call:{funcIterationNumber}");

            int iterationNumber = PrintIterations();

            Console.WriteLine($"Result of {nameof(iterationNumber)} from sync call:{iterationNumber}");

            Console.ReadKey();
        }

        private static int PrintIterations()
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(100);
            }

            return iterationNumber;
        }
    }
}
