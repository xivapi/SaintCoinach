using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing duty roulettes.
    /// </summary>
    public class ContentRoulette : ContentBase {
        #region Properties

        /// <summary>
        /// Gets the type of the current content.
        /// </summary>
        /// <value>The type of the current content.</value>
        public Text.XivString DutyType { get { return AsString("DutyType"); } }

        /// <summary>
        /// Gets a value indicating whether all duties in the current roulette have to be unlocked.
        /// </summary>
        /// <value>A value indicating whether all duties in the current roulette have to be unlocked.</value>
        public bool RequireAllDuties { get { return AsBoolean("RequireAllDuties"); } }

        /// <summary>
        /// Gets the numeric order of the current roulette.
        /// </summary>
        /// <value>The numeric order of the current roulette.</value>
        public int SortKey { get { return AsInt32("SortKey"); } }

        /// <summary>
        /// Gets the rewards received upon completing the current content.
        /// </summary>
        /// <value>The rewards received upon completing the current content.</value>
        public override IEnumerable<IContentReward> FixedRewards {
            get {
                // XXX: Magic numbers here
                const int SoldieryItemKey = 26;
                const int PoeticsItemKey = 28;

                var sold = AsInt32("Reward{Soldiery}");
                var poe = AsInt32("Reward{Poetics}");
                if (sold == 0 && poe == 0)
                    yield break;

                var items = Sheet.Collection.GetSheet<Item>();
                if (sold != 0)
                    yield return new ContentReward(items[SoldieryItemKey], sold);
                if (poe != 0)
                    yield return new ContentReward(items[PoeticsItemKey], poe);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRoulette"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public ContentRoulette(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
