# **Об'єктно-орієнтоване програмування:**

Версии на других языках:
- [English][readme.en]
- [Русский][readme.ru]

## **ООП (Об'єктно-орієнтоване програмування):** – парадигма програмування, в якій основними концепціями є поняття **об'єктів** та **класів**.

**Процедурні мови програмування** – попередній етап у розвитку комп'ютерних мов. Процедурні мови включають такі прості конструкції, як змінні, умовні і циклічні конструкції, методи, масиви, і в окремих випадках структури.

**Об'єктно-орієнтовані мови програмування** – наступний етап після процедурних мов програмування, що розширюється новими синтаксичними конструкціями, що дозволяють описати об'єкти з об'єктивної реальності.

**Клас грає роль креслення** для майбутніх своїх екземплярів та об'єктів. Об'єкт завжди первинний. Перед складанням креслення необхідно подати цей об'єкт.

**Клас у мові програмування** – синтаксична конструкція, що складається з трьох частин: ключове слово class, ім'я (ідентифікатор) та тіло.

**Об'єкт (або екземпляр)** – певна копія класу у пам'яті (клас, який у пам'яті), сутність виконання програми.

**Конструкція "клас"** є одним із **стереотипів** мови програмування.

**Стереотип у мові програмування** – конструкція, що використовується у мові.

**Інстанція класу** – створення його екземпляра (instance).

**Створення екземпляра класу за сильним посиланням** – створення екземпляра класу з поміщенням у змінну типу цього класу посилання на створений екземпляр.

**Створення екземпляра класу за слабким посиланням** – створення екземпляра класу без поміщення у змінну типу цього класу посилання на створений екземпляр. У цьому випадку звернення до членів класу можливе лише у місці створення екземпляра.

**Приклад створення класу:**

```cs
сlass Butterfly
{
}
```

**Приклад створення екземпляра класу за сильним посиланням:**

```cs
сlass Butterfly
{
    Butterfly mahaon = new();
}
```

**Приклад створення екземпляра класу за слабким посиланням:**

```cs
сlass Butterfly
{
    new Butterfly();
}
```

**Процес створення екземпляра (та об'єкта) класу:**
-	Вказується тип екземпляра (та об'єкта);
-	У стеку створюється змінна для зберігання посилання на екземпляр цього типу (опційно). Адреса екземпляра – адреса першого байта, з якого починається його тіло;
-	Використовується знак "присвоєння";
-	Використовується ключове слово "new", завдяки якому комп'ютер виділяє місце на купі (heap) для розміщення екземпляра (об'єкта);
-	Викликається конструктор (за замовчуванням або користувальницький).

<details>
<summary><b>Класи у мові програмування C#</b></summary>

**Клас у мові програмування C#** – це конструкція, що містить у собі поля та методи.

**Об'єкт (і екземпляр) у мові програмування C#** – це виділена область пам'яті, в якій знаходяться наші змінні (поля) та методи.

Клас може містити у своєму тілі: поля, методи, властивості та події.

Поля визначають стан, а методи поведінки майбутнього об'єкта.

**Об'єкти містять** у собі статичні поля та всі методи.
**Екземпляри містять** у собі нестатичні поля та пов'язані з об'єктом.

Члени класу мають **модифікатори доступу.**
**Модифікатори доступу** – ключові слова, які задають оголошену доступність члена або типу.

Доступ до цих полів здійснюється через **методи доступу**.

**Методи доступу поділяються на** методи-мутатори та методи-аксесори.

**Метод-мутатор (setter)** - метод, що змінює значення поля.

**Метод-аксесор (getter)** – метод, який повертає значення поля.

**Властивість з одним методом доступу** – властивість лише для запису (WriteOnly) або лише для читання (ReadOnly).

**Автоматично реалізовані властивості** – більш лаконічна форма властивостей, їх сенс використовувати, як у методах доступу "get" і "set" не потрібно додаткова логіка. При створенні властивостей, що автоматично реалізуються, компілятор створить закрите анонімне резервне поле, яке буде доступне за допомогою методів доступу "get" і "set".

**Конструктор за замовчуванням** - конструктор без параметрів, яким може бути явно визначений розробником,
так і створений компілятором, якщо в класі не визначено жодних інших конструкторів.

**Користувальницький конструктор** - конструктор з параметрами, визначений розробником (користувачем мови програмування).

За наявності в класі конструктора для виклику конструктора за замовчуванням необхідно його явно визначити.
Для виклику одного конструктора іншого конструктора необхідно використовувати ключове слово **this**.

</details>

# **Парадигмы ООП:**

<details>
<summary><b style="font-size: 1.2em">1.	Інкапсуляція;</b></summary>

**Інкапсуляція** - властивість системи, що дозволяє об'єднати дані та методи, що працюють з ними, у класі, а також приховати деталі реалізації від користувача.

Форми інкапсуляції:
1. **Приховування членів базового класу** - використання модифікаторів virtual/override:

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

2. **Приховування реалізації** – використання модифікаторів доступу, приведення до базового типу:

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

3. **Приховування частин програмних систем** – інкапсуляція варіацій (наприклад, шаблон проектування "Facade"):

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

Також в інформатиці існує окреме, але схоже поняття "Приховування інформації", яке складається з трьох основних частин:

-	**Приховування реалізації** членів класу:

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

-	**Інкапсуляція варіацій** як приховування частин програмних систем (Перетинається з третьою формою інкапсуляції);
-	**Приховування типів даних**, що часто реалізується через використання змінних типів "dynamic" та "var":

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
<summary><b style="font-size: 1.2em">2.	Успадкування;</b></summary>

**Успадкування** - властивість системи, що дозволяє описати новий клас на основі існуючого;

Варіанти найменувань класів у відносинах:
- Базовий клас – Похідний клас;
- Суперклас - Підклас (сабклас);
- Батьківський клас – Дочірній клас;
- Клас-батько – Клас-нащадок;

Приклад реалізації наслідування:

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
<summary><b style="font-size: 1.2em">3.	Поліморфізм;</b></summary>

**Поліморфізм** - можливість об'єктів з однаковою специфікацією мати різну реалізацію.

Типи поліморфізму:

- **Класичний примусовий** – має 2 форми:
    1. **Використання віртуальних членів:** перевизначення членів, що реалізується модифікаторами virtual/override:
    
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

    2. **Приховування членів класу**, що реалізується приведенням до базового типу:
    
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

    За наявності однієї конструкції відразу двох форм поліморфізму, перша форма нейтралізує другу.

- **Ad Hoc поліморфізм** - поліморфізм, заснований на навантаженні методів:

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

- **Поліморфізм "Качиної типізації"**:
    - Реалізація через тип **dynamic**:

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

    - Реалізація через успадкування спадкоємців від загального базового типу:

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
<summary><b style="font-size: 1.2em">4.	Абстракція;</b></summary>

**Абстракція** - надання об'єкту характеристик, що чітко визначають його концептуальні межі, відрізняючи від усіх інших об'єктів. Дозволяє працювати з об'єктом, не вдаючись особливо реалізації;

**Абстрагування** – це спосіб виділити набір значних показників об'єкта, виключаючи з розгляду незначні. Відповідно, абстракція – це набір таких характеристик.

**Інтерфейс** – семантична та синтаксична конструкція у коді програми, що використовується для специфікування послуг, що надаються класом або компонентом.

**Конструкція "інтерфейс" у мові програмування** – стереотип, що є аналогом чистого абстрактного класу, в якому заборонено будь-яку реалізацію.

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
<summary><b style="font-size: 1.2em">5.	Надсилання повідомлень;</b></summary>

**Посилання повідомлень** - спосіб передачі керування об'єкту. Якщо об'єкт повинен відповідати на це повідомлення, то у нього має бути метод, який відповідає цьому повідомленню;

Інакше кажучи, це організація інформаційних потоків між об'єктами:

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
<summary><b style="font-size: 1.2em">6.	Повторне використання.</b></summary>

**Повторне використання** - парадигма в ООП, у якій стверджується, що програми (комп'ютерні програми, програмні модулі) повинні частково чи повністю складатися з частин, написаних раніше компонентів та/або частин інших програм (систем). Це основна методологія, яка застосовується для скорочення трудовитрат розробки складних систем.

У категорію повторного використання входить все, що використовується більше одного разу: методи, класи/структури, успадкування, бібліотеки та фреймворки.

Код класу для перевикористання з тієї ж збірки, де здійснюється перевикористання:

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

Код класу для перевикористання з іншої збірки (наприклад, бібліотеки):

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

Код класу, що виконує перевикористання:

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
[readme.ru]: ./README_RU.md
