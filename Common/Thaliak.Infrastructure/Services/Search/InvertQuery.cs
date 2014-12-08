using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Services.Search {
    using Interfaces;

    public class InvertQuery : ISearchQuery {
        public ISearchQuery InnerQuery { get; set; }

        #region ISearchQuery Members
        void ISearchQuery.Set(string args) { throw new NotSupportedException(); }
        public IEnumerable<Type> MatchedTypes {
            get { return InnerQuery.MatchedTypes; }
        }

        public bool IsMatch(object value) {
            return !InnerQuery.IsMatch(value);
        }

        #endregion

        public override string ToString() {
            return string.Format("~{0}", InnerQuery);
        }
    }
}
