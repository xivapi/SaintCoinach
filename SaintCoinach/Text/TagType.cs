using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public enum TagType : byte {
        If              = 0x08,
        Switch          = 0x09,
        LineBreak       = 0x10,
        Wait            = 0x11,
        Gui             = 0x12,
        Color           = 0x13,
        Emphasis        = 0x1A,
        Dash            = 0x1F,
        Value           = 0x20,
        Format          = 0x22,
        Zero            = 0x24,
        Time            = 0x25,
        Sheet           = 0x28,
        Fixed           = 0x2E,
        SheetJa         = 0x30,
        SheetEn         = 0x31,
        SheetDe         = 0x32,
        SheetFr         = 0x33
    }
}
