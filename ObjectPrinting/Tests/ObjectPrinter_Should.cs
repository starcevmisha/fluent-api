using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{

    [TestFixture]
    public class ObjectPrinter_Should
    {
        private Person person;

        [SetUp]
        public void SetUp()
        {
            person = new Person { Name = "Alex", SurName = "Petrov",Age = 19, Height = 12.4};
        }
        [Test]
        public void ExcludeType_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                "Person",
                "\tId = Guid",
                "\tChild = null",
                "\tHeight = 12,4",
                "\tAge = 19")
                + Environment.NewLine;
            var actual = ObjectPrinter.For<Person>()
                .ExcludeType<string>()
                .PrintToString(person);

            actual.Should().Be(expected);
        }

        [Test]
        public void AlternativeSerializationMethodTest()
        {
            var expected =string.Join(Environment.NewLine,  
                              "Person",
                              "\tId = Guid",
                              "\tName = Alex",
                              "\tChild = null",
                              "\tSurName = Petrov",
                              "\tHeight = 12,4",
                              "\tAge = 20")
                          + Environment.NewLine;
            var actual = ObjectPrinter.For<Person>()
                .Printing<int>().Using(i => (i + 1).ToString())
                
                .PrintToString(person);
            actual.Should().Be(expected);
        }

        [Test]
        public void UseSpecificCulture_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                              "Person",
                              "\tId = Guid",
                              "\tName = Alex",
                              "\tChild = null",
                              "\tSurName = Petrov",
                              "\tHeight = 12,4",
                              "\tAge = 19")
                          + Environment.NewLine;
            var actual = ObjectPrinter.For<Person>()
                .Printing<double>().Using(CultureInfo.GetCultureInfo("ru"))
                .PrintToString(person);
            actual.Should().Be(expected);
            
        }

        [Test]
        public void SpecificSerializeForProperty_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                              "Person",
                              "\tId = Guid",
                              "\tName = Alex",
                              "\tChild = null",
                              "\tSurName = Petrov",
                              "\tHeight = System.Double",
                              "\tAge = 19")
                          + Environment.NewLine;
            var actual = ObjectPrinter.For<Person>()
                .Printing(p => p.Height).Using(height => (height.GetType()).ToString())
                .PrintToString(person);
            actual.Should().Be(expected);
        }

        [Test]
        public void StringTrimming_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                              "Person",
                              "\tId = Guid",
                              "\tName = Al",
                              "\tChild = null",
                              "\tSurName = Petro",
                              "\tHeight = 12,4",
                              "\tAge = 19")
                          + Environment.NewLine;
            var actual = ObjectPrinter.For<Person>()
                .Printing(p => p.Name).TrimToLength(2)
                .Printing(p => p.SurName).TrimToLength(5)
                .PrintToString(person);
            actual.Should().Be(expected);
        }

        [Test]
        public void ExcludeProperty_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                              "Person",
                              "\tId = Guid",
                              "\tChild = null",
                              "\tSurName = Petrov",
                              "\tHeight = 12,4",
                              "\tAge = 19")
                          + Environment.NewLine;
            var actual = ObjectPrinter.For<Person>()
                .ExcludeProp(p => p.Name)
                .PrintToString(person);
            actual.Should().Be(expected);
        }

        [Test]
        public void SyntaxSugarExtensions_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                              "Person",
                              "\tId = Guid",
                              "\tChild = null",
                              "\tSurName = Petrov",
                              "\tHeight = 12,4",
                              "\tAge = 19")
                          + Environment.NewLine;
            var actual = person.PrintToString(
                o => o
                    .ExcludeProp(p => p.Name)
            );
            actual.Should().Be(expected);
        }

        [Test]
        public void MaxNestedLevel_Is1_Test()
        {
            var testPerson = new Person
            {
                Name = "Alex", Age = 19, Height = 12.4,
                Child = new Person {
                    Name = "Alex", Age = 5, Height = 13,
                    Child = new Person{Name = "Ivan", Age = 1, Height = 10}
                }
            };
            
            var expected = string.Join(Environment.NewLine,
                               "Person",
                               "\tId = Guid",
                               "\tName = Alex",
                               "\tChild = Person{}",
                               "\tSurName = null",
                               "\tHeight = 12,4",
                               "\tAge = 19"
                           ) + Environment.NewLine;
            var actual = testPerson.PrintToString(
                o => o
                    .SetMaxNestedLevel(1)
                );
            actual.Should().Be(expected);
        }
        [Test]
        public void MaxNestedLevel_Is2_Test()
        {
            var testPerson = new Person
            {
                Name = "Alex", Age = 19, Height = 12.4,
                Child = new Person {
                    Name = "Alex", Age = 5, Height = 13,
                    Child = new Person{Name = "Ivan", Age = 1, Height = 10}
                }
            };
            
            var expected = string.Join(Environment.NewLine,
                               "Person",
                               "\tId = Guid",
                               "\tName = Alex",
                               "\tChild = Person",
                               "\t\tId = Guid",
                               "\t\tName = Alex",
                               "\t\tChild = Person{}",
                               "\t\tSurName = null",
                               "\t\tHeight = 13",
                               "\t\tAge = 5",
                               "\tSurName = null",
                               "\tHeight = 12,4",
                               "\tAge = 19"
                           ) + Environment.NewLine;
            var actual = testPerson.PrintToString(
                o => o
                    .SetMaxNestedLevel(2)
            );
            actual.Should().Be(expected);
        }
    }
}