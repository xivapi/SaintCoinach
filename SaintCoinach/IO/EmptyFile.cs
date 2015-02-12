namespace SaintCoinach.IO {
    public class EmptyFile : File {
        #region Constructors

        #region Constructor

        public EmptyFile(Directory directory, FileCommonHeader header) : base(directory, header) { }

        #endregion

        #endregion

        public override byte[] GetData() {
            return new byte[0];
        }
    }
}
