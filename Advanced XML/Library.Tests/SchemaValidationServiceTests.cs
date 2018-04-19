using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
	[TestClass]
	public class SchemaValidationServiceTests
	{
		private SchemaValidationService schemaValidationService;

		[TestInitialize]
		public void Initialize()
		{
			this.schemaValidationService = new SchemaValidationService();
		}

		[TestMethod]
		public void Validate_Valid()
		{
			bool valid = this.schemaValidationService.Validate("books.xml");

			Assert.IsTrue(valid);
		}

		[TestMethod]
		public void Validate_Invalid()
		{
			List<string> errors = new List<string>();

			bool valid = this.schemaValidationService.Validate("booksInvalid.xml", errors);

			Assert.IsFalse(valid);
			foreach (var item in errors)
			{
				Console.WriteLine(item);
			}
		}
	}
}
