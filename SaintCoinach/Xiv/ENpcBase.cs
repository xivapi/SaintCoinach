using System;
using System.Collections.Generic;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ENpcBase : XivRow {
        #region Static

        public const int DataCount = 32;

        #endregion

        #region Fields

        private IRelationalRow[] _AssignedData;

        #endregion

        #region Properties

        public IEnumerable<IRelationalRow> AssignedData { get { return _AssignedData ?? (_AssignedData = BuildAssignedData()); } }

        #endregion

        #region Constructors

        public ENpcBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        
        #endregion

        public IRelationalRow GetData(int index) {
            return As<IRelationalRow>("ENpcData", index);
        }
        public int GetRawData(int index) {
            var fulCol = BuildColumnName("ENpcData", index);
            var raw = ((IRelationalRow)this).GetRaw(fulCol);
            return Convert.ToInt32(raw);
        }

        private IRelationalRow[] BuildAssignedData() {
            var data = new List<IRelationalRow>();

            for (var i = 0; i < ENpcBase.DataCount; ++i) {
                var val = GetData(i);
                if (val != null)
                    data.Add(val);
            }

            return data.ToArray();
        }
    }
}
