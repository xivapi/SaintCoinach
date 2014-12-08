using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Interfaces {
    public interface ISearchQuery {
        IEnumerable<Type> MatchedTypes { get; }
        bool IsMatch(object value);
    }
}
