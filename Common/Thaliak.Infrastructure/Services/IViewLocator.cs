using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace Thaliak.Services {
    public interface IViewLocator {
        Uri GetUri(object value, string context);
    }
}
