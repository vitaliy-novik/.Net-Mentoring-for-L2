using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Library
{
	public class SchemaValidationService
	{
		private XmlReaderSettings settings;

		public SchemaValidationService()
		{
			this.settings = new XmlReaderSettings();
		}

		public bool Validate(string inputFile, ICollection<string> errors = null)
		{
			string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.xsd");
			settings.Schemas.Add("http://library.by/catalog", schemaPath);
			bool isValid = true;
			settings.ValidationEventHandler += (sender, args) =>
				{
					isValid = false;
					errors?.Add($"[{args.Exception.LineNumber}:{args.Exception.LinePosition}] {args.Message}");
				};

			settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
			settings.ValidationType = ValidationType.Schema;

			XmlReader reader = XmlReader.Create(inputFile, settings);
			while (reader.Read());

			return isValid;
		}
	}
}
