using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class LogKind : XivRow {
        #region Properties
        public string Format { get { return AsString("Format"); } }
        public string Name { get { return AsString("Name"); } }
        public string Example { get { return AsString("Example"); } }
        public LogKindCategoryText LogKindCategoryText { get { return As<LogKindCategoryText>(); } }
        #endregion

        #region Constructor
        public LogKind(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}