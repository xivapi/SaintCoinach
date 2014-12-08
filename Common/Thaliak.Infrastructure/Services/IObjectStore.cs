using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaliak.Services {
    public interface IObjectStore {
        Guid Store(object value);
        object Retrieve(Guid id);
    }
}
