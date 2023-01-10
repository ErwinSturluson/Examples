using System;
using System.Threading;

namespace Threads._8_AnonimousMethods
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread threadAnonimousMethod = new(delegate ()
            {
                while (true)
                {
                    Console.WriteLine("\tSecondary Anonimous Method");
                }
            });

            Thread threadLambdaOperator = new(delegate ()
            {
                while (true)
                {
                    Console.WriteLine("\tSecondary Lambda Operator");
                }
            });

            threadAnonimousMethod.Start();
            threadLambdaOperator.Start();

            Console.ReadKey();
        }
    }
}