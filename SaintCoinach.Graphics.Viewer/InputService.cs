using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Multimedia;
using SharpDX.RawInput;
using System.Windows.Forms;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;

    public class InputService {
        #region Fields
        private List<Keys> _DownKeys = new List<Keys>();
        private Point _MousePosition;
        #endregion

        public Point MousePosition { get { return _MousePosition; } }

        #region Constructor
        public InputService(Form form) {
            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
            form.MouseMove += Form_MouseMove;
        }

        #endregion

        #region Get
        public IEnumerable<Keys> GetDownKeys() {
            Keys[] keys;
            lock (_DownKeys)
                keys = _DownKeys.ToArray();
            return keys;
        }
        #endregion

        #region Form events
        void Form_MouseMove(object sender, MouseEventArgs e) {
            _MousePosition = new Point(e.X, e.Y);
        }
        void Form_KeyUp(object sender, KeyEventArgs e) {
            lock (_DownKeys)
                _DownKeys.Remove(e.KeyCode);
        }

        void Form_KeyDown(object sender, KeyEventArgs e) {
            lock (_DownKeys) {
                if (!_DownKeys.Contains(e.KeyCode))
                    _DownKeys.Add(e.KeyCode);
            }
        }
        #endregion
    }
}
