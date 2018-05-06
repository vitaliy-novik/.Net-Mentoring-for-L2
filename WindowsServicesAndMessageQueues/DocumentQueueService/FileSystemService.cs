using Common;
using System.IO;
using System.Text;

namespace DocumentQueueService
{
	class FileSystemService
	{
		private string settingsFile;
		private FileSystemWatcher watcher;
		private ServerQueueService queueService;

		public FileSystemService(string settingsFile)
		{
			this.settingsFile = settingsFile;
			if (!File.Exists(settingsFile))
			{
				File.Create(settingsFile);
			}

			this.watcher = new FileSystemWatcher(Path.GetDirectoryName(settingsFile));
			this.watcher.Filter = Path.GetFileName(settingsFile);
			this.queueService = new ServerQueueService();
		}

		public void Start()
		{
			this.SendSettings();
			this.watcher.EnableRaisingEvents = true;
			this.watcher.Changed += this.UpdateSettings;
		}

		private void SendSettings()
		{
			Settings settings = this.ReadSettings();
			this.queueService.SendSettings(settings);
		}

		private void UpdateSettings(object sender, FileSystemEventArgs e)
		{
			this.queueService.RecieveSettings();
			this.SendSettings();
		}

		private Settings ReadSettings()
		{
			using (FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
				{
					string settingsLine = sr.ReadLine();
					int timeout;
					Settings settings = new Settings();
					if (int.TryParse(settingsLine, out timeout))
					{
						settings.Timeout = timeout;
					}

					return settings;
				}
			}
		}
	}
}
