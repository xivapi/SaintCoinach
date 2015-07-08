using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CompanyCraftSequence : XivRow, IItemSource {
        #region Fields
        private CompanyCraftPart[] _CompanyCraftParts;
        #endregion

        #region Properties

        public Item ResultItem { get { return As<Item>("ResultItem"); } }
        public CompanyCraftDraftCategory CompanyCraftDraftCategory { get { return As<CompanyCraftDraftCategory>(); } }
        public CompanyCraftType CompanyCraftType { get { return As<CompanyCraftType>(); } }
        public CompanyCraftDraft CompanyCraftDraft { get { return As<CompanyCraftDraft>(); } }
        public IEnumerable<CompanyCraftPart> CompanyCraftParts { get { return _CompanyCraftParts ?? (_CompanyCraftParts = BuildCraftParts()); } }

        IEnumerable<Item> IItemSource.Items { get { yield return ResultItem; } }

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
