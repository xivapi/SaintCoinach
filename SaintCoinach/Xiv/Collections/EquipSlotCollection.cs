using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Collections {
    public class EquipSlotCollection : IEnumerable<EquipSlot> {
        #region Fields
        private EquipSlot[] _EquipSlots;
        private XivCollection _Collection;
        #endregion

        #region Properties
        public XivCollection Collection { get { return _Collection; } }
        #endregion

        #region Constructor
        protected internal EquipSlotCollection(XivCollection collection) {
            const int SlotCount = 14;

            _Collection = collection;
            _EquipSlots = new EquipSlot[SlotCount];
            for (int i = 0; i < SlotCount; ++i)
                _EquipSlots[i] = new EquipSlot(this, i);
        }
        #endregion

        #region this[]
        public EquipSlot this[int key] {
            get { return _EquipSlots[key]; }
        }
        #endregion

        #region IEnumerable<EquipSlot> Members
        public IEnumerator<EquipSlot> GetEnumerator() {
            return ((IEnumerable<EquipSlot>)_EquipSlots).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        #endregion
    }
}
