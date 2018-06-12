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
		private static object lockObj = new object();

		public static void LogCall(MethodBase method, object[] args)
		{
			Log(writer =>
			{
				writer.WriteStartElement("call");
				writer.WriteAttributeString("time", DateTime.Now.ToString());
				writer.WriteAttributeString("name", method.Name);

				foreach (ParameterInfo param in method.GetParameters())
				{
					writer.WriteStartElement(param.Name);
					Serialize(writer, args[param.Position], param.ParameterType);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
			});
		}

		public static void LogRet(MethodInfo method, object returnValue)
		{
			Log(writer =>
			{
				writer.WriteStartElement("return");
				writer.WriteAttributeString("time", DateTime.Now.ToString());
				writer.WriteAttributeString("name", method.Name);

				if (method.ReturnType.Equals(typeof(void)))
				{
					writer.WriteString("void");
				}
				else
				{
					Serialize(writer, returnValue, method.ReturnType);
				}

				writer.WriteEndElement();
			});
		}

		private static void Serialize(XmlWriter xmlWriter, object obj, Type type)
		{
			XmlSerializer serializer = new XmlSerializer(type);
			try
			{
				serializer.Serialize(xmlWriter, obj);
			}
			catch (InvalidOperationException)
			{
				xmlWriter.WriteString("Not serializable");
			}
		}

		private static void Log(Action<XmlWriter> action)
		{
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				action(writer);
			}

			Commit(sb.ToString());
		}

		private static void Commit(string log)
		{
			lock (lockObj)
			{
				using (StreamWriter sw = File.AppendText("logs.xml"))
				{
					sw.WriteLine(log);
				}
			}
		}
	}
}
