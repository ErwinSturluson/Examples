# **Object Oriented Programming**

Other language versions:
- [Русский][readme.ru]

## **OOP (Object Oriented Programming)** – is a programming paradigm where the main concepts are the concepts of **objects** and **classes**.

**Procedural programming languages** are the previous stage in the development of computer languages. Procedural languages include such simple constructs as variables, conditional and looping constructs, methods, arrays, and in some cases structures.

**Object Oriented Programming Languages** is the next stage after procedural programming languages, extending with new syntactic constructions that allow describing objects from objective reality.

**Class is a blueprint** for its future instances and objects. Object is always primary. Before drawing up the blueprint, it is necessary to imagine this object.

**Class in a programming language** – is a syntactic construction consisting of three parts: the class keyword, name (identifier) and body.

**Object (or an instance)** – a certain copy of the class in memory (a class that ended up in memory), the essence of the program execution.

**The "class" construct** is one of the programming language **stereotypes**.

**A stereotype in a programming language** is a construct used in a language.

**Instantiating a class** is creating an instance of it.

**Creating an instance of a class by a strong reference** is the creation of an instance of a class with a reference to the created instance placed in a variable of the type of this class.

**Creating an instance of a class by a weak reference** is the creation of an instance of a class without placing a reference to the created instance in a type variable of this class. In this case, class members can only be accessed at the instantiation site.

**An example of creating a class:**

```cs
сlass Butterfly
{
}
```

**An example of creating an instance of a class by strong reference:**

```cs
сlass Butterfly
{
    Butterfly mahaon = new();
}
```

**An example of creating an instance of a class by weak reference:**

```cs
сlass Butterfly
{
    new Butterfly();
}
```

**The process of creating an instance (and object) of a class:**
-	Specifies the type of the instance (and object);
-	A variable is created on the stack to store a reference to an instance of that type (Optional). The instance address is the address of the first byte at which its body begins;
-	The "assignment" symbol is used;
-	The "new" keyword is used, thanks to which the computer allocates space on the heap to accommodate the instance (object);
-	The constructor (default or custom) is called.

<details>
<summary><b>Classes in the C# programming language</b></summary>

**A class in C# programming language** is a construct that contains fields and methods.

**An object (and instance) in the C# programming language** is an allocated memory area that contains our variables (fields) and methods.

A class can contain in its body: **fields, methods, properties and events**.

Fields define the state, methods define the behavior of an object.

**Objects contain** static fields and all methods.
**Instances contain** non-static fields and are associated with an object.

Class members have **access modifiers**.
**Access modifiers** are keywords that specify the declared accessibility of a member or type.

These fields are accessed through **accessor methods**.

**Access methods are divided into** mutator methods and accessor methods.

**A mutator method (setter)** is a method that changes the value of a field.

**An accessor method (getter)** is a method that returns a field value.

**A property with a single accessor** is a write-only (WriteOnly) or read-only (ReadOnly) property.

**Auto-implemented properties** are a more concise form of properties and make sense when you don't need extra logic in your get and set accessors. When creating auto-implemented properties, the compiler will create a private, anonymous backing field that will be accessible via get and set accessors.

**The default constructor** is a parameterless constructor that can be either explicitly defined by the developer or created by the compiler if no other constructors are explicitly defined in the class.

**Custom constructor** is a constructor with parameters defined by the developer (user of the programming language).

If your class has a custom constructor you must explicitly define the default constructor to call the default constructor.
To call another constructor from one constructor you must use the **this** keyword.

</details>

# **OOP paradigms:**

<details>
<summary><b style="font-size: 1.2em">1.	Encapsulation;</b></summary>

**Encapsulation** is a system capability allows you to combine data and methods work with them in a class and hide implementation details from the user.

Encapsulation forms:

1. **Hiding base class members** - using virtual/override modifiers:

    ```cs
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
    ```

2. **Implementation Hiding** – using access modifiers, upcast to base type:

    ```cs
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
                // Error: private methods are not inherited: Uncomment the following
                // code to see the errors.
                # BaseClassPrivateMethod();

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
                // Uncomment the following code to see the errors.
                # BaseClass derivedInBaseClassVariable = new DerivedClass();
                # derivedInBaseClassVariable.DerivedClassPublicMethod();
            }
        }
    ```

