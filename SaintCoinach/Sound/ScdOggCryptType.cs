using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Sound {
    public enum ScdOggCryptType : short {
        None = 0x0000,
        VorbisHeaderXor = 0x2002,
        FullXorUsingTable = 0x2003,
    }
}
