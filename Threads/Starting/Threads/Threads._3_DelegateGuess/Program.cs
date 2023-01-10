namespace Threads._3_DelegateGuess
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread = new(PrintSecondary);

            thread.Start();

            while (true)
            {
                Console.WriteLine("Primary");
            }
        }

        private static void PrintSecondary()
        {
            while (true)
            {
                Console.WriteLine("\tSecondary");
            }
        }
    }
}