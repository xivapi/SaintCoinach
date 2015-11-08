using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public struct Quad {
        public short Value1;
        public short Value2;
        public short Value3;
        public short Value4;

        public Quad(long data) {
            Value1 = (short)data;
            Value2 = (short)(data >> 16);
            Value3 = (short)(data >> 32);
            Value4 = (short)(data >> 48);
        }

        public static Quad Read(byte[] buffer, int offset, bool bigEndian) {
            var data = OrderedBitConverter.ToInt64(buffer, offset, bigEndian);
            return new Quad(data);
        }

        public override string ToString() {
            return Value1 + ", " + Value2 + ", " + Value3 + ", " + Value4;
        }

        public Int64 ToInt64() {
            return (Int64)Value1 + ((Int64)Value2 << 16) + ((Int64)Value3 << 32) + ((Int64)Value4 << 48);
        }
    }
}
