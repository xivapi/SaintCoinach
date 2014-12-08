using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Behaviors {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SearchFunctionExportAttribute : ExportAttribute, ISearchFunctionRegistration {
        public SearchFunctionExportAttribute()
            : base(typeof(Interfaces.ISearchQuery)) { }

        public SearchFunctionExportAttribute(string contractName)
            : base(contractName, typeof(Interfaces.ISearchQuery)) { }

        public string Function { get; set; }
    }
}
