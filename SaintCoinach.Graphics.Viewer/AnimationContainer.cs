using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using Interop;

    public class AnimationContainer : IDisposable {
        #region Interop
        static class Interop {
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr loadAnimationContainer(IntPtr skeleton, byte[] animationData, int animationSize);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void unloadAnimationContainer(IntPtr ptr);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getNumAnimations(IntPtr ptr);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr getAnimation(IntPtr ptr, int index);
        }
        #endregion

        #region Fields
        private IntPtr _UnmanagedPtr;
        private Dictionary<string, int> _AnimationNameMap;
        private Dictionary<int, Animation> _Animations = new Dictionary<int, Animation>();
        #endregion

        #region Properties
        public Skeleton Skeleton { get; private set; }
        public PapFile File { get; private set; }
        public IEnumerable<string> AnimationNames { get { return _AnimationNameMap.Keys; } }
        public int AnimationCount { get { return File.Header.AnimationCount; } }
        #endregion

        #region Constructor
        public AnimationContainer(Skeleton skeleton, PapFile file) {
            Skeleton = skeleton;
            File = file;

            _AnimationNameMap = file.Animations.ToDictionary(_ => _.Name, _ => _.Index);
            _UnmanagedPtr = HavokInterop.Execute(() => Interop.loadAnimationContainer(skeleton._UnmanagedPtr, file.HavokData, file.HavokData.Length));

            var numAnim = HavokInterop.Execute(() => Interop.getNumAnimations(_UnmanagedPtr));
            if (AnimationCount != numAnim)
                throw new System.IO.InvalidDataException();
        }
        #endregion

        #region Things
        public Animation Get(string name) {
            return Get(_AnimationNameMap[name]);
        }
        public Animation Get(int index) {
            Animation anim;
            if (!_Animations.TryGetValue(index, out anim))
                _Animations.Add(index, anim = new Animation(this, HavokInterop.Execute(() => Interop.getAnimation(_UnmanagedPtr, index)), _AnimationNameMap.Where(_ => _.Value == index).Select(_ => _.Key).FirstOrDefault()));
            return anim;
        }
        #endregion

        #region IDisposable Support
        private bool _IsDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!_IsDisposed) {
                if (_UnmanagedPtr != IntPtr.Zero)
                    HavokInterop.Execute(() => Interop.unloadAnimationContainer(_UnmanagedPtr));
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
