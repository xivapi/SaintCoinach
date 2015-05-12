using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public static class XivStringDecoder {
        public static bool Trace = false;

        delegate void TagDecoder(TagType type, byte[] input, StreamWriter output);

        #region Settings
        const byte TagStartMarker = 0x02;
        const byte TagEndMarker = 0x03;

        static readonly Encoding UTF8NoBom = new UTF8Encoding(false);

        static readonly Dictionary<TagType, TagDecoder> TagDecoders;
        static XivStringDecoder() {
            TagDecoders = new Dictionary<TagType, TagDecoder> {
                { TagType.If, DecodeIf },
                { TagType.Switch, DecodeSwitch },
                { TagType.LineBreak, DecodeLineBreak },
                { TagType.Wait, DecodeWait },
                { TagType.Gui, DecodeGui },
                { TagType.Color, DecodeColor },
                { TagType.Emphasis, DecodeEmphasis },
                { TagType.Dash, DecodeDash },
                { TagType.Value, DecodeValue },
                { TagType.Format, DecodeFormat },
                { TagType.Zero, DecodeZero },
                { TagType.Time, DecodeTime },
                { TagType.Sheet, DecodeSheet },
                { TagType.Fixed, DecodeFixed },
                { TagType.SheetJa, DecodeSheetJa },
                { TagType.SheetEn, DecodeSheetEn },
                { TagType.SheetDe, DecodeSheetDe },
                { TagType.SheetFr, DecodeSheetFr },
            };
        }
        #endregion

        #region Decode
        public static byte[] DecodeBinary(byte[] input, int offset, int length) {
            if (offset < 0 || offset > input.Length)
                throw new ArgumentOutOfRangeException("offset");
            var end = offset + length;
            if (length < 0 || end > input.Length)
                throw new ArgumentOutOfRangeException("length");

            byte[] output;

            using (var ms = new MemoryStream()) {
                using (var w = new StreamWriter(ms, UTF8NoBom)) {
                    DecodeBinary(input, offset, length, w);
                    w.Flush();
                    output = ms.ToArray();
                }
            }

            return output;
        }

        static int Pass = 0;
        public static void DecodeBinary(byte[] input, int offset, int length, StreamWriter output) {
            if (offset < 0 || offset > input.Length)
                throw new ArgumentOutOfRangeException("offset");
            var end = offset + length;
            if (length < 0 || end > input.Length)
                throw new ArgumentOutOfRangeException("length");

            if (Trace) {
                System.IO.File.WriteAllBytes("Full." + (Pass).ToString() + ".bin", input);

                System.Diagnostics.Trace.WriteLine("");
                System.Diagnostics.Trace.WriteLine(string.Format("Begin decode #{0} > 0x{1:X} + 0x{2:X} {{", Pass++, offset, length));
                System.Diagnostics.Trace.WriteLine("");
            }

            while (offset < end) {
                var v = input[offset++];
                if (v == TagStartMarker) {
                    DecodeTag(input, output, ref offset);
                    if (offset > end)
                        throw new InvalidOperationException();
                } else {
                    output.Flush();
                    output.BaseStream.Write(input, offset - 1, 1);
                }
            }
            if (Trace) {
                output.Flush();
                System.Diagnostics.Trace.WriteLine("");
                System.Diagnostics.Trace.WriteLine(string.Format("}} = \"{0}\"", Encoding.UTF8.GetString(((MemoryStream)output.BaseStream).ToArray())));
                System.Diagnostics.Trace.WriteLine("");
            }
        }

        public static byte[] DecodeBinary(byte[] input) {
            return DecodeBinary(input, 0, input.Length);
        }

        public static string Decode(byte[] input) {
            var dec = DecodeBinary(input, 0, input.Length);
            return Encoding.UTF8.GetString(dec);
        }
        #endregion

        #region Tag decoding

        static void DecodeTag(byte[] input, System.IO.StreamWriter output, ref int offset) {
            var start = offset;
            var type = (TagType)input[offset++];
            var size = GetInteger(input, ref offset);

            if (Trace) {
                var sb = new StringBuilder();
                for (var i = start; i < offset; ++i)
                    sb.AppendFormat("{0:X2}", input[i]);
                System.Diagnostics.Trace.WriteLine(string.Format("    Tag {0} at 0x{1:X}, data at 0x{2:X} of size 0x{3:X} ({4}) {{ ", type, start, offset, size, sb));
            }

            if (input[offset + size] != TagEndMarker)
                throw new InvalidDataException();

            var tagContent = new byte[size];
            Array.Copy(input, offset, tagContent, 0, size);

            TagDecoder tagDecoder = null;
            TagDecoders.TryGetValue(type, out tagDecoder);
            (tagDecoder ?? DefaultTagDecoder)(type, tagContent, output);

            offset += size + 1;

            if (Trace)
                System.Diagnostics.Trace.WriteLine(string.Format("    }} 0x{0:X} ", offset));

            return;
        }


        #endregion

        #region Tag decoders
        static void DefaultTagDecoder(TagType type, byte[] tagData, StreamWriter output) {
            if (tagData.Length == 0) {
                output.Write("<{0} />", type);
            } else {
                output.Write("<{0}>", type);
                for (var i = 0; i < tagData.Length; ++i)
                    output.Write(tagData[i].ToString("X2"));
                output.Write("</{0}>", type);
            }
        }
        static void DecodeLineBreak(TagType type, byte[] tagData, StreamWriter output) { output.Write(Environment.NewLine); }
        static void DecodeWait(TagType type, byte[] tagData, StreamWriter output) { DefaultTagDecoder(type, tagData, output); }
        static void DecodeGui(TagType type, byte[] tagData, StreamWriter output) { DefaultTagDecoder(type, tagData, output); }
        static void DecodeColor(TagType type, byte[] tagData, StreamWriter output) {
            if (tagData.Length == 1 && tagData[0] == 0xEC) {
                output.Write("</{0}>", type);
                return;
            }

            output.Write("<{0}=#{1:X6}>", type, GetInteger(tagData, 0));
        }
        static void DecodeEmphasis(TagType type, byte[] tagData, StreamWriter output) {
            if (tagData.Length != 1)
                throw new InvalidDataException();
            if (tagData[0] == 1)
                output.Write("</{0}>", type);
            else if (tagData[0] == 2)
                output.Write("<{0}>", type);
            else
                throw new InvalidDataException();
        }
        static void DecodeDash(TagType type, byte[] tagData, StreamWriter output) {
            output.Write("–");
        }
        static void DecodeValue(TagType type, byte[] tagData, StreamWriter output) { DefaultTagDecoder(type, tagData, output); }
        static void DecodeFormat(TagType type, byte[] tagData, StreamWriter output) { DefaultTagDecoder(type, tagData, output); }
        static void DecodeZero(TagType type, byte[] tagData, StreamWriter output) { DefaultTagDecoder(type, tagData, output); }
        static void DecodeTime(TagType type, byte[] tagData, StreamWriter output) { DefaultTagDecoder(type, tagData, output); }
        static void DecodeSheet(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeVariable(tagData, ref o, output);     // Sheet name
            output.Write(',');
            DecodeVariable(tagData, ref o, output);     // Row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeVariable(tagData, ref o, output);     // Column
            }
            output.Write(")/>");
        }
        static void DecodeLangSheet(TagType type, byte[] tagData, StreamWriter output) {
        }
        static void DecodeFixed(TagType type, byte[] tagData, StreamWriter output) { DefaultTagDecoder(type, tagData, output); }
        static void DecodeSheetJa(TagType type, byte[] tagData, StreamWriter output) { DecodeSheet(type, tagData, output); }
        /* Attributive:
         * 0 - 1    ja
         * 2 - 7    en
         * 8 - 23   de
         * 24 - 40  fr
         */
        static void DecodeSheetEn(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeVariable(tagData, ref o, output);     // Target sheet name
            output.Write(',');
            DecodeVariable(tagData, ref o, output);     // Attributive row
            output.Write(',');
            DecodeVariable(tagData, ref o, output);     // Target row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeVariable(tagData, ref o, output);     // Target column
            }
            if (o < tagData.Length) {
                output.Write(',');
                DecodeVariable(tagData, ref o, output);     // Attributive column?
            }
            output.Write(")/>");
        }
        static void DecodeSheetDe(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeVariable(tagData, ref o, output);     // Target sheet name
            output.Write(',');
            DecodeVariable(tagData, ref o, output);     // Attributive row
            output.Write(',');
            DecodeVariable(tagData, ref o, output);     // Target row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeVariable(tagData, ref o, output);     // Target column
            }
            if (o < tagData.Length) {
                output.Write(',');
                DecodeVariable(tagData, ref o, output);     // Attributive column?
            }
            output.Write(")/>");
        }
        static void DecodeSheetFr(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeVariable(tagData, ref o, output);     // Target sheet name
            output.Write(',');
            DecodeVariable(tagData, ref o, output);     // Attributive row
            output.Write(',');
            DecodeVariable(tagData, ref o, output);     // Target row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeVariable(tagData, ref o, output);     // Target column
            }
            if (o < tagData.Length) {
                output.Write(',');
                DecodeVariable(tagData, ref o, output);     // Attributive column?
            }
            output.Write(")/>");
        }
        #endregion

        #region If

        static void DecodeIf(TagType type, byte[] tagData, StreamWriter output) {
            int offset = 0;

            // Condition
            output.Write("<{0}(", type);
            DecodeVariable(tagData, ref offset, output);
            output.Write(")>");

            // If true
            //DecodeIfResult(tagData, output, ref offset);
            DecodeVariable(tagData, ref offset, output);

            if (offset != tagData.Length) {
                // Else
                output.Write("<else/>");
                //DecodeIfResult(tagData, output, ref offset);
                DecodeVariable(tagData, ref offset, output);
            }

            // End
            output.Write("</{0}>", type);
        }
        static void DecodeIfResult(byte[] tagData, StreamWriter output, ref int offset) {
        }
        #endregion

        #region Switch
        static void DecodeSwitch(TagType type, byte[] tagData, StreamWriter output) {
            DefaultTagDecoder(type, tagData, output);

            return;

            output.Write("<{0}(", type);

            // Value

            output.Write(")>");

            // Foreach case
            {
                output.Write("<case(");

                // Case condition

                output.Write(")>");

                // case result

                output.Write("</case>");
            }

            output.Write("</{0}>", type);
        }
        #endregion

        #region Shared
        static void DecodeVariable(byte[] input, ref int offset, StreamWriter output) {
            var t = input[offset++];
            if (t < 0xE0) {
                output.Write(t - 1);
                return;
            }

            var varType = (VariableType)t;
            /*
            if (t == 0xFF) {
                ++offset;
                int endOfLength;
                var len = GetInteger(input, offset, out endOfLength);
                var innerLen = len - endOfLength + offset;

                DecodeBinary(input, endOfLength, len, output);

                offset = endOfLength + len;
            } else {
                var v = GetInteger(input, offset, out offset);
                output.Write(v);
            }*/

            // These types don't need the type ID.
            switch (varType) {
                case VariableType.Decode: {
                        var len = GetInteger(input, ref offset);
                        DecodeBinary(input, offset, len, output);
                        offset += len;
                    } return;
                case VariableType.Byte:
                    output.Write(GetInteger(input, IntegerType.Byte, ref offset));
                    return;
                case VariableType.Int16_MinusOne:
                    output.Write(GetInteger(input, IntegerType.Int16, ref offset) - 1);
                    return;
                case VariableType.Int16_1:
                case VariableType.Int16_2:
                    output.Write(GetInteger(input, IntegerType.Int16, ref offset));
                    return;
                case VariableType.Int24_MinusOne:
                    output.Write(GetInteger(input, IntegerType.Int24, ref offset) - 1);
                    return;
                case VariableType.Int24:
                    output.Write(GetInteger(input, IntegerType.Int24, ref offset));
                    return;
            }

            output.Write("{0}(", varType);
            switch (varType) {
                case VariableType.LessThanOrEqualTo:
                    DecodeVariable(input, ref offset, output);
                    output.Write(',');
                    DecodeVariable(input, ref offset, output);
                    break;
                case VariableType.Value1:
                    DecodeVariable(input, ref offset, output);
                    break;
                case VariableType.GreaterThanOrEqualTo:
                    DecodeVariable(input, ref offset, output);
                    output.Write(',');
                    DecodeVariable(input, ref offset, output);
                    break;
                case VariableType.Invert:
                    DecodeVariable(input, ref offset, output);
                    break;
                case VariableType.Equal:
                    DecodeVariable(input, ref offset, output);
                    output.Write(',');
                    DecodeVariable(input, ref offset, output);
                    break;
                case VariableType.InputParameter:
                    DecodeVariable(input, ref offset, output);
                    break;
                case VariableType.PlayerParameter:
                    DecodeVariable(input, ref offset, output);
                    break;
                case VariableType.UnknownParameter1:
                case VariableType.UnknownParameter2:
                    DecodeVariable(input, ref offset, output);
                    break;
                default:
                    throw new NotSupportedException();
            }
            output.Write(")");
        }
        static int GetInteger(byte[] input, int offset) {
            return GetInteger(input, ref offset);
        }
        static int GetInteger(byte[] input, ref int offset) {
            const byte ByteLengthCutoff = 0xF0;

            var t = input[offset++];
            if (t < ByteLengthCutoff)
                return t - 1;

            var type = (IntegerType)t;
            return GetInteger(input, type, ref offset);
        }
        static int GetInteger(byte[] input, IntegerType type, ref int offset) {
            switch (type) {
                case IntegerType.Byte:
                    return (input[offset++]);
                case IntegerType.BytePlus255:
                    return (input[offset++] + 255);
                case IntegerType.Int16: {
                        int v = 0;
                        v |= input[offset++] << 8;
                        v |= input[offset++];
                        return (v);
                    }
                case IntegerType.Int24: {
                        int v = 0;
                        v |= input[offset++] << 16;
                        v |= input[offset++] << 8;
                        v |= input[offset++];
                        return (v);
                    }
                case IntegerType.Int32: {
                        int v = 0;
                        v |= input[offset++] << 24;
                        v |= input[offset++] << 16;
                        v |= input[offset++] << 8;
                        v |= input[offset++];
                        return (v);
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        #endregion
    }
}
