using System;
using System.Collections.Generic;
using System.Linq;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class BNpcName : XivRow, ILocatable, IItemSource, IQuantifiableXivString {

        #region Fields
        private ILocation[] _Locations;
        private Item[] _Items;
        private InstanceContent[] _InstanceContents;
        #endregion

        #region Properties

        public Text.XivString Singular { get { return AsString("Singular"); } }
        public Text.XivString Plural { get { return Sheet.Collection.ActiveLanguage == Ex.Language.Japanese ? Singular : AsString("Plural"); } }

        public IEnumerable<BNpc> BNpcs {
            get {
                if (!Sheet.Collection.IsLibraAvailable)
                    return new BNpc[0];
                return Sheet.Collection.BNpcs.Where(i => i.Name == this);
            }
        }
        public IEnumerable<ILocation> Locations { get { return _Locations ?? (_Locations = BNpcs.SelectMany(i => i.Locations).ToArray()); } }
        public IEnumerable<Item> Items { get { return _Items ?? (_Items = BNpcs.SelectMany(i => i.Items).ToArray()); } }
        public IEnumerable<InstanceContent> InstanceContents { get { return _InstanceContents ?? (_InstanceContents = BNpcs.SelectMany(i => i.InstanceContents).ToArray()); } }

        #endregion

        #region Constructors

        public BNpcName(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Singular;
        }

        #region IQuantifiableName Members
        string IQuantifiable.Singular {
            get { return Singular; }
        }

        string IQuantifiable.Plural {
            get { return Plural; }
        }
        #endregion
    }
}
