namespace ObjectOrientedProgramming._1_Encapsulation._2_ImplementationHiding
{
    internal class ClassA
    {
        public void ClassAMethod()
        {
            DateTime dateTime = DateTime.Now;

            Console.WriteLine(dateTime);
        }
    }

    internal class ClassB
    {
        public void ClassBMethod()
        {
            ClassA classA = new();

            // The implementation (instructions inside 'ClassAMethod') is hidden
            // from the user.
            classA.ClassAMethod();
        }
    }
}
