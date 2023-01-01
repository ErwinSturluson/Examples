namespace ObjectOrientedProgramming._4_Abstraction
{
    internal abstract class ClassBase
    {
        public abstract int Function(string data);
    }

    internal interface IFunction
    {
        public int Function(string data);
    }

    internal class DerivedClass1 : ClassBase
    {
        public override int Function(string data)
        {
            return data.Length;
        }

        public void Procedure(string data)
        {
            Console.WriteLine(data);
        }
    }

    internal class DerivedClass2 : IFunction
    {
        public int Function(string data)
        {
            return data.Length;
        }

        public void Procedure(string data)
        {
            Console.WriteLine(data);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Abstraction discards implementation members it doesn't have:
            ClassBase instance1 = new DerivedClass1();
            instance1.Function(nameof(ClassBase));
            instance1.Procedure(nameof(DerivedClass1));

            IFunction instance2 = new DerivedClass2();
            instance1.Function(nameof(IFunction));
            instance1.Procedure(nameof(DerivedClass2));

            Console.WriteLine("Hello, Abstraction!");
        }
    }
}