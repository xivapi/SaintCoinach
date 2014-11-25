using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Definition {
    public class SheetDefinition : ICloneable {
        #region Fields
        private bool _IsCompiled = false;
        private ICollection<PositionedDataDefintion> _DataDefinitions = new List<PositionedDataDefintion>();
        private Dictionary<int, PositionedDataDefintion> _ColumnDefinitionMap = null;
        private Dictionary<string, int> _ColumnNameToIndexMap = null;
        private Dictionary<int, string> _ColumnIndexToNameMap = null;
        private Dictionary<int, string> _ColumnValueTypes = null;
        private int? _DefaultColumnIndex = null;
        private string _Name;
        private string _DefaultColumn;
        #endregion

        #region Properties
        public ICollection<PositionedDataDefintion> DataDefinitions {
            get { return _DataDefinitions; }
            internal set { _DataDefinitions = value; }
        }
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }
        public string DefaultColumn {
            get { return _DefaultColumn; }
            set { _DefaultColumn = value; }
        }
        #endregion

        #region Helpers
        public bool TryGetDefinition(int index, out PositionedDataDefintion definition) {
            if (_IsCompiled)
                return _ColumnDefinitionMap.TryGetValue(index, out definition);

            var res = DataDefinitions.Where(_ => _.Index <= index && index < (_.Index + _.Length));
            if (res.Any())
                definition = res.First();
            else
                definition = null;

            return definition != null;
        }
        public int? GetDefaultColumnIndex() {
            if (_IsCompiled)
                return _DefaultColumnIndex;
            if (string.IsNullOrWhiteSpace(DefaultColumn))
                return null;
            return FindColumn(DefaultColumn);
        }
        public int? FindColumn(string columnName) {
            if (_IsCompiled) {
                if (_ColumnNameToIndexMap.ContainsKey(columnName))
                    return _ColumnNameToIndexMap[columnName];
                return null;
            }

            foreach (var def in DataDefinitions) {
                for (var i = 0; i < def.Length; ++i) {
                    var n = def.GetName(def.Index + i);
                    if (string.Equals(columnName, n, StringComparison.OrdinalIgnoreCase))
                        return def.Index + i;
                }
            }

            return null;
        }
        public IEnumerable<string> GetAllColumnNames() {
            if (_IsCompiled) {
                foreach (var n in _ColumnNameToIndexMap.Keys)
                    yield return n;
                yield break;
            }

            foreach (var def in DataDefinitions) {
                for (var i = 0; i < def.Length; ++i)
                    yield return def.GetName(def.Index + i);
            }
        }
        public string GetColumnName(int index) {
            if (_IsCompiled) {
                if (_ColumnIndexToNameMap.ContainsKey(index))
                    return _ColumnIndexToNameMap[index];
                return null;
            }

            PositionedDataDefintion def;
            if (TryGetDefinition(index, out def))
                return def.GetName(index);
            return null;
        }
        public string GetValueType(int index) {
            if (_IsCompiled) {
                if (_ColumnValueTypes.ContainsKey(index))
                    return _ColumnValueTypes[index];
                return null;
            }

            PositionedDataDefintion def;
            if (TryGetDefinition(index, out def))
                return def.GetValueType(index);
            return null;
        }
        public object Convert(IDataRow row, object value, int index) {
            PositionedDataDefintion def;
            if (TryGetDefinition(index, out def))
                return def.Convert(row, value, index);
            return value;
        }
        #endregion

        #region Compile
        public void Compile() {
            _ColumnDefinitionMap = new Dictionary<int, PositionedDataDefintion>();
            _ColumnNameToIndexMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _ColumnIndexToNameMap = new Dictionary<int, string>();
            _ColumnValueTypes = new Dictionary<int, string>();
            foreach (var def in DataDefinitions) {
                for (var i = 0; i < def.Length; ++i) {
                    var offset = def.Index + i;
                    _ColumnDefinitionMap.Add(offset, def);

                    var name = def.GetName(offset);
                    _ColumnNameToIndexMap.Add(name, offset);
                    _ColumnIndexToNameMap.Add(offset, name);
                    _ColumnValueTypes.Add(offset, def.GetValueType(offset));
                }
            }

            if (!string.IsNullOrWhiteSpace(DefaultColumn)) {
                int defCol;
                if (_ColumnNameToIndexMap.TryGetValue(DefaultColumn, out defCol))
                    _DefaultColumnIndex = defCol;
                else
                    _DefaultColumnIndex = null;
            } else
                _DefaultColumnIndex = null;

            _IsCompiled = true;
        }
        #endregion

        #region ICloneable Members

        object ICloneable.Clone() {
            throw new NotImplementedException();
        }

        #endregion
    }
}
