using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SequentialReadingBenchmark
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MedianColumn, MinColumn, MaxColumn, MemoryDiagnoser]
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    public class TwoDimArray
    {
        private static readonly int[,] _arrayForRowAccessTest = new int[5000, 5000];
        private static readonly int[,] _arrayForColumnAccessTest = new int[5000, 5000];

        [Benchmark]
        public int RowAccessFirst()
        {
            int[,] arrayForRowAccessTest = _arrayForRowAccessTest;

            for (int i = 0; i < 5000; i++)
            {
                for (int j = 0; j < 5000; j++)
                {
                    arrayForRowAccessTest[i, j] = 1;
                }
            }

            return arrayForRowAccessTest[5000 - 1, 5000 - 1];
        }

        [Benchmark]
        public int ColumnAccessFirst()
        {
            int[,] arrayForColumnAccessTest = _arrayForColumnAccessTest;

            for (int i = 0; i < 5000; i++)
            {
                for (int j = 0; j < 5000; j++)
                {
                    arrayForColumnAccessTest[j, i] = 1;
                }
            }

            return arrayForColumnAccessTest[5000 - 1, 5000 - 1];
        }
    }
}
