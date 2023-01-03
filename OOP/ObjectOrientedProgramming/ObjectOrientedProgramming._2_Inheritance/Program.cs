namespace ObjectOrientedProgramming._2_Inheritance
{
    public class BaseClass
    {
        public void BaseClassPublicMethod()
        {
        }

        protected void BaseClassProtectedMethod()
        {
        }

        private void BaseClassPrivateMethod()
        {
        }
    }

    public class DerivedClass : BaseClass
    {
        public void DerivedClassPublicMethod()
        {
            // Error: private methods are not inherited:
            // Uncomment the following code to see the errors.
            //BaseClassPrivateMethod();

            // Protected and public methods are inherited, but only available
            // inside the derived classes:
            BaseClassProtectedMethod();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            BaseClass baseClass = new();
            baseClass.BaseClassPublicMethod();

            DerivedClass derivedClass = new();
            derivedClass.DerivedClassPublicMethod();

            // Public methods are inherited and available outside the derived classes:
            derivedClass.BaseClassPublicMethod();

            Console.WriteLine("Hello, Inheritance!");
        }
    }
}