3. **Hiding parts of software systems** – encapsulating variations(for example: the "Facade" design pattern):

    ```cs
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
    ```

There is also a separate but similar concept of **"Information hiding"** in computer science which consists of three main parts:

-	**Implementation Hiding** of class members:

    ```cs
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
    ```

-	**Variation encapsulation** as hiding parts of software systems (Like the third form of encapsulation);
-	**Data type hiding** often implemented through the use of variables of types "dynamic" and "var":

    ```cs
        internal class Class
        {
            public int Method1()
            {
                return new int();
            }

            public double Method2()
            {
                return new double();
            }

            public void ClassMethod()
            {
                // Data types are hidden by variables 'var' and 'dynamic':
                var varVariable = Method1();
                dynamic dynamicVariable = Method2();

                Console.WriteLine(varVariable);
                Console.WriteLine(dynamicVariable);
            }
        }
    ```

</details>

<details>
<summary><b style="font-size: 1.2em">2.	Inheritance;</b></summary>

**Inheritance** is a system capability allows you to describe a new class based on an existing one;

Options for naming classes in relationships:
- Base class – Derived class;
- Superclass – Subclass (сабкласс);
- Parent class – Child class;

Example of inheritance implementation:

```cs
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
        // Uncomment the following code to see the errors.
        # BaseClassPrivateMethod();

        // Protected and public methods are inherited, but only available
        // inside the derived classes:
        BaseClassProtectedMethod();
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        BaseClass baseClass = new();
        baseClass.BaseClassPublicMethod();

        DerivedClass derivedClass = new();
        derivedClass.DerivedClassPublicMethod();

        // Public methods are inherited and available outside the derived classes:
        derivedClass.BaseClassPublicMethod();

        Console.WriteLine("Hello, Inheritance!");
    }
}
```

</details>
<details>
<summary><b style="font-size: 1.2em">3.	Polymorphism;</b></summary>

**Polymorphism** is an capability of objects with the same specification to have a different implementation.

Polymorphism forms:

- **Classic forced** has 2 forms:
    1. **Using Virtual Members:** member override implemented by virtual/override modifiers:
    
        ```cs
        internal class BaseClass
        {
            public virtual void Action()
            {
                Console.WriteLine(nameof(BaseClass));
            }
        }

        internal class DerivedClass1 : BaseClass
        {
            public override void Action()
            {
                Console.WriteLine(nameof(DerivedClass1));
            }
        }

        internal class DerivedClass2 : BaseClass
        {
            public override void Action()
            {
                Console.WriteLine(nameof(DerivedClass2));
            }
        }

        internal class ClassDemo
        {
            public virtual void MethodDemo()
            {
                BaseClass instance = new();
                instance.Action();

                // Overriden method of the derived class replaces method of the base class:
                instance = new DerivedClass1();
                instance.Action();

                // Overriden method of the another derived class replaces overriden
                // method of the previous class:
                instance = new DerivedClass2();
                instance.Action();
            }
        }
        ```

    2. **Hiding class members** implemented by casting to the base type:
    
        ```cs
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
                // Instance of the derived class type in the variable of the base
                // class type has only access to base class method:
                //Uncomment the following code to see the errors.
                # baseClass.DerivedClassMethod();
                baseClass.BaseClassMethod();
            }
        }
        ```

    If there are two forms of polymorphism in one construction at once the first form neutralizes the second.

- **Ad Hoc polymorphism** is method overload based polymorphism:

        ```cs
        internal class AdHoc
        {
            public void AdHocMethod(int arg)
            {
                Console.WriteLine("int arg method");
            }

            public void AdHocMethod(string arg)
            {
                Console.WriteLine("string arg method");
            }

            public void AdHocMethod(bool arg)
            {
                Console.WriteLine("bool arg method");
            }
        }

        internal class ClassDemo
        {
            public void MethodDemo()
            {
                AdHoc adHoc = new();

                // Different method implementations are called when arguments of
                // different types are passed to a method:
                adHoc.AdHocMethod(new int());
                adHoc.AdHocMethod(new string('-', 10));
                adHoc.AdHocMethod(new bool());
            }
        }
        ```

