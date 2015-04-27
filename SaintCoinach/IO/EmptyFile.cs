namespace SaintCoinach.IO {
    public class EmptyFile : File {
        #region Constructors

        public EmptyFile(Pack pack, FileCommonHeader header) : base(pack, header) { }

        #endregion

        public override byte[] GetData() {
            return new byte[0];
        }
    }
}
