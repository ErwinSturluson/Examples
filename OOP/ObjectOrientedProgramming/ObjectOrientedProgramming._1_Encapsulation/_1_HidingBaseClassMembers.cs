namespace ObjectOrientedProgramming._1_Encapsulation._1_HidingBaseClassMembers
{
    internal class BaseClass
    {
        public virtual void BaseClassMethod()
        {
            Console.WriteLine(nameof(BaseClass));
        }
    }

    internal class DerivedClass : BaseClass
    {
        public override void BaseClassMethod()
        {
            Console.WriteLine(nameof(DerivedClass));
        }
    }

    internal class DemoClass
    {
        private void DemoMethod()
        {
            DerivedClass derivedClass = new DerivedClass();
            // Implementation of "BaseClassMethod" method is hidden by derived
            // class implementation via virtual/override modifiers:
            derivedClass.BaseClassMethod();
        }
    }
}
