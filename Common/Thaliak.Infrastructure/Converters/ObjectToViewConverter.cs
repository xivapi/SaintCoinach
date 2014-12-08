using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Data;
using Microsoft.Practices.Prism;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Converters {
    [ValueConversion(typeof(object), typeof(FrameworkElement))]
    public class ObjectToViewConverter : IValueConverter {
        private Services.IViewLocator _ViewLocator;
        private IServiceLocator _ServiceLocator;
        private Services.IViewLocator ViewLocator { get { return _ViewLocator ?? (_ViewLocator = ServiceLocator.Current.GetInstance<Services.IViewLocator>()); } }

        public string Context { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value == null)
                return null;

            var targetUri = ViewLocator.GetUri(value, Context);
            if (targetUri == null)
                return value;

            var contract = UriParsingHelper.GetAbsolutePath(targetUri);
            contract = contract.TrimStart('/');
            var item = ServiceLocator.Current.GetInstance<object>(contract);
            var asFrameworkEle = item as FrameworkElement;
            if (asFrameworkEle == null)
                throw new NotSupportedException();
            asFrameworkEle.DataContext = value;
            return asFrameworkEle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
