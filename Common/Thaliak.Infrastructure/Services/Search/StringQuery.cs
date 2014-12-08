using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Services.Search {
    [Behaviors.SearchFunctionExport(Function = "text")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class StringQuery : Interfaces.ISearchQuery {
        public string SearchString { get; set; }

        public void Set(string args) {
            SearchString = args;
        }

        #region ISearchQuery Members

        public IEnumerable<Type> MatchedTypes {
            get { yield return typeof(object); }
        }

        public bool IsMatch(object value) {
            if (value == null)
                return false;
            var asStr = value.ToString();
            return (asStr.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        #endregion

        public override string ToString() {
            return string.Format("text:\"{0}\"", SearchString);
        }
    }
}
