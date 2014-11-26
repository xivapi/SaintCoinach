using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.D3DCompiler;

namespace SaintCoinach.Graphics.Viewer {
    public class ShaderCompilerInclude : Include {
        #region Fields
        private DirectoryInfo _SourceDirectory;
        private List<Stream> _OpenStreams = new List<Stream>();
        private IDisposable _Shadow;
        #endregion

        #region Constructor
        public ShaderCompilerInclude(string path) {
            _SourceDirectory = new DirectoryInfo(path);
        }
        #endregion

        #region Include Members

        public void Close(Stream stream) {
            stream.Dispose();
            _OpenStreams.Remove(stream);
        }

        public Stream Open(IncludeType type, string fileName, Stream parentStream) {
            var s = File.OpenRead(Path.Combine(_SourceDirectory.FullName, fileName));
            _OpenStreams.Add(s);
            return s;
        }

        #endregion

        #region ICallbackable Members

        IDisposable SharpDX.ICallbackable.Shadow {
            get {
                return _Shadow;
            }
            set {
                _Shadow = value;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            foreach (var s in _OpenStreams)
                s.Dispose();
            _OpenStreams.Clear();

            if (_Shadow != null)
                _Shadow.Dispose();
            _Shadow = null;
        }

        #endregion
    }
}
