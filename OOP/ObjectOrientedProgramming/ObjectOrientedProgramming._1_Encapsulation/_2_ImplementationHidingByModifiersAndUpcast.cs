namespace ObjectOrientedProgramming._1_Encapsulation._2_ImplementationHiding
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
            //Uncomment the following code to see the errors.
            //BaseClassPrivateMethod();

            // Protected and public methods are inherited, but only available
            // inside the derived classes:
            BaseClassProtectedMethod();
        }
    }

    internal class DemoClass
    {
        private void DemoMethod()
        {
            BaseClass baseClass = new();
            baseClass.BaseClassPublicMethod();

            DerivedClass derivedClass = new();
            derivedClass.DerivedClassPublicMethod();

            // Public methods are inherited and available outside the derived classes:
            derivedClass.BaseClassPublicMethod();

            // Error: derived class members are not available in the base class variables:
            //Uncomment the following code to see the errors.
            //BaseClass derivedInBaseClassVariable = new DerivedClass();
            //derivedInBaseClassVariable.DerivedClassPublicMethod();
        }
    }
}
