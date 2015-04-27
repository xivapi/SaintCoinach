using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    partial class Index2Source {
        public class Enumerator : IEnumerator<File> {
            private readonly Index2Source _Source;
            private readonly IEnumerator<Index2File> _FileEnumerator;

            public Enumerator(Index2Source source) {
                _Source = source;
                _FileEnumerator = source.Index.Files.Values.GetEnumerator();
            }

            #region IEnumerator<File> Members

            public File Current {
                get { return _Source.GetFile(_FileEnumerator.Current.FileKey); }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _FileEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                return _FileEnumerator.MoveNext();
            }

            public void Reset() {
                _FileEnumerator.Reset();
            }

            #endregion
        }

        public IEnumerator<File> GetEnumerator() {
            return new Enumerator(this);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
