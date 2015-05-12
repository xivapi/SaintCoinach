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
                { TagType.IfEquals, DecodeIfEquals },
                { TagType.LineBreak, DecodeLineBreak },
                { TagType.Gui, DecodeGui },
                { TagType.Color, DecodeColor },
                { TagType.Emphasis2, DecodeEmphasis },
                { TagType.Emphasis, DecodeEmphasis },
                { TagType.CommandIcon, DecodeCommandIcon },
                { TagType.Dash, DecodeDash },
                { TagType.Value, DecodeValue },
                { TagType.Format, DecodeFormat },
                { TagType.TwoDigitValue, DecodeTwoDigitValue },
                { TagType.Sheet, DecodeSheet },
                { TagType.Highlight, DecodeHighlight },
                { TagType.Clickable, DecodeClickable },
                { TagType.Split, DecodeSplit },
                { TagType.Fixed, DecodeFixed },
                { TagType.SheetJa, DecodeSheetJa },
                { TagType.SheetEn, DecodeSheetEn },
                { TagType.SheetDe, DecodeSheetDe },
                { TagType.SheetFr, DecodeSheetFr },
                { TagType.InstanceContent, DecodeInstanceContent },
                { TagType.ZeroPaddedValue, DecodeZeroPaddedValue },
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
        static void DecodeIf(TagType type, byte[] tagData, StreamWriter output) {
            int offset = 0;

            // Condition
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref offset, output, true);
            output.Write(")>");

            // If true
            DecodeExpression(tagData, ref offset, output, false);

            if (offset != tagData.Length) {
                // Else
                output.Write("<else/>");
                DecodeExpression(tagData, ref offset, output, false);
            }

            // End
            output.Write("</{0}>", type);
        }
        #endregion

        #region Switch
        static void DecodeSwitch(TagType type, byte[] tagData, StreamWriter output) {
            int offset = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref offset, output, true);
            output.Write(")>");

            int i = 1;
            // Foreach case
            while (offset != tagData.Length) {
                output.Write("<case({0})>", i);

                DecodeExpression(tagData, ref offset, output, false);
                output.Write("</case>");

                ++i;
            }

            output.Write("</{0}>", type);
        }
        static void DecodeIfEquals(TagType type, byte[] tagData, StreamWriter output) {
            int offset = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref offset, output, true);
            output.Write(',');
            DecodeExpression(tagData, ref offset, output, true);
            output.Write(")>");
            DecodeExpression(tagData, ref offset, output, false);
            if (offset != tagData.Length) {
                output.Write("<else/>");
                DecodeExpression(tagData, ref offset, output, false);
            }
            output.Write("</{0}>", type);
        }
        static void DecodeLineBreak(TagType type, byte[] tagData, StreamWriter output) { output.Write(Environment.NewLine); }
        static void DecodeGui(TagType type, byte[] tagData, StreamWriter output) {
            DecodeTagWithExpressionArgument(type, tagData, output);
        }
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
        static void DecodeCommandIcon(TagType type, byte[] tagData, StreamWriter output) {
            DecodeTagWithExpressionArgument(type, tagData, output);
        }
        static void DecodeDash(TagType type, byte[] tagData, StreamWriter output) {
            output.Write("–");
        }
        static void DecodeValue(TagType type, byte[] tagData, StreamWriter output) {
            DecodeTagWithExpressionContent(type, tagData, output);
        }
        static void DecodeFormat(TagType type, byte[] tagData, StreamWriter output) {
            int offset = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref offset, output, true);
            output.Write(',');
            // TODO: Figure this out, not just copy the values as hex
            //DecodeExpression(tagData, ref offset, output);
            while (offset < tagData.Length) {
                output.Write("{0:X2}", tagData[offset]);
                ++offset;
            }
            output.Write(")/>");
        }
        static void DecodeTwoDigitValue(TagType type, byte[] tagData, StreamWriter output) {
            DecodeTagWithExpressionContent(type, tagData, output);
        }
        static void DecodeHighlight(TagType type, byte[] tagData, StreamWriter output) {
            DecodeTagWithExpressionContent(type, tagData, output);
        }
        static void DecodeClickable(TagType type, byte[] tagData, StreamWriter output) {
            DecodeTagWithExpressionContent(type, tagData, output);
        }
        static void DecodeSplit(TagType type, byte[] tagData, StreamWriter output) {
            int offset = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref offset, output, true);  // Input
            output.Write(',');
            DecodeExpression(tagData, ref offset, output, true);  // Separator
            output.Write(',');
            DecodeExpression(tagData, ref offset, output, true);  // Used index
            output.Write(")/>");
        }
        static void DecodeSheet(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref o, output, true);     // Sheet name
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeExpression(tagData, ref o, output, true);     // Column
            }
            output.Write(")/>");
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
            DecodeExpression(tagData, ref o, output, true);     // Target sheet name
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Attributive row
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Target row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeExpression(tagData, ref o, output, true);     // Target column
            }
            if (o < tagData.Length) {
                output.Write(',');
                DecodeExpression(tagData, ref o, output, true);     // Attributive column?
            }
            output.Write(")/>");
        }
        static void DecodeSheetDe(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref o, output, true);     // Target sheet name
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Attributive row
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Target row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeExpression(tagData, ref o, output, true);     // Target column
            }
            if (o < tagData.Length) {
                output.Write(',');
                DecodeExpression(tagData, ref o, output, true);     // Attributive column?
            }
            output.Write(")/>");
        }
        static void DecodeSheetFr(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref o, output, true);     // Target sheet name
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Attributive row
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Target row
            if (o < tagData.Length) {
                output.Write(',');
                DecodeExpression(tagData, ref o, output, true);     // Target column
            }
            if (o < tagData.Length) {
                output.Write(',');
                DecodeExpression(tagData, ref o, output, true);     // Attributive column?
            }
            output.Write(")/>");
        }
        static void DecodeInstanceContent(TagType type, byte[] tagData, StreamWriter output) {
            DecodeTagWithExpressionContent(type, tagData, output);
        }
        static void DecodeZeroPaddedValue(TagType type, byte[] tagData, StreamWriter output) {
            int o = 0;
            output.Write("<{0}(", type);
            DecodeExpression(tagData, ref o, output, true);     // Value
            output.Write(',');
            DecodeExpression(tagData, ref o, output, true);     // Target length
            output.Write(")/>");
        }
        #endregion

        #region Shared
        static void DefaultTagDecoder(TagType type, byte[] tagData, StreamWriter output) {
            if (tagData.Length == 0) {
                output.Write("<{0}/>", type);
            } else {
                output.Write("<{0}>", type);
                for (var i = 0; i < tagData.Length; ++i)
                    output.Write(tagData[i].ToString("X2"));
                output.Write("</{0}>", type);
            }
        }
        static void DecodeTagWithExpressionArgument(TagType type, byte[] tagData, StreamWriter output) {
            output.Write("<{0}(", type);
            DecodeExpression(tagData, 0, output, true);
            output.Write(")/>");
        }
        static void DecodeTagWithExpressionContent(TagType type, byte[] tagData, StreamWriter output) {
            if (tagData.Length == 0) {
                output.Write("<{0}/>", type);
                return;
            }
            output.Write("<{0}>", type);
            DecodeExpression(tagData, 0, output, false);
            output.Write("</{0}>", type);
        }
        static void DecodeExpression(byte[] input, int offset, StreamWriter output, bool outputZero) {
            DecodeExpression(input, ref offset, output, outputZero);
        }
        static void DecodeExpression(byte[] input, ref int offset, StreamWriter output, bool outputZero) {
            var t = input[offset++];
            if (t < 0xE0) {
                if (outputZero || t > 1)
                    output.Write(t - 1);
                return;
            }

            var exprType = (ExpressionType)t;
            // These types don't need the type ID.
            switch (exprType) {
                case ExpressionType.Decode: {
                        var len = GetInteger(input, ref offset);
                        DecodeBinary(input, offset, len, output);
                        offset += len;
                    } return;
                case ExpressionType.Byte:
                    output.Write(GetInteger(input, IntegerType.Byte, ref offset));
                    return;
                case ExpressionType.Int16_MinusOne:
                    output.Write(GetInteger(input, IntegerType.Int16, ref offset) - 1);
                    return;
                case ExpressionType.Int16_1:
                case ExpressionType.Int16_2:
                    output.Write(GetInteger(input, IntegerType.Int16, ref offset));
                    return;
                case ExpressionType.Int24_MinusOne:
                    output.Write(GetInteger(input, IntegerType.Int24, ref offset) - 1);
                    return;
                case ExpressionType.Int24:
                    output.Write(GetInteger(input, IntegerType.Int24, ref offset));
                    return;
            }

            output.Write("{0}(", exprType);
            switch (exprType) {
                case ExpressionType.LessThanOrEqualTo:
                    DecodeExpression(input, ref offset, output, true);
                    output.Write(',');
                    DecodeExpression(input, ref offset, output, true);
                    break;
                case ExpressionType.Value1:
                    DecodeExpression(input, ref offset, output, true);
                    break;
                case ExpressionType.GreaterThanOrEqualTo:
                    DecodeExpression(input, ref offset, output, true);
                    output.Write(',');
                    DecodeExpression(input, ref offset, output, true);
                    break;
                case ExpressionType.Invert:
                    DecodeExpression(input, ref offset, output, true);
                    break;
                case ExpressionType.Equal:
                    DecodeExpression(input, ref offset, output, true);
                    output.Write(',');
                    DecodeExpression(input, ref offset, output, true);
                    break;
                case ExpressionType.InputParameter:
                    DecodeExpression(input, ref offset, output, true);
                    break;
                case ExpressionType.PlayerParameter:
                    DecodeExpression(input, ref offset, output, true);
                    break;
                case ExpressionType.UnknownParameter1:
                case ExpressionType.UnknownParameter2:
                    DecodeExpression(input, ref offset, output, true);
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
