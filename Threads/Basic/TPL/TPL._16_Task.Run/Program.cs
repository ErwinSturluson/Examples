namespace TPL._16_Task.Run
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task task1 = Task.Run(PrintIterations);

            Task task2 = Task.Run(() =>
            {
                int iterationNumber = 0;

                while (iterationNumber < 10)
                {
                    iterationNumber++;

                    Console.WriteLine($"TaskId:{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                    Thread.Sleep(100);
                }
            });

            Task.WaitAll(task1, task2);
        }

        private static void PrintIterations()
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"TaskId:{Task.CurrentId} - Thread#{Environment.CurrentManagedThreadId} - [{iterationNumber}]");
                Thread.Sleep(100);
            }
        }
    }
}
