using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class LogFilter : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public LogKind LogKind { get { return As<LogKind>(); } }
        public string Example { get { return AsString("Example"); } }
        #endregion

        #region Constructor
        public LogFilter(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}