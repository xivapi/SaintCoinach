using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public class MultiRow : IMultiRow {
        #region Fields
        private IMultiSheet _Sheet;
        private int _Key;
        #endregion

        #region Properties
        public IMultiSheet Sheet { get { return _Sheet; } }
        ISheet IRow.Sheet { get { return Sheet; } }
        public int Key { get { return _Key; } }
        #endregion

        #region Constructor
        public MultiRow(IMultiSheet sheet, int key) {
            _Sheet = sheet;
            _Key = key;
        }
        #endregion

        #region IMultiRow Members
        public object this[int columnIndex, Language language] {
            get { return Sheet.GetLocalisedSheet(language)[Key, columnIndex]; }
        }

        public object this[int columnIndex] {
            get { return Sheet.ActiveSheet[Key, columnIndex]; }
        }

        #endregion
    }
}
