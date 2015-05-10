using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Multimedia;

using Keys = System.Windows.Forms.Keys;

namespace SaintCoinach.Graphics.Viewer {
    public class Keyboard : IUpdateableComponent {
        #region Fields
        private List<Keys> _ReleasedKeys = new List<Keys>();
        private List<Keys> _PressedKeys = new List<Keys>();
        private List<Keys> _DownKeys = new List<Keys>();

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
        #endregion

        #region IUpdateable Members
        private bool _IsEnabled = true;
        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public void Update(EngineTime time) {
            _ReleasedKeys.Clear();
            _PressedKeys.Clear();

            var downKeys = _Engine.InputService.GetDownKeys();
            _PressedKeys.AddRange(downKeys.Except(_DownKeys));
            _ReleasedKeys.AddRange(_DownKeys.Except(downKeys));

            _DownKeys.AddRange(_PressedKeys);
            foreach (var k in _ReleasedKeys)
                _DownKeys.Remove(k);
        }
        #endregion
    }
}
