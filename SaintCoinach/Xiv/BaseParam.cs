using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class BaseParam : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        #endregion

        #region Constructor
        public BaseParam(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Helpers
        public int GetValue(EquipSlotCategory category) {
            const int Offset = 2;
            if (category.Key == 0)
                return 0;
            return Convert.ToInt32(this[Offset + category.Key]);
        }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}