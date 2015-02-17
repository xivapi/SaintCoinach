using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Interface for objects defining a location in a zone (in map-coordinates).
    /// </summary>
    public interface ILocation {
        /// <summary>
        /// Gets the x-coordinate of the current object.
        /// </summary>
        /// <value>The x-coordinate of the current object.</value>
        double MapX { get; }

        /// <summary>
        /// Gets the y-coordinate of the current object.
        /// </summary>
        /// <value>The y-coordinate of the current object.</value>
        double MapY { get; }

        /// <summary>
        /// Gets the <see cref="PlaceName"/> of the current object's location.
        /// </summary>
        /// <value>The <see cref="PlaceName"/> of the current object's location.</value>
        PlaceName PlaceName { get; }
    }
}
