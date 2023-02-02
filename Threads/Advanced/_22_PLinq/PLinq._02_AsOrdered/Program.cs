using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLinq._02_AsOrdered
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<int> dataSet = CreateData();

            var query =
                from number in dataSet
                .AsParallel()
                where QueryPredicate(number)
                select number;

            Console.WriteLine("Selected ordered Results:");

            foreach (int number in query)
            {
                Console.WriteLine($"Selected value: [{number}].");
            }

            var orderedQuery =
                from number in dataSet
                .AsParallel()
                .AsOrdered()
                where QueryPredicate(number)
                select number;

            Console.WriteLine(Environment.NewLine + "Selected ordered Results:");

            foreach (int number in orderedQuery)
            {
                Console.WriteLine($"Selected value: [{number}].");
            }
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
