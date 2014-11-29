using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ENpc {
        #region Fields
        private int _Key;
        private ENpcResident _Resident;
        private ENpcBase _Base;
        private Collections.ENpcCollection _Collection;
        #endregion

        #region Properties
        public int Key { get { return _Key; } }
        public Collections.ENpcCollection Collection { get { return _Collection; } }
        public ENpcResident Resident { get { return _Resident ?? (_Resident = Collection.ResidentSheet[Key]); } }
        public ENpcBase Base { get { return _Base ?? (_Base = Collection.BaseSheet[Key]); } }
        #endregion

        #region Constructor
        public ENpc(Collections.ENpcCollection collection, int key) {
            _Key = key;
            _Collection = collection;
        }
        #endregion

        public override string ToString() {
            return Resident.Singular;
        }
    }
}
