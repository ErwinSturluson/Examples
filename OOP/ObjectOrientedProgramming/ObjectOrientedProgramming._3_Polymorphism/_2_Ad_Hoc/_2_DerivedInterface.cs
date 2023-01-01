namespace ObjectOrientedProgramming._3_Polymorphism._2_Ad_Hoc._2_DerivedInterface
{
    public class ClassA
    {
        public void Action()
        {
        }
    }

    public class ClassB
    {
        public void Action()
        {
        }
    }

    public interface IAction
    {
        public void Action();
    }

    public class DerivedClassA : ClassA, IAction
    {
    }

    public class DerivedClassB : ClassB, IAction
    {
    }

    internal class AdHocDemoClass
    {
        public void AdHocDemoMethod()
        {
            IAction iActionVariable = new DerivedClassA();
            iActionVariable.Action();

            iActionVariable = new DerivedClassB();
            iActionVariable.Action();
        }
    }
}
