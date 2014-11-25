using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach {
    public static class OrderedBitConverter {
        #region CheckEndian
        private static void CheckEndian(ref byte[] data, bool bigEndian) {
            if (bigEndian == BitConverter.IsLittleEndian)
                Array.Reverse(data);
        }
        #endregion

        #region GetBytes
        public static byte[] GetBytes(Int16 value, bool bigEndian) {
            byte[] ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }
        public static byte[] GetBytes(UInt16 value, bool bigEndian) { return GetBytes((Int16)value, bigEndian); }

        public static byte[] GetBytes(Int32 value, bool bigEndian) {
            byte[] ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }
        public static byte[] GetBytes(UInt32 value, bool bigEndian) { return GetBytes((Int32)value, bigEndian); }

        public static byte[] GetBytes(Int64 value, bool bigEndian) {
            byte[] ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }
        public static byte[] GetBytes(UInt64 value, bool bigEndian) { return GetBytes((Int64)value, bigEndian); }

        public static byte[] GetBytes(Single value, bool bigEndian) {
            byte[] ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }

        public static byte[] GetBytes(Double value, bool bigEndian) {
            byte[] ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }
        #endregion

        #region ToX
        public static Int16 ToInt16(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 2 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            byte[] tmp = new byte[2];
            Array.Copy(buffer, offset, tmp, 0, 2);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToInt16(tmp, 0);
        }
        public static UInt16 ToUInt16(byte[] buffer, int offset, bool bigEndian) { return (UInt16)ToInt16(buffer, offset, bigEndian); }

        public static Int32 ToInt32(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 4 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            byte[] tmp = new byte[4];
            Array.Copy(buffer, offset, tmp, 0, 4);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToInt32(tmp, 0);
        }
        public static UInt32 ToUInt32(byte[] buffer, int offset, bool bigEndian) { return (UInt32)ToInt32(buffer, offset, bigEndian); }

        public static Int64 ToInt64(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 8 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            byte[] tmp = new byte[8];
            Array.Copy(buffer, offset, tmp, 0, 8);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToInt64(tmp, 0);
        }
        public static UInt64 ToUInt64(byte[] buffer, int offset, bool bigEndian) { return (UInt64)ToInt64(buffer, offset, bigEndian); }

        public static Single ToSingle(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 4 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            byte[] tmp = new byte[4];
            Array.Copy(buffer, offset, tmp, 0, 4);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToSingle(tmp, 0);
        }
        public static Double ToDouble(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 8 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            byte[] tmp = new byte[8];
            Array.Copy(buffer, offset, tmp, 0, 8);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToDouble(tmp, 0);
        }
        #endregion
    }
}
