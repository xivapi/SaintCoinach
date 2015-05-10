using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Multimedia;
using System.Windows.Forms;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;

    public class FormInputService : IInputService {
        #region Fields
        private List<Keys> _DownKeys = new List<Keys>();
        private Point _MousePosition;
        private List<MouseButtons> _DownMouseButtons = new List<MouseButtons>();
        #endregion

        public Point MousePosition { get { return _MousePosition; } }

        #region Constructor
        public FormInputService(Form form) {
            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
            form.MouseMove += Form_MouseMove;
            form.MouseDown += Form_MouseDown;
            form.MouseUp += Form_MouseUp;
            form.LostFocus += Form_LostFocus;
        }
        #endregion

        #region Get
        public IEnumerable<Keys> GetDownKeys() {
            Keys[] keys;
            lock (_DownKeys)
                keys = _DownKeys.ToArray();
            return keys;
        }
        public IEnumerable<MouseButtons> GetDownMouseButtons() {
            MouseButtons[] buttons;
            lock (_DownMouseButtons)
                buttons = _DownMouseButtons.ToArray();
            return buttons;
        }
        #endregion

        #region Form events
        void Form_LostFocus(object sender, EventArgs e) {
            lock (_DownMouseButtons)
                _DownMouseButtons.Clear();
            lock (_DownKeys)
                _DownKeys.Clear();
        }
        void Form_MouseUp(object sender, MouseEventArgs e) {
            lock (_DownMouseButtons)
                _DownMouseButtons.Remove(e.Button);
        }

        void Form_MouseDown(object sender, MouseEventArgs e) {
            lock (_DownMouseButtons) {
                if (!_DownMouseButtons.Contains(e.Button))
                    _DownMouseButtons.Add(e.Button);
            }
        }

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
