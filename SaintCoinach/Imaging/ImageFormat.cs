namespace SaintCoinach.Imaging {
    public enum ImageFormat {
        Unknown = 0,
        R16G16F = 0x2250,
        A16R16G16B16Float = 0x2460,
        A32R32G32B32Float = 0x2470,

        A8R8G8B8_1 = 0x1131,
        A8R8G8B8_2 = 0x1450,
        A8R8G8B8_Cube = 0x1451,
        A8R8G8B82 = 0x1452,
        A8R8G8B8_4 = 0x2150,
        A8R8G8B8_5 = 0x4401,

        A4R4G4B4 = 0x1440,
        A1R5G5B5 = 0x1441,
        R3G3B2 = 0x1130,
        L8 = 0x1130,
        A8 = 0x1131,

        Dxt1 = 0x3420,
        Dxt3 = 0x3430,
        Dxt5 = 0x3431,

        D16 = 0x4140,
        D24S8 = 0x4250,

        Shadow16 = 0x5140,
        Shadow24 = 0x5150,
        BC5 = 0x6230,
        BC7 = 0x6432,
    }
}
