using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2.DownloadManager
{
	public partial class DownloadManager : Form
	{
		List<Download> downloads;
		HttpService httpService;
		FileService fileService;

		public DownloadManager()
		{
			InitializeComponent();
			downloads = new List<Download>();
			httpService = new HttpService();
			fileService = new FileService();
		}

		private async void DownloadButton_Click(object sender, EventArgs e)
		{
			string input = this.urlTextBox.Text;
			string[] urls = input.Split(' ');

			//urlTextBox.Clear();

			foreach (var item in urls)
			{
				CancellationTokenSource cts = new CancellationTokenSource();
				Download download = new Download
				{
					Url = item,
					Cts = cts,
					Status = DownloadStatus.NotStarted
				};
				this.downloads.Add(download);
				int rowNumber = AddRaw(download);
				Stream stream;
				try
				{
					stream = await this.httpService.GetStreamAsync(item, cts.Token);
					await fileService.SaveToFileAsync(stream, rowNumber.ToString());
					this.downloadsTable.Rows[rowNumber].Cells["Status"].Value = "Succeed";
				}
				catch (OperationCanceledException)
				{
					this.downloadsTable.Rows[rowNumber].Cells["Status"].Value = "Canceled";
				}
				catch (Exception)
				{
					this.downloadsTable.Rows[rowNumber].Cells["Status"].Value = "Failed";
				}
			}

		}

		private int AddRaw(Download download)
		{
			int rowNumber = this.downloadsTable.Rows.Add();
			this.downloadsTable.Rows[rowNumber].Cells["Url"].Value = download.Url;
			this.downloadsTable.Rows[rowNumber].Cells["Status"].Value = "Downloading";
			this.downloadsTable.Rows[rowNumber].Cells["Cancel"].Value = "Cancel";

			return rowNumber;
		}

		private void downloadsTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			var senderGrid = (DataGridView)sender;
			if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
				e.RowIndex >= 0)
			{
				string url = senderGrid.Rows[e.RowIndex].Cells["Url"].Value.ToString();
				downloads.First(d => d.Url == url).Cts.Cancel();
			}
		}

		async Task<int> AccessTheWebAsync(string url, CancellationToken ct)
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = await client.GetAsync(url, ct);

			byte[] urlContents = await response.Content.ReadAsByteArrayAsync();

			return urlContents.Length;
		}

	}
}
