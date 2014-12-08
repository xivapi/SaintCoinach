using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Services.Search {
    public class AndQuery : QueryCollection {
        public override IEnumerable<Type> MatchedTypes {
            get {
                if (!SubQueries.Any())
                    return new Type[0];

                List<Type> valid = null;
                foreach (var q in SubQueries) {
                    if (valid == null)
                        valid = new List<Type>(q.MatchedTypes);
                    else {
                        var qt = q.MatchedTypes.ToArray();
                        valid.AddRange(qt.Where(_ => _.IsInterface));

                        foreach (var t in valid.Where(_ => !_.IsInterface).ToArray()) {
                            if(qt.Any(_ => _.IsAssignableFrom(t)))
                                continue;
                            var reverse = qt.FirstOrDefault(_ => t.IsAssignableFrom(_));
                            if (reverse != null) {
                                valid.Remove(t);
                                valid.Add(reverse);
                            }
                        }
                    }
                }
                return valid;
            }
        }

        public override bool IsMatch(object value) {
            return SubQueries.All(_ => _.IsMatch(value));
        }

        public override string ToString() {
            return string.Join("+", SubQueries.Select(_ => _.ToString()));
        }
    }
}
