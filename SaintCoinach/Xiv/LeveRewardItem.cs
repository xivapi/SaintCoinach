using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing possible item reward groups for leves.
    /// </summary>
    public class LeveRewardItem : XivRow {
        #region Fields

        /// <summary>
        /// The <see cref="LeveRewardItemGroup"/>s and their chance of appearing for the current reward.
        /// </summary>
        private ProbabilityPair<LeveRewardItemGroup>[] _ItemGroups;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="LeveRewardItemGroup"/>s and their chance of appearing for the current reward.
        /// </summary>
        /// <value>The <see cref="LeveRewardItemGroup"/>s and their chance of appearing for the current reward.</value>
        public IEnumerable<ProbabilityPair<LeveRewardItemGroup>> ItemGroups {
            get { return _ItemGroups ?? (_ItemGroups = BuildItemGroups()); }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LeveRewardItem" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public LeveRewardItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build
        /// <summary>
        /// Build an array of the <see cref="LeveRewardItemGroup"/>s and their chance of appearing for the current reward.
        /// </summary>
        /// <returns>An array of the <see cref="LeveRewardItemGroup"/>s and their chance of appearing for the current reward.</returns>
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
