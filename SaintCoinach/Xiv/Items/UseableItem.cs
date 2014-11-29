using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public class UseableItem : InventoryItem {
        #region Properties
        public TimeSpan Cooldown { get { return TimeSpan.FromSeconds(AsDouble("Cooldown<s>")); } }
        #endregion

        #region Constructor
        public UseableItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}