using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ItemComparer : IComparer<SaintCoinach.Xiv.ItemBase> {
        public enum Option {
            None = 0,
            Ascending = 1,
            Descending = -1
        }

        #region Static

        public static readonly ItemComparer Default = new ItemComparer();

        #endregion

        #region Fields

        private Option _CategoryOption = Option.Ascending;
        private Option _EquipLevelOption = Option.Descending;
        private Option _ItemLevelOption = Option.Descending;

        #endregion

        #region Properties

        public Option CategoryOption { get { return _CategoryOption; } set { _CategoryOption = value; } }
        public Option EquipLevelOption { get { return _EquipLevelOption; } set { _EquipLevelOption = value; } }
        public Option ItemLevelOption { get { return _ItemLevelOption; } set { _ItemLevelOption = value; } }

        #endregion

        #region IComparer<InventoryItem> Members

        public int Compare(SaintCoinach.Xiv.ItemBase x, SaintCoinach.Xiv.ItemBase y) {
            var ix = x as Item;
            var iy = y as Item;
            if (x == y)
                return 0;
            if (x == null)
                return -1;

            if (ix == null)
                return -1;
            if (iy == null)
                return 1;
            if (ix != null && iy != null) {
                var comp = CompareCategoryMajor(ix, iy);
                if (comp != 0)
                    return comp;

                comp = CompareCategoryMinor(ix, iy);
                if (comp != 0)
                    return comp;

                comp = CompareEquipLevel(ix, iy);
                if (comp != 0)
                    return comp;

                comp = CompareItemLevel(ix, iy);
                if (comp != 0)
                    return comp;
            }

            return x.Key.CompareTo(y.Key);
        }

        public int CompareCategoryMajor(SaintCoinach.Xiv.Item x, SaintCoinach.Xiv.Item y) {
            return ((int)CategoryOption) * x.ItemUICategory.MajorOrder.CompareTo(y.ItemUICategory.MajorOrder);
        }

        public int CompareCategoryMinor(SaintCoinach.Xiv.Item x, SaintCoinach.Xiv.Item y) {
            return ((int)CategoryOption) * x.ItemUICategory.MinorOrder.CompareTo(y.ItemUICategory.MinorOrder);
        }

        public int CompareEquipLevel(SaintCoinach.Xiv.Item x, SaintCoinach.Xiv.Item y) {
            var ex = 0;
            var ey = 0;
            if (x is SaintCoinach.Xiv.Items.Equipment)
                ex = ((SaintCoinach.Xiv.Items.Equipment)x).EquipmentLevel;
            if (y is SaintCoinach.Xiv.Items.Equipment)
                ey = ((SaintCoinach.Xiv.Items.Equipment)y).EquipmentLevel;
            return ((int)EquipLevelOption) * ex.CompareTo(ey);
        }

        public int CompareItemLevel(SaintCoinach.Xiv.Item x, SaintCoinach.Xiv.Item y) {
            return ((int)ItemLevelOption) * x.ItemLevel.Key.CompareTo(y.ItemLevel.Key);
        }

        #endregion
    }
}
