using System.Collections.Generic;


namespace Nez.Persistence.Binary {
	public static class IPersistableWriterExtensions {
		public static void Write(this IPersistableWriter self, bool? value) {
			bool hasValue = value.HasValue;
			self.Write(hasValue);

			if (hasValue) {
				self.Write(value.Value);
			}
		}

		public static void Write(this IPersistableWriter self, List<string> list) {
			int cnt = list.Count;
			self.Write(cnt);

			for (int i = 0; i < cnt; i++) {
				self.Write(list[i]);
			}
		}

		public static void Write(this IPersistableWriter self, List<int> list) {
			int cnt = list.Count;
			self.Write(cnt);

			for (int i = 0; i < cnt; i++) {
				self.Write(list[i]);
			}
		}

		public static void Write(this IPersistableWriter self, List<float> list) {
			int cnt = list.Count;
			self.Write(cnt);

			for (int i = 0; i < cnt; i++) {
				self.Write(list[i]);
			}
		}

		public static void Write(this IPersistableWriter self, string[] arr) {
			self.Write(arr.Length);
			for (int i = 0; i < arr.Length; i++) {
				self.Write(arr[i]);
			}
		}

		public static void Write(this IPersistableWriter self, int[] arr) {
			self.Write(arr.Length);
			for (int i = 0; i < arr.Length; i++) {
				self.Write(arr[i]);
			}
		}

		public static void Write(this IPersistableWriter self, float[] arr) {
			self.Write(arr.Length);
			for (int i = 0; i < arr.Length; i++) {
				self.Write(arr[i]);
			}
		}

		public static void Write(this IPersistableWriter self, IPersistable[] arr) {
			self.Write(arr.Length);
			for (int i = 0; i < arr.Length; i++) {
				self.Write(arr[i]);
			}
		}
	}
}