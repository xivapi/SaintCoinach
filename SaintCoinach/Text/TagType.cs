using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public enum TagType : byte {
        None            = 0x00,

        /* Appears to set values for an input time.
         * - 222 / DEh  Year
         * - 221 / DDh  Month
         * - 220 / DCh  Day of week
         * - 219 / DBh  Day of month
         * - 218 / DAh  Hour
         * - 217 / D9h  Minute
         */
        Time            = 0x07,     // TODO: It seems to set the time used further on.
        If              = 0x08,
        Switch          = 0x09,
        IfEquals        = 0x0C,
        Unknown0A       = 0x0A,     // TODO
        LineBreak       = 0x10,
        // Wait            = 0x11, // Not present anywhere in game data up to 2015.04.17.0001.0000

        /* Font icon.
         * GraphicsFileTextureDefinition is 'common/font/gfdata.gfd'
         * Texture is one of:
         * - 'common/font/fonticon_ps3.tex'
         * - 'common/font/fonticon_ps4.tex'
         * - 'common/font/fonticon_xinput.tex'
         */
        Gui             = 0x12,
        Color           = 0x13,
        Unknown14       = 0x14,     // TODO
        Unknown16       = 0x16,     // TODO
        Unknown17       = 0x17,     // TODO: Used exclusively in Japanese and at start of new lines.
        Emphasis2       = 0x19,     // TODO: See if this is bold, only used very little. 0x1A emphasis is italic.
        Emphasis        = 0x1A,
        Indent          = 0x1D,
        CommandIcon     = 0x1E,
        Dash            = 0x1F,
        Value           = 0x20,
        Format          = 0x22,
        TwoDigitValue   = 0x24,     // A single-digit value is formatted with a leading zero. 
        // Time = 0x25, // Not present anywhere in game data up to 2015.04.17.0001.0000
        Sheet           = 0x28,
        Highlight       = 0x29,
        Clickable       = 0x2B,     // Seemingly anything that has an action associated with it (NPCs, PCs, Items, etc.)
        Split           = 0x2C,
        Unknown2D       = 0x2D,     // TODO
        Fixed           = 0x2E,
        Unknown2F       = 0x2F,     // TODO
        SheetJa         = 0x30,
        SheetEn         = 0x31,
        SheetDe         = 0x32,
        SheetFr         = 0x33,
        InstanceContent = 0x40,     // Presumably so it can be clicked?
        ZeroPaddedValue = 0x50,
        Unknown60       = 0x60,     // TODO: Used as prefix in Gold Saucer announcements.
    }
}
