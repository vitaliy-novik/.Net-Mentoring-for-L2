using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocumentQueueService
{
	class XmlService
	{
		private readonly string path;

		public XmlService(string path)
		{
			this.path = path;
			if (!File.Exists(path))
			{
				this.CreateXmlStub(path);
			}
		}

		public void SaveStatus(ServiceState state)
		{
			XDocument doc = XDocument.Load(path);
		}

		private void CreateXmlStub(string path)
		{
			XDocument doc = new XDocument(
				new XDeclaration("1.0", "utf-8", string.Empty),
				new XElement("Clients")
			);

			doc.Save(path);
		}
	}
}
