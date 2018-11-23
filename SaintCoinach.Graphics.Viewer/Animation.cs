using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
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
            public static extern int getPose(IntPtr ptr, float timestamp, [In, Out] InteropTransform[] output);
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
        public string Name { get; private set; }
        #endregion

        #region Constructor
        internal Animation(AnimationContainer container, IntPtr ptr, string name) {
            Container = container;
            _UnmanagedPtr = ptr;
            Name = name;

            Duration = HavokInterop.Execute(() => Interop.getDuration(_UnmanagedPtr));
            FrameCount = HavokInterop.Execute(() => Interop.getNumFrames(_UnmanagedPtr));
            FrameDuration = Duration / FrameCount;
        }
        #endregion

        #region Get
        public Matrix[] GetPose(float timestamp) {
            var transforms = new InteropTransform[Container.Skeleton.BoneCount];
            HavokInterop.Execute(() => Interop.getPose(_UnmanagedPtr, timestamp, transforms));
            return transforms.Select(t => t.ToTransformationMatrix()).ToArray();
        }
        #endregion
    }
}
