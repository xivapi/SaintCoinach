using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public class GatheringTool : Equipment {
        #region Properties
        public override IEnumerable<Parameter> PrimaryParameters {
            get { yield break; }
        }
        #endregion

        #region Constructor
        public GatheringTool(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}