using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Imaging {
    using Ex;
    using IO;

    public static class IconHelper {
        static bool PreferHighRes = false;

        const string IconFileFormat = "ui/icon/{0:D3}000/{1}{2:D6}.tex";
        const string IconHr1FileFormat = "ui/icon/{0:D3}000/{1}{2:D6}_hr1.tex";

        private static File GetIconFile(IO.PackCollection pack, string filePathFormat, string type, int nr) {
            var filePath = string.Format(filePathFormat, nr / 1000, type, nr);
            if (!pack.TryGetFile(filePath, out var file) && type.Length > 0) {
                // Couldn't get specific type, try for generic version.
                filePath = string.Format(filePathFormat, nr / 1000, string.Empty, nr);
                if (!pack.TryGetFile(filePath, out file)) {
                    // Couldn't get generic version either, that's a shame.
                    file = null;
                }
            }
            return file;
        }

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

            File file = null;
            // we prefer hr1 then.
            if (PreferHighRes) {
                file = GetIconFile(pack, IconHr1FileFormat, type, nr);
            }
            if (file == null) {
                file = GetIconFile(pack, IconFileFormat, type, nr);
            }

            return file as Imaging.ImageFile;
        }
    }
}
