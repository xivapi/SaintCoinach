//  This file is part of YamlDotNet - A .NET library for YAML.
//  Copyright (c) 2013 Antoine Aubry and contributors

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Ex.Relational.ValueConverters;

using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace SaintCoinach.Ex.Relational.Serialization {
    public sealed class ExRelationEventEmitter : ChainedEventEmitter {
        #region Fields

        private readonly Dictionary<Type, string> _Tags = new Dictionary<Type, string> {
            {
                typeof(MultiReferenceConverter), "multiref_conv"
            }, {
                typeof(GenericReferenceConverter), "ref_conv"
            }, {
                typeof(ColorConverter), "color_conv"
            }, {
                typeof(IconConverter), "icon_conv"
            }, {
                typeof(SheetLinkConverter), "link_conv"
            }, {
                typeof(GroupDataDefinition), "group_def"
            }, {
                typeof(RepeatDataDefinition), "repeat_def"
            }, {
                typeof(SingleDataDefinition), "single_def"
            }
        };

        #endregion

        #region Constructors

        public ExRelationEventEmitter(IEventEmitter nextEmitter)
            : base(nextEmitter) { }

        #endregion

        public override void Emit(ScalarEventInfo eventInfo) {
            eventInfo.IsPlainImplicit = true;
            eventInfo.Style = ScalarStyle.Plain;

            var typeCode = eventInfo.Source.Value != null
                               ? Type.GetTypeCode(eventInfo.Source.Type)
                               : TypeCode.Empty;

            switch (typeCode) {
                case TypeCode.Boolean:
                    eventInfo.Tag = "tag:yaml.org,2002:bool";
                    eventInfo.RenderedValue = YamlFormatter.FormatBoolean(eventInfo.Source.Value);
                    break;

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    eventInfo.Tag = "tag:yaml.org,2002:int";
                    eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
                    break;

                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    eventInfo.Tag = "tag:yaml.org,2002:float";
                    eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
                    break;

                case TypeCode.String:
                case TypeCode.Char:
                    eventInfo.Tag = "tag:yaml.org,2002:str";
                    if (eventInfo.Source != null)
                        eventInfo.RenderedValue = eventInfo.Source.Value == null
                                                      ? string.Empty
                                                      : eventInfo.Source.Value.ToString();
                    eventInfo.Style = ScalarStyle.Any;
                    break;

                case TypeCode.DateTime:
                    eventInfo.Tag = "tag:yaml.org,2002:timestamp";
                    eventInfo.RenderedValue = YamlFormatter.FormatDateTime(eventInfo.Source.Value);
                    break;

                case TypeCode.Empty:
                    eventInfo.Tag = "tag:yaml.org,2002:null";
                    eventInfo.RenderedValue = "";
                    break;

                default:
                    if (eventInfo.Source.Type != typeof(TimeSpan))
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
                            "TypeCode.{0} is not supported.", typeCode));
                    eventInfo.RenderedValue = YamlFormatter.FormatTimeSpan(eventInfo.Source.Value);
                    break;
            }

            base.Emit(eventInfo);
        }

        public override void Emit(MappingStartEventInfo eventInfo) {
            AssignTypeIfDifferent(eventInfo);
            base.Emit(eventInfo);
        }

        public override void Emit(SequenceStartEventInfo eventInfo) {
            AssignTypeIfDifferent(eventInfo);
            base.Emit(eventInfo);
        }

        private void AssignTypeIfDifferent(ObjectEventInfo eventInfo) {
            if (eventInfo.Source.Value == null) return;
            if (eventInfo.Source.Type == eventInfo.Source.StaticType) return;

            var tagRes =
                _Tags.Where(_ => _.Key.IsAssignableFrom(eventInfo.Source.Type)).Select(_ => _.Value).ToArray();
            if (tagRes.Any())
                eventInfo.Tag = "!!" + tagRes.First();
        }
    }
}
