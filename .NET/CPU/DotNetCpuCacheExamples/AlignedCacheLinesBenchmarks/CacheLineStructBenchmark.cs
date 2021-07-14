using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace AlignedCacheLinesBenchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MedianColumn, MinColumn, MaxColumn, MemoryDiagnoser]
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    public class CacheLineStructBenchmark
    {
        private const int CacheLineScturctCount = 200_000_000;

        private static readonly OneCacheLineStruct[] _oneCacheLineStructs = new OneCacheLineStruct[CacheLineScturctCount];
        private static readonly TwoCacheLineStruct[] _twoCacheLineStructs = new TwoCacheLineStruct[CacheLineScturctCount];

        [Benchmark]
        public long OneLineStructReading()
        {
            int n = _oneCacheLineStructs.Length;

            for (int i = 0; i < n; i += 4)
            {
                _oneCacheLineStructs[i].Field1 = 1;
                _oneCacheLineStructs[i].Field2 = 1;
                _oneCacheLineStructs[i].Field3 = 1;
                _oneCacheLineStructs[i].Field4 = 1;
                _oneCacheLineStructs[i].Field5 = 1;
                _oneCacheLineStructs[i].Field6 = 1;
                _oneCacheLineStructs[i].Field7 = 1;
                _oneCacheLineStructs[i].Field8 = 1;
            }

            return _oneCacheLineStructs[n - 1].Field1;
        }

        [Benchmark]
        public long TwoLineStructReading()
        {
            int n = _twoCacheLineStructs.Length;

            for (int i = 0; i < n; i += 4)
            {
                _oneCacheLineStructs[i].Field1 = 1;
                _oneCacheLineStructs[i].Field2 = 1;
                _oneCacheLineStructs[i].Field3 = 1;
                _oneCacheLineStructs[i].Field4 = 1;
                _oneCacheLineStructs[i].Field5 = 1;
                _oneCacheLineStructs[i].Field6 = 1;
                _oneCacheLineStructs[i].Field7 = 1;
                _oneCacheLineStructs[i].Field8 = 1;
                _twoCacheLineStructs[i].Field9 = 1;
            }

            return _twoCacheLineStructs[n - 1].Field1;
        }
    }
}
