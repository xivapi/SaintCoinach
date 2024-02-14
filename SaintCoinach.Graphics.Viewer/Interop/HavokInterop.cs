using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SaintCoinach.Graphics.Viewer.Interop {
    public static class HavokInterop {
        static object _Lock = new object();

        static bool _Initialized;

        static bool _IsThreaded;
        static volatile  bool _IsLive;

        static volatile bool _IsAwaiting = false;
        static Func<object> _CurrentAction;
        static object _CurrentResult;

        static HavokInterop() {
        }

        public static void InitializeSTA() {
            if (_Initialized)
                throw new InvalidOperationException();
            initHkInterop();

            _IsThreaded = false;
            _Initialized = true;
        }
        public static void InitializeMTA() {
            if (_Initialized)
                throw new InvalidOperationException();
            var t = new System.Threading.Thread(HavokLoop);
            t.Name = "Havok thread";
            t.IsBackground = true;
            t.Start();

            _IsThreaded = true;
            _IsLive = true;
        }

        internal static void Execute(Action action) {
            Execute<object>(() => { action(); return null; });
        }
        internal static T Execute<T>(Func<T> func) {
            if (!_Initialized)
                throw new InvalidOperationException();

            if (!_IsThreaded)
                return func();

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
                _Initialized = true;

                while (true) {
                    if (_IsAwaiting) {
                        _CurrentResult = _CurrentAction();
                        _IsAwaiting = false;
                    } else
                        System.Threading.Thread.Sleep(5);
                }
            } catch (BadImageFormatException e) {
                var result = MessageBox.Show("Failed to load Havok interop library.\nPlease ensure that the 32-bit (x86) Visual C++ Redistributable is installed.\nWould you like to download it now?", "Havok interop error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(new ProcessStartInfo("https://www.microsoft.com/en-us/download/details.aspx?id=30679")
                    {
                        UseShellExecute = true,
                    });
                    Environment.Exit(0);
                }
                else
                {
                    result = MessageBox.Show("Would you like to run Godbert anyways, without 3D animation support?", "Havok interop error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                    {
                        Environment.Exit(0);
                    }
                }
            } finally {
                _IsLive = false;
                if (_Initialized)
                    quitHkInterop();
            }
        }

        [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void initHkInterop();
        [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void quitHkInterop();
    }
}
