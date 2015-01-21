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
            // { 0x08, IfProcessor }, // TODO: Do the thing again
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

            var fullHex = new StringBuilder();
            for (var i = 0; i < binary.Length; ++i)
                fullHex.AppendFormat("{0:X2}", binary[i]);

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

                int specialLength;
                //var specialLength = binary[nextSpecial + SpecialLengthOffset];  // TODO: Get int proper
                var lengthLength = GetInteger(binary, nextSpecial + SpecialLengthOffset, out specialLength);
                if (type != null) {
                    processor(binary, ref decoded, nextSpecial + SpecialLengthOffset, type);
                }

                var nI = nextSpecial + specialLength + SpecialLengthOffset + lengthLength;
                if (nI < currentIndex)
                    throw new NotSupportedException();
                currentIndex = nI;
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
            int length;
            var paramOffset = offset;
            paramOffset += GetInteger(data, paramOffset, out length);
            //var length = data[offset + SpecialLengthOffset];    // TODO: Get int proper

            if (length >= 2) {
                if (length == 2 && data[paramOffset] == 0xEC)
                    isClose = true;
                else {
                    var isFirst = true;

                    temp.Add((byte)'(');
                    /*
                    for (var i = 0; i < length - 1 && paramOffset + i < data.Length; ++i) {
                        if (isFirst)
                            isFirst = false;
                        else
                            temp.Add((byte)',');

                        int tmpInt;
                        i += GetInteger(data, paramOffset + i, out tmpInt);
                        temp.AddRange(Encoding.GetBytes(tmpInt.ToString()));
                    }
                    */
                    var builder = new StringBuilder();
                    for (int i = 0; i < length - 1 && paramOffset + i < data.Length; ++i)    // Last one is always 03h
                        builder.Append(data[paramOffset + i].ToString("X2"));
                    temp.AddRange(Encoding.GetBytes(builder.ToString()));

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
            int fullLen;
            offset += GetInteger(data, offset, out fullLen);
            var fType = data[offset];

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
        private static void IfProcessor(byte[] data, ref byte[] output, int offset, string type) {
            var strBuilder = new StringBuilder("<" + type + "(");

            // TODO: Check if presence of true;false is stored somehow

            int fullLen;
            var currentOffset = offset;
            var lenLen = GetInteger(data, offset, out fullLen);
            currentOffset += lenLen;
            var condLen = ParseIfCondition(data, currentOffset, strBuilder);
            //if (condLen < 0) {
            if(true) {  // TODO: Change this back, but need it not crashing for now.
                strBuilder = new StringBuilder("<" + type + "?(");

                for (var i = 0; i < fullLen && offset + lenLen + i < data.Length; ++i)
                    strBuilder.AppendFormat("{0:X2}", data[offset + lenLen + i]);
            } else {
                currentOffset += condLen;
                if (data[currentOffset] != 0x03) {
                    currentOffset += ParseIfBlock(data, currentOffset, strBuilder); // True
                    if (data[currentOffset] != 0x03)
                        currentOffset += ParseIfBlock(data, currentOffset, strBuilder); // False
                }
            }
            strBuilder.Append(")>");

            var b = Encoding.GetBytes(strBuilder.ToString());
            AddRange(ref output, b, 0, b.Length);
        }
        private static int ParseIfCondition(byte[] data, int offset, StringBuilder output) {
            /*
             * E3: x <= y
             * E4: x = y
             * Default: x?
             * E9 -> argument
             */
            
            switch (data[offset]) {
                case 0xE0: {
                        var arg = (int)BitConverter.ToUInt16(data, offset + 1);
                        //arg |= (int)(data[offset + 2] >> 16);

                        //var comp = data[offset + 3];
                        int comp;
                        var l = GetInteger(data, offset + 3, out comp);
                        output.AppendFormat("{0:X4} >= {1}", arg, comp);
                        return 3 + l;
                    }
                case 0xE3: {
                        var arg = (int)BitConverter.ToUInt16(data, offset + 1);
                        arg |= (int)(data[offset + 2] >> 16);

                        //var comp = data[offset + 3];
                        int comp;
                        var l = GetInteger(data, offset + 3, out comp);
                        output.AppendFormat("{0:X4} <= {1}", arg, comp);
                        return 4 + l;
                    }
                case 0xE4: {
                        var arg = (int)BitConverter.ToUInt16(data, offset + 1);
                        arg |= (int)(data[offset + 2] >> 16);

                        //var comp = data[offset + 3];
                        int comp;
                        var l = GetInteger(data, offset + 3, out comp);
                        output.AppendFormat("{0:X4} = {1}", arg, comp);
                        return 4 + l;
                    }
                case 0xE9: {
                        var arg = BitConverter.ToUInt16(data, offset);
                        output.AppendFormat("{0:X4}", arg);
                        return 3;
                    }
            }
            return -1;
        }
        private static int ParseIfBlock(byte[] data, int offset, StringBuilder output) {
            if (data[offset] == 0xFF) {
                // String
                int blockLength;
                var ll = GetInteger(data, offset + 1, out blockLength);
                blockLength -= ll;

                //var blockLength = data[offset + 1] - 1;
                var block = new byte[blockLength];
                Array.Copy(data, offset + ll + 1, block, 0, block.Length);

                output.Append(",");
                output.Append(Decode(block));

                return blockLength + ll + 1;
            } else if (data[offset] < 0xF0)  // TODO: I just put the value here in case there are more special cases, who knows
                return data[offset];
            throw new NotSupportedException();
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

        #region GetInt
        private static int GetInteger(byte[] data, int offset, out int result) {
            var t = data[offset];
            if (t < 0xF2) {
                result = t;
                return 1;
            }

            switch (data[offset]) {
                case 0xF2: {
                        var v = 0;
                        v |= data[offset + 1] << 8;
                        v |= data[offset + 2];
                        result = v;
                        return 3;
                    }
                case 0xFA: {
                        var v = 0;
                        v |= data[offset + 1] << 16;
                        v |= data[offset + 2] << 8;
                        v |= data[offset + 3];
                        result = v;
                        return 4;
                    }
                case 0xFE: {
                        result = BitConverter.ToInt32(data, offset + 1);
                        return 5;
                    }
                case 0xF0:
                case 0xFF: {
                        result = data[offset + 1];
                        return 2;
                    }
            }
            var sb = new StringBuilder();
            for (var i = Math.Max(0, offset - 8); i < data.Length; ++i) {
                if (i == offset)
                    sb.Append(">");
                sb.AppendFormat("{0:X2} ", data[i]);
            }
            throw new NotSupportedException();
        }
        #endregion
    }
}
