using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach {
    [Serializable]
    public class UpdateReport {
        #region Fields
        private string _PreviousVersion;
        private string _UpdatedVersion;

        private ICollection<Ex.Relational.Update.IChange> _ExChanges;
        #endregion

        #region Properties
        public string PreviousVersion {
            get { return _PreviousVersion; }
            internal set { _PreviousVersion = value; }
        }
        public string UpdateVersion {
            get { return _UpdatedVersion; }
            internal set { _UpdatedVersion = value; }
        }
        public ICollection<Ex.Relational.Update.IChange> ExChanges {
            get { return _ExChanges; }
            internal set { _ExChanges = value; }
        }
        #endregion

        #region Constructor
        public UpdateReport(string previousVersion, string updatedVersion, IEnumerable<Ex.Relational.Update.IChange> exChanges) {
            _PreviousVersion = previousVersion;
            _UpdatedVersion = updatedVersion;
            _ExChanges = new List<Ex.Relational.Update.IChange>(exChanges);
        }
        #endregion
    }
}
