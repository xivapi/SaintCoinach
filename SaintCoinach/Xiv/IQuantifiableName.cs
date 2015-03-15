using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IQuantifiableName {
        string Singular { get; }
        string Plural { get; }
    }
}
