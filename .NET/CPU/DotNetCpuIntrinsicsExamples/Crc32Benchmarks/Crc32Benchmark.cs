using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Crc32Examples;
using System;
using System.Linq;

namespace Crc32Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MedianColumn, MinColumn, MaxColumn, MemoryDiagnoser]
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    internal class Crc32Benchmark
    {
        private static readonly byte[] _testingBytes;

        static Crc32Benchmark()
        {
            Random random = new Random();

            _testingBytes = Enumerable.Range(1, 1024).Select(x => (byte)random.Next(0, sizeof(byte))).ToArray();
        }

        public static int GetTestArraySize()
        {
            return _testingBytes.Length;
        }

        [Benchmark]
        public int ComputeInt32Number()
        {
            int result = Crc32HashComputer.ComputeInt32Number(_testingBytes);

            return result;
        }

        [Benchmark]
        public long ComputeInt64Number()
        {
            long result = Crc32HashComputer.ComputeInt64Number(_testingBytes);

            return result;
        }
    }
}
