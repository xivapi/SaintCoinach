using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Search.Sources {
    public abstract class GenericXivSource<T> : Interfaces.ISearchDataSource where T : Xiv.IXivRow {
        [Import]
        private Xiv.XivCollection Data { get; set; }

        #region ISearchDataSource Members

        public IEnumerable<Type> ContainedTypes {
            get { yield return typeof(T); }
        }
        public abstract bool IncludeByDefault { get; }

        public System.Collections.IEnumerator GetEnumerator() {
            return Data.GetSheet<T>().GetEnumerator();
        }

        public System.Collections.IEnumerable GetEnumerable() {
            return Data.GetSheet<T>();
        }

        #endregion
    }
}
