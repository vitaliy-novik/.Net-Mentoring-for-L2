using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

		public DownloadManager()
		{
			InitializeComponent();
			downloads = new List<Download>();
		}

		private async void DownloadButton_Click(object sender, EventArgs e)
		{
			string input = this.urlTextBox.Text;
			string[] urls = input.Split(' ');
			

			urlTextBox.Clear();

			foreach (var item in urls)
			{
				CancellationTokenSource cts = new CancellationTokenSource();
				this.downloads.Add(new Download
				{
					Url = item,
					Cts = cts
				});
				int rowNumber = this.downloadsTable.Rows.Add();
				this.downloadsTable.Rows[rowNumber].Cells["Url"].Value = item;
				this.downloadsTable.Rows[rowNumber].Cells["Status"].Value = "Downloading";
				this.downloadsTable.Rows[rowNumber].Cells["Cancel"].Value = "Cancel";
				try
				{
					int contentLength = await AccessTheWebAsync(item, cts.Token);
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
