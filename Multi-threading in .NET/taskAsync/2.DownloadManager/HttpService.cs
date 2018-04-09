using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2.DownloadManager
{
	class HttpService
	{
		public async Task<Stream> GetStreamAsync(string url, CancellationToken cToken)
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = await client.GetAsync(url, cToken);

			Stream responseStream = await response.Content.ReadAsStreamAsync();

			return responseStream;
		}
	}
}
