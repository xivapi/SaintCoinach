using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class BeastTribe : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string RelationName { get { return AsString("Name{Relation}"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public Imaging.ImageFile ReputationIcon { get { return AsImage("Icon{Reputation}"); } }
        #endregion

        #region Constructor
        public BeastTribe(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}