using Newtonsoft.Json.Linq;
using SaintCoinach.Ex.Relational.Definition;
using System;
using System.Drawing;

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

        public string TargetTypeName { get { return "Color"; } }
        public Type TargetType { get { return typeof(Color); } }

        public object Convert(IDataRow row, object rawValue) {
            var argb = System.Convert.ToInt32(rawValue);

            if (!IncludesAlpha)
                argb = (int)(((uint)argb) | 0xFF000000);

            return Color.FromArgb(argb);
        }

        #endregion

        #region Serialization

        public JObject ToJson() {
            return new JObject() {
                ["type"] = "color"
            };
        }

        public static ColorConverter FromJson(JToken obj) {
            return new ColorConverter();
        }

        public void ResolveReferences(SheetDefinition sheetDef) { }

        #endregion
    }
}
