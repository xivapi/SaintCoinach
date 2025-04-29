using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using DotSquish;

namespace SaintCoinach.Imaging {

    struct DDS_PIXELFORMAT {
        public uint dwSize;
        public uint dwFlags;
        public uint dwFourCC;
        public uint dwRGBBitCount;
        public uint dwRBitMask;
        public uint dwGBitMask;
        public uint dwBBitMask;
        public uint dwABitMask;
    };

    struct DDS_HEADER {
        public uint dwSize;
        public uint dwFlags;
        public uint dwHeight;
        public uint dwWidth;
        public uint dwPitchOrLinearSize;
        public uint dwDepth;
        public uint dwMipMapCount;
        public uint dwReserved0;
        public uint dwReserved1;
        public uint dwReserved2;
        public uint dwReserved3;
        public uint dwReserved4;
        public uint dwReserved5;
        public uint dwReserved6;
        public uint dwReserved7;
        public uint dwReserved8;
        public uint dwReserved19;
        public uint dwReserved10;

        public DDS_PIXELFORMAT ddspf;
        public uint dwCaps;
        public uint dwCaps2;
        public uint dwCaps3;
        public uint dwCaps4;
        public uint dwReserved11;
    };

    enum DDSD_ENUM : uint {
        DDSD_CAPS = 0x1,//Required in every.dds file. 	
        DDSD_HEIGHT = 0x2,//Required in every.dds file.
        DDSD_WIDTH = 0x4,//Required in every.dds file.
        DDSD_PITCH = 0x8,//Required when pitch is provided for an uncompressed texture.
        DDSD_PIXELFORMAT = 0x1000,//Required in every.dds file.
        DDSD_MIPMAPCOUNT = 0x20000,//Required in a mipmapped texture.
        DDSD_LINEARSIZE = 0x80000,//Required when pitch is provided for a compressed texture.
        DDSD_DEPTH = 0x800000,//Required in a depth texture
    }

    enum DDPF_ENUM : uint {
        DDPF_ALPHAPIXELS = 0x1, // Texture contains alpha data; dwRGBAlphaBitMask contains valid data.
        DDPF_ALPHA = 0x2, // Used in some older DDS files for alpha channel only uncompressed data (dwRGBBitCount contains the alpha channel bitcount; dwABitMask contains valid data)
        DDPF_FOURCC = 0x4, // Texture contains compressed RGB data; dwFourCC contains valid data.
        DDPF_RGB = 0x40, // Texture contains uncompressed RGB data; dwRGBBitCountand the RGB masks(dwRBitMask, dwGBitMask, dwBBitMask) contain valid data.
    }
    /// <summary>
    ///     Helper class for converting image files as stored in SqPack
    ///     to formats useable in .NET
    /// </summary>
    public class ImageConverter {
        /// <summary>
        ///     Method signature for processing data as stored in SqPack into ARGB.
        /// </summary>
        /// <param name="src">Source buffer.</param>
        /// <param name="dst">_Buffer to write into.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        private delegate void Preprocessor(byte[] src, byte[] dst, int width, int height);

        private static readonly Dictionary<ImageFormat, Preprocessor> Preprocessors =
            new Dictionary<ImageFormat, Preprocessor> {
                {
                    ImageFormat.A16R16G16B16Float, ProcessA16R16G16B16_Float
                }, {
                    ImageFormat.A1R5G5B5, ProcessA1R5G5B5
                }, {
                    ImageFormat.A4R4G4B4, ProcessA4R4G4B4
                }, {
                    ImageFormat.A8R8G8B8_1, ProcessA8R8G8B8
                }, {
                    ImageFormat.A8R8G8B8_2, ProcessA8R8G8B8
                }, {
                    ImageFormat.A8R8G8B8_Cube, ProcessA8R8G8B8
                }, {
                    ImageFormat.A8R8G8B8_4, ProcessA8R8G8B8
                }, {
                    ImageFormat.A8R8G8B8_5, ProcessA8R8G8B8
                }, {
                    ImageFormat.Dxt1, ProcessDxt1
                }, {
                    ImageFormat.Dxt3, ProcessDxt3
                }, {
                    ImageFormat.Dxt5, ProcessDxt5
                }, {
                    ImageFormat.R3G3B2, ProcessR3G3B2
                }, {
                    ImageFormat.BC5, ProcessBC5
                }, {
                    ImageFormat.BC7, ProcessBC7
                }
            };

        #region Convert

        public static Image Convert(ImageFile file) {
            return Convert(file.GetData(), file.Format, file.Width, file.Height);
        }

