using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing basic content reward.
    /// </summary>
    public class ContentReward : IContentReward {
        #region Properties

        /// <summary>
        /// Gets the rewarded <see cref="Item"/>.
        /// </summary>
        /// <value>The rewarded <see cref="Item"/>.</value>
        public Item Item { get; private set; }

        /// <summary>
        ///     Gets the count for the current reward.
        /// </summary>
        /// <value>The count for the current reward.</value>
        public int Count { get; private set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        /// <param name="item">The rewarded <see cref="Item"/>.</param>
        /// <param name="count">The count for the current reward.</param>
        public ContentReward(Item item, int count) {
            this.Item = item;
            this.Count = count;
        }
        #endregion
    }
}
