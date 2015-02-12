using System;

using SaintCoinach.Graphics.Assets;
using SaintCoinach.Imaging;

namespace SaintCoinach.IO {
    public static class FileFactory {
        public static File Get(Directory directory, IndexFile file) {
            var stream = directory.Pack.GetDataStream(file.DatFile);
            stream.Position = file.Offset;

            var header = new FileCommonHeader(file, stream);

            switch (header.FileType) {
                case FileType.Empty:
                    return new EmptyFile(directory, header);
                case FileType.Default:
                    return new FileDefault(directory, header);
                case FileType.Image:
                    return new ImageFile(directory, header);
                case FileType.Model:
                    return new ModelFile(directory, header);
                default:
                    throw new NotSupportedException(string.Format("Unknown file type {0:X2}h", (int)header.FileType));
            }
        }
    }
}
