# **Объектно-ориентированное программирование**

Версии на других языках:
- [English][readme.en]
- [Українська][readme.ua]

## **ООП (Объектно-ориентированное программирование)** – парадигма программирования, в которой основными концепциями являются понятия **объектов** и **классов**.

**Процедурные языки программирования** – предшествующий этап в развитии компьютерных языков. Процедурные языки включают в себя такие простейшие конструкции, как переменные, условные и циклические конструкции, методы, массивы, и в отдельных случаях структуры.

**Объектно-ориентированные языки программирования** – следующий этап после процедурных языков программирования, расширяющийся новыми синтаксическими конструкциями, позволяющими описать объекты из объективной реальности.

**Класс играет роль чертежа** для будущих своих экземпляров и объектов. Объект всегда первичен. Перед составлением чертежа необходимо представить этот объект.

**Класс в языке программирования** – синтаксическая конструкция, состоящая из трех частей: ключевое слово class, имя (идентификатор) и тело.

**Объект (или экземпляр)** – некая копия класса в памяти (класс, оказавшийся в памяти), сущность выполнения программы.

**Конструкция "класс"** является одним из **стереотипов** языка программирования.

**Стереотип в языке программирования** – конструкция, используемая в языке.

**Инстанцирование класса** – создание его экземпляра (instance).

**Создание экземпляра класса по сильной ссылке** – создание экземпляра класса с помещением в переменную типа этого класса ссылки на созданный экземпляр.

**Создание экземпляра класса по слабой ссылке** – создание экземпляра класса без помещения в переменную типа этого класса ссылки на созданный экземпляр. В этом случае обращение к членам класса возможно только в месте создания экземпляра.

**Пример создания класса:**

```cs
сlass Butterfly
{
}
```

**Пример создания экземпляра класса по сильной ссылке:**

```cs
сlass Butterfly
{
    Butterfly mahaon = new();
}
```

**Пример создания экземпляра класса по слабой ссылке:**

```cs
сlass Butterfly
{
    new Butterfly();
}
```

**Процесс создания экземпляра (и объекта) класса:**
-	Указывается тип экземпляра (и объекта);
-	В стеке создается переменная для хранения ссылки на экземпляр этого типа (Опционально). Адрес экземпляра – адрес первого байта, с которого начинается его тело;
-	Используется знак "присвоения";
-	Используется ключевое слово "new", благодаря которому компьютер выделяет место на куче (heap) для размещения экземпляра (объекта);
-	Вызывается конструктор (по умолчанию или пользовательский).

<details>
<summary><b>Классы в языке программирования C#</b></summary>

**Класс в языке программирования C#** – это конструкция, содержащая в себе поля и методы.

**Объект (и экземпляр) в языке программирования C#** – это выделенная область памяти, в которой находятся наши переменные (поля) и методы.  

Класс может содержать в своем теле: **поля, методы, свойства и события**.

Поля определяют состояние, а методы поведение будущего объекта.

**Объекты содержат** в себе статические поля и все методы.
**Экземпляры содержат** в себе нестатические поля и связаны с объектом.

Члены класса имеют **модификаторы доступа.**
**Модификаторы доступа** – ключевые слова, задающие объявленную доступность члена или типа.

Доступ к данным полям осуществляется через **методы доступа**.

**Методы доступа делятся на** методы-мутаторы и методы-аксессоры.

**Метод-мутатор (setter)** – метод, изменяющий значение поля.

**Метод-аксессор (getter)** – метод, возвращающий значение поля.

**Свойство с одним методом доступа** – свойство только для записи (WriteOnly) или только для чтения (ReadOnly).

**Автоматически реализуемые свойства** – более лаконичная форма свойств, их есть смысл использовать, когда в методах доступа "get" и "set" не требуется дополнительная логика. При создании автоматически реализуемых свойств компилятор создаст закрытое анонимное резервное поле, которое будет доступно с помощью методов доступа "get" и "set".

**Конструктор по умолчанию** - конструктор без параметров, которым может быть как явно определён разработчиком, 
так и создан компилятором, если в классе явно не определено никаких других конструкторов.

**Пользовательский конструктор** - конструктор с параметрами, определённый разработчиком (пользователем языка программирования).

