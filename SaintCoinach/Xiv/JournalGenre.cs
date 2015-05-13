using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class JournalGenre : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        public JournalCategory JournalCategory { get { return As<JournalCategory>(); } }

        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        public JournalGenre(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
