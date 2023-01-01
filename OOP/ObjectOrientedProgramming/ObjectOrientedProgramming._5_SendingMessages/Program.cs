namespace ObjectOrientedProgramming._5_SendingMessages
{
    internal class ClassDemo
    {
        public void MethodDemo()
        {
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            ClassDemo class1 = new();
            // Sending the message is named 'MethodDemo' from object of class
            // 'Program' to object of class 'ClassDemo' even without any arguments
            class1.MethodDemo();

            Console.WriteLine("Hello, World!");
        }
    }
}