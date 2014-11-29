using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class EventItem : Item {
        #region Properties
        public Quest Quest { get { return As<Quest>(); } }
        #endregion

        #region Constructor
        public EventItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}