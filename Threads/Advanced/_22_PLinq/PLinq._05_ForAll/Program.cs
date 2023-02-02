using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLinq._05_ForAll
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<int> dataSet = CreateData();

            var parallelQuery =
                from number in dataSet
                .AsParallel()
                where QueryPredicate1(number)
                select number;

            Console.WriteLine("Selected ordered Results:");

            parallelQuery.ForAll(x => Console.WriteLine($"Selected value: [{x}]."));
        }

        private static bool QueryPredicate1(int number)
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
