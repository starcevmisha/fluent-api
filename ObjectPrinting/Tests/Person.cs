using System;

namespace ObjectPrinting.Tests
{
	public class Person
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Person Child { get; set; }
		public string SurName { get; set; }
		public double Height { get; set; }
		public int Age { get; set; }
	}
}