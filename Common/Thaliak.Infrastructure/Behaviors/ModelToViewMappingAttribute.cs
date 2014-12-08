using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Behaviors {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [MetadataAttribute]
    public class ModelToViewMappingAttribute : ExportAttribute {
        public ModelToViewMappingAttribute()
            : base(typeof(object)) { }

        public ModelToViewMappingAttribute(string viewName)
            : base(viewName, typeof(object)) { }

        public string ViewName { get { return base.ContractName; } }

        public Type ModelType { get; set; }
        public bool AllowDerivedTypes { get; set; }
    }
}
