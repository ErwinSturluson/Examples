using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace AlignedCacheLinesBenchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new CacheLineStructBenchmark().OneLineStructReading();

            Summary[] summaries = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
