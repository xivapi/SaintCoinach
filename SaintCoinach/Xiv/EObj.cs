using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class EObj : XivRow, ILocatable {
        #region Fields

        private ILocation[] _Locations;

        #endregion

        #region Properties

        public Text.XivString Singular { get { return AsString("Singular"); } }
        public Text.XivString Plural { get { return Sheet.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : AsString("Plural"); } }

        public int Data { get { return AsInt32("Data"); } }

        public IEnumerable<ILocation> Locations { get { return _Locations ?? (_Locations = BuildLocations()); } }

        #endregion

        #region Constructors

        public EObj(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        private Level[] BuildLocations() {
            return Sheet.Collection.GetSheet<Level>().Where(_ => _.ObjectKey == Key).ToArray();
        }
    }
}
