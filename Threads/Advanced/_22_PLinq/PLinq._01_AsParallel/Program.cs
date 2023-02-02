using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLinq._01_AsParallel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<int> dataSet = CreateData();

            Stopwatch stopwatch = new();
            Console.WriteLine($"Query has started sequentially...");
            stopwatch.Start();

            var sequentialQuery =
                from number in dataSet
                where QueryPredicate(number)
                select number;

            int sequentialResultsNumber = sequentialQuery.Count();

            Console.WriteLine($"Query has finished sequentially after [{stopwatch.ElapsedMilliseconds}] milliseconds. Number of results is [{sequentialResultsNumber}].{Environment.NewLine}");
            stopwatch.Restart();
            Console.WriteLine($"Query has started parallel...");

            var parallelQuery =
                from number in dataSet
                .AsParallel()
                where QueryPredicate(number)
                select number;

            int parallelResultsNumber = parallelQuery.Count();

            Console.WriteLine($"Query has finished parallel after [{stopwatch.ElapsedMilliseconds}] milliseconds. Number of results is [{parallelResultsNumber}].");
            stopwatch.Stop();
        }

        private static bool QueryPredicate(int number)
        {
            Thread.Sleep(100);
            return number < 1;
        }

        private static IEnumerable<int> CreateData()
        {
            int[] dataSet = new int[10];

            Parallel.For(0, dataSet.Count(), (i, _) => { dataSet[i] = i; });

            for (int i = 0; i < dataSet.Length; i += 2)
            {
                dataSet[i] = dataSet[i] * -1;
            }

            return dataSet;
        }
    }
}
