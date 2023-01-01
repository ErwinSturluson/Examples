namespace ObjectOrientedProgramming._1_Encapsulation._3_HidingPartsOfSoftwareSystems
{
    internal class ClassA
    {
        private readonly string _content;

        public ClassA(string content)
        {
            _content = content;
        }

        public void DisplayContent()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(_content);
            Console.ResetColor();
        }
    }

    internal class ClassB
    {
        private readonly string _content;

        public ClassB(string content)
        {
            _content = content;
        }

        public void DisplayContent()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_content);
            Console.ResetColor();
        }
    }

    internal class ClassFacade
    {
        private readonly ClassA _classA;
        private readonly ClassB _classB;

        public ClassFacade(string contentClassA, string contentClassB)
        {
            _classA = new ClassA(contentClassA);
            _classB = new ClassB(contentClassB);
        }

        public void DisplayContent()
        {
            _classA.DisplayContent();
            _classB.DisplayContent();
        }
    }

    internal class DemopClass
    {
        public void DemoMethod()
        {
            // Parts of the software system (classes A and B) is hidden behind
            // the Facade object 'ClassFacade':
            ClassFacade classFacade = new("ContentClassA", "ContentClassB");

            classFacade.DisplayContent();

            // Instead of directly infocation parts (classes A and B) of the
            // software system separatly:
            ClassA classA = new("ContentClassA");
            ClassB classB = new("ContentClassB");

            classA.DisplayContent();
            classB.DisplayContent();
        }
    }
}
