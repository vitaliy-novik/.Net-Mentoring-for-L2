using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ImageBondingService.Logging
{
	static class Logger
	{

		public static void LogCall(MethodBase method, object[] args)
		{
			StringBuilder sb = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(sb))
			{
				writer.WriteStartElement("call");
				writer.WriteAttributeString("time", DateTime.Now.ToString());
				writer.WriteAttributeString("name", method.Name);

				foreach (ParameterInfo param in method.GetParameters())
				{
					writer.WriteStartElement(param.Name);
					XmlSerializer serializer = new XmlSerializer(param.ParameterType);
					serializer.Serialize(writer, args[param.Position]);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
			}

			Commit(sb.ToString());
		}

		public static void LogRet(MethodInfo method, object returnValue)
		{
			StringBuilder sb = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(sb))
			{
				writer.WriteStartElement("return");
				writer.WriteAttributeString("time", DateTime.Now.ToString());
				writer.WriteAttributeString("name", method.Name);

				XmlSerializer serializer = new XmlSerializer(method.ReturnType);
				serializer.Serialize(writer, returnValue);

				writer.WriteEndElement();
			}

			Commit(sb.ToString());
		}

		private static void Commit(string log)
		{
			using (StreamWriter sw = File.AppendText("logs.xml"))
			{
				sw.WriteLine(log);
			}
		}
	}
}
