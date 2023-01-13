namespace Apm._06_IAsyncResult.IsCompleted
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Action<string, int> actionDelegate = new(PrintIterations);

            IAsyncResult actionResult = actionDelegate.BeginInvoke("AsyncTask", 100, null, null);

            while (!actionResult.IsCompleted)
            {
                Thread.Sleep(100);
            }

            PrintIterations("SyncTask", 200);

            Console.ReadKey();
        }

        private static void PrintIterations(string taskName, int iterationsDelay)
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskName:{taskName} - {iterationNumber}");
                Thread.Sleep(iterationsDelay);
            }
        }
    }
}
