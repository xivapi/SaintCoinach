using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach {
    public static class ByteArrayExtensions {
        public static T ToStructure<T>(this byte[] bytes, int offset) where T : struct {
            return ToStructure<T>(bytes, ref offset);
        }
        public static T ToStructure<T>(this byte[] bytes, ref int offset) where T : struct {
            var t = typeof(T);
            var size = Marshal.SizeOf(t);
            if (offset < 0 || size < 0 || offset > bytes.Length - size)
                throw new System.IO.InvalidDataException($"Structure read out of range. Type={t.Name}, Offset={offset}, Size={size}, BufferLength={bytes.Length}.");
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try {
                Marshal.Copy(bytes, offset, ptr, size);
                offset += size;
                return (T)Marshal.PtrToStructure(ptr, t);
            } finally {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static T[] ToStructures<T>(this byte[] bytes, int count, int offset) where T : struct {
            return ToStructures<T>(bytes, count, ref offset);
        }
        public static T[] ToStructures<T>(this byte[] bytes, int count, ref int offset) where T : struct {
            T[] values = new T[count];
            for (var i = 0; i < count; ++i)
                values[i] = ToStructure<T>(bytes, ref offset);
            return values;
        }

        public static string ReadString(this byte[] buffer, int offset) {
            return ReadString(buffer, ref offset);
        }
        public static string ReadString(this byte[] buffer, ref int offset) {
            if (offset < 0 || offset >= buffer.Length)
                return string.Empty;

            var strEnd = Array.IndexOf(buffer, (byte)0, offset);
            if (strEnd < 0)
                strEnd = buffer.Length;

            var size = strEnd - offset;
            if (size <= 0) {
                offset = strEnd < buffer.Length ? strEnd + 1 : buffer.Length;
                return string.Empty;
            }

            var value = Encoding.ASCII.GetString(buffer, offset, size);
            offset = strEnd < buffer.Length ? strEnd + 1 : buffer.Length;
            return value;
        }
    }
}
