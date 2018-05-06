namespace Common
{
	public class Document
	{
		public string ClientId { get; set; }

		public string DocumentId { get; set; }

		public int ChunkNumber { get; set; }

		public int CountOfChunks { get; set; }

		public byte[] Content { get; set; }
	}
}
