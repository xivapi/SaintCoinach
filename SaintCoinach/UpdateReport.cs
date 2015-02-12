using System;
using System.Collections.Generic;

using SaintCoinach.Ex.Relational.Update;

namespace SaintCoinach {
    /// <summary>
    ///     Class containing the changes found during a definition update.
    /// </summary>
    [Serializable]
    public class UpdateReport {
        #region Fields

        /// <summary>
        ///     Collection of all detected changes.
        /// </summary>
        private ICollection<IChange> _Changes;

        /// <summary>
        ///     Version string before the update.
        /// </summary>
        private string _PreviousVersion;

        /// <summary>
        ///     Version string after the update.
        /// </summary>
        private string _UpdatedVersion;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the version string before the update.
        /// </summary>
        /// <value>The version string before the update.</value>
        public string PreviousVersion { get { return _PreviousVersion; } internal set { _PreviousVersion = value; } }

        /// <summary>
        ///     Gets the version string after the update.
        /// </summary>
        /// <value>The version string after the update.</value>
        public string UpdateVersion { get { return _UpdatedVersion; } internal set { _UpdatedVersion = value; } }

        /// <summary>
        ///     Gets the collection of changes.
        /// </summary>
        /// <value>The collection of changes.</value>
        public ICollection<IChange> Changes { get { return _Changes; } internal set { _Changes = value; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateReport" /> class.
        /// </summary>
        /// <param name="previousVersion">Version string before the update.</param>
        /// <param name="updatedVersion">Version string after the update.</param>
        /// <param name="changes">Enumerable of the changes.</param>
        public UpdateReport(string previousVersion, string updatedVersion, IEnumerable<IChange> changes) {
            _PreviousVersion = previousVersion;
            _UpdatedVersion = updatedVersion;
            _Changes = new List<IChange>(changes);
        }

        #endregion
    }
}
