using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class BuddyEquipUnlock : ItemAction {
        const int BuddyEquipIndex = 0;

        #region Properties
        public BuddyEquip BuddyEquip {
            get {
                var key = GetData(BuddyEquipIndex);
                return Sheet.Collection.GetSheet<BuddyEquip>()[key];
            }
        }
        #endregion

        #region Constructor
        public BuddyEquipUnlock(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}