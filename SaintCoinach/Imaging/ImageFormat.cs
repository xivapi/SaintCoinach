using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Imaging {
    public enum ImageFormat {
        Unknown = 0,
        A16R16G16B16_Float = 0x2460,

        A8R8G8B8_1 = 0x1131,
        A8R8G8B8_2 = 0x1450,
        A8R8G8B8_3 = 0x1451,
        A8R8G8B8_4 = 0x2150,
        A8R8G8B8_5 = 0x4401,

        A4R4G4B4 = 0x1440,
        A1R5G5B5 = 0x1441,
        R3G3B2 = 0x1130,

        DXT1 = 0x3420,
        DXT3 = 0x3430,
        DXT5 = 0x3431,
    }
}
