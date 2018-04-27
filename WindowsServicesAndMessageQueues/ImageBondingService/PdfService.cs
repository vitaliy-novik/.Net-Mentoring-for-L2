﻿using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using System.IO;

namespace ImageBondingService
{
	class PdfService
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
			image.Height = document.DefaultPageSetup.PageHeight;
			image.Width = document.DefaultPageSetup.PageWidth;

			section.AddPageBreak();
		}

		public byte[] SaveDocument(string documentPath)
		{
			renderer.Document = this.document;
			renderer.RenderDocument();

			MemoryStream stream = new MemoryStream();
			renderer.Save(stream, false);
			this.ResetDocument();

			return stream.ToArray();
		}

		private void ResetDocument()
		{
			this.renderer = new PdfDocumentRenderer();
			this.document = new Document();
			this.section = this.document.AddSection();
		}
	}
}
