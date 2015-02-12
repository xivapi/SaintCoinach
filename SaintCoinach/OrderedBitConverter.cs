using System;

namespace SaintCoinach {
    /// <summary>
    ///     Static class for reading values from byte buffers using a specified endianness.
    /// </summary>
    /// <remarks>
    ///     This class is a wrapper around <see cref="BitConverter" /> inverting the order of bytes, if necessary, before
    ///     passing them on.
    /// </remarks>
    public static class OrderedBitConverter {
        #region CheckEndian

        /// <summary>
        ///     Checks the requested endianness against the one used by the system, and reversed the byte buffer if neccessary.
        /// </summary>
        /// <param name="data">Byte array containing the data to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if the data should be or is in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        private static void CheckEndian(ref byte[] data, bool bigEndian) {
            if (bigEndian == BitConverter.IsLittleEndian)
                Array.Reverse(data);
        }

        #endregion

        #region GetBytes

        /// <summary>
        ///     Returns the specified 16-bit signed integer as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified 16-bit signed integer as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(Int16 value, bool bigEndian) {
            var ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }

        /// <summary>
        ///     Returns the specified 16-bit unsigned integer as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified 16-bit unsigned integer as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(UInt16 value, bool bigEndian) {
            return GetBytes((Int16)value, bigEndian);
        }

        /// <summary>
        ///     Returns the specified 32-bit signed integer as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified 32-bit signed integer as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(Int32 value, bool bigEndian) {
            var ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }

        /// <summary>
        ///     Returns the specified 32-bit unsigned integer as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified 32-bit unsigned integer as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(UInt32 value, bool bigEndian) {
            return GetBytes((Int32)value, bigEndian);
        }

        /// <summary>
        ///     Returns the specified 64-bit signed integer as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified 64-bit signed integer as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(Int64 value, bool bigEndian) {
            var ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }

        /// <summary>
        ///     Returns the specified 64-bit unsigned integer as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified 64-bit unsigned integer as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(UInt64 value, bool bigEndian) {
            return GetBytes((Int64)value, bigEndian);
        }

        /// <summary>
        ///     Returns the specified single-precision floating point value as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified single-precision floating point value as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(Single value, bool bigEndian) {
            var ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }

        /// <summary>
        ///     Returns the specified double-precision floating point value as an array of bytes using the specified endianness.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>The specified double-precision floating point value as an array of bytes using the specified endianness.</returns>
        public static byte[] GetBytes(Double value, bool bigEndian) {
            var ret = BitConverter.GetBytes(value);
            CheckEndian(ref ret, bigEndian);
            return ret;
        }

        #endregion

        #region ToX

        /// <summary>
        ///     Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A 16-bit signed integer converted from two bytes at a specified position in a byte array using the specified
        ///     endianness.
        /// </returns>
        public static Int16 ToInt16(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 2 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            var tmp = new byte[2];
            Array.Copy(buffer, offset, tmp, 0, 2);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToInt16(tmp, 0);
        }

        /// <summary>
        ///     Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A 16-bit unsigned integer converted from two bytes at a specified position in a byte array using the specified
        ///     endianness.
        /// </returns>
        public static UInt16 ToUInt16(byte[] buffer, int offset, bool bigEndian) {
            return (UInt16)ToInt16(buffer, offset, bigEndian);
        }

        /// <summary>
        ///     Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A 32-bit signed integer converted from four bytes at a specified position in a byte array using the specified
        ///     endianness.
        /// </returns>
        public static Int32 ToInt32(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 4 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            var tmp = new byte[4];
            Array.Copy(buffer, offset, tmp, 0, 4);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToInt32(tmp, 0);
        }

        /// <summary>
        ///     Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A 32-bit unsigned integer converted from four bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </returns>
        public static UInt32 ToUInt32(byte[] buffer, int offset, bool bigEndian) {
            return (UInt32)ToInt32(buffer, offset, bigEndian);
        }

        /// <summary>
        ///     Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A 64-bit signed integer converted from eight bytes at a specified position in a byte array using the specified
        ///     endianness.
        /// </returns>
        public static Int64 ToInt64(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 8 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            var tmp = new byte[8];
            Array.Copy(buffer, offset, tmp, 0, 8);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToInt64(tmp, 0);
        }

        /// <summary>
        ///     Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A 64-bit unsigned integer converted from eight bytes at a specified position in a byte array using the
        ///     specified endianness.
        /// </returns>
        public static UInt64 ToUInt64(byte[] buffer, int offset, bool bigEndian) {
            return (UInt64)ToInt64(buffer, offset, bigEndian);
        }

        /// <summary>
        ///     Returns a single-precision floating point value converted from four bytes at a specified position in a byte array
        ///     using the specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A single-precision floating point value converted from four bytes at a specified position in a byte array
        ///     using the specified endianness.
        /// </returns>
        public static Single ToSingle(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 4 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            var tmp = new byte[4];
            Array.Copy(buffer, offset, tmp, 0, 4);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToSingle(tmp, 0);
        }

        /// <summary>
        ///     Returns a double-precision floating point value converted from eight bytes at a specified position in a byte array
        ///     using the specified endianness.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The starting position within <c>buffer</c>.</param>
        /// <param name="bigEndian">
        ///     <c>true</c> if target data should be in big-endian order; <c>false</c> if it is in
        ///     little-endian.
        /// </param>
        /// <returns>
        ///     A double-precision floating point value converted from eight bytes at a specified position in a byte array
        ///     using the specified endianness.
        /// </returns>
        public static Double ToDouble(byte[] buffer, int offset, bool bigEndian) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset + 8 > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");

            var tmp = new byte[8];
            Array.Copy(buffer, offset, tmp, 0, 8);
            CheckEndian(ref tmp, bigEndian);
            return BitConverter.ToDouble(tmp, 0);
        }

        #endregion
    }
}
