using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TripleTriadCard : XivRow {
        public const int PlateIconOffset = 82100;
        public const int IconOffset = 82300;

        #region Fields
        private TripleTriadCardResident _Resident;
        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Description { get { return AsString("Description"); } }

        public Imaging.ImageFile Icon { get { return Imaging.IconHelper.GetIcon(Sheet.Collection.PackCollection, Sheet.Collection.ActiveLanguage, IconOffset + Key); } }
        public Imaging.ImageFile PlateIcon { get { return Imaging.IconHelper.GetIcon(Sheet.Collection.PackCollection, Sheet.Collection.ActiveLanguage, PlateIconOffset + Key); } }

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
