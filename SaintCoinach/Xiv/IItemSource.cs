using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Interface for objects from which <see cref="Item"/>s can be obtained.
    /// </summary>
    public interface IItemSource {
        /// <summary>
        /// Gets the <see cref="Item"/>s that can be obtained from the current object.
        /// </summary>
        /// <value>The <see cref="Item"/>s that can be obtained from the current object.</value>
        IEnumerable<Item> Items { get; }
    }
}
