using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mapper.Tests.Entities;

namespace Mapper.Tests
{
	[TestClass]
	public class MapperTests
	{
		[TestMethod]
		public void Map()
		{
			MappingGenerator mapGenerator = new MappingGenerator();
			Mapper<Foo, Bar> mapper = mapGenerator.Generate<Foo, Bar>();
			Foo foo = new Foo
			{
				Name = "Jesse",
				Age = 23
			};

			Bar bar = mapper.Map(foo);
			Assert.AreEqual(foo.Name, bar.Name);
			Assert.AreEqual(foo.Age, bar.Age);
		}
	}
}
