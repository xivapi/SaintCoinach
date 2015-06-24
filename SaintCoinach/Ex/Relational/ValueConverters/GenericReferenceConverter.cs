using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class GenericReferenceConverter : IValueConverter {

        #region IValueConverter Members

        [YamlIgnore]
        public string TargetTypeName { get { return "Row"; } }
        [YamlIgnore]
        public Type TargetType { get { return typeof(IRelationalRow); } }

        public object Convert(IDataRow row, object rawValue) {
            var coll = (RelationalExCollection)row.Sheet.Collection;
            var key = System.Convert.ToInt32(rawValue);
            return coll.FindReference(key);
        }

        #endregion
    }
}
