using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;

namespace Crc32Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(Crc32Benchmark.GetTestArraySize());

            try
            {
                Summary[] summaries = BenchmarkRunner.Run(typeof(Program).Assembly);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
