using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ClassJob : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Abbreviation { get { return AsString("Abbreviation"); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public ClassJob ParentClassJob { get { return As<ClassJob>("ClassJob{Parent}"); } }
        public Item StartingWeapon { get { return As<Item>("Item{StartingWeapon}"); } }
        #endregion

        #region Constructor
        public ClassJob(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}