using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Apm._12_Stream.BeginWrite_EndWrite
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stream stream = new FileStream("data.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            string appText = "[====THE CONTENT OF THE APP====]";

            byte[] appTextBytes = Encoding.GetEncoding("UTF-8").GetBytes(appText);

            IAsyncResult asyncResult = stream.BeginWrite(appTextBytes, 0, appTextBytes.Length, new AsyncCallback(StreamWriteCallback), stream);

            Console.WriteLine("The app is writing the following text to the file asynchronously:");
            Console.WriteLine(appText);

            Console.WriteLine($"{Environment.NewLine}Another work: print iterations while file is reading asynchronously...");

            PrintIterations();

            Console.WriteLine($"Another work has finished.{Environment.NewLine}");

            stream.EndWrite(asyncResult);

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

        private static void StreamWriteCallback(IAsyncResult asyncResult)
        {
            Console.WriteLine("The text was written asynchronously.");

            Stream stream = asyncResult.AsyncState as Stream;

            if (stream != null)
            {
                stream.Close();
            }
        }
    }
}
