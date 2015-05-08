using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    /// <summary>
    /// Very much not complete
    /// </summary>
    public class BoneDeformer {
        #region Properties
        public IO.File File { get; private set; }
        public int EntryCount { get; private set; }
        public BoneDeformerEntryHeader[] EntryHeaders { get; private set; }
        public BoneDeformerEntryHeader2[] EntryHeaders2 { get; private set; }
        #endregion

        #region Constructor
        public BoneDeformer(IO.File file) {
            this.File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            var buffer = File.GetData();

            EntryCount = BitConverter.ToInt32(buffer, 0);

            var offset = 4;
            EntryHeaders = buffer.ToStructures<BoneDeformerEntryHeader>(EntryCount, ref offset);
            EntryHeaders2 = buffer.ToStructures<BoneDeformerEntryHeader2>(EntryCount, ref offset);
        }
        #endregion
    }
}
