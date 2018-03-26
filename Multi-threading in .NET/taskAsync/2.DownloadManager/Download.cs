using System.Threading;

namespace _2.DownloadManager
{
	class Download
	{
		public string Url { get; set; }

		public DownloadStatus Status { get; set; }

		public CancellationTokenSource Cts { get; set; }
	}
}
