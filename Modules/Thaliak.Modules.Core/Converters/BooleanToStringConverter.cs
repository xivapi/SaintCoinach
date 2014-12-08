using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;

namespace Thaliak.Modules.Core.Converters {
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToStringConverter : IValueConverter {
        public string ValueOnTrue { get; set; }
        public string ValueOnFalse { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value == null)
                return ValueOnFalse;
            var b = System.Convert.ToBoolean(value);

            return b ? ValueOnTrue : ValueOnFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
