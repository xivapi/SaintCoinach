using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Interface for content rewards.
    /// </summary>
    public interface IContentReward {
        /// <summary>
        /// Gets the rewarded <see cref="Item"/>.
        /// </summary>
        /// <value>The rewarded <see cref="Item"/>.</value>
        Item Item { get; }

        /// <summary>
        ///     Gets the count for the current reward.
        /// </summary>
        /// <value>The count for the current reward.</value>
        int Count { get; }
    }
}
