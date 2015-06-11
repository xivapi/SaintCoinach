using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Imaging {
    using Ex;
    using IO;

    public static class IconHelper {
        const string IconFileFormat = "ui/icon/{0:D3}000/{1}{2:D6}.tex";
        public static Imaging.ImageFile GetIcon(IO.PackCollection pack, int nr) {
            return GetIcon(pack, string.Empty, nr);
        }
        public static Imaging.ImageFile GetIcon(IO.PackCollection pack, Language language, int nr) {
            var type = language.GetCode();
            if (type.Length > 0)
                type = type + "/";
            return GetIcon(pack, type, nr);
        }
        public static Imaging.ImageFile GetIcon(IO.PackCollection pack, string type, int nr) {
            type = type ?? string.Empty;
            if (type.Length > 0 && !type.EndsWith("/"))
                type = type + "/";

            var filePath = string.Format(IconFileFormat, nr / 1000, type, nr);

            File file;
            if (!pack.TryGetFile(filePath, out file) && type.Length > 0) {
                // Couldn't get specific type, try for generic version.
                filePath = string.Format(IconFileFormat, nr / 1000, string.Empty, nr);
                if (!pack.TryGetFile(filePath, out file)) {
                    // Couldn't get generic version either, that's a shame.
                    file = null;
                }
            }
            return file as Imaging.ImageFile;
        }
    }
}