        public static unsafe Image Convert(byte[] src, ImageFormat format, int width, int height) {
            var argb = GetA8R8G8B8(src, format, width, height);

            Image image;
            fixed (byte* p = argb) {
                var ptr = (IntPtr)p;
                using (var tempImage = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, ptr))
                    image = new Bitmap(tempImage);
            }
            return image;
        }

        public static byte[] GetA8R8G8B8(ImageFile file) {
            return GetA8R8G8B8(file.GetData(), file.Format, file.Width, file.Height);
        }

        public static byte[] GetA8R8G8B8(byte[] src, ImageFormat format, int width, int height) {
            if (!Preprocessors.TryGetValue(format, out var proc))
                throw new NotSupportedException(string.Format("Unsupported image format {0}", format));

            var argb = new byte[width * height * 4];
            proc(src, argb, width, height);
            return argb;
        }

        /// <summary>
        /// Attempt to convert ImageFile to DDS file. Returns null if no DXT1/3/5 texture found.
        /// </summary>

        public static byte[] GetDDS(ImageFile file) {
            var bytes2 = file.GetData();
            //var offset = bytes2[file.ImageHeader.EndOfHeader];
            var width = file.ImageHeader.Width;
            var height = file.ImageHeader.Height;

            DDS_HEADER header = new DDS_HEADER();
            DDS_PIXELFORMAT format = header.ddspf;


            format.dwFlags = (uint)(DDPF_ENUM.DDPF_ALPHAPIXELS | DDPF_ENUM.DDPF_FOURCC);
            header.dwFlags |= (uint)(DDSD_ENUM.DDSD_CAPS | DDSD_ENUM.DDSD_HEIGHT | DDSD_ENUM.DDSD_WIDTH | DDSD_ENUM.DDSD_PIXELFORMAT | DDSD_ENUM.DDSD_LINEARSIZE);
            header.dwFlags |= (uint)DDSD_ENUM.DDSD_MIPMAPCOUNT;

            switch (file.ImageHeader.Format) {
                case ImageFormat.Dxt1:
                    format.dwFourCC = 0x31545844;
                    header.dwPitchOrLinearSize = (uint)(Math.Max((uint)1, ((width + 3) / 4)) * Math.Max((uint)1, ((height + 3) / 4)) * 8);
                    break;
                case ImageFormat.Dxt3:
                    format.dwFourCC = 0x33545844;
                    header.dwPitchOrLinearSize = (uint)(Math.Max((uint)1, ((width + 3) / 4)) * Math.Max((uint)1, ((height + 3) / 4)) * 16);
                    break;
                case ImageFormat.Dxt5:
                    format.dwFourCC = 0x35545844;
                    header.dwPitchOrLinearSize = (uint)(Math.Max((uint)1, ((width + 3) / 4)) * Math.Max((uint)1, ((height + 3) / 4)) * 16);
                    break;
                /*
                case ImageFormat.A8R8G8B8_1:
                case ImageFormat.A8R8G8B8_2:
                case ImageFormat.A8R8G8B8_4:
                case ImageFormat.A8R8G8B8_5:
                    format.dwFlags = (uint)(DDPF_ENUM.DDPF_ALPHA | DDPF_ENUM.DDPF_RGB);
                    format.dwFourCC = 0x42475241;
                    header.dwPitchOrLinearSize = (uint)(width * height);
                    header.dwFlags &= ~(uint)DDSD_ENUM.DDSD_LINEARSIZE;
                    header.dwFlags |= (uint)DDSD_ENUM.DDSD_PITCH;
                    break;
                */
                default:
                    System.Diagnostics.Debug.WriteLine("Texture format " + file.ImageHeader.Format.ToString() + " DDS export not supported!\n");
                    return null;
                    break;
            }

            format.dwSize = 32;

            header.dwSize = 124; // why set this if it MUST be 124?
            header.dwHeight = (uint)height;
            header.dwWidth = (uint)width;
            header.dwMipMapCount = file.ImageHeader._Buffer[0x0E];
            header.dwCaps = 0x08 | 0x400000 | 0x1000; // DDSCAPS_COMPLEX | DDSCAPS_MIPMAP | DDSCAPS_TEXTURE

            header.ddspf = format;

            List<byte> data = new List<byte>();

            int size = System.Runtime.InteropServices.Marshal.SizeOf<DDS_HEADER>();
            byte[] headerBytes = new byte[size];

            var ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.StructureToPtr(header, ptr, false);
            System.Runtime.InteropServices.Marshal.Copy(ptr, headerBytes, 0, size);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);

