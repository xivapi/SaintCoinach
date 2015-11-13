using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv
{
    public class MinionRace : XivRow
    {
        #region Constructors

        #region Constructor

        public MinionRace(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        #endregion
    }
}
