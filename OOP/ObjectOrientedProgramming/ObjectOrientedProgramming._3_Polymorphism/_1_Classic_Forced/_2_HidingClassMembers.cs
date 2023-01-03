namespace ObjectOrientedProgramming._3_Polymorphism._1_Classic_Forced._2_HidingClassMembers
{
    internal class BaseClass
    {
        public void BaseClassMethod()
        {
        }
    }

    internal class DerivedClass : BaseClass
    {
        public void DerivedClassMethod()
        {
        }
    }

    internal class ClassDemo
    {
        public void MethodDemo()
        {
            DerivedClass derivedClass = new();

            // Instance of the derived class type in the variable of the derived
            // class type has access to derived class and base class methods:
            derivedClass.DerivedClassMethod();
            derivedClass.BaseClassMethod();

            BaseClass baseClass = derivedClass;
            // Uncomment the following code to see the errors.
            //// Instance of the derived class type in the variable of the base
            //// class type has only access to base class method:
            //baseClass.DerivedClassMethod();
            //baseClass.BaseClassMethod();
        }
    }
}
