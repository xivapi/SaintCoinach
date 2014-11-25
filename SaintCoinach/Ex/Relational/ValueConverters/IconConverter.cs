using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class IconConverter : IValueConverter {
        #region IValueConverter Members
        [YamlDotNet.Serialization.YamlIgnore]
        public string TargetTypeName {
            get { return "Image"; }
        }

        public object Convert(IDataRow row, object rawValue) {
            const string IconFileFormat = "ui/icon/{0:D3}000/{1}{2:D6}.tex";

            var nr = System.Convert.ToInt32(rawValue);
            if (nr < 0 || nr > 999999)
                return null;

            var sheet = row.Sheet;
            var type = sheet.Language.GetCode();
            if (type.Length > 0)
                type = type + "/";

            var filePath = string.Format(IconFileFormat, nr / 1000, type, nr);

            var pack = sheet.Collection.PackCollection;
            IO.File file;
            if (!pack.TryGetFile(filePath, out file)) {
                // Couldn't get language specific, try for generic version.
                filePath = string.Format(IconFileFormat, nr / 1000, string.Empty, nr);
                if (!pack.TryGetFile(filePath, out file)) {
                    // Couldn't get generic version either, that's a shame.
                    file = null;
                }
            }
            return file;
        }

        #endregion
    }
}
