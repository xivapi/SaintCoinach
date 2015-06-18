using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CompanyCraftDraftCategory : XivRow {
        #region Fields

        private CompanyCraftType[] _CompanyCraftTypes;

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        public IEnumerable<CompanyCraftType> CompanyCraftTypes { get { return _CompanyCraftTypes ?? (_CompanyCraftTypes = BuildCompanyCraftTypes()); } }

        #endregion

        #region Constructors

        public CompanyCraftDraftCategory(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        private CompanyCraftType[] BuildCompanyCraftTypes() {
            const int Count = 10;

            var craftTypes = new List<CompanyCraftType>();
            for (var i = 0; i < Count; ++i) {
                var type = As<CompanyCraftType>(i);
                if (type != null && type.Key != 0)
                    craftTypes.Add(type);
            }

            return craftTypes.ToArray();
        }

        public override string ToString() {
            return Name;
        }
    }
}