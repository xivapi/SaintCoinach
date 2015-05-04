using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public interface IIndexFile : IEquatable<IIndexFile> {
        PackIdentifier PackId { get; }
        uint FileKey { get; }
        int Offset { get; }
        byte DatFile { get; }
    }
}
