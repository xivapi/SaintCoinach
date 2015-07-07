using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class MultiReferenceConverter : IValueConverter {
        #region Properties

        public string[] Targets { get; set; }

        #endregion

        #region IValueConverter Members

        [YamlIgnore]
        public string TargetTypeName { get { return "Row"; } }
        [YamlIgnore]
        public Type TargetType { get { return typeof(IRelationalRow); } }

        public object Convert(IDataRow row, object rawValue) {
            var key = System.Convert.ToInt32(rawValue);
            if (Targets == null)
                return null;

            foreach(var target in Targets) {
                var sheet = row.Sheet.Collection.GetSheet(target);
                if (!sheet.Header.DataFileRanges.Any(r => r.Contains(key)))
                    continue;

                if (sheet.ContainsRow(key))
                    return sheet[key];
            }
            return null;
        }

        #endregion
    }
}
