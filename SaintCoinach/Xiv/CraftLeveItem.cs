using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CraftLeveItem {
        #region Fields
        private Item _Item;
        private int _Count;
        private bool _RequireHq;
        #endregion

        #region Properties
        public Item Item { get { return _Item; } }
        public int Count { get { return _Count; } }
        public bool RequireHq { get { return _RequireHq; } }
        #endregion

        #region Constructor
        public CraftLeveItem(Item item, int count, bool requireHq) {
            _Item = item;
            _Count = count;
            _RequireHq = requireHq;
        }
        #endregion
    }
}
