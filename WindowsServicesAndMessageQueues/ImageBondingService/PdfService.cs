using ImageBondingService.Interfaces;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using System.IO;

namespace ImageBondingService
{
	public class PdfService : IPdfService
	{
		private PdfDocumentRenderer renderer;
		private Document document;
		private Section section;

		public PdfService()
		{
			this.renderer = new PdfDocumentRenderer();
			this.document = new Document();
			this.section = this.document.AddSection();
		}

		public void InsetImage(string filePath)
		{
			Image image = section.AddImage(filePath);
			image.Width = document.DefaultPageSetup.PageWidth;
		}

		public Stream GetDocument()
		{
			renderer.Document = this.document;
			renderer.RenderDocument();

			MemoryStream stream = new MemoryStream();
			renderer.Save(stream, false);
			this.ResetDocument();

			return stream;
		}

		private void ResetDocument()
		{
			this.renderer = new PdfDocumentRenderer();
			this.document = new Document();
			this.section = this.document.AddSection();
		}
	}
}
