using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using SharpDX.Windows;
    using Buffer = SharpDX.Direct3D11.Buffer;
    using Device = SharpDX.Direct3D11.Device;

    public class FormEngine : Engine {
        #region Fields
        private SwapChain _SwapChain;

        private IInputService _InputService;

        private string _Title;
        private RenderForm _Form;
        #endregion

        #region Properties
        public override IInputService InputService {
            get { return _InputService; }
        }
        public RenderForm Form { get { return _Form; } }
        public override bool IsActive {
            get { return Form.ContainsFocus; }
        }
        #endregion

        #region Constructor
        public FormEngine(string title) {
            _Title = title;
        }
        #endregion

        #region Run
        public void Run() {
            using (_Form = new RenderForm(_Title)) {
                var assembly = Assembly.GetExecutingAssembly();
                using (var iconStream = assembly.GetManifestResourceStream("SaintCoinach.Graphics.Viewer.Viewer.ico"))
                    _Form.Icon = new System.Drawing.Icon(iconStream);

                SetUp(Form.ClientSize.Width, Form.ClientSize.Height);
                Form.ClientSizeChanged += Form_ClientSizeChanged;

                _InputService = new FormInputService(Form);

                Initialize();
                Load();

                RenderLoop.Run(Form, EngineLoop);

                Unload();
            }
        }
        void Form_ClientSizeChanged(object sender, EventArgs e) {
            Resize(Form.ClientSize.Width, Form.ClientSize.Height);
        }
        #endregion

        protected override Device CreateDevice(int width, int height) {
            var desc = new SwapChainDescription {
                BufferCount = 1,
                Flags = SwapChainFlags.None,
                IsWindowed = true,
                ModeDescription = new ModeDescription(
                    width, height,
                    new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = Form.Handle,
                SampleDescription = new SampleDescription(8, 0), //new SampleDescription(8, Device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, 8)),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput | Usage.BackBuffer | Usage.Shared,
            };

            SharpDX.Direct3D11.Device device;
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out device, out _SwapChain);
            return device;
        }
        private bool _IsResizing;
        protected override void Resize(int newWidth, int newHeight) {
            _IsResizing = true;
            base.Resize(newWidth, newHeight);
        }
        protected override Texture2D CreateRenderTarget(int width, int height) {
            if(_IsResizing)
                _SwapChain.ResizeBuffers(1, width, height, Format.Unknown, SwapChainFlags.None);
            _IsResizing = false;
            return Texture2D.FromSwapChain<Texture2D>(_SwapChain, 0);
        }
        protected override void Present() {
            _SwapChain.Present(0, PresentFlags.None);
        }
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (disposing) {
                if (_SwapChain != null)
                    _SwapChain.Dispose();
                _SwapChain = null;

                if (_Form != null)
                    _Form.Dispose();
                _Form = null;
            }
        }
    }
}
