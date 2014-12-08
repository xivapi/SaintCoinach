using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ClassJob : XivRow {
        const int IconOffset = 62000;
        const string IconFormat = "ui/icon/{0:D3}000/{1:D6}.tex";

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Abbreviation { get { return AsString("Abbreviation"); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public ClassJob ParentClassJob { get { return As<ClassJob>("ClassJob{Parent}"); } }
        public Item StartingWeapon { get { return As<Item>("Item{StartingWeapon}"); } }
        public Imaging.ImageFile Icon {
            get {
                var nr = IconOffset + Key;
                var path = string.Format(IconFormat, nr / 1000, nr);
                IO.File file;
                if (Sheet.Collection.PackCollection.TryGetFile(path, out file))
                    return file as Imaging.ImageFile;
                return null;
            }
        }
        #endregion

        #region Constructor
        public ClassJob(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}