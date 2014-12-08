using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach {
    public static class StringHelper {
#if DEBUG
        public static bool Debug = true;
#else
        public static bool Debug = false;
#endif

        #region Types
        private static Dictionary<byte, string> SpecialTypeNames = new Dictionary<byte, string> {
            { 0x08, "IF" },
            { 0x09, "SWITCH" },
            { 0x10, Environment.NewLine },
            { 0x11, "WAIT" },
            { 0x12, "ICON" },
            { 0x1F, "–" },
            { 0x20, "VALUE" },
            { 0x22, "FORMAT" },
            { 0x24, "ZERO" },
            { 0x25, "TIME" },
            { 0x28, "SHEET" },
            { 0x2E, "FIXED" },
            { 0x31, "SHEETEN" },
            { 0x32, "SHEETDE" },
            { 0x33, "SHEETFR" },
        };
        private delegate void SpecialProcessor(byte[] data, ref byte[] output, int offset, string typeName);
        private static Dictionary<byte, SpecialProcessor> SpecialTypeProcessors = new Dictionary<byte, SpecialProcessor> {
            { 0x10, SkipProcessor },
            { 0x1F, SkipProcessor },
        };
        #endregion

        const int SpecialTypeOffset = 0x01;
        const int SpecialLengthOffset = 0x02;

        public static string Decode(byte[] binary) {
            const byte SpecialIdentifier = 0x02;
            const int SpecialMinimumLength = 0x04;

            if (binary == null)
                throw new ArgumentNullException("binary");

            var decoded = new byte[0];

            int currentIndex = 0;
            while (currentIndex < binary.Length) {
                var nextSpecial = Array.IndexOf<byte>(binary, SpecialIdentifier, currentIndex);

                if (nextSpecial < 0 || binary.Length - nextSpecial < SpecialMinimumLength) {
                    AddRange(ref decoded, binary, currentIndex, binary.Length - currentIndex);
                    break;
                }

                AddRange(ref decoded, binary, currentIndex, nextSpecial - currentIndex);

                var typeKey = binary[nextSpecial + SpecialTypeOffset];
                SpecialProcessor processor;
                string type;

                if (!SpecialTypeNames.TryGetValue(typeKey, out type)) {
                    if (Debug)
                        type = string.Format("{0:X2}", type);
                    else
                        type = null;
                }
                if (!SpecialTypeProcessors.TryGetValue(typeKey, out processor))
                    processor = DefaultProcessor;

                var specialLength = binary[nextSpecial + SpecialLengthOffset];
                if (type != null)
                    processor(binary, ref decoded, nextSpecial, type);

                currentIndex = nextSpecial + specialLength + SpecialLengthOffset + 1;
            }

            return Encoding.UTF8.GetString(decoded);
        }

        #region Processors
        private static void DefaultProcessor(byte[] data, ref byte[] output, int offset, string type) {
            var temp = new List<byte>();
            temp.AddRange(Encoding.UTF8.GetBytes("<@" + type));

            if (Debug) {
                var length = data[offset + SpecialLengthOffset];

                temp.Add((byte)'(');

                var builder = new StringBuilder();
                for (int i = 0; i < length - 1 && offset + 3 + i < data.Length; ++i)    // Last one is always 03h
                    builder.Append(data[offset + 3 + i].ToString("X2"));

                temp.Add((byte)')');
            }

            temp.Add((byte)'>');

            AddRange(ref output, temp.ToArray(), 0, temp.Count);
        }
        private static void SkipProcessor(byte[] data, ref byte[] output, int offset, string type) {
            var b = Encoding.UTF8.GetBytes(type);
            AddRange(ref output, b, 0, b.Length);
        }
        #endregion

        #region Extensions
        private static void AddRange(ref byte[] self, byte[] data, int offset, int count) {
            var oLen = self.Length;
            Array.Resize(ref self, oLen + count);
            //Array.Copy(self, oLen, data, offset, count);
            Array.Copy(data, offset, self, oLen, count);
        }
        #endregion
    }
}
