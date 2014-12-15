using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Thaliak {
    public static class DragDropExtensions {
        public static object TryGetDragObject(this Services.IObjectStore self, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.StringFormat)) {
                var str = (string)e.Data.GetData(DataFormats.StringFormat);
                Guid id;
                if (Guid.TryParse(str, out id))
                    return self.Retrieve(id);
            }
            return null;
        }
    }
}
