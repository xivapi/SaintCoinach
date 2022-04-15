using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    using Nodes;

    public class XivStringDecoder {
        // Tag Decoder has a binary input, a tag, and a length
        public delegate INode TagDecoder(BinaryReader input, TagType tag, int length, String lengthByteStr);

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

            SetDecoder(TagType.Clickable, (i, t, l, h) => DecodeGenericElementWithVariableArguments(i, t, l, h, 1, int.MaxValue));    // I have no idea.
            SetDecoder(TagType.Color, DecodeColor);
            SetDecoder(TagType.CommandIcon, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 1, false));
            SetDecoder(TagType.Dash, (i, t, l, h) => new Nodes.StaticString(this.Dash));
            SetDecoder(TagType.Emphasis, DecodeGenericSurroundingTag);
            SetDecoder(TagType.Emphasis2, DecodeGenericSurroundingTag);
            // TODO: Fixed
            SetDecoder(TagType.Format, DecodeFormat);
            SetDecoder(TagType.Gui, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 1, false));
            SetDecoder(TagType.Highlight, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 0, true));
            SetDecoder(TagType.If, DecodeIf);
            SetDecoder(TagType.IfEquals, DecodeIfEquals);
            // Indent
            SetDecoder(TagType.InstanceContent, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 0, true));
            SetDecoder(TagType.LineBreak, (i, t, l, h) => new Nodes.StaticString("<hex:02100103>"));
            SetDecoder(TagType.Sheet, (i, t, l, h) => DecodeGenericElementWithVariableArguments(i, t, l, h, 2, int.MaxValue));   // Sheet name, Row[, Column[, Parameters]+]
            SetDecoder(TagType.SheetDe, (i, t, l, h) => DecodeGenericElementWithVariableArguments(i, t, l, h, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.SheetEn, (i, t, l, h) => DecodeGenericElementWithVariableArguments(i, t, l, h, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.SheetFr, (i, t, l, h) => DecodeGenericElementWithVariableArguments(i, t, l, h, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.SheetJa, (i, t, l, h) => DecodeGenericElementWithVariableArguments(i, t, l, h, 3, int.MaxValue)); // Sheet name, Attributive row, Sheet row[, Sheet column[, Attributive index[, Parameters]+]
            SetDecoder(TagType.Split, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 3, false));                    // Input expression, Seperator, Index to use
            SetDecoder(TagType.Switch, DecodeSwitch);
            SetDecoder(TagType.Time, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 1, false));
            SetDecoder(TagType.TwoDigitValue, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 0, true));
            // Unknowns
            SetDecoder(TagType.Value, (i, t, l, h) => DecodeGenericElement(i, t, l, h, 0, true));
            SetDecoder(TagType.ZeroPaddedValue, DecodeZeroPaddedValue);
        }
        #endregion

        #region Helper
        public void SetDecoder(TagType tag, TagDecoder decoder) {
            // Tag Decoder has a binary input, a tag, and a length
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
                    return Decode(r, buffer.Length, new List<byte> { } );
            }
        }
        public XivString Decode(BinaryReader input, int length, List<byte> lenByte) {
            // check input size
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            // set the end of the input
            var end = input.BaseStream.Position + length;
            if (end > input.BaseStream.Length)
                throw new ArgumentOutOfRangeException("length");

            var parts = new List<INode>();
            var pendingStatic = new List<byte>();

            // Add the bytes representing decode tag (if exist) and length to pendingStatic
            if (lenByte.Count > 0) {
                String forText = BitConverter.ToString(lenByte.ToArray()).Replace("-", String.Empty) + ">";
                parts.Add(new Nodes.StaticString(forText));
            }

            // while loop until reaching the end
            while (input.BaseStream.Position < end) {
                var v = input.ReadByte();
                if (v == TagStartMarker) {
                    // What this function does:
                    //   If no item in "pending", just return
                    //   A list of interface can take any instance of that interface
                    //   TargetParts adds an element, which is the string version of "pending"
                    //   Finally, remove everything in "pending"
                    // P.S. it modifies the references directly. (list = reference type)
                    AddStatic(pendingStatic, parts);
                    // DecodeTag: byte -> string; added into "parts"
                    parts.Add(DecodeTag(input));
                    if (input.BaseStream.Position > end)
                        throw new InvalidOperationException();
                } else
                    pendingStatic.Add(v);
            }

            AddStatic(pendingStatic, parts);

            // Add <hex: if needed
            if (lenByte.Count > 0) {
                parts.Add(new Nodes.StaticString("<hex:"));
            }

            return new XivString(parts);
        }

        private INode DecodeTag(BinaryReader input) {
            // the first byte means the tag type
            var tag = (TagType)input.ReadByte();

            // edited
            // the second byte(s) means the length of commnad
            List<byte> lengthByte = new List<byte> { };
            var length = GetInteger(input, lengthByte);
            String lengthByteStr = BitConverter.ToString(lengthByte.ToArray()).Replace("-", String.Empty);
            
            var end = input.BaseStream.Position + length;
            // System.Diagnostics.Trace.WriteLine(string.Format("{0} @ {1:X}h+{2:X}h", tag, input.BaseStream.Position, length));
            TagDecoder decoder = null;
            // ref and out:
            //   both means to modify the reference;
            //   "out" means it may not be initialized yet, so has to be done in the function.
            //   這個方法傳回時，如果找到索引鍵，則包含與指定索引鍵相關聯的值，否則為 value 參數類型的預設值。這個參數會以未初始化的狀態傳遞。
            _TagDecoders.TryGetValue(tag, out decoder);
            // "??" operator: return the left side if it is not null; otherwise, return the right side.
            // If tag in _TagDecoders, decoder will be that decoder; otherwise, it will be DefaultTagDecoder
            var result = (decoder ?? DefaultTagDecoder)(input, tag, length, lengthByteStr);
            if (input.BaseStream.Position != end)
            {
                // Triggered by two entries in LogMessage as of 3.15.
                // Looks like a tag has some extra bits, as the end length is a proper TagEndMarker.
                System.Diagnostics.Debug.WriteLine(string.Format("Position mismatch in XivStringDecoder.DecodeTag.  Position {0} != predicted {1}.", input.BaseStream.Position, end));
                input.BaseStream.Position = end;
            }
            if (input.ReadByte() != TagEndMarker)
                throw new InvalidDataException();
            return result;
        }

        private void AddStatic(List<byte> pending, List<INode> targetParts) {
            if (pending.Count == 0)
                return;
            targetParts.Add(new Nodes.StaticString(this.Encoding.GetString(pending.ToArray())));
            pending.Clear();
        }
        #endregion

        #region Generic
        protected INode DecodeTagDefault(BinaryReader input, TagType tag, int length, String lenByte) {
            return new Nodes.DefaultElement(tag, input.ReadBytes(length), lenByte);
        }
        
        protected INode DecodeExpression(BinaryReader input) {
            var t = input.ReadByte();
            // expressionTypeByte = t;
            return DecodeExpression(input, (DecodeExpressionType)t);
        }
        
        protected INode DecodeExpression(BinaryReader input, DecodeExpressionType exprType) {
            var t = (byte)exprType;
            if (t < 0xD0) {
                return new Nodes.StaticInteger(t - 1, ((byte)t).ToString("X2"));
            }
            if (t < 0xE0) {
                return new Nodes.TopLevelParameter(t - 1, ((byte)t).ToString("X2"));
            }

            List<byte> addByte = new List<byte> { };
            addByte.Add((Byte)exprType);
            switch (exprType) {
                case DecodeExpressionType.Decode: {
                        var len = GetInteger(input, addByte);
                        // XIVString is also an INode
                        return Decode(input, len, addByte);
                    }
                case DecodeExpressionType.Byte: {
                        var expr = GetInteger(input, IntegerType.Byte, addByte);
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);
                        return new Nodes.StaticInteger(expr, lenByte);
                    }
                case DecodeExpressionType.Int16_MinusOne: {
                        var expr = GetInteger(input, IntegerType.Int16, addByte) - 1;
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);
                        return new Nodes.StaticInteger(expr, lenByte);
                    }
                case DecodeExpressionType.Int16_1:
                case DecodeExpressionType.Int16_2: {
                        var expr = GetInteger(input, IntegerType.Int16, addByte);
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);
                        return new Nodes.StaticInteger(expr, lenByte);
                    }
                case DecodeExpressionType.Int24_MinusOne: {
                        var expr = GetInteger(input, IntegerType.Int24, addByte) - 1;
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);
                        return new Nodes.StaticInteger(expr, lenByte);
                    }
                case DecodeExpressionType.Int24: {
                        var expr = GetInteger(input, IntegerType.Int24, addByte);
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);
                        return new Nodes.StaticInteger(expr, lenByte);
                    }
                case DecodeExpressionType.Int24_Lsh8: {
                        var expr = GetInteger(input, IntegerType.Int24, addByte) << 8;
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);
                        return new Nodes.StaticInteger(expr, lenByte);
                    }
                case DecodeExpressionType.Int24_SafeZero: {
                        var v16 = input.ReadByte();
                        var v8 = input.ReadByte();
                        var v0 = input.ReadByte();
                        addByte.Add(v16);
                        addByte.Add(v8);
                        addByte.Add(v0);
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);

                        int v = 0;
                        if (v16 != byte.MaxValue)
                            v |= v16 << 16;
                        if (v8 != byte.MaxValue)
                            v |= v8 << 8;
                        if (v0 != byte.MaxValue)
                            v |= v0;

                        return new Nodes.StaticInteger(v, lenByte);
                    }
                case DecodeExpressionType.Int32: {
                        var expr = GetInteger(input, IntegerType.Int32, addByte);
                        var lenByte = BitConverter.ToString(addByte.ToArray()).Replace("-", String.Empty);
                        return new Nodes.StaticInteger(expr, lenByte);
                    }
                case DecodeExpressionType.GreaterThanOrEqualTo:
                case DecodeExpressionType.GreaterThan:
                case DecodeExpressionType.LessThanOrEqualTo:
                case DecodeExpressionType.LessThan:
                case DecodeExpressionType.NotEqual:
                case DecodeExpressionType.Equal: {
                        var left = DecodeExpression(input);
                        var right = DecodeExpression(input);
                        return new Nodes.Comparison(exprType, left, right);
                    }
                case DecodeExpressionType.IntegerParameter:
                case DecodeExpressionType.PlayerParameter:
                case DecodeExpressionType.StringParameter:
                case DecodeExpressionType.ObjectParameter: {
                        var parameter = DecodeExpression(input);
                        return new Nodes.Parameter(exprType, parameter);
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        
        protected INode DecodeGenericElement(BinaryReader input, TagType tag, int length, String lenByte, int argCount, bool hasContent) {
            if (length == 0) {
                /*if (argCount > 0)
                    throw new ArgumentOutOfRangeException("argCount");*/
                return new Nodes.EmptyElement(tag, lenByte);
            }
            var arguments = new INode[argCount];
            for (var i = 0; i < argCount; ++i) {
                arguments[i] = DecodeExpression(input);
            }
            INode content = null;
            if (hasContent) {
                content = DecodeExpression(input);
            }

            return new Nodes.GenericElement(tag, content, lenByte, arguments);
        }
        
        protected INode DecodeGenericElementWithVariableArguments(BinaryReader input, TagType tag, int length, String lenByte, int minCount, int maxCount) {
            var end = input.BaseStream.Position + length;
            var args = new List<INode>();
            for (var i = 0; i < maxCount && input.BaseStream.Position < end; ++i) {
                args.Add(DecodeExpression(input));
            }
            return new Nodes.GenericElement(tag, null, lenByte, args);
        }
        
        protected INode DecodeGenericSurroundingTag(BinaryReader input, TagType tag, int length, String lenByte) {
            if (length != 1)
                throw new ArgumentOutOfRangeException("length");

            List<byte> insideLenByte = new List<byte> { };
            var status = GetInteger(input, insideLenByte);
            lenByte += BitConverter.ToString(insideLenByte.ToArray()).Replace("-", String.Empty);
            if (status == 0)
                return new Nodes.CloseTag(tag, lenByte);
            if (status == 1)
                return new Nodes.OpenTag(tag, lenByte, null); /* should be lenByte or insideLenByte? */
            throw new InvalidDataException();
        }
        #endregion

        #region Specific
        protected INode DecodeZeroPaddedValue(BinaryReader input, TagType tag, int length, String lenByte) {
            var val = DecodeExpression(input);
            var arg = DecodeExpression(input);
            return new GenericElement(tag, val, lenByte, arg);
        }
        
        protected INode DecodeColor(BinaryReader input, TagType tag, int length, String lenByte) {
            var t = input.ReadByte();
            // I think the byte should be added
            if (length == 1 && t == 0xEC)
                return new Nodes.CloseTag(tag, lenByte + t.ToString("X2"));
            var color = DecodeExpression(input, (DecodeExpressionType)t);
            return new Nodes.OpenTag(tag, lenByte, color);
        }
        
        protected INode DecodeFormat(BinaryReader input, TagType tag, int length, String lenByte) {
            var end = input.BaseStream.Position + length;

            var arg1 = DecodeExpression(input);
            var arg2 = new Nodes.StaticByteArray(input.ReadBytes((int)(end - input.BaseStream.Position)));
            return new Nodes.GenericElement(tag, null, lenByte, arg1, arg2);
        }
        
        protected INode DecodeIf(BinaryReader input, TagType tag, int length, String lenByte) {
            var end = input.BaseStream.Position + length;

            var condition = DecodeExpression(input);
            INode trueValue, falseValue;
            DecodeConditionalOutputs(input, (int)end, out trueValue, out falseValue);

            return new Nodes.IfElement(tag, condition, trueValue, falseValue, lenByte);
        }
        
        protected INode DecodeIfEquals(BinaryReader input, TagType tag, int length, String lenByte) {
            var end = input.BaseStream.Position + length;

            var left = DecodeExpression(input);
            var right = DecodeExpression(input);
            /*
            var trueValue = DecodeExpression(input);
            INode falseValue = null;
            if (input.BaseStream.Position != end)
                falseValue = DecodeExpression(input);*/

            INode trueValue, falseValue;
            DecodeConditionalOutputs(input, (int)end, out trueValue, out falseValue);

            return new Nodes.IfEqualsElement(tag, left, right, trueValue, falseValue, lenByte);
        }
        
        protected void DecodeConditionalOutputs(BinaryReader input, int end, out INode trueValue, out INode falseValue) {
            var exprs = new List<INode>();
            while (input.BaseStream.Position != end) {
                var expr = DecodeExpression(input);
                exprs.Add(expr);
            }

            // Only one instance with more than two expressions (LogMessage.en[1115][4])
            // TODO: Not sure how it should be handled, discarding all but first and second for now.
            if (exprs.Count > 0)
                trueValue = exprs[0];
            else
                trueValue = null;

            if (exprs.Count > 1)
                falseValue = exprs[1];
            else
                falseValue = null;
        }
        protected INode DecodeSwitch(BinaryReader input, TagType tag, int length, String lenByte) {
            var end = input.BaseStream.Position + length;
            var caseSwitch = DecodeExpression(input);

            var cases = new Dictionary<int, INode>();
            var i = 1;
            while (input.BaseStream.Position < end)
                cases.Add(i++, DecodeExpression(input));

            return new Nodes.SwitchElement(tag, caseSwitch, cases, lenByte);
        }
        #endregion

        #region Shared
        protected static int GetInteger(BinaryReader input, List<byte> lenByte) {
            // added new function
            var t = input.ReadByte();
            var type = (IntegerType)t;
            lenByte.Add(t);

            return GetInteger(input, type, lenByte);
        }
        
        protected static int GetInteger(BinaryReader input, IntegerType type, List<byte> lenByte) {
            const byte ByteLengthCutoff = 0xF0;

            var t = (byte)type;
            if (t < ByteLengthCutoff)
                return t - 1;

            switch (type) {
                case IntegerType.Byte: {
                        byte res = input.ReadByte();
                        lenByte.Add(res);
                        return (res);
                    }
                case IntegerType.ByteTimes256: {
                        byte res = input.ReadByte();
                        lenByte.Add(res);
                        return (res * 256);
                    }
                case IntegerType.Int16: {
                        int v = 0;
                        byte res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res << 8;
                        res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res;
                        return (v);
                    }
                case IntegerType.Int24: {
                        int v = 0;
                        byte res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res << 16;
                        res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res << 8;
                        res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res;
                        return (v);
                    }
                case IntegerType.Int32: {
                        int v = 0;
                        byte res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res << 24;
                        res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res << 16;
                        res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res << 8;
                        res = input.ReadByte();
                        lenByte.Add(res);
                        v |= res;
                        return (v);
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        #endregion
    }
}
