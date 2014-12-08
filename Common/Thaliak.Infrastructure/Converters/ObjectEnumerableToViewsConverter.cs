using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;

namespace Thaliak.Converters {
    [ValueConversion(typeof(IEnumerable), typeof(IEnumerable))]
    public class ObjectEnumerableToViewsConverter : IValueConverter {
        private ObjectToViewConverter _ItemConverter = new ObjectToViewConverter();

        public string Context {
            get { return _ItemConverter.Context; }
            set { _ItemConverter.Context = value; }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var en = value as IEnumerable;
            if (en == null)
                return null;

            var views = new List<object>();
            foreach(var o in en)
                views.Add(_ItemConverter.Convert(o, typeof(object), null, culture));
            return views;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
