using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class SheetLinkConverter : IValueConverter {
        #region Fields
        private string _TargetSheet;
        #endregion

        #region Properties
        public string TargetSheet {
            get { return _TargetSheet; }
            set { _TargetSheet = value; }
        }
        #endregion

        #region IValueConverter Members

        [YamlDotNet.Serialization.YamlIgnore]
        public string TargetTypeName {
            get { return TargetSheet; }
        }

        public object Convert(IDataRow row, object rawValue) {
            var coll = row.Sheet.Collection;
            if (!coll.SheetExists(TargetSheet))
                return null;

            var sheet = coll.GetSheet(TargetSheet);

            var key = System.Convert.ToInt32(rawValue);
            if (!sheet.ContainsRow(key))
                return null;

            return sheet[key];
        }
        #endregion
    }
}
