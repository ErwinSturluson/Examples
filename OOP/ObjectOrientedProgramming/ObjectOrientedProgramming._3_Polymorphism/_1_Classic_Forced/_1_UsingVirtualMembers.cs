namespace ObjectOrientedProgramming._3_Polymorphism._1_Classic_Forced._1_UsingVirtualMembers
{
    internal class BaseClass
    {
        public virtual void Action()
        {
            Console.WriteLine(nameof(BaseClass));
        }
    }

    internal class DerivedClass1 : BaseClass
    {
        public override void Action()
        {
            Console.WriteLine(nameof(DerivedClass1));
        }
    }

    internal class DerivedClass2 : BaseClass
    {
        public override void Action()
        {
            Console.WriteLine(nameof(DerivedClass2));
        }
    }

    internal class ClassDemo
    {
        public virtual void MethodDemo()
        {
            BaseClass instance = new();
            instance.Action();

            // Overriden method of the derived class replaces method of the base class:
            instance = new DerivedClass1();
            instance.Action();

            // Overriden method of the another derived class replaces overriden
            // method of the previous class:
            instance = new DerivedClass2();
            instance.Action();
        }
    }
}
