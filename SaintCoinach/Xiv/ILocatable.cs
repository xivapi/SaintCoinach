using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Interface for objects that have specific locations.
    /// </summary>
    public interface ILocatable {
        /// <summary>
        /// Gets the locations of the current object.
        /// </summary>
        /// <value>The locations of the current object.</value>
        IEnumerable<ILocation> Locations { get; }
    }
}
