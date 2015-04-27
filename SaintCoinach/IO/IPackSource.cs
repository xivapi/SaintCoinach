using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public interface IPackSource : IEnumerable<File> {
        bool FileExists(string path);
        bool TryGetFile(string path, out File value);
        File GetFile(string path);
    }
}
