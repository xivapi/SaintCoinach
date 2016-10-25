using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions
{
    public class MgpCard : ItemAction {
        #region Static

        private const int AmountKey = 0;

        #endregion

        #region Properties

        public int Amount {
            get { return GetData(AmountKey); }
        }

        #endregion

        #region Constructors

        #region Constructor

        public MgpCard(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
