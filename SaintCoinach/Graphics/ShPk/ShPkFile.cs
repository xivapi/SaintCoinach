using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.ShPk {
    public class ShPkFile {
        #region Fields
        private IO.File _SourceFile;
        private ShPkHeader _Header;

        private IReadOnlyList<Parameter> _Parameters;
        #endregion

        #region Properties
        public IO.File SourceFile { get { return _SourceFile; } }
        public ShPkHeader Header { get { return _Header; } }
        public IReadOnlyList<Parameter> Parameters { get { return _Parameters; } }
        #endregion

        #region Constructor
        public ShPkFile(IO.File sourceFile) {
            _SourceFile = sourceFile;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            var buffer = SourceFile.GetData();

            _Header = new ShPkHeader(buffer);

            // TODO: All the other things

            var para = new List<Parameter>();
            foreach (var paraHeader in Header.ParameterHeaders)
                para.Add(new Parameter(this, paraHeader, buffer));
            _Parameters = new ReadOnlyCollection<Parameter>(para);
        }
        #endregion
    }
}
