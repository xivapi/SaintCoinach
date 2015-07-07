using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CompanyCraftSequence : XivRow {
        #region Fields
        private CompanyCraftPart[] _CompanyCraftParts;
        #endregion

        #region Properties

        public Item ResultItem { get { return As<Item>("ResultItem"); } }

        public IEnumerable<CompanyCraftPart> CompanyCraftParts { get { return _CompanyCraftParts ?? (_CompanyCraftParts = BuildCraftParts()); } }

        #endregion

        #region Constructors

        public CompanyCraftSequence(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        private CompanyCraftPart[] BuildCraftParts() {
            const int Count = 8;

            var parts = new List<CompanyCraftPart>();

            for(var i = 0; i < Count; ++i) {
                var part = As<CompanyCraftPart>(i);
                if (part == null || part.Key == 0)
                    continue;

                parts.Add(part);
            }

            return parts.ToArray();
        }
    }
}
