using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Tests
{
	[TestClass]
	public class TransformationServiceTests
	{
		private TransformationService transformationService;

		[TestInitialize]
		public void Initialize()
		{
			this.transformationService = new TransformationService();
		}

		[TestMethod]
		public void TransformToRss()
		{
			this.transformationService.TransformToRss("books.xml", "../../rss.xml");
		}

		[TestMethod]
		public void TransformToHtml()
		{
			this.transformationService.TransformToHtml("books.xml", "../../report.html");
		}
	}
}
