using System.Collections.Generic;
using System.Collections.ObjectModel;

using SaintCoinach.IO;

namespace SaintCoinach.Graphics.ShPk {
    public class ShPkFile {
        #region Properties

        public File SourceFile { get; private set; }
        public ShPkHeader Header { get; private set; }
        public IReadOnlyList<Parameter> Parameters { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ShPkFile(File sourceFile) {
            SourceFile = sourceFile;

            Build();
        }

        #endregion

        #endregion

        #region Build

        private void Build() {
            var buffer = SourceFile.GetData();

            Header = new ShPkHeader(buffer);

            // TODO: All the other things

            var para = new List<Parameter>();
            foreach (var paraHeader in Header.ParameterHeaders)
                para.Add(new Parameter(this, paraHeader, buffer));
            Parameters = new ReadOnlyCollection<Parameter>(para);
        }

        #endregion
    }
}
