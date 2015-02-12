using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class DesynthSkillReset : ItemAction {
        #region Static

        private const int ClassJobKey = 0;

        #endregion
        
        #region Properties

        public ClassJob ClassJob {
            get {
                var key = GetData(ClassJobKey);
                return Sheet.Collection.GetSheet<ClassJob>()[key];
            }
        }

        #endregion

        #region Constructors

        public DesynthSkillReset(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
