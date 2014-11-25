using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach {
    public static class StringHelper {
        public static string Decode(byte[] binary) {
            // XXX: Actually do this
            return Encoding.UTF8.GetString(binary);
        }
    }
}
