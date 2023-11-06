using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;
using SharpDX.Windows;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using Buffer = SharpDX.Direct3D11.Buffer;
    using Device = SharpDX.Direct3D11.Device;

    public class FormEngine : Engine {
        const int ScreenshotWidth = 2048;
        const int ScreenshotHeight = 2048;
        const long ResizeDelay = 250;

        #region Fields
        private bool _IsResizePending;
        private Stopwatch _ResizeTimer = new Stopwatch();
        private SwapChain _SwapChain;

        private IInputService _InputService;

        private string _Title;
        private RenderForm _Form;

        private Texture2D _ScreenshotTarget;
        private Texture2D _ScreenshotStencil;

        private RenderTargetView _ScreenshotTargetView;
        private DepthStencilView _ScreenshotStencilView;
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
            _IsResizePending = true;
            _ResizeTimer.Restart();
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
        protected override void Draw(EngineTime time) {
            Device.ImmediateContext.ClearRenderTargetView(RenderTargetView, Color.CornflowerBlue);

            base.Draw(time);
        }
        protected override void Present() {
            _SwapChain.Present(0, PresentFlags.None);
        }
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (disposing) {
                if (_ScreenshotTargetView != null)
                    _ScreenshotTargetView.Dispose();
                if (_ScreenshotStencilView != null)
                    _ScreenshotStencilView.Dispose();

                if (_ScreenshotTarget != null)
                    _ScreenshotTarget.Dispose();
                if (_ScreenshotStencil != null)
                    _ScreenshotStencil.Dispose();

                _ScreenshotStencil = null;
                _ScreenshotStencilView = null;
                _ScreenshotTarget = null;
                _ScreenshotTargetView = null;

                if (_SwapChain != null)
                    _SwapChain.Dispose();
                _SwapChain = null;

                if (_Form != null)
                    _Form.Dispose();
                _Form = null;
            }
        }
        protected void ResizeToClient() {
            Resize(Form.ClientSize.Width, Form.ClientSize.Height);
        }
        protected void ResizeToScreenshot() {
            Resize(ScreenshotWidth, ScreenshotHeight);
        }
        protected void TakeScreenshot(EngineTime time) {
            Camera.Update(time);

            Device.ImmediateContext.OutputMerger.SetDepthStencilState(DepthStencilState);
            Device.ImmediateContext.OutputMerger.SetBlendState(BlendState);
            Device.ImmediateContext.Rasterizer.State = RasterizerState;
            Device.ImmediateContext.OutputMerger.SetTargets(_ScreenshotStencilView, _ScreenshotTargetView);
            Device.ImmediateContext.ClearDepthStencilView(_ScreenshotStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);

            Device.ImmediateContext.ClearRenderTargetView(_ScreenshotTargetView, Color.Transparent);
            base.Draw(time);

            // Texture2D.ToFile(Device.ImmediateContext, _ScreenshotTarget, ImageFileFormat.Png, string.Format("SS-{0:yyyyMMdd-HHmmss}.png", DateTime.Now));
            ResizeToClient();
        }
        protected override void Update(EngineTime time) {
            if (_IsResizePending) {
                var elapsed = _ResizeTimer.ElapsedMilliseconds;
                if (elapsed > ResizeDelay) {
                    ResizeToClient();
                    _ResizeTimer.Reset();
                    _IsResizePending = false;
                }
            }
            if (Keyboard.WasKeyPressed(System.Windows.Forms.Keys.F12)) {
                if (_ScreenshotTargetView == null) {
                    var rtTexDesc = RenderTarget.Description;
                    rtTexDesc.SampleDescription = new SampleDescription(1, 0);
                    rtTexDesc.Width = ScreenshotWidth;
                    rtTexDesc.Height = ScreenshotHeight;

                    _ScreenshotTarget = new Texture2D(Device, rtTexDesc);
                    _ScreenshotTargetView = new RenderTargetView(Device, _ScreenshotTarget);

                    var dsTexDesc = new Texture2DDescription {
                        Format = Format.D24_UNorm_S8_UInt,
                        ArraySize = 1,
                        MipLevels = 1,
                        Width = _ScreenshotTarget.Description.Width,
                        Height = _ScreenshotTarget.Description.Height,
                        SampleDescription = _ScreenshotTarget.Description.SampleDescription,
                        Usage = ResourceUsage.Default,
                        BindFlags = BindFlags.DepthStencil,
                        CpuAccessFlags = CpuAccessFlags.None,
                        OptionFlags = ResourceOptionFlags.None,
                    };
                    _ScreenshotStencil = new Texture2D(Device, dsTexDesc);
                    _ScreenshotStencilView = new DepthStencilView(Device, _ScreenshotStencil, new DepthStencilViewDescription {
                        Flags = DepthStencilViewFlags.None,
                        Dimension = DepthStencilViewDimension.Texture2DMultisampled,
                        Format = dsTexDesc.Format
                    });
                }
                ResizeToScreenshot();
                TakeScreenshot(time);
                ResizeToClient();
            }

            base.Update(time);
        }
    }
}
