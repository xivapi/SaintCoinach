using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IXivSubRow : IXivRow {
        #region Properties

        IRow ParentRow { get; }

        int ParentKey { get; }

        #endregion
    }
}
