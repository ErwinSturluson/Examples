using System;
using System.Threading.Tasks;

namespace AsyncAwait.AdditionalMethods._01_Task.Delay
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Code before [Task.Delay(2000)]");

            await Task.Delay(2000);

            Console.WriteLine("Code after [Task.Delay(2000)]");
        }
    }
}
