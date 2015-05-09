using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.Multimedia;
    using SharpDX.RawInput;

    public class Mouse : IUpdateableComponent {
        #region Fields
        private List<MouseInputEventArgs> _EventQueue = new List<MouseInputEventArgs>();

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

            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericMouse, DeviceFlags.None);
            Device.MouseInput += Device_MouseInput;
        }

        void Device_MouseInput(object sender, MouseInputEventArgs e) {
            lock (_EventQueue)
                _EventQueue.Add(e);
        }
        #endregion

        #region IUpdateableComponent Members

        private bool _IsEnabled = true;
        public bool IsEnabled {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
        }

        public void Update(EngineTime engineTime) {
            MouseInputEventArgs[] q;
            lock (_EventQueue) {
                q = _EventQueue.ToArray();
                _EventQueue.Clear();
            }

            _CurrentState.AbsolutePosition = _Engine.InputService.MousePosition;
            _CurrentState.RelativePosition = new Vector2(_CurrentState.AbsolutePosition.X / (float)_Engine.Form.ClientSize.Width, _CurrentState.AbsolutePosition.Y / (float)_Engine.Form.ClientSize.Height);
            _CurrentState.MouseWheel = 0;

            foreach (var e in q) {
                _CurrentState.MouseWheel = e.WheelDelta;
                
                switch (e.ButtonFlags) {
                    case MouseButtonFlags.LeftButtonDown:
                        _CurrentState.LeftButton = true;
                        break;
                    case MouseButtonFlags.LeftButtonUp:
                        _CurrentState.LeftButton = false;
                        break;
                    case MouseButtonFlags.RightButtonDown:
                        _CurrentState.RightButton = true;
                        break;
                    case MouseButtonFlags.RightButtonUp:
                        _CurrentState.RightButton = false;
                        break;
                    case MouseButtonFlags.MiddleButtonDown:
                        _CurrentState.MiddleButton = true;
                        break;
                    case MouseButtonFlags.MiddleButtonUp:
                        _CurrentState.MiddleButton = false;
                        break;
                }
            }
        }

        #endregion
    }
}
