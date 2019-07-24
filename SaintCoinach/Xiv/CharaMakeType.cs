using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;
using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    public class CharaMakeType : XivRow, IXivRow {
        private CharaMakeFeatureIcon[] _Items;
        #region Properties
        public Race Race { get { return As<Race>(); } }
        public Tribe Tribe { get { return As<Tribe>(); } }
        public IEnumerable<CharaMakeFeatureIcon> FacialFeatureIcon { get { return _Items ?? (_Items = BuildItems()); } }
        public int Gender { get { return AsInt32("Gender"); } }
        #endregion


        public CharaMakeType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        private CharaMakeFeatureIcon[] BuildItems() {
            const int Count = 56;

            var items = new List<CharaMakeFeatureIcon>();

            for (var i = 0; i < Count; ++i) {
                var item = AsImage("FacialFeatureIcon", i);
                if (item == null) continue;
                items.Add(new CharaMakeFeatureIcon(item, i));
            }

            return items.ToArray();
        }
        public class CharaMakeFeatureIcon {
            public ImageFile FacialFeatureIcon { get; private set; }
            public int Count { get; private set; }
            public CharaMakeFeatureIcon(ImageFile image, int count) {
                FacialFeatureIcon = image;
                Count = count;
            }
        }
    }
}
