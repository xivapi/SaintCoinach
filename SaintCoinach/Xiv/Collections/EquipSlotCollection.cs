using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Xiv.Collections {
    public class EquipSlotCollection : IEnumerable<EquipSlot> {
        #region Fields

        private readonly EquipSlot[] _EquipSlots;

        #endregion

        #region Properties

        public XivCollection Collection { get; private set; }

        #region this[]

        public EquipSlot this[int key] { get { return _EquipSlots[key]; } }

        #endregion

        #endregion

        #region Constructors

        #region Constructor

        protected internal EquipSlotCollection(XivCollection collection) {
            const int SlotCount = 14;

            Collection = collection;
            _EquipSlots = new EquipSlot[SlotCount];
            for (var i = 0; i < SlotCount; ++i)
                _EquipSlots[i] = new EquipSlot(this, i);
        }

        #endregion

        #endregion

        #region IEnumerable<EquipSlot> Members

        public IEnumerator<EquipSlot> GetEnumerator() {
            return ((IEnumerable<EquipSlot>)_EquipSlots).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
