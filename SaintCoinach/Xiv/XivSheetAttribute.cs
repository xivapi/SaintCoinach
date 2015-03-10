using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class XivSheetAttribute : Attribute {
        public string SheetName { get; set; }

        public XivSheetAttribute(string sheetName) {
            this.SheetName = sheetName;
        }
    }
}
