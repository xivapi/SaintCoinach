using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using DotSquish;

namespace SaintCoinach.Imaging {
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
            Preprocessor proc;
            if (!Preprocessors.TryGetValue(format, out proc))
                throw new NotSupportedException(string.Format("Unsupported image format {0}", format));

            var argb = new byte[width * height * 4];
            proc(src, argb, width, height);
            return argb;
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
            Array.Copy(src, dst, dst.Length);
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

        #endregion
    }
}
