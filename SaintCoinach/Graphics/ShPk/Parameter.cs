using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.ShPk {
    public class Parameter {
        #region Fields
        private ParameterHeader _Header;
        private string _Name;
        #endregion

        #region Properties
        public ParameterHeader Header { get { return _Header; } }
        public ParameterType Type { get { return Header.Type; } }
        public int Id { get { return Header.Id; } }
        public string Name { get { return _Name; } }
        #endregion

        #region Constructor
        public Parameter(ShPkFile file, ParameterHeader header, byte[] buffer) {
            _Header = header;
            var off = file.Header.ParameterListOffset + header.NameOffset;
            _Name = Encoding.ASCII.GetString(buffer, off, header.NameLength);
        }
        #endregion
    }
}