При наличии в классе пользовательского конструктора для вызова конструктора по умолчанию необходимо его явно определить.
Для вызова из одного конструктора другого конструктора необходимо использовать ключевое слово **this**.

</details>

# **Парадигмы ООП:**

<details>
<summary><b style="font-size: 1.2em">1.	Инкапсуляция;</b></summary>

**Инкапсуляция** - свойство системы, позволяющее объединить данные и методы, работающие с ними, в классе, а также скрыть детали реализации от пользователя.

Формы инкапсуляции:
1. **Сокрытие членов базового класса** - использование модификаторов virtual/override:

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

2. **Сокрытие реализации** – использование модификаторов доступа, приведение к базовому типу:

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

3. **Сокрытие частей программных систем** – инкапсуляция вариаций (например, шаблон проектирования "Facade"):

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

Также в информатике существует отдельное, но похожее понятие **"Сокрытие информации"**, которое состоит из трёх основных частей:

-	**Сокрытие реализации** членов класса:

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

-	**Инкапсуляция вариаций** как сокрытие частей программных систем (Пересекается с третьей формой инкапсуляции);
-	**Сокрытие типов данных**, часто реализуемое через использование переменных типов "dynamic" и "var":

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
<summary><b style="font-size: 1.2em">2.	Наследование;</b></summary>

**Наследование** - свойство системы, позволяющее описать новый класс на основе существующего;

Варианты наименований классов в отношениях:
- Базовый класс – Производный класс;
- Суперкласс – Подкласс (сабкласс);
- Родительский класс – Дочерний класс;
- Класс-родитель – Класс-потомок;

Пример реализации наследования:

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
<summary><b style="font-size: 1.2em">3.	Полиморфизм;</b></summary>

**Полиморфизм** - возможность объектов с одинаковой спецификацией иметь разную реализацию. 

Типы полиморфизма:

- **Классический принудительный** – имеет 2 формы:
    1. **Использование виртуальных членов:** переопределение членов, реализуемое модификаторами virtual/override:
    
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

    2. **Сокрытие членов класса**, реализуемое приведением к базовому типу:
    
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

    При наличии в одной конструкции сразу двух форм полиморфизма, первая форма нейтрализует вторую.

- **Ad Hoc полиморфизм** - полиморфизм, основанный перегрузке методов:

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

- **Полиморфизм "Утиной типизации"**:
    - Реализация через тип **dynamic**:

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

    - Реализация через **наследование наследников от общего базового типа**:

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
<summary><b style="font-size: 1.2em">4.	Абстракция;</b></summary>

**Абстракция** - придание объекту характеристик, чётко определяющих его концептуальные границы, отличая от всех других объектов. Позволяет работать с объектом, не вдаваясь в особенности реализации;

**Абстрагирование** – это способ выделить набор значимых характеристик объекта, исключая из рассмотрения незначимые. Соответственно, абстракция – это набор всех таких характеристик.

**Интерфейс** – семантическая и синтаксическая конструкция в коде программы, используемая для специфицирования услуг, предоставляемых классом или компонентом.

**Конструкция "интерфейс" в языке программирования** – стереотип, являющийся аналогом чистого абстрактного класса, в котором запрещена любая реализация.

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
<summary><b style="font-size: 1.2em">5.	Посылка сообщений;</b></summary>

**Посылка сообщений** - способ передачи управления объекту. Если объект должен отвечать на это сообщение, то у него должен быть метод, соответствующий этому сообщению;

Иначе говоря, это организация информационных потоков между объектами:

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
<summary><b style="font-size: 1.2em">6.	Повторное использование.</b></summary>

**Повторное использование** - парадигма в ООП, в которой утверждается, что программы (компьютерные программы, программные модули) должны частично либо полностью состоять из частей, написанных ранее компонентов и/или частей других программ (систем). Это основная методология, которая применяется для сокращения трудозатрат при разработке сложных систем.

В категорию повторного использования входит всё, используемое более одного раза: методы, классы/структуры, наследование, библиотеки и фреймворки.

Код класса для переиспользования из той же сборки, где производится переиспользование:

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

Код класса для переиспользования из другой сборки (например, библиотеки):

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

Код класса, выполняющего переиспользование:

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

[readme.en]: ./README.md
[readme.ua]: ./README_UA.md
