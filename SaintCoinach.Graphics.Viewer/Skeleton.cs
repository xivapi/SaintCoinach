using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using Interop;

    public class Skeleton : IDisposable {
        #region Interop
        static class Interop {
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr loadSkeleton(byte[] rigData, int rigSize);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void unloadSkeleton(IntPtr ptr);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getNumBones(IntPtr ptr);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getBoneNames(IntPtr ptr, [In, Out] string[] output);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getReferencePose(IntPtr ptr, [In, Out] InteropTransform[] output);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getParentIndices(IntPtr ptr, [In, Out] int[] output);
        }
        #endregion

        #region Fields
        internal IntPtr _UnmanagedPtr;
        #endregion

        #region Properties
        public SklbFile File { get; private set; }
        public int BoneCount { get; private set; }
        public int[] ParentBoneIndices { get; private set; }
        public Matrix[] ReferencePose { get; private set; }
        public string[] BoneNames { get; private set; }
        #endregion

        #region Constructor
        public Skeleton(SklbFile file) {
            this.File = file;

            _UnmanagedPtr = HavokInterop.Execute(() => Interop.loadSkeleton(file.HavokData, file.HavokData.Length));

            BoneCount = HavokInterop.Execute(() => Interop.getNumBones(_UnmanagedPtr));

            BoneNames = new string[BoneCount];
            HavokInterop.Execute(() => Interop.getBoneNames(_UnmanagedPtr, BoneNames));

            ParentBoneIndices = new int[BoneCount];
            HavokInterop.Execute(() => Interop.getParentIndices(_UnmanagedPtr, ParentBoneIndices));

            ReferencePose = new Matrix[BoneCount];
            var referencePoseLocal = new InteropTransform[BoneCount];
            HavokInterop.Execute(() => Interop.getReferencePose(_UnmanagedPtr, referencePoseLocal));
            for (var target = 0; target < BoneCount; ++target) {
                var current = target;
                ReferencePose[target] = Matrix.Identity;
                while (current >= 0) {
                    ReferencePose[target] = ReferencePose[target] * referencePoseLocal[current].ToTransformationMatrix();

                    current = ParentBoneIndices[current];
                }
            }
        }
        #endregion

        #region IDisposable Support
        private bool _IsDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!_IsDisposed) {
                if (_UnmanagedPtr != IntPtr.Zero)
                    HavokInterop.Execute(() => Interop.unloadSkeleton(_UnmanagedPtr));
                _UnmanagedPtr = IntPtr.Zero;

                _IsDisposed = true;
            }
        }

        ~Skeleton() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
