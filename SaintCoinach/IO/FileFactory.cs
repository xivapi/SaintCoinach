using System;

using SaintCoinach.Graphics;
using SaintCoinach.Imaging;

namespace SaintCoinach.IO {
    public static class FileFactory {
        public static File Get(Pack pack, IIndexFile file) {
            var stream = pack.GetDataStream(file.DatFile);
            stream.Position = file.Offset;

            var header = new FileCommonHeader(file, stream);

            switch (header.FileType) {
                case FileType.Empty:
                    return new EmptyFile(pack, header);
                case FileType.Default:
                    return new FileDefault(pack, header);
                case FileType.Image:
                    return new ImageFile(pack, header);
                case FileType.Model:
                    return new ModelFile(pack, header);
                default:
                    throw new NotSupportedException(string.Format("Unknown file type {0:X2}h", (int)header.FileType));
            }
        }
    }
}
