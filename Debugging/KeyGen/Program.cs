using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace KeyGen
{
	class Program
	{
		static void Main(string[] args)
		{
			var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
			var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
			var date = DateTime.Now.Date;
			var dateBytes = BitConverter.GetBytes(date.ToBinary());
			var source = addressBytes
				.Select((b, i) => b ^ dateBytes[i])
				.Select(e => e * 10)
				.ToArray();

			Console.WriteLine(string.Join("-", source.ToArray()));
			Console.ReadKey();
		}
	}
}
