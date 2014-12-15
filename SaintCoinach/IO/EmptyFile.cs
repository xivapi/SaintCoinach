using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public class EmptyFile : File {
        
        #region Constructor
        public EmptyFile(Directory directory, FileCommonHeader header) : base(directory, header) { }
        #endregion
        
        public override byte[] GetData() {
            return new byte[0];
        }
    }
}
