using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TripleTriadCardResident : XivRow {
        #region Properties

        public int Top { get { return AsInt32("Top"); } }
        public int Bottom { get { return AsInt32("Bottom"); } }
        public int Left { get { return AsInt32("Left"); } }
        public int Right { get { return AsInt32("Right"); } }

        public TripleTriadCardRarity TripleTriadCardRarity { get { return As<TripleTriadCardRarity>(); } }
        public TripleTriadCardType TripleTriadCardType { get { return As<TripleTriadCardType>(); } }

        public int SaleValue { get { return AsInt32("SaleValue"); } }
        public int SortKey { get { return AsInt32("SortKey"); } }

        #endregion

        #region Constructors

        public TripleTriadCardResident(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
