using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Multimedia;
using SharpDX.RawInput;

using Keys = System.Windows.Forms.Keys;

namespace SaintCoinach.Graphics.Viewer {
    public class Keyboard : IUpdateableComponent {
        #region Fields
        private List<KeyboardInputEventArgs> _EventQueue = new List<KeyboardInputEventArgs>();

        private List<Keys> _ReleasedKeys = new List<Keys>();
        private List<Keys> _PressedKeys = new List<Keys>();
        private List<Keys> _DownKeys = new List<Keys>();

        private Engine _Engine;
        #endregion

        #region Constructor
        public Keyboard(Engine engine) {
            _Engine = engine;
            Device.KeyboardInput += Device_KeyboardInput;
        }

        void Device_KeyboardInput(object sender, KeyboardInputEventArgs e) {
            lock (_EventQueue)
                _EventQueue.Add(e);
        }
        #endregion

        #region Query
        public bool IsKeyDown(Keys key) {
            return _DownKeys.Contains(key);
        }
        public bool IsKeyUp(Keys key) {
            return !_DownKeys.Contains(key);
        }
        public bool WasKeyReleased(Keys key) {
            return _ReleasedKeys.Contains(key);
        }
        public bool WasKeyPressed(Keys key) {
            return _PressedKeys.Contains(key);
        }
        #endregion

        #region IUpdateable Members
        private bool _IsEnabled = true;
        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public void Update(EngineTime time) {
            KeyboardInputEventArgs[] events;
            lock (_EventQueue) {
                events = _EventQueue.ToArray();
                _EventQueue.Clear();
            }

            _ReleasedKeys.Clear();
            _PressedKeys.Clear();

            var downKeys = _Engine.InputService.GetDownKeys();
            _PressedKeys.AddRange(downKeys.Except(_DownKeys));
            _ReleasedKeys.AddRange(_DownKeys.Except(downKeys));

            _DownKeys.AddRange(_PressedKeys);
            foreach (var k in _ReleasedKeys)
                _DownKeys.Remove(k);
        }

        private void Process(KeyboardInputEventArgs e) {
            switch (e.State) {
                case KeyState.KeyDown:
                case KeyState.ImeKeyDown:
                case KeyState.SystemKeyDown:
                    OnKeyDown(e.Key);
                    break;
                case KeyState.KeyUp:
                case KeyState.ImeKeyUp:
                case KeyState.SystemKeyUp:
                    OnKeyUp(e.Key);
                    break;
            }
        }
        private void OnKeyDown(Keys key) {
            var wasDown = _DownKeys.Contains(key);
            if (!wasDown) {
                _PressedKeys.Add(key);
                _DownKeys.Add(key);
            }
        }
        private void OnKeyUp(Keys key) {
            var wasDown = _DownKeys.Remove(key);
            if (wasDown)
                _ReleasedKeys.Add(key);
        }
        #endregion
    }
}
