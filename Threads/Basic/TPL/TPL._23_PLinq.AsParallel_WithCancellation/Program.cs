using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TPL._22_PLinq.AsParallel_WithCancellation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<Document> documentArchive = Enumerable.Range(0, 100_000)
                .Select(i => new Document(i, $"Document #{i + 1} by {DateTime.UtcNow}"));

            CancellationTokenSource cts = new();

            ParallelQuery<Document> filteredDocs = from doc in documentArchive
                                                   .AsParallel()
                                                   .AsOrdered()
                                                   .WithCancellation(cts.Token)
                                                   where doc.Id > 50_000
                                                   select doc;

            cts.CancelAfter(100);

            List<Document> documents;

            try
            {
                documents = filteredDocs.ToList();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Document searching has failed due to operation was canceled.");
                throw;
            }

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
