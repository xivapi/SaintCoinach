using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class LogMessage : XivRow {
        #region Properties
        public string Text { get { return AsString("Text"); } }
        public LogKind LogKind { get { return As<LogKind>(); } }
        #endregion

        #region Constructor
        public LogMessage(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Text;
        }
    }
}