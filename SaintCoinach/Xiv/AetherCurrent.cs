using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    [Obsolete("Replaced by QuestRewardOther.")]
    public class AetherCurrent : XivRow, ILocatable {
        #region Fields

        bool _HasEObj = false;
        EObj _EObj;

        #endregion

        #region Properties

        public Quest Quest { get { return As<Quest>(); } }

        public EObj EObj {
            get {
                if (!_HasEObj) {
                    _EObj = Sheet.Collection.GetSheet<EObj>().FirstOrDefault(_ => _.Data == this.Key);
                    _HasEObj = true;
                }
                return _EObj;
            }
        }

        public IEnumerable<ILocation> Locations {
            get {
                if (EObj == null)
                    return new ILocation[0];
                return EObj.Locations;
            }
        }

        #endregion

        #region Constructors

        public AetherCurrent(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
