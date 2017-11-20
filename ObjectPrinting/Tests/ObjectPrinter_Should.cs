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
            person = new Person { Name = "Alex", Age = 19, Height = 12.4};
        }
        [Test]
        public void ExcludeType_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                "Person",
                "\tId = Guid",
                $"\tHeight = {person.Height}",
                $"\tAge = {person.Age}")
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
                              $"\tName = {person.Name}",
                              $"\tHeight = {person.Height}",
                              $"\tAge = {person.Age+1}")
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
                              $"\tName = {person.Name}",
                              $"\tHeight = {person.Height.ToString(CultureInfo.GetCultureInfo("ru"))}",
                              $"\tAge = {person.Age}")
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
                              $"\tName = {person.Name}",
                              $"\tHeight = {person.Height.GetType().ToString()}",
                              $"\tAge = {person.Age}")
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
                              $"\tName = {person.Name.Substring(0,3)}",
                              $"\tHeight = {person.Height}",
                              $"\tAge = {person.Age}")
                          + Environment.NewLine;
            var actual = ObjectPrinter.For<Person>()
                .Printing(p => p.Name).TrimToLength(3)
                .PrintToString(person);
            actual.Should().Be(expected);
        }

        [Test]
        public void ExcludeProperty_Test()
        {
            var expected =string.Join(Environment.NewLine,  
                              "Person",
                              "\tId = Guid",
                              $"\tHeight = {person.Height}",
                              $"\tAge = {person.Age}")
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
                              $"\tHeight = {person.Height}",
                              $"\tAge = {person.Age}")
                          + Environment.NewLine;
            var actual = person.PrintToString(
                o => o
                    .ExcludeProp(p => p.Name)
            );
            actual.Should().Be(expected);
        }
    }
}