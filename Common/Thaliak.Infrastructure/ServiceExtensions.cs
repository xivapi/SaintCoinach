using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak {
    public static class ServiceExtensions {
        public static IEnumerable Search(this Services.ISearchProvider self, string queryString) {
            var query = self.Parse(queryString);
            return self.Search(queryString);
        }
    }
}
