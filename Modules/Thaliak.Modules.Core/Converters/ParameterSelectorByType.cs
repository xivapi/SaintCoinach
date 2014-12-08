using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;

namespace Thaliak.Modules.Core.Converters {
    [ValueConversion(typeof(IEnumerable<SaintCoinach.Xiv.Parameter>), typeof(IEnumerable<SaintCoinach.Xiv.Parameter>))]
    public class ParameterSelectorByType : IValueConverter {
        public SaintCoinach.Xiv.ParameterType ParameterType { get; set; }
        public bool IsReversed { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var en = value as IEnumerable<SaintCoinach.Xiv.Parameter>;
            if (en == null)
                return null;

            return en.Where(p => IsReversed ^ p.Values.Any(v => v.Type == ParameterType));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
