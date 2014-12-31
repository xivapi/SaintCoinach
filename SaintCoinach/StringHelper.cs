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

        static readonly Encoding Encoding = Encoding.UTF8;

        #region Types
        private static Dictionary<byte, string> SpecialTypeNames = new Dictionary<byte, string> {
            { 0x08, "IF" },
            { 0x09, "SWITCH" },
            { 0x10, Environment.NewLine },
            { 0x11, "WAIT" },
            { 0x12, "GUI" },
            { 0x13, "COLOR" },
            { 0x1A, "EM" },
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
            { 0x1A, FormattingProcessor },
            { 0x1F, SkipProcessor },
        };
        #endregion

        const int SpecialTypeOffset = 0x01;
        const int SpecialLengthOffset = 0x02;

        private static byte[] DecodeToBinary(byte[] binary) {
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
                        type = string.Format("{0:X2}", typeKey);
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

            return decoded;
        }
        public static string Decode(byte[] binary) {
            var decoded = DecodeToBinary(binary);

            return Encoding.GetString(decoded);
        }

        #region Processors
        private static void DefaultProcessor(byte[] data, ref byte[] output, int offset, string type) {
            var temp = new List<byte>();

            var isClose = false;
            //if (Debug) {
            var length = data[offset + SpecialLengthOffset];
            var paramOffset = offset + 3;

            if (length >= 2) {
                if (length == 2 && data[paramOffset] == 0xEC)
                    isClose = true;
                else {
                    var isFirst = true;

                    temp.Add((byte)'(');

                    for (var i = 0; i < length - 1 && paramOffset + i < data.Length; ++i) {
                        if (isFirst)
                            isFirst = false;
                        else
                            temp.Add((byte)',');

                        switch (data[paramOffset + i]) {
                            case 0xFE: {
                                var v = BitConverter.ToInt32(data, paramOffset + i + 1);
                                temp.AddRange(Encoding.GetBytes(v.ToString("X8")));
                                i += 4;
                                } break;
                            case 0xFA: {
                                var v = 0;
                                v |= data[paramOffset + i + 0] << 16;
                                v |= data[paramOffset + i + 1] << 8;
                                v |= data[paramOffset + i + 2];
                                temp.AddRange(Encoding.GetBytes(v.ToString("X6")));
                                i += 3;
                                } break;
                            default: {
                                temp.Add((byte)'?');
                                var startI = i;
                                for (; i < length - 1 && paramOffset + i < data.Length; ++i)
                                    temp.AddRange(Encoding.GetBytes(data[paramOffset + i].ToString("X2")));
                                /*var inner = new byte[i - startI];
                                Array.Copy(data, paramOffset + startI, inner, 0, inner.Length);
                                temp.Add((byte)'[');
                                temp.AddRange(DecodeToBinary(inner));
                                temp.Add((byte)']');*/
                                } break;
                        }
                    }

                    /*var builder = new StringBuilder();
                    for (int i = 0; i < length - 1 && paramOffset + i < data.Length; ++i)    // Last one is always 03h
                        builder.Append(data[paramOffset + i].ToString("X2"));
                    temp.AddRange(Encoding.GetBytes(builder.ToString()));*/

                    temp.Add((byte)')');
                }
            }
            //}

            var startStr = "<";
            if (isClose)
                startStr += "/";
            startStr += type;
            var start = Encoding.GetBytes(startStr);
            for (var i = 0; i < start.Length; ++i)
                temp.Insert(i, start[i]);
            temp.Add((byte)'>');

            AddRange(ref output, temp.ToArray(), 0, temp.Count);
        }
        private static void SkipProcessor(byte[] data, ref byte[] output, int offset, string type) {
            var b = Encoding.GetBytes(type);
            AddRange(ref output, b, 0, b.Length);
        }
        private static void FormattingProcessor(byte[] data, ref byte[] output, int offset, string type) {
            var fType = data[offset + 3];

            string str;
            if (fType == 0x01)
                str = string.Format("</{0}>", type);
            else if (fType == 0x02)
                str = string.Format("<{0}>", type);
            else
                str = string.Format("<?{0}({1})>", type, fType);

            var b = Encoding.GetBytes(str);
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
