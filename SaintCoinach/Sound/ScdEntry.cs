using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Sound {
    public abstract class ScdEntry {
        #region Fields

        #endregion

        #region Properties
        public ScdFile File { get; private set; }
        public ScdEntryHeader Header { get; private set; }
        #endregion

        #region Constructor
        protected ScdEntry(ScdFile file, ScdEntryHeader header) {
            this.File = file;
            this.Header = header;
        }
        #endregion

        public abstract byte[] GetDecoded();
    }
}
