using System;
using System.Collections.Generic;
using System.Linq;

namespace TPL._20_PLinq.AsParallel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<Document> documentArchive = Enumerable.Range(0, 100_000)
                .Select(i => new Document(i, $"Document #{i + 1} by {DateTime.UtcNow}"));

            ParallelQuery<Document> filteredDocs = from doc in documentArchive.AsParallel()
                                                   where doc.Id > 50_000
                                                   select doc;

            List<Document> documents = filteredDocs.ToList();

            if (documents.Count != 0)
            {
                Console.WriteLine($"First document Name:{documents.First().Name}");
                Console.WriteLine($"Last document Name:{documents.Last().Name}");
            }
            else
            {
                Console.WriteLine($"There are not any documents for those conditions.");
            }
        }
    }

    internal readonly struct Document
    {
        public int Id { get; }

        public string Name { get; }

        public Document(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
