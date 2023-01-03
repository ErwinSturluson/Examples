namespace ObjectOrientedProgramming._3_Polymorphism._3_DuckTypingPolymorphism
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

    internal class AdHocDemoClass
    {
        public void AdHocDemoMethod()
        {
            dynamic dynamicVariable = new ClassA();
            dynamicVariable.Action();

            dynamicVariable = new ClassB();
            dynamicVariable.Action();
        }
    }
}
