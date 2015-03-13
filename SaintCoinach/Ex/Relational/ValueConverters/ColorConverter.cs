using System;
using System.Drawing;

using YamlDotNet.Serialization;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class ColorConverter : IValueConverter {
        #region Properties

        public bool IncludesAlpha { get; set; }

        #endregion

        #region Constructors

        public ColorConverter() {
            IncludesAlpha = false;
        }

        #endregion

        #region IValueConverter Members

        [YamlIgnore]
        public string TargetTypeName { get { return "Color"; } }
        [YamlIgnore]
        public Type TargetType { get { return typeof(Color); } }

        public object Convert(IDataRow row, object rawValue) {
            var argb = System.Convert.ToInt32(rawValue);

            if (!IncludesAlpha)
                argb = (int)(((uint)argb) | 0xFF000000);

            return Color.FromArgb(argb);
        }

        #endregion
    }
}
