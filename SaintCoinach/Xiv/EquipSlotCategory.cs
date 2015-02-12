using System;
using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing equipment slot categories from the game data.
    /// </summary>
    public class EquipSlotCategory : XivRow {
        #region Fields

        /// <summary>
        ///     <see cref="EquipSlot" />s that get blocked when using the current <see cref="EquipSlotCategory" />.
        /// </summary>
        private EquipSlot[] _BlockedSlots;

        /// <summary>
        ///     <see cref="EquipSlot" />s on which the current <see cref="EquipSlotCategory" /> can be used.
        /// </summary>
        private EquipSlot[] _PossibleSlots;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets <see cref="EquipSlot" />s that get blocked when using the current <see cref="EquipSlotCategory" />.
        /// </summary>
        /// <value>The <see cref="EquipSlot" />s that get blocked when using the current <see cref="EquipSlotCategory" />.</value>
        public IEnumerable<EquipSlot> BlockedSlots { get { return _BlockedSlots; } }

        /// <summary>
        ///     Gets <see cref="EquipSlot" />s on which the current <see cref="EquipSlotCategory" /> can be used.
        /// </summary>
        /// <value>The <see cref="EquipSlot" />s on which the current <see cref="EquipSlotCategory" /> can be used.</value>
        public IEnumerable<EquipSlot> PossibleSlots { get { return _PossibleSlots; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EquipSlotCategory" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public EquipSlotCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) {
            Build();
        }

        #endregion

        #region Build

        /// <summary>
        ///     Build the list of blocked and possible <see cref="EquipSlot" />s for the current <see cref="EquipSlotCategory" />.
        /// </summary>
        /// <remarks>
        ///     Columns of the EquipSlotCategory are used as keys for <see cref="EquipSlot" />s.
        ///     A column value of -1 means that the <see cref="EquipSlot" /> gets blocked and a value of 1 is used for possible
        ///     <see cref="EquipSlot" />s.
        /// </remarks>
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
