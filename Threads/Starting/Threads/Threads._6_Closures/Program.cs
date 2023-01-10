using System;
using System.Threading;

namespace Threads._6_Closures
{
    internal class Program
    {
        private static int _fieldIterationNumber = 0;

        private static void Main(string[] args)
        {
            int variableIterationNumber = 0;

            Thread thread = new(PrintIterations);

            thread.Start();

            Console.ReadKey();

            void PrintIterations()
            {
                while (_fieldIterationNumber < 10 && variableIterationNumber < 10)
                {
                    _fieldIterationNumber++;
                    variableIterationNumber++;

                    Console.WriteLine($"{nameof(_fieldIterationNumber)}:{_fieldIterationNumber}|{nameof(variableIterationNumber)}:{variableIterationNumber}");
                    Thread.Sleep(100);
                }
            }
        }
    }
}