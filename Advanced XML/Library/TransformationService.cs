using System;
using System.IO;
using System.Xml.Xsl;

namespace Library
{
	public class TransformationService
	{
		public void TransformToRss(string inputFile, string outputFile)
		{
			string rssXsltPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "booksRss.xslt");
			XslCompiledTransform xsl = new XslCompiledTransform();
			xsl.Load(rssXsltPath);

			XsltArgumentList xslParams = new XsltArgumentList();
			xslParams.AddExtensionObject(
				"http://epam.com/xsl/ext",
				new XsltExtensions());

			using (FileStream fileStream = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write))
			{
				xsl.Transform(inputFile, xslParams, fileStream);
			}
		}

		public void TransformToHtml(string inputFile, string outputFile)
		{
			string reportXsltPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "htmlReport.xslt");
			XslCompiledTransform xsl = new XslCompiledTransform();
			xsl.Load(reportXsltPath);

			XsltArgumentList xslParams = new XsltArgumentList();
			xslParams.AddParam("Date", "", DateTime.Now.ToShortDateString());

			using (FileStream fileStream = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write))
			{
				xsl.Transform(inputFile, xslParams, fileStream);
			}
		}
	}
}
