using System.IO;


namespace Nez.Persistence.Binary {
	public class BinaryPersistableReader : BinaryReader, IPersistableReader {
		public BinaryPersistableReader(string filename) : base(File.OpenRead(filename)) {
		}

		public BinaryPersistableReader(Stream input) : base(input) {
		}

		public uint ReadUInt() {
			return ReadUInt32();
		}

		public int ReadInt() {
			return ReadInt32();
		}

		public float ReadFloat() {
			return ReadSingle();
		}

		public bool ReadBool() {
			return ReadBoolean();
		}
	}
}