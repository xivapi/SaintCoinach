﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using SaintCoinach.Ex.Relational.Definition.EXDSchema;

namespace SaintCoinach.Ex.Relational.Definition {
    public class GroupDataDefinition : IDataDefinition {
        #region Fields

        private ICollection<IDataDefinition> _Members = new List<IDataDefinition>();
        private int _Length;

        #endregion

        #region Properties

        public ICollection<IDataDefinition> Members {
            get { return _Members; }
            internal set {
                _Members = value;
                _Length = _Members.Sum(_ => _.Length);
            }
        }

        #endregion

        public int Length { get { return _Length; } }

        public IDataDefinition Clone() {
            var clone = new GroupDataDefinition();

            foreach (var member in Members)
                clone.Members.Add(member.Clone());

            return clone;
        }

        #region IDataDefinition Members

        public object Convert(IDataRow row, object value, int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            var convertedValue = value;
            var pos = 0;
            foreach (var member in Members) {
                var newPos = pos + member.Length;
                if (newPos > index) {
                    var innerIndex = index - pos;

                    convertedValue = member.Convert(row, value, innerIndex);

                    break;
                }
                pos = newPos;
            }
            return convertedValue;
        }

        public string GetName(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            string value = null;
            var pos = 0;
            foreach (var member in Members) {
                var newPos = pos + member.Length;
                if (newPos > index) {
                    var innerIndex = index - pos;
                    value = member.GetName(innerIndex);
                    break;
                }
                pos = newPos;
            }
            return value;
        }

        public string GetValueTypeName(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            string value = null;
            var pos = 0;
            foreach (var member in Members) {
                var newPos = pos + member.Length;
                if (newPos > index) {
                    var innerIndex = index - pos;
                    value = member.GetValueTypeName(innerIndex);
                    break;
                }
                pos = newPos;
            }
            return value;
        }

        public Type GetValueType(int index) {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException("index");

            Type value = null;
            var pos = 0;
            foreach (var member in Members) {
                var newPos = pos + member.Length;
                if (newPos > index) {
                    var innerIndex = index - pos;
                    value = member.GetValueType(innerIndex);
                    break;
                }
                pos = newPos;
            }
            return value;
        }

        #endregion

        #region Serialization

        public static GroupDataDefinition FromJson(JToken obj) {
            return new GroupDataDefinition() {
                Members = obj["members"].Select(m => DataDefinitionSerializer.FromJson(m)).ToList()
            };
        }
        
        public static GroupDataDefinition FromYaml(Field field) {
            return new GroupDataDefinition() {
                Members = field.Fields.Select(m => DataDefinitionSerializer.FromYaml(m)).ToList()
            };
        }

        public void ResolveReferences(SheetDefinition sheetDef) {
            foreach (var member in _Members)
                member.ResolveReferences(sheetDef);
        }

        #endregion
    }
}
