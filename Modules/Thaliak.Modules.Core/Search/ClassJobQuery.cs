using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Search {
    using Behaviors;
    using Interfaces;

    [SearchFunctionExport(Function = "cl")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ClassJobQuery : ISearchQuery {
        [Import]
        private Xiv.XivCollection Data { get; set; }

        public Xiv.ClassJob ClassJob { get; set; }
        #region ISearchQuery Members

        public IEnumerable<Type> MatchedTypes {
            get { yield return typeof(Xiv.Items.Equipment); }
        }

        public bool IsMatch(object value) {
            if (ClassJob == null)
                return false;
            var asEq = value as Xiv.Items.Equipment;
            if (asEq == null)
                return false;
            return asEq.ClassJobCategory.ClassJobs.Contains(ClassJob);
        }

        public void Set(string args) {
            int key;
            var clSheet = Data.GetSheet<Xiv.ClassJob>();
            if (int.TryParse(args, out key))
                ClassJob = clSheet.FirstOrDefault(_ => _.Key == key);
            else {
                ClassJob = clSheet.FirstOrDefault(_ => string.Equals(_.Abbreviation, args, StringComparison.OrdinalIgnoreCase));
                if(ClassJob == null)
                    ClassJob = clSheet.FirstOrDefault(_ => string.Equals(_.Name, args, StringComparison.OrdinalIgnoreCase));
            }
        }

        #endregion

        public override string ToString() {
            return string.Format("cl:{0}", ClassJob == null ? "" : ClassJob.Abbreviation);
        }
    }
}
