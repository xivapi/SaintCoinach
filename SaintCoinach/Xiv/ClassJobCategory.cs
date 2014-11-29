using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ClassJobCategory : XivRow {
        #region Fields
        private ClassJob[] _ClassJobs = null;
        #endregion

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public IEnumerable<ClassJob> ClassJobs { get { return _ClassJobs ?? (_ClassJobs = BuildClassJobs()); } }
        #endregion

        #region Constructor
        public ClassJobCategory(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ClassJob[] BuildClassJobs() {
            const int ColumnOffset = 1;

            var cjs = new List<ClassJob>();
            var cjSheet = Sheet.Collection.GetSheet<ClassJob>();
            foreach (var cj in cjSheet) {
                var isValid = Convert.ToBoolean(this[ColumnOffset + cj.Key]);
                if (isValid)
                    cjs.Add(cj);
            }
            return cjs.ToArray();
        }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}