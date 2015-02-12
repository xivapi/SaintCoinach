using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Xiv.Collections;

namespace SaintCoinach.Xiv {
    public class ENpc {
        #region Fields

        private ENpcBase _Base;
        private Level[] _Levels;
        private ENpcResident _Resident;

        #endregion

        #region Properties

        public int Key { get; private set; }
        public ENpcCollection Collection { get; private set; }
        public ENpcResident Resident { get { return _Resident ?? (_Resident = Collection.ResidentSheet[Key]); } }
        public ENpcBase Base { get { return _Base ?? (_Base = Collection.BaseSheet[Key]); } }
        public IEnumerable<Level> Levels { get { return _Levels ?? (_Levels = BuildLevels()); } }
        public string Singular { get { return Resident.Singular; } }
        public string Plural { get { return Resident.Plural; } }
        public string Title { get { return Resident.Title; } }

        #endregion

        #region Constructors

        #region Constructor

        public ENpc(ENpcCollection collection, int key) {
            Key = key;
            Collection = collection;
        }

        #endregion

        #endregion

        #region Build

        private Level[] BuildLevels() {
            return Collection.Collection.GetSheet<Level>().Where(_ => _.ObjectKey == Key).ToArray();
        }

        #endregion

        public override string ToString() {
            return Resident.Singular;
        }
    }
}
