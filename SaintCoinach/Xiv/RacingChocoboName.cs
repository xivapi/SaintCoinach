using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RacingChocoboName : XivRow {
        #region Fields
        private RacingChocoboNameInfo _Info;
        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        public RacingChocoboNameInfo Info {
            get { return _Info ?? (_Info = Sheet.Collection.GetSheet<RacingChocoboNameInfo>()[this.Key]); }
        }

        public RacingChocoboNameCategory Category { get { return Info.Category; } }

        #endregion

        #region Constructors

        public RacingChocoboName(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
