using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class GatheringSubCategory : XivRow {
        #region Properties

        public Item Item => As<Item>();
        public string FolkloreBookName => AsString("FolkloreBook");

        #endregion

        public GatheringSubCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
    }
}
