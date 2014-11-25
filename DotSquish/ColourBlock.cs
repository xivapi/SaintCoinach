using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSquish {
    internal static class ColourBlock {
        private static int Unpack565(byte[] packed, int packedOffset, byte[] colour, int colourOffset) {
            // Build the packed value.
            var value = (int)packed[packedOffset] | ((int)packed[packedOffset + 1] << 8);

            // Get the components in the stored range.
            var red = (byte)((value >> 11) & 0x1F);
            var green = (byte)((value >> 5) & 0x3F);
            var blue = (byte)(value & 0x1F);

            // Scale up to 8 bits
            colour[colourOffset + 0] = (byte)((red << 3) | (red >> 2));
            colour[colourOffset + 1] = (byte)((green << 2) | (green >> 4));
            colour[colourOffset + 2] = (byte)((blue << 3) | (blue >> 2));
            colour[colourOffset + 3] = 255;

            return value;
        }
        public static byte[] DecompressColour(byte[] block, int blockOffset, bool isDxt1) {
            // Unpack the endpoints
            var codes = new byte[16];
            var a = Unpack565(block, blockOffset + 0, codes, 0);
            var b = Unpack565(block, blockOffset + 2, codes, 4);

            // Generate the midpoints.
            for (int i = 0; i < 3; ++i) {
                var c = codes[i];
                var d = codes[4 + i];

                if (isDxt1 && a <= b) {
                    codes[8 + i] = (byte)((c + d) / 2);
                    codes[12 + i] = 0;
                } else {
                    codes[8 + i] = (byte)(((2 * c) + d) / 3);
                    codes[12 + i] = (byte)((c + (2 * d)) / 3);
                }
            }

            // Fill in alpha for the intermediate values.
            codes[8 + 3] = 255;
            codes[12 + 3] = (byte)((isDxt1 && a <= b) ? 0 : 255);

            // Unpack the indices
            var indices = new byte[16];
            for (int i = 0; i < 4; i++) {
                var packed = block[blockOffset + 4 + i];

                indices[4 * i + 0] = (byte)(packed & 0x3);
                indices[4 * i + 1] = (byte)((packed >> 2) & 0x3);
                indices[4 * i + 2] = (byte)((packed >> 4) & 0x3);
                indices[4 * i + 3] = (byte)((packed >> 6) & 0x3);
            }

            // Store the colours
            var rgba = new byte[4 * 16];
            for (int i = 0; i < 16; ++i) {
                var offset = 4 * indices[i];
                for (int j = 0; j < 4; ++j)
                    rgba[4 * i + j] = codes[offset + j];
            }
            return rgba;
        }
    }
}
