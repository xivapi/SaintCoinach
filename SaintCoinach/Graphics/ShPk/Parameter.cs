using System.Text;

namespace SaintCoinach.Graphics.ShPk {
    public class Parameter {
        #region Properties

        public ParameterHeader Header { get; private set; }
        public ParameterType Type { get { return Header.Type; } }
        public int Id { get { return Header.Id; } }
        public string Name { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public Parameter(ShPkFile file, ParameterHeader header, byte[] buffer) {
            Header = header;
            var off = file.Header.ParameterListOffset + header.NameOffset;
            Name = Encoding.ASCII.GetString(buffer, off, header.NameLength);
        }

        #endregion

        #endregion
    }
}