- **Duck Typing Polymorphism**:
    - Implementation via **dynamic** type:

        ```cs
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
        ```

    - Implementation via **inheriting derived classes from a common base type**:

        ```cs
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
        ```

</details>
<details>
<summary><b style="font-size: 1.2em">4.	Abstraction;</b></summary>

**Abstraction (noun)** is giving an object characteristics that clearly define its conceptual boundaries distinguishing it from all other objects. Allows you to work with the object without going into implementation details.

**Abstractioning** is a way to extract a set of significant characteristics of an object excluding insignificant ones from consideration. Accordingly, an abstraction is a set of all such characteristics.

**An Interface** is a semantic and syntactic construct in program code used to specify the services provided by a class or component.

**The "interface" construct in a programming language** is a stereotype which is an analogue of a pure abstract class in which any implementation is prohibited.

```cs
internal abstract class ClassBase
{
    public abstract int Function(string data);
}

internal interface IFunction
{
    public int Function(string data);
}

internal class DerivedClass1 : ClassBase
{
    public override int Function(string data)
    {
        return data.Length;
    }

    public void Procedure(string data)
    {
        Console.WriteLine(data);
    }
}

internal class DerivedClass2 : IFunction
{
    public int Function(string data)
    {
        return data.Length;
    }

    public void Procedure(string data)
    {
        Console.WriteLine(data);
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        // Abstraction discards implementation members it doesn't have:
        ClassBase instance1 = new DerivedClass1();
        instance1.Function(nameof(ClassBase));
        //Uncomment the following code to see the errors.
        #instance1.Procedure(nameof(DerivedClass1));

        IFunction instance2 = new DerivedClass2();
        instance1.Function(nameof(IFunction));
        //Uncomment the following code to see the errors.
        #instance1.Procedure(nameof(DerivedClass2));

        Console.WriteLine("Hello, Abstraction!");
    }
}
```

</details>
<details>
<summary><b style="font-size: 1.2em">5.	Sending messages;</b></summary>

**Sending messages** is a way to transfer control to an object. If the object is to respond to this message then it must have a method corresponding to this message;

In other words, this is the organization of information flows between objects:

```cs
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
```

</details>
<details>
<summary><b style="font-size: 1.2em">6.	Reuse.</b></summary>

**Reuse** is a paradigm in OOP that states that programs (computer programs, program modules) should partially or completely consist of parts, previously written components and/or parts of other programs (systems). This is the main methodology that is used to reduce labor costs in the development of complex systems.

The category of reuse includes anything that is used more than once: methods, classes/structs, inheritance, libraries, and frameworks.

The code of the class to be reused from the same assembly where the reuse is made:

```cs
namespace Reuse
{
    internal class ReusableClass1
    {
        public void ReusableMethod()
        {
        }
    }
}

```

Class code to be reused from another assembly (for example: a library):

```cs
namespace ReusableCodeLibrary
{
    public class ReusableClass3
    {
        public void ReusableMethod()
        {
        }
    }
}
```

The code of the class that performs the reuse:

```cs
namespace Reuse
{
    internal class ReusableClass2
    {
        private ReusableClass1 ReusableClass1 { get; set; } = new();

        private ReusableClass3 ReusableClass3 { get; set; } = new();

        public void ReusableMethod()
        {
            ReusableClass1.ReusableMethod();
            ReusableClass3.ReusableMethod();
        }
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        // Reuse of instructions in methods and objects described earlier:
        ReusableClass2 reusableClass2 = new();
        reusableClass2.ReusableMethod();

        ReusableClass2 reusableClass1 = new();
        reusableClass2.ReusableMethod();

        ReusableClass2 reusableClass3 = new();
        reusableClass3.ReusableMethod();

        Console.WriteLine("Hello, Reuse!");
    }
}
```

</details>



<!-- LINKS -->

[readme.ru]: ./README_RU.md
[readme.ua]: ./README_UA.md
