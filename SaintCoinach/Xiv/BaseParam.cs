using System;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class BaseParam : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }

        #endregion

        #region Constructors

        #region Constructor

        public BaseParam(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }

        #region Helpers

        public int GetMaximum(EquipSlotCategory category) {
            const int Offset = 2;
            return category.Key == 0 ? 0 : Convert.ToInt32(this[Offset + category.Key]);
        }

        public int GetModifier(int key) {
            const int Offset = 24;
            const int Maximum = 12;
            if (key < 0 || key > Maximum)
                return 0;
            return Convert.ToInt32(this[Offset + key]);
        }

        #endregion
    }
}
