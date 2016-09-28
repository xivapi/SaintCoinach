using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public abstract class ActionBase : XivRow {
        #region Fields
        private ActionTransient _ActionTransient;
        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public virtual Text.XivString Description { get { return ActionTransient.Description;  } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }


        public ActionTransient ActionTransient {  get { return _ActionTransient ?? (_ActionTransient = this.Sheet.Collection.GetSheet<ActionTransient>()[Key]); } }
        #endregion

        #region Constructors

        protected ActionBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
