using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions
{
    public class TripleTriadCardUnlock : ItemAction {
        #region Static

        private const int CardKey = 0;

        #endregion

        #region Properties

        public TripleTriadCard TripleTriadCard {
            get {
                var key = GetData(CardKey);
                return Sheet.Collection.GetSheet<TripleTriadCard>()[key];
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public TripleTriadCardUnlock(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
