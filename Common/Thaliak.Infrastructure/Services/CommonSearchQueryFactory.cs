using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

namespace Thaliak.Services {
    [Export(typeof(Interfaces.ISearchQueryFactory))]
    public class CommonSearchQueryFactory : Interfaces.ISearchQueryFactory {
        #region ISearchQueryFactory Members

        public IEnumerable<string> HandledFunctions {
            get { yield return "text"; }
        }

        public Interfaces.ISearchQuery CreateQuery(string function, string args) {
            if (!string.Equals(function, "text", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException();

            return new Search.StringQuery { SearchString = args };
        }

        #endregion
    }
}
