using System;
using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Adventure : XivRow, ILocatable {
        #region Properties

        public Level Level { get { return As<Level>(); } }

        public Emote Emote { get { return As<Emote>(); } }

        public int MinTime { get { return AsInt32("MinTime"); } }

        public int MaxTime { get { return AsInt32("MaxTime"); } }

        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        public Imaging.ImageFile ListIcon { get { return AsImage("Icon{List}"); } }
        public Imaging.ImageFile DiscoveredIcon { get { return AsImage("Icon{Discovered}"); } }
        public Imaging.ImageFile UndiscoveredIcon { get { return AsImage("Icon{Undiscovered}"); } }

        public bool IsInitial { get { return AsBoolean("IsInitial"); } }

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Impression { get { return AsString("Impression"); } }
        public Text.XivString Description { get { return AsString("Description"); } }

        IEnumerable<ILocation> ILocatable.Locations {
            get { yield return Level; }
        }

        #endregion

        #region Constructors

        #region Constructor

        public Adventure(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
