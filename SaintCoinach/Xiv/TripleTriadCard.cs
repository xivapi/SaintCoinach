using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TripleTriadCard : XivRow {
        #region Fields
        private TripleTriadCardResident _Resident;
        #endregion

        #region Properties

        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }

        public TripleTriadCardResident TripleTriadCardResident {
            get { return _Resident ?? (_Resident = Sheet.Collection.GetSheet<TripleTriadCardResident>()[this.Key]); }
        }

        public int Top { get { return TripleTriadCardResident.Top; } }
        public int Bottom { get { return TripleTriadCardResident.Bottom; ; } }
        public int Left { get { return TripleTriadCardResident.Left; ; } }
        public int Right { get { return TripleTriadCardResident.Right; ; } }

        public TripleTriadCardRarity TripleTriadCardRarity {
            get { return TripleTriadCardResident.TripleTriadCardRarity; ; }
        }
        public TripleTriadCardType TripleTriadCardType {
            get { return TripleTriadCardResident.TripleTriadCardType; ; }
        }

        public int SaleValue { get { return TripleTriadCardResident.SaleValue; ; } }
        public int SortKey { get { return TripleTriadCardResident.SortKey; ; } }

        #endregion

        #region Constructors

        public TripleTriadCard(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
