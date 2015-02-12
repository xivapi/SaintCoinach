using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LeveRewardItem : XivRow {
        #region Fields

        private ProbabilityPair<LeveRewardItemGroup>[] _ItemGroups;

        #endregion

        #region Properties

        public IEnumerable<ProbabilityPair<LeveRewardItemGroup>> ItemGroups {
            get { return _ItemGroups ?? (_ItemGroups = BuildItemGroups()); }
        }

        #endregion

        #region Constructors

        #region Constructor

        public LeveRewardItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private ProbabilityPair<LeveRewardItemGroup>[] BuildItemGroups() {
            const int Count = 8;

            var itemGroups = new List<ProbabilityPair<LeveRewardItemGroup>>();
            for (var i = 0; i < Count; ++i) {
                var probability = AsInt32("Probability<%>", i);
                if (probability == 0)
                    continue;

                var group = As<LeveRewardItemGroup>(i);
                if (group.Key == 0)
                    continue;

                itemGroups.Add(new ProbabilityPair<LeveRewardItemGroup>(group, probability / 100.0));
            }

            return itemGroups.ToArray();
        }

        #endregion
    }
}
