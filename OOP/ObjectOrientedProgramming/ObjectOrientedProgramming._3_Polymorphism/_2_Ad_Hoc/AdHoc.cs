namespace ObjectOrientedProgramming._3_Polymorphism._2_Ad_Hoc
{
    internal class AdHoc
    {
        public void AdHocMethod(int arg)
        {
            Console.WriteLine("int arg method");
        }

        public void AdHocMethod(string arg)
        {
            Console.WriteLine("string arg method");
        }

        public void AdHocMethod(bool arg)
        {
            Console.WriteLine("bool arg method");
        }
    }

    internal class ClassDemo
    {
        public void MethodDemo()
        {
            AdHoc adHoc = new();

            // Different method implementations are called when arguments of
            // different types are passed to a method:
            adHoc.AdHocMethod(new int());
            adHoc.AdHocMethod(new string('-', 10));
            adHoc.AdHocMethod(new bool());
        }
    }
}
