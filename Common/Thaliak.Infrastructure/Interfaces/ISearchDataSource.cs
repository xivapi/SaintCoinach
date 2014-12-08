using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Interfaces {
    public interface ISearchDataSource {
        IEnumerable<Type> ContainedTypes { get; }
        IEnumerator GetEnumerator();
        IEnumerable GetEnumerable();
    }
}
