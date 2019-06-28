using System;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational.Update;

using Newtonsoft.Json.Linq;

namespace SaintCoinach {
    /// <summary>
    ///     Class containing the changes found during a definition update.
    /// </summary>
    public class UpdateReport {
        #region Properties

        /// <summary>
        ///     Gets the version string before the update.
        /// </summary>
        /// <value>The version string before the update.</value>
        public string PreviousVersion { get; internal set; }

        /// <summary>
        ///     Gets the version string after the update.
        /// </summary>
        /// <value>The version string after the update.</value>
        public string UpdateVersion { get; internal set; }

        /// <summary>
        ///     Gets the collection of changes.
        /// </summary>
        /// <value>The collection of changes.</value>
        public ICollection<IChange> Changes { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateReport" /> class.
        /// </summary>
        /// <param name="previousVersion">Version string before the update.</param>
        /// <param name="updatedVersion">Version string after the update.</param>
        /// <param name="changes">Enumerable of the changes.</param>
        public UpdateReport(string previousVersion, string updatedVersion, IEnumerable<IChange> changes) {
            PreviousVersion = previousVersion;
            UpdateVersion = updatedVersion;
            Changes = new List<IChange>(changes);
        }

        #endregion

        #region Serialization

        public JObject ToJson() {
            return new JObject() {
                ["previousVersion"] = PreviousVersion,
                ["updateVersion"] = UpdateVersion,
                // I got lazy here.  Can add cleaner serialization for changes if needed.
                ["changes"] = new JArray(Changes.Select(c => JObject.FromObject(c)))
            };
        }
        
        #endregion
    }
}
