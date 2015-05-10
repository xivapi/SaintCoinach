using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.Multimedia;

    public class Mouse : IUpdateableComponent {
        #region Fields
        private MouseState _CurrentState;

        private Engine _Engine;
        #endregion

        #region Properties
        public MouseState GetState() {
            return _CurrentState;
        }
        #endregion

        #region Constructor
        public Mouse(Engine engine) {
            _Engine = engine;
        }
        #endregion

        #region IUpdateableComponent Members

        private bool _IsEnabled = true;
        public bool IsEnabled {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
        }

        public void Update(EngineTime engineTime) {
            var buttons = _Engine.InputService.GetDownMouseButtons();
            var pos = _Engine.InputService.MousePosition;
            _CurrentState.AbsolutePosition = new Vector2(pos.X, pos.Y);
            _CurrentState.RelativePosition = new Vector2(_CurrentState.AbsolutePosition.X / (float)_Engine.ViewportSize.Width, _CurrentState.AbsolutePosition.Y / (float)_Engine.ViewportSize.Height);
            _CurrentState.LeftButton = buttons.Contains(System.Windows.Forms.MouseButtons.Left);
            _CurrentState.RightButton = buttons.Contains(System.Windows.Forms.MouseButtons.Right);
            _CurrentState.MiddleButton = buttons.Contains(System.Windows.Forms.MouseButtons.Middle);
        }

        #endregion
    }
}
