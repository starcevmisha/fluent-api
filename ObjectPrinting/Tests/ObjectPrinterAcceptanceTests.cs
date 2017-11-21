using System;
using System.Globalization;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {


        [Test]
        public void Demo()
        {
            var person = new Person { Name = "Alex", Age = 19, Height = 12.4};

            var printer = ObjectPrinter.For<Person>()
                //1. Исключить из сериализации свойства определенного типа
                .ExcludeType<Guid>()
                //2. Указать альтернативный способ сериализации для определенного типа
                .Printing<int>().Using(i => (i+1).ToString())
                //3. Для числовых типов указать культуру
                .Printing<double>().Using(CultureInfo.CurrentCulture)
                //4. Настроить сериализацию конкретного свойства
                .Printing(p => p.Height).Using(height => (height.GetType()).ToString())
                //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
                .Printing(p => p.Name).TrimToLength(3)
                //6. Исключить из сериализации конкретного свойства
                .ExcludeProp(p => p.Name)
                ;
            string s1 = printer.PrintToString(person);
            Console.WriteLine(s1);
            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию		
            string s2 = person.PrintToString();
            Console.WriteLine(s2);
            //8. ...с конфигурированием
            string s3 = person.PrintToString(
                o => o
                    .ExcludeProp(p => p.Name)
                );
            Console.WriteLine(s3);
        }
    }
}