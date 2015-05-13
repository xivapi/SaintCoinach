using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Race : XivRow {
        #region Properties

        public Text.XivString Masculine { get { return AsString("Masculine"); } }
        public Text.XivString Feminine { get { return AsString("Feminine"); } }

        public IEnumerable<Item> MaleRse {
            get {
                yield return As<Item>("RSE{M}{Body}");
                yield return As<Item>("RSE{M}{Hands}");
                yield return As<Item>("RSE{M}{Legs}");
                yield return As<Item>("RSE{M}{Feet}");
            }
        }
        public IEnumerable<Item> FemaleRse {
            get {
                yield return As<Item>("RSE{F}{Body}");
                yield return As<Item>("RSE{F}{Hands}");
                yield return As<Item>("RSE{F}{Legs}");
                yield return As<Item>("RSE{F}{Feet}");
            }
        }

        #endregion

        #region Constructor

        public Race(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Feminine;
        }
    }
}
