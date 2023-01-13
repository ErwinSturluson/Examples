using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Apm._11_Stream.BeginRead_EndRead
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stream stream = new FileStream("data.txt", FileMode.Open, FileAccess.Read);

            byte[] fileTextBytes = new byte[stream.Length];

            IAsyncResult asyncResult = stream.BeginRead(fileTextBytes, 0, fileTextBytes.Length, null, null);

            Console.WriteLine("Reading the file...");

            Console.WriteLine($"{Environment.NewLine}Another work: print iterations while file is reading asynchronously...");

            PrintIterations();

            Console.WriteLine($"Another work has finished.{Environment.NewLine}");

            stream.EndRead(asyncResult);

            stream.Close();

            Console.WriteLine("The file was readed asynchronously. It contains the following text:");

            string fileText = Encoding.GetEncoding("UTF-8").GetString(fileTextBytes);

            Console.WriteLine($"{Environment.NewLine}{fileText}{Environment.NewLine}");

            Console.WriteLine("All the tasks is completed.");
        }

        private static void PrintIterations()
        {
            int iterationNumber = 0;

            while (iterationNumber < 10)
            {
                iterationNumber++;

                Console.WriteLine($"Thread#{Environment.CurrentManagedThreadId} - {iterationNumber}");
                Thread.Sleep(100);
            }
        }
    }
}
