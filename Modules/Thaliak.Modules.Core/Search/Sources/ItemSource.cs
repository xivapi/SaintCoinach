using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Search.Sources {
    [Export(typeof(Interfaces.ISearchDataSource))]
    public class ItemSource : Interfaces.ISearchDataSource {
        [Import]
        private Xiv.XivCollection Data { get; set; }

        #region ISearchDataSource Members

        public IEnumerable<Type> ContainedTypes {
            get { yield return typeof(Xiv.Item); }
        }

        public bool IncludeByDefault { get { return true; } }

        public System.Collections.IEnumerator GetEnumerator() {
            return Data.Items.GetEnumerator();
        }

        public System.Collections.IEnumerable GetEnumerable() {
            return Data.Items;
        }

        #endregion
    }
}
