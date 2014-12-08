using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class CompanionUnlock : ItemAction {
        const int CompanionKey = 0;

        #region Properties
        public Companion Companion {
            get {
                var key = GetData(CompanionKey);
                return Sheet.Collection.GetSheet<Companion>()[key];
            }
        }
        #endregion

        #region Constructor
        public CompanionUnlock(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}