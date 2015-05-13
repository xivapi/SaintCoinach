using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public class XivStringDecoder {
        public delegate IStringNode TagDecoder(BinaryReader input, TagType tag, int length);

        static readonly Encoding UTF8NoBom = new UTF8Encoding(false);
        const byte TagStartMarker = 0x02;
        const byte TagEndMarker = 0x03;

        private static XivStringDecoder _Default;
        public static XivStringDecoder Default { get { return _Default ?? (_Default = new XivStringDecoder()); } }

        #region Fields
        private Encoding _Encoding = UTF8NoBom;
        private Dictionary<TagType, TagDecoder> _TagDecoders = new Dictionary<TagType, TagDecoder>();
        private TagDecoder _DefaultTagDecoder;
        private string _Dash = "–";
        private string _NewLine = Environment.NewLine;
        #endregion

        #region Properties
        public Encoding Encoding {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        public TagDecoder DefaultTagDecoder {
            get { return _DefaultTagDecoder; }
            set {
                if (value == null)
                    throw new ArgumentNullException();
                _DefaultTagDecoder = value;
            }
        }
        public string Dash {
            get { return _Dash; }
            set { _Dash = value; }
        }
        public string NewLine {
            get { return _NewLine; }
            set { _NewLine = value; }
        }
        #endregion

        #region Constructor
        public XivStringDecoder() {
            this.DefaultTagDecoder = DecodeTagDefault;

            SetDecoder(TagType.Clickable, (i, t, l) => DecodeGenericElementWithVariableArguments(i, t, l, 1, int.MaxValue));    // I have no idea.
            SetDecoder(TagType.Color, DecodeColor);
            SetDecoder(TagType.CommandIcon, (i, t, l) => DecodeGenericElement(i, t, l, 1, false));
            SetDecoder(TagType.Dash, (i, t, l) => new Nodes.StaticString(this.Dash));
            SetDecoder(TagType.Emphasis, DecodeGenericSurroundingTag);
            SetDecoder(TagType.Emphasis2, DecodeGenericSurroundingTag);
            // TODO: Fixed
            SetDecoder(TagType.Format, DecodeFormat);
            SetDecoder(TagType.Gui, (i, t, l) => DecodeGenericElement(i, t, l, 1, false));
            SetDecoder(TagType.Highlight, (i, t, l) => DecodeGenericElement(i, t, l, 0, true));
            SetDecoder(TagType.If, DecodeIf);
            SetDecoder(TagType.IfEquals, DecodeIfEquals);
            // Indent
            SetDecoder(TagType.InstanceContent, (i, t, l) => DecodeGenericElement(i, t, l, 0, true));
            SetDecoder(TagType.LineBreak, (i, t, l) => new Nodes.StaticString(this.NewLine));
            SetDecoder(TagType.Sheet, (i, t, l) => DecodeGenericElementWithVariableArguments(i, t, l, 2, int.MaxValue));   // Sheet name, Row[, Column[, Parameters]+]
            SetDecoder(TagType.SheetDe, (i, t, l) => DecodeGenericElementWithVariableArguments(i, t, l, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.SheetEn, (i, t, l) => DecodeGenericElementWithVariableArguments(i, t, l, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.SheetFr, (i, t, l) => DecodeGenericElementWithVariableArguments(i, t, l, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.SheetJa, (i, t, l) => DecodeGenericElementWithVariableArguments(i, t, l, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.Split, (i, t, l) => DecodeGenericElement(i, t, l, 3, false));                    // Input expression, Seperator, Index to use
            SetDecoder(TagType.Switch, DecodeSwitch);
            // Time
            SetDecoder(TagType.TwoDigitValue, (i, t, l) => DecodeGenericElement(i, t, l, 0, true));
            // Unknowns
            SetDecoder(TagType.Value, (i, t, l) => DecodeGenericElement(i, t, l, 0, true));
            SetDecoder(TagType.ZeroPaddedValue, (i, t, l) => DecodeGenericElement(i, t, l, 1, true));
        }
        #endregion

        #region Helper
        public void SetDecoder(TagType tag, TagDecoder decoder) {
            if (_TagDecoders.ContainsKey(tag))
                _TagDecoders[tag] = decoder;
            else
                _TagDecoders.Add(tag, decoder);
        }
        #endregion

        #region Decode
        public XivString Decode(byte[] buffer) {
            using (var ms = new MemoryStream(buffer)) {
                using (var r = new BinaryReader(ms, this.Encoding))
                    return Decode(r, buffer.Length);
            }
        }
        public XivString Decode(BinaryReader input, int length) {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            var end = input.BaseStream.Position + length;
            if (end > input.BaseStream.Length)
                throw new ArgumentOutOfRangeException("length");

            var parts = new List<IStringNode>();
            var pendingStatic = new List<byte>();

            while (input.BaseStream.Position < end) {
                var v = input.ReadByte();
                if (v == TagStartMarker) {
                    AddStatic(pendingStatic, parts);
                    parts.Add(DecodeTag(input));
                    if (input.BaseStream.Position > end)
                        throw new InvalidOperationException();
                } else
                    pendingStatic.Add(v);
            }

            AddStatic(pendingStatic, parts);

            return new XivString(parts);
        }

        private IStringNode DecodeTag(BinaryReader input) {
            var tag = (TagType)input.ReadByte();
            var length = GetInteger(input);
            var end = input.BaseStream.Position + length;
            //System.Diagnostics.Trace.WriteLine(string.Format("{0} @ {1:X}h+{2:X}h", tag, input.BaseStream.Position, length));
            TagDecoder decoder = null;
            _TagDecoders.TryGetValue(tag, out decoder);
            var result = (decoder ?? DefaultTagDecoder)(input, tag, length);
            if (input.BaseStream.Position != end)
                throw new InvalidOperationException();
            if (input.ReadByte() != TagEndMarker)
                throw new InvalidDataException();
            return result;
        }

        private void AddStatic(List<byte> pending, List<IStringNode> targetParts) {
            if (pending.Count == 0)
                return;
            targetParts.Add(new Nodes.StaticString(this.Encoding.GetString(pending.ToArray())));
            pending.Clear();
        }
        #endregion

        #region Generic
        protected IStringNode DecodeTagDefault(BinaryReader input, TagType tag, int length) {
            return new Nodes.DefaultElement(tag, input.ReadBytes(length));
        }
        protected IStringNode DecodeExpression(BinaryReader input) {
            var t = input.ReadByte();
            return DecodeExpression(input, (ExpressionType)t);
        }
        protected IStringNode DecodeExpression(BinaryReader input, ExpressionType exprType) {
            var t = (byte)exprType;
            if (t < 0xE0)
                return new Nodes.StaticInteger(t - 1);

            switch (exprType) {
                case ExpressionType.Decode: {
                        var len = GetInteger(input);
                        return Decode(input, len);
                    }
                case ExpressionType.Byte:
                    return new Nodes.StaticInteger(GetInteger(input, IntegerType.Byte));
                case ExpressionType.Int16_MinusOne:
                    return new Nodes.StaticInteger(GetInteger(input, IntegerType.Int16) - 1);
                case ExpressionType.Int16_1:
                case ExpressionType.Int16_2:
                    return new Nodes.StaticInteger(GetInteger(input, IntegerType.Int16));
                case ExpressionType.Int24_MinusOne:
                    return new Nodes.StaticInteger(GetInteger(input, IntegerType.Int24) - 1);
                case ExpressionType.Int24:
                    return new Nodes.StaticInteger(GetInteger(input, IntegerType.Int24));
                case ExpressionType.Int24_Color:
                    return new Nodes.StaticInteger(GetInteger(input, IntegerType.Int24) | (0xFF << 24));
                case ExpressionType.Int32:
                    return new Nodes.StaticInteger(GetInteger(input, IntegerType.Int32));
                case ExpressionType.GreaterThanOrEqualTo:
                case ExpressionType.UnknownComparisonE1:
                case ExpressionType.LessThanOrEqualTo:
                case ExpressionType.NotEqual:
                case ExpressionType.Equal: {
                        var left = DecodeExpression(input);
                        var right = DecodeExpression(input);
                        return new Nodes.Comparison(exprType, left, right);
                    }
                case ExpressionType.InputParameter:
                case ExpressionType.PlayerParameter:
                case ExpressionType.UnknownParameterEA:
                case ExpressionType.UnknownParameterEB:
                    return new Nodes.Parameter(exprType, DecodeExpression(input));
                default:
                    throw new NotSupportedException();
            }
        }
        protected IStringNode DecodeGenericElement(BinaryReader input, TagType tag, int length, int argCount, bool hasContent) {
            if (length == 0) {
                if (argCount > 0)
                    throw new ArgumentOutOfRangeException("argCount");
                return new Nodes.EmptyElement(tag);
            }
            var arguments = new IStringNode[argCount];
            for (var i = 0; i < argCount; ++i)
                arguments[i] = DecodeExpression(input);
            IStringNode content = null;
            if (hasContent)
                content = DecodeExpression(input);

            return new Nodes.GenericElement(tag, content, arguments);
        }
        protected IStringNode DecodeGenericElementWithVariableArguments(BinaryReader input, TagType tag, int length, int minCount, int maxCount) {
            var end = input.BaseStream.Position + length;
            var args = new List<IStringNode>();
            for (var i = 0; i < maxCount && input.BaseStream.Position < end; ++i)
                args.Add(DecodeExpression(input));
            return new Nodes.GenericElement(tag, null, args);
        }
        protected IStringNode DecodeGenericSurroundingTag(BinaryReader input, TagType tag, int length) {
            if (length != 1)
                throw new ArgumentOutOfRangeException("length");
            var status = GetInteger(input);
            if (status == 0)
                return new Nodes.CloseTag(tag);
            if (status == 1)
                return new Nodes.OpenTag(tag, null);
            throw new InvalidDataException();
        }
        #endregion

        #region Specific
        protected IStringNode DecodeColor(BinaryReader input, TagType tag, int length) {
            var t = input.ReadByte();
            if (length == 1 && t == 0xEC)
                return new Nodes.CloseTag(tag);
            var color = DecodeExpression(input, (ExpressionType)t);
            return new Nodes.OpenTag(tag, color);
        }
        protected IStringNode DecodeFormat(BinaryReader input, TagType tag, int length) {
            var end = input.BaseStream.Position + length;

            var arg1 = DecodeExpression(input);
            var arg2 = new Nodes.StaticByteArray(input.ReadBytes((int)(end - input.BaseStream.Position)));
            return new Nodes.GenericElement(tag, null, arg1, arg2);
        }
        protected IStringNode DecodeIf(BinaryReader input, TagType tag, int length) {
            var end = input.BaseStream.Position + length;

            var condition = DecodeExpression(input);
            var trueValue = DecodeExpression(input);
            IStringNode falseValue = null;
            if (input.BaseStream.Position != end)
                falseValue = DecodeExpression(input);

            return new Nodes.IfElement(tag, condition, trueValue, falseValue);
        }
        protected IStringNode DecodeIfEquals(BinaryReader input, TagType tag, int length) {
            var end = input.BaseStream.Position + length;

            var left = DecodeExpression(input);
            var right = DecodeExpression(input);
            var trueValue = DecodeExpression(input);
            IStringNode falseValue = null;
            if (input.BaseStream.Position != end)
                falseValue = DecodeExpression(input);

            return new Nodes.IfEqualsElement(tag, left, right, trueValue, falseValue);
        }
        protected IStringNode DecodeSwitch(BinaryReader input, TagType tag, int length) {
            var end = input.BaseStream.Position + length;
            var caseSwitch = DecodeExpression(input);

            var cases = new Dictionary<int, IStringNode>();
            var i = 1;
            while (input.BaseStream.Position < end)
                cases.Add(i++, DecodeExpression(input));

            return new Nodes.SwitchElement(tag, caseSwitch, cases);
        }
        #endregion

        #region Shared
        protected static int GetInteger(BinaryReader input) {
            var t = input.ReadByte();
            var type = (IntegerType)t;
            return GetInteger(input, type);
        }
        protected static int GetInteger(BinaryReader input, IntegerType type) {
            const byte ByteLengthCutoff = 0xF0;

            var t = (byte)type;
            if (t < ByteLengthCutoff)
                return t - 1;

            switch (type) {
                case IntegerType.Byte:
                    return (input.ReadByte());
                case IntegerType.BytePlus255:
                    return (input.ReadByte() + 255);
                case IntegerType.Int16: {
                        int v = 0;
                        v |= input.ReadByte() << 8;
                        v |= input.ReadByte();
                        return (v);
                    }
                case IntegerType.Int24: {
                        int v = 0;
                        v |= input.ReadByte() << 16;
                        v |= input.ReadByte() << 8;
                        v |= input.ReadByte();
                        return (v);
                    }
                case IntegerType.Int32: {
                        int v = 0;
                        v |= input.ReadByte() << 24;
                        v |= input.ReadByte() << 16;
                        v |= input.ReadByte() << 8;
                        v |= input.ReadByte();
                        return (v);
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        #endregion
    }
}
