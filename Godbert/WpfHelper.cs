using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Godbert {
    public static class WpfHelper {
        public static T FindParent<T>(DependencyObject child) where T : class {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
                return null;

            var parentAsType = parent as T;
            if (parentAsType != null)
                return parentAsType;

            return FindParent<T>(parent);
        }
    }
}
