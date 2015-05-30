using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Animation {
    using SharpDX;
    using Interop;

    public class AnimationContainer : IDisposable {
        #region Interop
        static class Interop {
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getNumBones(IntPtr ptr);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getNumAnimations(IntPtr ptr);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr getAnimation(IntPtr ptr, int index);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getBoneNames(IntPtr ptr, [In, Out] string[] output);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getReferencePose(IntPtr ptr, [In, Out] InteropTransform[] output);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getParentIndices(IntPtr ptr, [In, Out] int[] output);
        }
        #endregion

        #region Fields
        private IntPtr _UnmanagedPtr;
        private Dictionary<string, int> _AnimationNameMap;
        private Dictionary<int, Animation> _Animations = new Dictionary<int, Animation>();
        #endregion

        #region Properties
        public SklbFile Skeleton { get; private set; }
        public PapFile Pap { get; private set; }
        public IEnumerable<string> AnimationNames { get { return _AnimationNameMap.Keys; } }
        public int AnimationCount { get { return Pap.Header.AnimationCount; } }
        public int SkeletonBoneCount { get; private set; }
        public int[] ParentBoneIndices { get; private set; }
        public Matrix[] ReferencePose { get; private set; }
        public string[] SkeletonBoneNames { get; private set; }
        #endregion

        #region Constructor
        public AnimationContainer(SklbFile skeleton, PapFile pap) {
            Skeleton = skeleton;
            Pap = pap;

            _AnimationNameMap = pap.Animations.ToDictionary(_ => _.Name, _ => _.Index);
            _UnmanagedPtr = HavokInterop.Execute(() => HavokInterop.loadAnimationContainer(skeleton.HavokData, skeleton.HavokData.Length, pap.HavokData, pap.HavokData.Length));

            var numAnim = HavokInterop.Execute(() => Interop.getNumAnimations(_UnmanagedPtr));
            if (AnimationCount != numAnim)
                throw new System.IO.InvalidDataException();

            SkeletonBoneCount = HavokInterop.Execute(() => Interop.getNumBones(_UnmanagedPtr));

            SkeletonBoneNames = new string[SkeletonBoneCount];
            HavokInterop.Execute(() => Interop.getBoneNames(_UnmanagedPtr, SkeletonBoneNames));

            ParentBoneIndices = new int[SkeletonBoneCount];
            HavokInterop.Execute(() => Interop.getParentIndices(_UnmanagedPtr, ParentBoneIndices));

            ReferencePose = new Matrix[SkeletonBoneCount];
            var referencePoseLocal = new InteropTransform[SkeletonBoneCount];
            HavokInterop.Execute(() => Interop.getReferencePose(_UnmanagedPtr, referencePoseLocal));
            for (var target = 0; target < SkeletonBoneCount; ++target) {
                var current = target;
                ReferencePose[target] = Matrix.Identity;
                while (current >= 0) {
                    ReferencePose[target] = ReferencePose[target] * referencePoseLocal[current].ToTransformationMatrix();

                    current = ParentBoneIndices[current];
                }
            }
        }
        #endregion

        #region Things
        public Animation Get(string name) {
            return Get(_AnimationNameMap[name]);
        }
        public Animation Get(int index) {
            Animation anim;
            if (!_Animations.TryGetValue(index, out anim))
                _Animations.Add(index, anim = new Animation(this, HavokInterop.Execute(() => Interop.getAnimation(_UnmanagedPtr, index))));
            return anim;
        }
        #endregion

        #region IDisposable Support
        private bool _IsDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!_IsDisposed) {
                if (_UnmanagedPtr != IntPtr.Zero)
                    HavokInterop.Execute(() => HavokInterop.unloadAnimationContainer(_UnmanagedPtr));
                _UnmanagedPtr = IntPtr.Zero;

                _IsDisposed = true;
            }
        }

        ~AnimationContainer() {
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
