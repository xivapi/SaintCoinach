using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using SchemaUtil = SaintCoinach.Ex.Relational.Definition.EXDSchema.SchemaUtil;
using Sheet = SaintCoinach.Ex.Relational.Definition.EXDSchema.Sheet;

namespace SaintCoinach.Ex.Relational.Definition {
    public class SheetDefinition {
        #region Fields

        private Dictionary<int, PositionedDataDefinition> _ColumnDefinitionMap;
        private Dictionary<int, string> _ColumnIndexToNameMap;
        private Dictionary<string, int> _ColumnNameToIndexMap;
        private Dictionary<int, string> _ColumnValueTypeNames;
        private Dictionary<int, Type> _ColumnValueTypes;
        private ICollection<PositionedDataDefinition> _DataDefinitions = new List<PositionedDataDefinition>();
        private int? _DefaultColumnIndex;
        private bool _IsCompiled;

        #endregion

        #region Properties

        public ICollection<PositionedDataDefinition> DataDefinitions {
            get { return _DataDefinitions; }
            internal set { _DataDefinitions = value; }
        }

        public string Name { get; set; }
        public string DefaultColumn { get; set; }
        public bool IsGenericReferenceTarget { get; set; }
        public bool IsPostProcessed { get; set; }

        #endregion

        #region Compile

        public void Compile()
        {
            if (!IsPostProcessed || _IsCompiled) return;
            
            _ColumnDefinitionMap = new Dictionary<int, PositionedDataDefinition>();
            _ColumnNameToIndexMap = new Dictionary<string, int>();
            _ColumnIndexToNameMap = new Dictionary<int, string>();
            _ColumnValueTypeNames = new Dictionary<int, string>();
            _ColumnValueTypes = new Dictionary<int, Type>();
            DataDefinitions = DataDefinitions.OrderBy(d => d.ColumnBasedIndex).ToList();
            foreach (var def in DataDefinitions) {
                for (var i = 0; i < def.Length; ++i) {
                    var offset = def.ColumnBasedIndex + i;
                    _ColumnDefinitionMap.Add(offset, def);

                    var name = def.GetName(offset);
                    _ColumnNameToIndexMap.Add(name, offset);
                    _ColumnIndexToNameMap.Add(offset, name);
                    _ColumnValueTypeNames.Add(offset, def.GetValueTypeName(offset));
                    _ColumnValueTypes.Add(offset, def.GetValueType(offset));
                }
            }

            if (!string.IsNullOrWhiteSpace(DefaultColumn)) {
                if (_ColumnNameToIndexMap.TryGetValue(DefaultColumn, out var defCol))
                    _DefaultColumnIndex = defCol;
                else
                    _DefaultColumnIndex = null;
            } else
                _DefaultColumnIndex = null;

            _IsCompiled = true;
        }

        #endregion

        #region Helpers

        public bool TryGetDefinition(int index, out PositionedDataDefinition definition) {
            if (_IsCompiled && IsPostProcessed)
                return _ColumnDefinitionMap.TryGetValue(index, out definition);

            var res = DataDefinitions.Where(_ => _.ColumnBasedIndex <= index && index < (_.ColumnBasedIndex + _.Length)).ToArray();
            definition = res.Any() ? res.First() : null;

            return definition != null;
        }

        public int? GetDefaultColumnIndex() {
            if (_IsCompiled)
                return _DefaultColumnIndex;
            return string.IsNullOrWhiteSpace(DefaultColumn) ? null : FindColumn(DefaultColumn);
        }

        public int? FindColumn(string columnName) {
            if (_IsCompiled && IsPostProcessed) {
                if (_ColumnNameToIndexMap.ContainsKey(columnName))
                    return _ColumnNameToIndexMap[columnName];
                return null;
            }

            foreach (var def in DataDefinitions) {
                for (var i = 0; i < def.Length; ++i) {
                    var n = def.GetName(def.ColumnBasedIndex + i);
                    if (string.Equals(columnName, n))
                        return def.ColumnBasedIndex + i;
                }
            }

            return null;
        }
        
        public void PostProcess(IEnumerable<Column> columns)
        {
            if (IsPostProcessed)
                return;

            var remap = columns.ToDictionary(c => c.OffsetBasedIndex, c => c.ColumnBasedIndex);
            for (int i = 0; i < DataDefinitions.Count; i++)
            {
                var def = DataDefinitions.ElementAt(i);
                var offsetBasedIndex = def.OffsetBasedIndex;
                var columnBasedIndex = remap[offsetBasedIndex];
                def.ColumnBasedIndex = columnBasedIndex;
            }
            
            Compile();

            IsPostProcessed = true;
        }

        public IEnumerable<string> GetAllColumnNames() {
            if (_IsCompiled) {
                foreach (var n in _ColumnNameToIndexMap.Keys)
                    yield return n;
                yield break;
            }

            foreach (var def in DataDefinitions) {
                for (var i = 0; i < def.Length; ++i)
                    yield return def.GetName(def.ColumnBasedIndex + i);
            }
        }

        public string GetColumnName(int index) {
            if (_IsCompiled && IsPostProcessed)
                return _ColumnIndexToNameMap.ContainsKey(index) ? _ColumnIndexToNameMap[index] : null;

            return TryGetDefinition(index, out var def) ? def.GetName(index) : null;
        }

        public string GetValueTypeName(int index) {
            if (_IsCompiled && IsPostProcessed)
                return _ColumnValueTypeNames.ContainsKey(index) ? _ColumnValueTypeNames[index] : null;

            return TryGetDefinition(index, out var def) ? def.GetValueTypeName(index) : null;
        }

        public Type GetValueType(int index) {
            if (_IsCompiled && IsPostProcessed)
                return _ColumnValueTypes.ContainsKey(index) ? _ColumnValueTypes[index] : null;

            return TryGetDefinition(index, out var def) ? def.GetValueType(index) : null;
        }

        public object Convert(IDataRow row, object value, int index) {
            return TryGetDefinition(index, out var def) ? def.Convert(row, value, index) : value;
        }

        #endregion

        #region Serialization

        public static SheetDefinition FromJson(JToken obj) {
            var sheetDef = new SheetDefinition() {
                Name = (string)obj["sheet"],
                DefaultColumn = (string)obj["defaultColumn"],
                IsGenericReferenceTarget = (bool?)obj["isGenericReferenceTarget"] ?? false,
                DataDefinitions = new List<PositionedDataDefinition>(obj["definitions"].Select(j => PositionedDataDefinition.FromJson(j)))
            };

            foreach (var dataDef in sheetDef.DataDefinitions)
                dataDef.ResolveReferences(sheetDef);

            return sheetDef;
        }

        public static SheetDefinition FromYaml(Sheet sheet) {
            var sheetDef = new SheetDefinition() {
                Name = sheet.Name,
                DefaultColumn = sheet.DisplayField,
                IsGenericReferenceTarget = false,
                DataDefinitions = new List<PositionedDataDefinition>(sheet.Fields.Select(j => PositionedDataDefinition.FromYaml(j)))
            };

            foreach (var dataDef in sheetDef.DataDefinitions)
                dataDef.ResolveReferences(sheetDef);

            return sheetDef;
        }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
