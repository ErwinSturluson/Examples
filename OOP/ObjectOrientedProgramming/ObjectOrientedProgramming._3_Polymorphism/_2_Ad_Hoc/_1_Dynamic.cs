namespace ObjectOrientedProgramming._3_Polymorphism._2_Ad_Hoc._1_Dynamic
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
