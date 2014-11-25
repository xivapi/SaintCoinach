using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.ValueConverters {
    public class ColorConverter : IValueConverter {
        #region Fields
        private bool _IncludesAlpha = false;
        #endregion

        #region Properties
        public bool IncludesAlpha {
            get { return _IncludesAlpha; }
            set { _IncludesAlpha = value; }
        }
        #endregion

        #region IValueConverter Members

        [YamlDotNet.Serialization.YamlIgnore]
        public string TargetTypeName {
            get { return "Color"; }
        }

        public object Convert(IDataRow row, object rawValue) {
            var argb = System.Convert.ToInt32(rawValue);

            if (!IncludesAlpha)
                argb = (int)(((uint)argb) | 0xFF000000);

            return System.Drawing.Color.FromArgb(argb);
        }

        #endregion
    }
}
