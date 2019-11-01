using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Interop {

    /// <summary>
    /// Obtains an IntPtr to an Animation object from fbxInterop.dll.
    /// </summary>
    class InteropAnimation : IDisposable {
        static class Interop {
            [DllImport("fbxInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr loadAnimation(int count, int length,
                                                        [In, Out] byte[] data,
                                                        [In, Out] string[] names);

            [DllImport("fbxInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void unloadAnimation(IntPtr a);
        }

        internal IntPtr _UnmanagedPtr;

        public InteropAnimation(PapFile pap)
        {
            _UnmanagedPtr = Interop.loadAnimation(pap.Animations.Length,
                pap.HavokData.Length,
                pap.HavokData,
                pap.Animations.Select(_ => _.Name).ToArray());
        }

        private bool _IsDisposed = false;

        protected virtual void Dispose(bool disposing) {

            if (!_IsDisposed) {
                if (_UnmanagedPtr != IntPtr.Zero)
                    Interop.unloadAnimation(_UnmanagedPtr);
                _UnmanagedPtr = IntPtr.Zero;

                _IsDisposed = true;
            }
        }

        ~InteropAnimation() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
