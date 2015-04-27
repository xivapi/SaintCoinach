using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public interface IIndexFile {
        uint FileKey { get; }
        int Offset { get; }
        byte DatFile { get; }
    }
}
