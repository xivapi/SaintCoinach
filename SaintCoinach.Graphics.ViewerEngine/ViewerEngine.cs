using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace SaintCoinach.Graphics.Viewer {
    public class ViewerEngine : ComponentContainer {
        #region Fields
        private string _Title;
        private RenderForm _Form;
        private Device _Device;
        private SwapChain _SwapChain;
        private RenderTargetView _RenderTarget;

        private Stopwatch _RunTimer;
        private long _TotalElapsedTime;
        #endregion

        #region Properties
        public RenderForm Form { get { return _Form; } }
        public Device Device { get { return _Device; } }
        #endregion

        #region Constructor
        public ViewerEngine(string title) {
            _Title = title;
        }
        #endregion

        #region Run
        public void Run() {
            _Form = new RenderForm(_Title);

            var desc = new SwapChainDescription {
                BufferCount = 1,
                Flags = SwapChainFlags.None,
                IsWindowed = true,
                ModeDescription = new ModeDescription(
                    Form.ClientSize.Width, Form.ClientSize.Height,
                    new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = Form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out _Device, out _SwapChain);

            var factory = _SwapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(Form.Handle, WindowAssociationFlags.IgnoreAll);

            var backBuffer = Texture2D.FromSwapChain<Texture2D>(_SwapChain, 0);
            _RenderTarget = new RenderTargetView(Device, backBuffer);

            _RunTimer = new Stopwatch();
            _RunTimer.Start();
            
            RenderLoop.Run(Form, EngineLoop);
        }
        private void EngineLoop() {
            var elapsed = _RunTimer.Elapsed;
            _RunTimer.Restart();
            _TotalElapsedTime += elapsed.Ticks;
            var time = new EngineTime(TimeSpan.FromTicks(_TotalElapsedTime), elapsed);

            Update(time);
            Draw(Device, time);

            _SwapChain.Present(0, PresentFlags.None);
        }
        #endregion
    }
}