            data.AddRange(System.Text.ASCIIEncoding.UTF8.GetBytes("DDS "));
            data.AddRange(headerBytes);
            data.AddRange(bytes2);

            return data.ToArray();
        }

        #endregion

        #region Preprocessors

        private static void ProcessA16R16G16B16_Float(byte[] src, byte[] dst, int width, int height) {
            // Clipping can, and will occur since values go outside 0..1
            for (var i = 0; i < width * height; ++i) {
                var srcOff = i * 4 * 2;
                var dstOff = i * 4;

                for (var j = 0; j < 4; ++j)
                    dst[dstOff + j] = (byte)(HalfHelper.Unpack(src, srcOff + j * 2) * byte.MaxValue);
            }
        }

        private static void ProcessA1R5G5B5(byte[] src, byte[] dst, int width, int height) {
            for (var i = 0; (i + 2) <= 2 * width * height; i += 2) {
                var v = BitConverter.ToUInt16(src, i);

                var a = (uint)(v & 0x8000);
                var r = (uint)(v & 0x7C00);
                var g = (uint)(v & 0x03E0);
                var b = (uint)(v & 0x001F);

                var rgb = ((r << 9) | (g << 6) | (b << 3));
                var argbValue = (a * 0x1FE00 | rgb | ((rgb >> 5) & 0x070707));

                for (var j = 0; j < 4; ++j)
                    dst[i * 2 + j] = (byte)(argbValue >> (8 * j));
            }
        }

        private static void ProcessA4R4G4B4(byte[] src, byte[] dst, int width, int height) {
            for (var i = 0; (i + 2) <= 2 * width * height; i += 2) {
                var v = BitConverter.ToUInt16(src, i);

                for (var j = 0; j < 4; ++j)
                    dst[i * 2 + j] = (byte)(((v >> (4 * j)) & 0x0F) << 4);
            }
        }

        private static void ProcessA8R8G8B8(byte[] src, byte[] dst, int width, int height) {
            // Some transparent images have larger dst lengths than their src.
            var length = Math.Min(src.Length, dst.Length);
            Array.Copy(src, dst, length);
        }

        private static void ProcessDxt1(byte[] src, byte[] dst, int width, int height) {
            var dec = Squish.DecompressImage(src, width, height, SquishOptions.DXT1);
            Array.Copy(dec, dst, dst.Length);
        }

        private static void ProcessDxt3(byte[] src, byte[] dst, int width, int height) {
            var dec = Squish.DecompressImage(src, width, height, SquishOptions.DXT3);
            Array.Copy(dec, dst, dst.Length);
        }

        private static void ProcessDxt5(byte[] src, byte[] dst, int width, int height) {
            var dec = Squish.DecompressImage(src, width, height, SquishOptions.DXT5);
            Array.Copy(dec, dst, dst.Length);
        }

        private static void ProcessR3G3B2(byte[] src, byte[] dst, int width, int height) {
            for (var i = 0; i < width * height; ++i) {
                var r = (uint)(src[i] & 0xE0);
                var g = (uint)(src[i] & 0x1C);
                var b = (uint)(src[i] & 0x03);

                dst[i * 4 + 0] = (byte)(b | (b << 2) | (b << 4) | (b << 6));
                dst[i * 4 + 1] = (byte)(g | (g << 3) | (g << 6));
                dst[i * 4 + 2] = (byte)(r | (r << 3) | (r << 6));
                dst[i * 4 + 3] = 0xFF;
            }
        }

        // https://github.com/0ceal0t/Dalamud-VFXEditor/blob/main/VFXEditor/Formats/TextureFormat/TextureDataFile.cs#L275

        private static void ProcessBC5(byte[] src, byte[] dst, int width, int height) {
            ProcessBC(src, dst, width, height, CompressionFormat.Bc5);
        }

        private static void ProcessBC7(byte[] src, byte[] dst, int width, int height) {
            ProcessBC(src, dst, width, height, CompressionFormat.Bc7);
        }

        private static void ProcessBC(byte[] src, byte[] dst, int width, int height, CompressionFormat format) {
            var decoder = new BcDecoder();
            var output = decoder.DecodeRaw2D(src, width, height, format).ToArray();

            var p = 0;
            for (var i = 0; i < height; i++) {
                for (var j = 0; j < width; j++) {
                    var pixel = output[i, j];
                    dst[p] = pixel.b;
                    dst[p + 1] = pixel.g;
                    dst[p + 2] = pixel.r;
                    dst[p + 3] = pixel.a;
                    p += 4;
                }
            }
        }
        #endregion
    }
}
