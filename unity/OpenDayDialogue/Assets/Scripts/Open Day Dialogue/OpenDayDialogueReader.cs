using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenDayDialogue
{
	public class OReader : BinaryReader
	{
		Encoding encoding;

		public OReader(Stream input) : base(input, Encoding.UTF8)
		{
			this.encoding = Encoding.UTF8;
		}

		public OReader(Stream input, Encoding encoding) : base(input, encoding)
		{
			this.encoding = encoding;
		}

		private byte[] ReadLittleEndianBytes(int count)
		{
			byte[] data = base.ReadBytes(count);
			if(!BitConverter.IsLittleEndian)
				Array.Reverse(data);
			return data;
		}

		public override ushort ReadUInt16()
		{
			return BitConverter.ToUInt16(ReadLittleEndianBytes(2), 0);
		}

		public override short ReadInt16()
		{
			return BitConverter.ToInt16(ReadLittleEndianBytes(2), 0);
		}

		public override uint ReadUInt32()
		{
			return BitConverter.ToUInt32(ReadLittleEndianBytes(4), 0);
		}

		public override int ReadInt32()
		{
			return BitConverter.ToInt32(ReadLittleEndianBytes(4), 0);
		}

		public override ulong ReadUInt64()
		{
			return BitConverter.ToUInt64(ReadLittleEndianBytes(8), 0);
		}

		public override long ReadInt64()
		{
			return BitConverter.ToInt64(ReadLittleEndianBytes(8), 0);
		}

		public override double ReadDouble()
		{
			return BitConverter.ToDouble(ReadLittleEndianBytes(8), 0);
		}

		public string ReadZeroTermString()
		{
			List<byte> data = new List<byte>();

			byte b = base.ReadByte();
			while(b != 0 && base.BaseStream.Position < base.BaseStream.Length)
			{
				data.Add(b);
				b = base.ReadByte();
			}

			return encoding.GetString(data.ToArray());
		}
	}
}