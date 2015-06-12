using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public interface IQuantifiableXivString : IQuantifiable {
        new Text.XivString Singular { get; }
        new Text.XivString Plural { get; }
    }
}
