using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak {
    [Export(typeof(Interfaces.ISearchDataSource))]
    public class TestItemSource : Interfaces.ISearchDataSource {

        [Import]
        private SaintCoinach.Xiv.XivCollection _Xiv;

        #region ISearchDataSource Members

        public IEnumerable<Type> ContainedTypes {
            get { yield return typeof(SaintCoinach.Xiv.Item); }
        }

        public System.Collections.IEnumerator GetEnumerator() {
            return _Xiv.GetSheet<SaintCoinach.Xiv.Item>().GetEnumerator();
        }

        public System.Collections.IEnumerable GetEnumerable() {
            return _Xiv.GetSheet<SaintCoinach.Xiv.Item>();
        }

        #endregion
    }
}
