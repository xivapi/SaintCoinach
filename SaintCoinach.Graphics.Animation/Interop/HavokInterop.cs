using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Animation.Interop {
    public static class HavokInterop {
        static object _Lock = new object();
        
        static bool _IsLive;

        static bool _IsAwaiting = false;
        static Func<object> _CurrentAction;
        static object _CurrentResult;

        static HavokInterop() {
            _IsLive = true;

            var t = new System.Threading.Thread(HavokLoop);
            t.Name = "Havok thread";
            t.IsBackground = true;
            t.Start();
        }

        internal static void Execute(Action action) {
            Execute<object>(() => { action(); return null; });
        }
        internal static T Execute<T>(Func<T> func) {
            T result;
            lock (_Lock) {
                _CurrentAction = () => (object)func();
                _IsAwaiting = true;
                while (_IsAwaiting) {
                    if (!_IsLive)
                        throw new InvalidProgramException();
                }
                result = (T)_CurrentResult;
            }
            return result;
        }

        static void HavokLoop() {
            try {
                initHkInterop();

                while (true) {
                    if (_IsAwaiting) {
                        _CurrentResult = _CurrentAction();
                        _IsAwaiting = false;
                    } else
                        System.Threading.Thread.Sleep(5);
                }
            } finally {
                _IsLive = false;
                quitHkInterop();
            }
        }

        [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void initHkInterop();
        [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void quitHkInterop();
        
        [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr loadAnimationContainer(byte[] rigData, int rigSize, byte[] animationData, int animationSize);
        [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void unloadAnimationContainer(IntPtr ptr);
    }
}
