using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class RelicNoteCategory : XivRow {
        #region Properties
        public string Text { get { return AsString("Text"); } }
        public RelicNoteCategoryText RelicNoteCategoryText { get { return As<RelicNoteCategoryText>(); } }
        #endregion

        #region Constructor
        public RelicNoteCategory(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Text;
        }
    }
}