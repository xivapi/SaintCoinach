using System;
using SaintCoinach.IO;

using YamlDotNet.Serialization;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class IconConverter : IValueConverter {
        #region IValueConverter Members

        [YamlIgnore]
        public string TargetTypeName { get { return "Image"; } }
        [YamlIgnore]
        public Type TargetType { get { return typeof(Imaging.ImageFile); } }

        public object Convert(IDataRow row, object rawValue) {

            var nr = System.Convert.ToInt32(rawValue);
            if (nr < 0 || nr > 999999)
                return null;

            var sheet = row.Sheet;
            return Imaging.IconHelper.GetIcon(sheet.Collection.PackCollection, sheet.Language, nr);
        }

        #endregion
    }
}
