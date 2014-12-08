using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class BuddySummon : ItemAction {
        const int MountKey = 0;

        #region Properties
        public Mount Mount {
            get {
                var key = GetData(MountKey);
                return Sheet.Collection.GetSheet<Mount>()[key];
            }
        }
        #endregion

        #region Constructor
        public BuddySummon(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}