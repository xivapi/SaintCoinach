using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalExCollection : ExCollection {
        #region Fields
        private Definition.RelationDefinition _Definition = new Definition.RelationDefinition();
        #endregion

        #region Properties
        public Definition.RelationDefinition Definition {
            get { return _Definition; }
            set { _Definition = value; }
        }
        #endregion

        #region Constructor
        public RelationalExCollection(IO.PackCollection packCollection) : base(packCollection) { }
        #endregion

        #region Factory
        protected override Header CreateHeader(string name, IO.File file) {
            return new RelationalHeader(this, name, file);
        }
        protected override ISheet CreateSheet(Header header) {
            var relHeader = (RelationalHeader)header;
            if (header.AvailableLanguages.Count() > 1)
                return new RelationalMultiSheet<RelationalMultiRow, RelationalDataRow>(this, relHeader);
            return new RelationalDataSheet<RelationalDataRow>(this, relHeader, header.AvailableLanguages.First());
        }
        #endregion

        #region Get
        public new IRelationalSheet GetSheet(string name) {
            return (IRelationalSheet)base.GetSheet(name);
        }
        #endregion
    }
}
