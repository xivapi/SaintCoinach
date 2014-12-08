using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Services {
    public interface ISearchProvider {
        Interfaces.ISearchQuery Parse(string queryString);
        IEnumerable Search(Interfaces.ISearchQuery query);
    }
}
