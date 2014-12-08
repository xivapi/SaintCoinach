using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Services.Search {
    public class OrQuery : QueryCollection {
        public override IEnumerable<Type> MatchedTypes {
            get {
                if (!SubQueries.Any())
                    return new Type[0];
                return SubQueries.SelectMany(_ => _.MatchedTypes).Distinct();
            }
        }

        public override bool IsMatch(object value) {
            return SubQueries.Any(_ => _.IsMatch(value));
        }

        public override string ToString() {
            return string.Join("|", SubQueries.Select(_ => _.ToString()));
        }
    }
}
