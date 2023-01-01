using ObjectOrientedProgramming._6_Reuse.ReusableCodeLibrary;

namespace ObjectOrientedProgramming._6_Reuse
{
    internal class ReusableClass2
    {
        private ReusableClass1 ReusableClass1 { get; set; } = new();

        private ReusableClass3 ReusableClass3 { get; set; } = new();

        public void ReusableMethod()
        {
            ReusableClass1.ReusableMethod();
            ReusableClass3.ReusableMethod();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Reuse of instructions in methods and objects described earlier:
            ReusableClass2 reusableClass2 = new();
            reusableClass2.ReusableMethod();

            ReusableClass2 reusableClass1 = new();
            reusableClass2.ReusableMethod();

            ReusableClass2 reusableClass3 = new();
            reusableClass3.ReusableMethod();

            Console.WriteLine("Hello, Reuse!");
        }
    }
}