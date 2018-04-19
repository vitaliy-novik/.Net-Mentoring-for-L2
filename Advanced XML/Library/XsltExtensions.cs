namespace Library
{
	public class XsltExtensions
	{
		private const string uri = "http://my.safaribooksonline.com/{0}/";

		public string FormatUri(string isbn)
		{
			return string.Format(uri, isbn);
		}
	}
}
