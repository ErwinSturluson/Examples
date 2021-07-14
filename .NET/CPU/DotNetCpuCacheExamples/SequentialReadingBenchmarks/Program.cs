using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SequentialReadingBenchmark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Summary[] summaries = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
