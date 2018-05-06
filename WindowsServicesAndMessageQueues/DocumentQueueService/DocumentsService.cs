using Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocumentQueueService
{
	class DocumentsService
	{
		private string outDir;
		private List<Document> chunks;

		public DocumentsService(string outDir)
		{
			this.outDir = outDir;
			this.chunks = new List<Document>();
		}

		public void SaveDocument(Document doc)
		{
			if (doc.CountOfChunks == 1)
			{
				this.Save(doc);
			}
			else
			{
				this.SaveChunk(doc);
			}
		}

		private void SaveChunk(Document doc)
		{
			this.chunks.Add(doc);
			if (doc.ChunkNumber == doc.CountOfChunks - 1)
			{
				var docChunks = this.chunks.Where(ch => ch.ClientId == doc.ClientId && ch.DocumentId == doc.DocumentId);
				if (docChunks.Count() == doc.CountOfChunks)
				{
					this.Save(docChunks);
				}
				else
				{
					foreach (Document chunk in docChunks)
					{
						this.chunks.Remove(chunk);
					}
				}
			}
		}

		private void Save(IEnumerable<Document> chunks)
		{
			string filePath = Path.Combine(this.outDir, $"{chunks.First().DocumentId}.pdf");
			using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			{
				foreach (Document chunk in chunks)
				{
					fs.Write(chunk.Content, 0, chunk.Content.Length);
				}
			}
		}

		private void Save(Document doc)
		{
			string filePath = Path.Combine(this.outDir, $"{doc.DocumentId}.pdf");
			using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			{
				fs.Write(doc.Content, 0, doc.Content.Length);
			}
		}
	}
}
