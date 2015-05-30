using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Animation {
    using SharpDX;
    using Interop;

    public class Animation {
        #region Interop
        static class Interop {
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern float getDuration(IntPtr ptr);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern float getFrameDuration(IntPtr ptr);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getNumFrames(IntPtr ptr);
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getTransforms(IntPtr ptr, float timestamp, [In, Out] InteropTransform[] output);

            /*[DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getBoneIndexMap(IntPtr ptr, [In, Out] int[] output, int maximum);*/
        }
        #endregion

        #region Fields
        private IntPtr _UnmanagedPtr;
        #endregion

        #region Properties
        public AnimationContainer Container { get; private set; }
        public float Duration { get; private set; }
        public float FrameDuration { get; private set; }
        public int FrameCount { get; private set; }
        #endregion

        #region Constructor
        internal Animation(AnimationContainer container, IntPtr ptr) {
            Container = container;
            _UnmanagedPtr = ptr;

            Duration = HavokInterop.Execute(() => Interop.getDuration(_UnmanagedPtr));
            FrameCount = HavokInterop.Execute(() => Interop.getNumFrames(_UnmanagedPtr));
            FrameDuration = Duration / FrameCount;
        }
        #endregion

        #region Get
        public Matrix[] GetTransformationMatrices(float timestamp) {
            var transforms = new InteropTransform[Container.SkeletonBoneCount];
            HavokInterop.Execute(() => Interop.getTransforms(_UnmanagedPtr, timestamp, transforms));
            return transforms.Select(t => t.ToTransformationMatrix()).ToArray();
        }
        #endregion
    }
}
