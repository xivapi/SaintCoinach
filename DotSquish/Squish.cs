using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSquish {
    public static class Squish {
        public static int GetStorageRequirements(int width, int height, SquishOptions flags) {
            var blockCount = ((width + 3) / 4) * ((height + 3) / 4);
            var blockSize = flags.HasFlag(SquishOptions.DXT1) ? 8 : 16;
            return blockCount * blockSize;
        }

        #region On buffer
        public static byte[] CompressBlock(byte[] rgba, SquishOptions flags) {
            return CompressBlockMasked(rgba, 0xFFFF, flags);
        }
        public static byte[] CompressBlockMasked(byte[] rgba, int mask, SquishOptions flags) {
            throw new NotImplementedException();
        }
        public static byte[] DecompressBlock(byte[] block, int blockOffset, SquishOptions flags) {
            // Get the block locations
            var colOff = blockOffset;
            var alphaOff = blockOffset;
            if ((flags & (SquishOptions.DXT3 | SquishOptions.DXT5)) != 0)
                colOff += 8;

            // Decompress colour.
            var rgba = ColourBlock.DecompressColour(block, colOff, flags.HasFlag(SquishOptions.DXT1));

            // Decompress alpha seperately if necessary.
            if (flags.HasFlag(SquishOptions.DXT3))
                Alpha.DecompressAlphaDxt3(block, alphaOff, rgba, 0);
            else if (flags.HasFlag(SquishOptions.DXT5))
                Alpha.DecompressAlphaDxt5(block, alphaOff, rgba, 0);

            return rgba;
        }

        public static byte[] CompressImage(byte[] rgba, int width, int height, SquishOptions flags) {
            throw new NotImplementedException();
        }
        public static byte[] DecompressImage(byte[] blocks, int width, int height, SquishOptions flags) {
            return DecompressImage(blocks, 0, width, height, flags);
        }
        public static byte[] DecompressImage(byte[] blocks, int offset, int width, int height, SquishOptions flags) {
            var argb = new byte[4 * width * height];
            var bytesPerBlock = flags.HasFlag(SquishOptions.DXT1) ? 8 : 16;
            
            var blockOffset = offset;
            // Loop over blocks.
            for (int y = 0; y < height; y += 4) {
                for (int x = 0; x < width; x += 4) {
                    // Decompress the block.
                    var targetRgba = DecompressBlock(blocks, blockOffset, flags);

                    // Write the decompressed pixels to the correct image locations.
                    var sourcePixelOffset = 0;
                    for (int py = 0; py < 4; ++py) {
                        for (int px = 0; px < 4; ++px) {
                            // Get the target location.
                            var sx = x + px;
                            var sy = y + py;
                            if (sx < width && sy < height) {
                                var targetPixelOffset = 4 * ((width * sy) + sx);
                                // Copy the rgba value
                                argb[targetPixelOffset + 0] = targetRgba[sourcePixelOffset + 2];
                                argb[targetPixelOffset + 1] = targetRgba[sourcePixelOffset + 1];
                                argb[targetPixelOffset + 2] = targetRgba[sourcePixelOffset + 0];
                                argb[targetPixelOffset + 3] = targetRgba[sourcePixelOffset + 3];
                            }
                            sourcePixelOffset += 4;
                        }
                    }

                    // advance
                    blockOffset += bytesPerBlock;
                }
            }
            return argb;
        }
        public static Image DecompressToBitmap(byte[] blocks, int width, int height, SquishOptions flags) {
            return DecompressToBitmap(blocks, 0, width, height, flags);
        }
        public static unsafe Image DecompressToBitmap(byte[] blocks, int offset, int width, int height, SquishOptions flags) {
            var fullBuffer = new byte[4 * width * height];
            var bufferOffset = 0;

            var bytesPerBlock = flags.HasFlag(SquishOptions.DXT1) ? 8 : 16;
            var blockOffset = offset;
            // Loop over blocks.
            for (int y = 0; y < height; y += 4) {
                for (int x = 0; x < width; x += 4) {
                    // Decompress the block.
                    var targetRgba = DecompressBlock(blocks, blockOffset, flags);


                    // Write the decompressed pixels to the correct image locations.
                    var sourcePixelOffset = 0;
                    for (int py = 0; py < 4; ++py) {
                        for (int px = 0; px < 4; ++px) {
                            // Get the target location.
                            var sx = x + px;
                            var sy = y + py;
                            if (sx < width && sy < height) {
                                var i = 4 * (sx + (sy * width));
                                fullBuffer[bufferOffset + i + 0] = targetRgba[sourcePixelOffset + 2];
                                fullBuffer[bufferOffset + i + 1] = targetRgba[sourcePixelOffset + 1];
                                fullBuffer[bufferOffset + i + 2] = targetRgba[sourcePixelOffset + 0];
                                fullBuffer[bufferOffset + i + 3] = targetRgba[sourcePixelOffset + 3];
                            }

                            sourcePixelOffset += 4; // Skip this pixel as it is outside the image.
                        }
                    }

                    // advance
                    blockOffset += bytesPerBlock;
                }
            }
            Image ret;
            fixed (byte* p = fullBuffer) {
                var ptr = (IntPtr)p;
                var tempImage = new Bitmap(width, height, 4 * width, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ptr);
                ret = new Bitmap(tempImage);
            }
            return ret;
        }
        #endregion

        #region On stream
        public static void CompressBlock(Stream input, Stream output, SquishOptions flags) {
            CompressBlockMasked(input, output, 0xFFFF, flags);
        }
        public static void CompressBlockMasked(Stream input, Stream output, int mask, SquishOptions flags){
            throw new NotImplementedException();
        }
        public static void DecompressBlock(Stream input, Stream output, SquishOptions flags) {
            throw new NotImplementedException();
        }
        public static void CompressImage(Stream input, Stream output, int width, int height, SquishOptions flags) {
            throw new NotImplementedException();
        }
        public static void DecompressImage(Stream input, Stream output, int width, int height, SquishOptions flags) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
