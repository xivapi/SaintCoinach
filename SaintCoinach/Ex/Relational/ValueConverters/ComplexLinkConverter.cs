using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Xiv;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class ComplexLinkConverter : IValueConverter {
        SheetLinkData[] _Links;

        #region IValueConverter Members

        public string TargetTypeName => "Row";

        public Type TargetType => typeof(IRelationalRow);

        public object Convert(IDataRow row, object rawValue) {
            var key = System.Convert.ToInt32(rawValue);
            if (key == 0)
                return null;

            var coll = row.Sheet.Collection;

            foreach (var link in _Links) {
                if (link.When != null && !link.When.Match(row))
                    continue;

                var result = link.GetRow(key, coll);
                if (result == null)
                    continue;

                return link.Projection.Project(result);
            }

            return null;
        }


        #endregion

        #region Serialization

        public JObject ToJson() {
            return new JObject() {
                ["type"] = "complexlink",
                ["links"] = new JArray(_Links.Select(l => l.ToJson()))
            };
        }

        public static ComplexLinkConverter FromJson(JToken obj) {
            return new ComplexLinkConverter() {
                _Links = obj["links"].Select(o => SheetLinkData.FromJson((JObject)o)).ToArray()
            };
        }

        #endregion

        #region SheetLinkData

        interface IRowProducer {
            IRow GetRow(IRelationalSheet sheet, int key);
        }

        class PrimaryKeyRowProducer : IRowProducer {
            public IRow GetRow(IRelationalSheet sheet, int key) {
                return !sheet.ContainsRow(key) ? null : sheet[key];
            }
        }

        class IndexedRowProducer : IRowProducer {
            public string KeyColumnName;

            public IRow GetRow(IRelationalSheet sheet, int key) {
                return sheet.IndexedLookup(KeyColumnName, key);
            }
        }

        interface IProjectable {
            object Project(IRow row);
        }

        class IdentityProjection : IProjectable {
            public object Project(IRow row) {
                return row;
            }
        }

        class ColumnProjection : IProjectable {
            public string ProjectedColumnName;

            public object Project(IRow row) {
                var relationalRow = (IRelationalRow)row;
                return relationalRow[ProjectedColumnName];
            }
        }

        class LinkCondition {
            public string KeyColumnName;
            public int KeyColumnIndex;
            public object Value;
            bool _ValueTypeChanged;

            public bool Match(IDataRow row) {
                var rowValue = row[KeyColumnIndex];
                if (!_ValueTypeChanged && rowValue != null) {
                    Value = System.Convert.ChangeType(Value, rowValue.GetType());
                    _ValueTypeChanged = true;
                }
                return Equals(rowValue, Value);
            }
        }

        abstract class SheetLinkData {
            public string ProjectedColumnName;
            public string KeyColumnName;

            public IRowProducer RowProducer;
            public IProjectable Projection;

            public LinkCondition When;

            public abstract IRow GetRow(int key, ExCollection collection);

            public virtual JObject ToJson() {
                var obj = new JObject();
                if (ProjectedColumnName != null)
                    obj["project"] = ProjectedColumnName;
                if (KeyColumnName != null)
                    obj["key"] = KeyColumnName;
                if (When != null) {
                    obj["when"] = new JObject() {
                        ["key"] = When.KeyColumnName,
                        ["value"] = new JValue(When.Value)
                    };
                }

                return obj;
            }

            public static SheetLinkData FromJson(JObject obj) {
                SheetLinkData data;
                if (obj["sheet"] != null) {
                    data = new SingleSheetLinkData() {
                        SheetName = (string)obj["sheet"]
                    };
                } else if (obj["sheets"] != null) {
                    data = new MultiSheetLinkData() {
                        SheetNames = ((JArray)obj["sheets"]).Select(t => (string)t).ToArray()
                    };
                } else
                    throw new InvalidOperationException("complexlink link must contain either 'sheet' or 'sheets'.");

                if (obj["project"] == null)
                    data.Projection = new IdentityProjection();
                else {
                    data.ProjectedColumnName = (string)obj["project"];
                    data.Projection = new ColumnProjection() { ProjectedColumnName = data.ProjectedColumnName };
                }

                if (obj["key"] == null)
                    data.RowProducer = new PrimaryKeyRowProducer();
                else {
                    data.KeyColumnName = (string)obj["key"];
                    data.RowProducer = new IndexedRowProducer() { KeyColumnName = data.KeyColumnName };
                }

                var when = obj["when"];
                if (when != null) {
                    var condition = new LinkCondition();
                    condition.KeyColumnName = (string)when["key"];
                    condition.Value = when["value"].ToObject<object>();
                    data.When = condition;
                }

                return data;
            }
        }

        class SingleSheetLinkData : SheetLinkData {
            public string SheetName;

            public override JObject ToJson() {
                var obj = base.ToJson();
                obj["sheet"] = SheetName;
                return obj;
            }

            public override IRow GetRow(int key, ExCollection collection) {
                var sheet = (IRelationalSheet)collection.GetSheet(SheetName);
                return RowProducer.GetRow(sheet, key);
            }
        }

        class MultiSheetLinkData : SheetLinkData {
            public string[] SheetNames;

            public override JObject ToJson() {
                var obj = base.ToJson();
                obj["sheets"] = new JArray(SheetNames);
                return obj;
            }

            public override IRow GetRow(int key, ExCollection collection) {
                foreach (var sheetName in SheetNames) {
                    var sheet = (IRelationalSheet)collection.GetSheet(sheetName);
                    if (!sheet.Header.DataFileRanges.Any(r => r.Contains(key)))
                        continue;

                    var row = RowProducer.GetRow(sheet, key);
                    if (row != null)
                        return row;
                }
                return null;
            }
        }

        public void ResolveReferences(SheetDefinition sheetDef) {
            foreach (var link in _Links) {
                if (link.When != null) {
                    var keyDefinition = sheetDef.DataDefinitions
                        .FirstOrDefault(d => d.InnerDefinition.GetName(0) == link.When.KeyColumnName);
                    if (keyDefinition == null)
                        throw new InvalidOperationException($"Can't find conditional key column '{link.When.KeyColumnName}' in sheet '{sheetDef.Name}'");

                    link.When.KeyColumnIndex = keyDefinition.Index;
                }
            }
        }

        #endregion
    }
}
