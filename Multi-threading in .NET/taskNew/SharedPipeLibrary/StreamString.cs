using System;
using System.IO.Pipes;
using System.Text;

namespace SharedPipeLibrary
{
	public class StreamString
	{
		private PipeStream pipeStream;
		private UnicodeEncoding streamEncoding;

		public StreamString(PipeStream pipeStream)
		{
			this.pipeStream = pipeStream;
			streamEncoding = new UnicodeEncoding();
		}

		public string ReadString()
		{
			int len = 0;

			len = pipeStream.ReadByte() * 256;
			len += pipeStream.ReadByte();
			byte[] inBuffer = new byte[len];
			pipeStream.Read(inBuffer, 0, len);

			return streamEncoding.GetString(inBuffer);
		}

		public int WriteString(string outString)
		{
			byte[] outBuffer = streamEncoding.GetBytes(outString);
			int len = outBuffer.Length;
			if (len > UInt16.MaxValue)
			{
				len = (int)UInt16.MaxValue;
			}
			pipeStream.WriteByte((byte)(len / 256));
			pipeStream.WriteByte((byte)(len & 255));
			pipeStream.Write(outBuffer, 0, len);
			pipeStream.Flush();

			return outBuffer.Length + 2;
		}
	}
}
