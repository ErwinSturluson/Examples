namespace ObjectOrientedProgramming._1_Encapsulation._4_HidingDataTypes
{
    internal class Class
    {
        public int Method1()
        {
            return new int();
        }

        public double Method2()
        {
            return new double();
        }

        public void ClassMethod()
        {
            // Data types are hidden by variables 'var' and 'dynamic':
            var varVariable = Method1();
            dynamic dynamicVariable = Method2();

            Console.WriteLine(varVariable);
            Console.WriteLine(dynamicVariable);
        }
    }
}
