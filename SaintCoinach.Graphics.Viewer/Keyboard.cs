using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Multimedia;

using Keys = System.Windows.Forms.Keys;

namespace SaintCoinach.Graphics.Viewer {
    public class Keyboard : IUpdateableComponent {
        #region Fields
        private List<Keys> _ReleasedKeys = new List<Keys>();
        private List<Keys> _PressedKeys = new List<Keys>();
        private List<Keys> _DownKeys = new List<Keys>();
        private Dictionary<Keys, int> _FirstRepeatKeys = new Dictionary<Keys, int>();
        private Dictionary<Keys, int> _SubsequentRepeatKeys = new Dictionary<Keys, int>();
        private List<Keys> _RepeatKeys = new List<Keys>();

        private Engine _Engine;
        #endregion

        #region Constructor
        public Keyboard(Engine engine) {
            _Engine = engine;
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
        public bool WasKeyPressedRepeatable(Keys key) {
            return _RepeatKeys.Contains(key);
        }
        #endregion

        #region IUpdateable Members
        private bool _IsEnabled = true;
        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public void Update(EngineTime time) {
            _ReleasedKeys.Clear();
            _PressedKeys.Clear();
            _RepeatKeys.Clear();

            var downKeys = _Engine.InputService.GetDownKeys();
            _PressedKeys.AddRange(downKeys.Except(_DownKeys));
            _ReleasedKeys.AddRange(_DownKeys.Except(downKeys));

            _DownKeys.AddRange(_PressedKeys);
            foreach (var k in _ReleasedKeys) {
                _DownKeys.Remove(k);
                _FirstRepeatKeys.Remove(k);
                _SubsequentRepeatKeys.Remove(k);
            }

            var firstRepeatDelay = 250 * (1 + SystemInformation.KeyboardDelay);
            var subsequentRepeatDelay = (int)(1000 / (2.5 + 27.5 * SystemInformation.KeyboardSpeed / 31));

            foreach (var key in downKeys.Except(_SubsequentRepeatKeys.Keys)) {
                if (_FirstRepeatKeys.TryGetValue(key, out var next)) {
                    if (next <= Environment.TickCount) {
                        _FirstRepeatKeys.Remove(key);
                        _SubsequentRepeatKeys[key] = Environment.TickCount + subsequentRepeatDelay;
                        _RepeatKeys.Add(key);
                    }
                } else {
                    _FirstRepeatKeys[key] = Environment.TickCount + firstRepeatDelay;
                    _RepeatKeys.Add(key);
                }
            }
            foreach (var pair in _SubsequentRepeatKeys.ToArray()) {
                if (pair.Value <= Environment.TickCount) {
                    _SubsequentRepeatKeys[pair.Key] = Environment.TickCount + subsequentRepeatDelay;
                    _RepeatKeys.Add(pair.Key);
                }
            }
        }
        #endregion
    }
}
