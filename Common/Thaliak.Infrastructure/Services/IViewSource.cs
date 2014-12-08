using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace Thaliak.Services {
    public interface IViewSource {
        bool IsHandled(Type modelType, string context);
        Uri GetViewUri(Type modelType, object value, string context);
    }
}
