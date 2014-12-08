using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Thaliak.Converters {
    [ValueConversion(typeof(IEnumerable), typeof(Visibility))]
    public class EnumerableToVisibilityConverter : IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var enumerable = value as IEnumerable;
            var show = false;
            if (parameter != null)
                show = System.Convert.ToBoolean(parameter);

            if (enumerable != null)
                show ^= enumerable.Cast<object>().Any();

            return show ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
