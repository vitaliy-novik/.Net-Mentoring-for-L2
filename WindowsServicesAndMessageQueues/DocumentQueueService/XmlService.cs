using Common;
using System.IO;
using System.Linq;
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

		public void SaveStatus(Status status)
		{
			XDocument doc = XDocument.Load(path);
			var clients = doc.Element("Clients");
			var client = clients.Elements("Client").FirstOrDefault(e => e.Attribute("id").Value == status.ClientId);
			if (client == null)
			{
				clients.Add(
					new XElement(
						"Client", 
						new XAttribute("id", status.ClientId), 
						status.ClientStatus.ToString()));
			}
			else
			{
				client.Value = status.ClientStatus.ToString();
			}

			doc.Save(path);
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
