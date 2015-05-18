using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Lgb {
    public enum LgbEntryType : int {
        Model = 0x01,
        // TODO: Following are unconfirmed
        Light = 0x03,
        Gimmick = 0x06,
        Environment = 0x2F,
    }
}
