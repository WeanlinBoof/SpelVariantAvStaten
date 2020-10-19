using System.IO;


namespace Nez.Persistence.Binary {
	public class TextPersistableReader : StreamReader, IPersistableReader {
		public TextPersistableReader(string filename) : base(File.OpenRead(filename)) {
		}

		public TextPersistableReader(Stream stream) : base(stream) {
		}

		public string ReadString() {
			int length = ReadInt();
			char[] buff = new char[length];
			ReadBlock(buff, 0, length);

			// chomp the newline that is after our string
			ReadLine();

			return new string(buff);
		}


		public bool ReadBool() {
			return bool.Parse(ReadLine());
		}

		public double ReadDouble() {
			return double.Parse(ReadLine());
		}

		public float ReadFloat() {
			return float.Parse(ReadLine());
		}

		public int ReadInt() {
			return int.Parse(ReadLine());
		}

		public uint ReadUInt() {
			return uint.Parse(ReadLine());
		}
	}
}