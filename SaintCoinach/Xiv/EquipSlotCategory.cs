using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class EquipSlotCategory : XivRow {
        #region Fields
        private EquipSlot[] _PossibleSlots;
        private EquipSlot[] _BlockedSlots;
        #endregion

        #region Properties
        public IEnumerable<EquipSlot> PossibleSlots { get { return _PossibleSlots; } }
        public IEnumerable<EquipSlot> BlockedSlots { get { return _BlockedSlots; } }
        #endregion

        #region Constructor
        public EquipSlotCategory(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) {
            Build();
        }
        #endregion

        #region Build
        private void Build() {
            var possible = new List<EquipSlot>();
            var blocked = new List<EquipSlot>();

            foreach (var slot in Sheet.Collection.EquipSlots) {
                var val = Convert.ToInt32(this[slot.Key]);

                if (val > 0)
                    possible.Add(slot);
                else if (val < 0)
                    blocked.Add(slot);
            }

            _PossibleSlots = possible.ToArray();
            _BlockedSlots = blocked.ToArray();
        }
        #endregion
    }
}