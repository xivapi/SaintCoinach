using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
                var sheet = (IRelationalSheet)coll.GetSheet(link.SheetName);
                var result = link.RowProducer.GetRow(sheet, key);
                if (result == null)
                    continue;

                return link.Projection.Project(result);
            }

            return rawValue;
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
                return sheet[key];
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

        class SheetLinkData {
            public string SheetName;
            public string ProjectedColumnName;
            public string KeyColumnName;

            public IRowProducer RowProducer;
            public IProjectable Projection;

            public JObject ToJson() {
                var obj = new JObject() { ["sheet"] = SheetName };
                if (ProjectedColumnName != null)
                    obj["project"] = ProjectedColumnName;
                if (KeyColumnName != null)
                    obj["key"] = KeyColumnName;

                return obj;
            }

            public static SheetLinkData FromJson(JObject obj) {
                var data = new SheetLinkData() {
                    SheetName = (string)obj["sheet"]
                };

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

                return data;
            }
        }

        #endregion
    }
}
