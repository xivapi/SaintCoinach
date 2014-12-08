using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Behaviors {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [MetadataAttribute]
    public class ViewExportAttribute : ExportAttribute, IViewRegionRegistration {
        public ViewExportAttribute()
            : base(typeof(object)) { }

        public ViewExportAttribute(string viewName)
            : base(viewName, typeof(object)) { }

        public string ViewName { get { return base.ContractName; } }

        public string RegionName { get; set; }
    }
}
