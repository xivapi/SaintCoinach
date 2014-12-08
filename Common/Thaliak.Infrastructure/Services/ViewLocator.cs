using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Services {
    [Export(typeof(IViewLocator))]
    public class ViewLocator : IViewLocator {
        #region Fields
        [ImportMany(AllowRecomposition = true)]
        public Lazy<IViewSource>[] RegisteredFactories { get; set; }
        [Import]
        private ILoggerFacade _Logger;
        #endregion

        #region IViewLocator Members
        public Uri GetUri(object value, string context) {
            if (value == null)
                throw new ArgumentNullException("value");

            var t = value.GetType();
            foreach (var factory in RegisteredFactories) {
                if (factory.Value.IsHandled(t, context))
                    return factory.Value.GetViewUri(t, value, context);
            }
            //throw new NotSupportedException(string.Format("Could not locate view for type '{0}'.", t.FullName));
            _Logger.Log(string.Format("Unable to locate view for type '{0}'.", t.FullName), Category.Warn, Priority.High);
            return null;
        }
        #endregion
    }
}
