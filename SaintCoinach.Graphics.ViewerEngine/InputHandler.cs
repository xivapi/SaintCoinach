using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Multimedia;
using SharpDX.RawInput;
using System.Windows.Forms;

namespace SaintCoinach.Graphics {
    static class InputHandler {
        #region Keyboard
        static List<Keys> _DownKeys = new List<Keys>();
        static void Device_KeyboardInput(object sender, KeyboardInputEventArgs e) {
            switch (e.State) {
                case KeyState.KeyDown:
                case KeyState.ImeKeyDown:
                case KeyState.SystemKeyDown:
                    lock (_DownKeys) {
                        if (!_DownKeys.Contains(e.Key))
                            _DownKeys.Add(e.Key);
                    }
                    break;
                case KeyState.KeyUp:
                case KeyState.ImeKeyUp:
                case KeyState.SystemKeyUp:
                    lock (_DownKeys)
                        _DownKeys.Remove(e.Key);
                    break;
            }
        }

        public static IEnumerable<Keys> GetDownKeys() {
            Keys[] keys;
            lock (_DownKeys)
                keys = _DownKeys.ToArray();
            return keys;
        }
        #endregion

        public static void Init(Form form) {
            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
        }

        public static void Unload(Form form) {
            form.KeyDown -= Form_KeyDown;
            form.KeyUp -= Form_KeyUp;
        }

        static void Form_KeyUp(object sender, KeyEventArgs e) {
            lock (_DownKeys)
                _DownKeys.Remove(e.KeyCode);
        }

        static void Form_KeyDown(object sender, KeyEventArgs e) {
            lock (_DownKeys) {
                if (!_DownKeys.Contains(e.KeyCode))
                    _DownKeys.Add(e.KeyCode);
            }
        }

    }
}
