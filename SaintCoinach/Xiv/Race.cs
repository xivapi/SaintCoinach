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
                yield return As<Item>("RSEM}{Body");
                yield return As<Item>("RSEM}{Hands");
                yield return As<Item>("RSEM}{Legs");
                yield return As<Item>("RSEM}{Feet");
            }
        }
        public IEnumerable<Item> FemaleRse {
            get {
                yield return As<Item>("RSEF}{Body");
                yield return As<Item>("RSEF}{Hands");
                yield return As<Item>("RSEF}{Legs");
                yield return As<Item>("RSEF}{Feet");
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
