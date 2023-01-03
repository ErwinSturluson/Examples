namespace ObjectOrientedProgramming._1_Encapsulation._1_HidingBaseClassMembers
{
    internal class BaseClass
    {
        private int _field1;

        public BaseClass(int field1)
        {
            Initialize();
            _field1 = field1;
        }

        private void Initialize()
        {
            _field1 = 100;
        }
    }

    internal class DerivedClass : BaseClass
    {
        //Uncomment the following code to see the errors.
        //public DerivedClass(int field1)
        //{
        //    // Error: base class private method not available in derived class
        //    // because it is hidden.
        //    Initialize();

        //    // Error: base class private field not available in derived class
        //    // because it is hidden.
        //    _field1 = field1;
        //}

        // Private class members can only be accessed through public or
        // protected members.
        public DerivedClass(int field1) : base(field1)
        {
        }
    }
}
