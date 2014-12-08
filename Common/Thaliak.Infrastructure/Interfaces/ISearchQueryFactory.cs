using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Interfaces {
    public interface ISearchQueryFactory {
        IEnumerable<string> HandledFunctions { get; }
        ISearchQuery CreateQuery(string function, string args);
    }
}
