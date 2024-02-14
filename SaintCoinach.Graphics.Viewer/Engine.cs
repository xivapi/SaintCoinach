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
    using Buffer = SharpDX.Direct3D11.Buffer;
    using Device = SharpDX.Direct3D11.Device;

    public abstract class Engine : IDisposable {

        #region Fields
        private Device _Device;

        private Keyboard _Keyboard;
        private Mouse _Mouse;
        private Camera _Camera;

        private Texture2D _RenderTarget;
        private RasterizerState _RasterizerState;
        private DepthStencilState _StencilState;
        private RenderTargetView _RenderTargetView;
        private BlendState _BlendState;

        private Texture2D _DepthStencil;
        private DepthStencilView _DepthStencilView;

        private Content.Cube _Cube;

        private Stopwatch _RunTimer;
        private long _TotalElapsedTime;

        private ComponentContainer _CoreComponents;
        private ComponentContainer _Components;

        private ModelFactory _ModelFactory;
        private TextureFactory _TextureFactory;
        private EffectFactory _EffectFactory;
        private MaterialFactory _MaterialFactory;

        private Size2 _ViewportSize;
        #endregion

        #region Properties
        protected Texture2D RenderTarget { get { return _RenderTarget; } }
        protected RenderTargetView RenderTargetView { get { return _RenderTargetView; } }

        public Device Device { get { return _Device; } }
        public Keyboard Keyboard { get { return _Keyboard; } }
        public Mouse Mouse { get { return _Mouse; } }
        public Camera Camera { get { return _Camera; } }

        public Content.Cube Cube { get { return _Cube; } }

        protected ComponentContainer CoreComponents { get { return _CoreComponents; } }
        public ComponentContainer Components { get { return _Components; } }

        public ModelFactory ModelFactory { get { return _ModelFactory; } }
        public TextureFactory TextureFactory { get { return _TextureFactory; } }
        public EffectFactory EffectFactory { get { return _EffectFactory; } }
        public MaterialFactory MaterialFactory { get { return _MaterialFactory; } }
        public abstract IInputService InputService { get; }
        public virtual Size2 ViewportSize { get { return _ViewportSize; } }
        public abstract bool IsActive { get; }

        public DepthStencilState DepthStencilState {
            get { return _StencilState; }
            protected set {
                if (value == _StencilState)
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (_StencilState != null)
                    _StencilState.Dispose();
                _StencilState = value;
            }
        }
        public BlendState BlendState {
            get { return _BlendState; }
            protected set {
                if (value == _BlendState)
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (_BlendState != null)
                    _BlendState.Dispose();
                _BlendState = value;
            }
        }
        public RasterizerState RasterizerState {
            get { return _RasterizerState; }
            protected set {
                if (value == _RasterizerState)
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (_RasterizerState != null)
                    _RasterizerState.Dispose();
                _RasterizerState = value;
            }
        }
        #endregion

        #region Constructor
        protected Engine() {
            _CoreComponents = new ComponentContainer();
            _Components = new ComponentContainer();
        }
        #endregion

        #region Shared
        protected abstract Device CreateDevice(int width, int height);
        protected abstract Texture2D CreateRenderTarget(int width, int height);
        protected virtual void Resize(int newWidth, int newHeight) {
            Device.ImmediateContext.OutputMerger.ResetTargets();

            _RenderTargetView.Dispose();
            _RenderTarget.Dispose();

            _DepthStencilView.Dispose();
            _DepthStencil.Dispose();

            CreateView(newWidth, newHeight);
        }
        protected void SetUp(int width, int height) {
            _Device = CreateDevice(width, height);

            CreateView(width, height);

            var depthDesc = DepthStencilStateDescription.Default();
            _StencilState = new DepthStencilState(Device, depthDesc);
            var blendDesc = BlendStateDescription.Default();
            blendDesc.RenderTarget[0].IsBlendEnabled = true;
            blendDesc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
            blendDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
            blendDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
            blendDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            _BlendState = new BlendState(Device, blendDesc);

            var raster = RasterizerStateDescription.Default();
            raster.CullMode = CullMode.Front;
            raster.IsMultisampleEnabled = RenderTarget.Description.SampleDescription.Count > 1;

            _RasterizerState = new RasterizerState(Device, raster);
        }

        protected void CreateView(int width, int height) {
            _ViewportSize = new Size2(width, height);
            _RenderTarget = CreateRenderTarget(width, height);
            _RenderTargetView = new RenderTargetView(Device, _RenderTarget);

            var dsTexDesc = new Texture2DDescription {
                Format = Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = _RenderTarget.Description.Width,
                Height = _RenderTarget.Description.Height,
                SampleDescription = _RenderTarget.Description.SampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
            };
            _DepthStencil = new Texture2D(Device, dsTexDesc);
            _DepthStencilView = new DepthStencilView(Device, _DepthStencil, new DepthStencilViewDescription {
                Flags = DepthStencilViewFlags.None,
                Dimension = DepthStencilViewDimension.Texture2DMultisampled,
                Format = dsTexDesc.Format
            });

            Device.ImmediateContext.Rasterizer.SetViewport(new Viewport(0, 0, width, height));
        }
        #endregion

        #region Initialize
        protected virtual void Initialize() {
            CoreComponents.Add(_Keyboard = new Keyboard(this));
            CoreComponents.Add(_Mouse = new Mouse(this));
            CoreComponents.Add(_Camera = new Camera(this));

            _ModelFactory = new ModelFactory(this);
            _TextureFactory = new TextureFactory(this);
            _EffectFactory = new EffectFactory(this);
            _MaterialFactory = new MaterialFactory(this);
        }
        #endregion

        #region Content
        protected void Load() {
            CoreComponents.LoadContent();
            Components.LoadContent();

            _Cube = new Content.Cube(this);
            _Cube.LoadContent();

            LoadContent();
        }
        protected virtual void LoadContent() { }

        protected void Unload() {
            Components.UnloadContent();
            CoreComponents.UnloadContent();

            if (_Cube != null)
                _Cube.UnloadContent();
            _Cube = null;

            ModelFactory.UnloadAll();
            MaterialFactory.UnloadAll();
            EffectFactory.UnloadAll();
            TextureFactory.UnloadAll();

            UnloadContent();
        }
        protected virtual void UnloadContent() { }
        #endregion

        #region Loop
        protected void EngineLoop() {
            if (_RunTimer == null) {
                _RunTimer = new Stopwatch();
                _RunTimer.Start();
            }
            var elapsed = _RunTimer.Elapsed;
            _RunTimer.Restart();
            _TotalElapsedTime += elapsed.Ticks;
            var time = new EngineTime(TimeSpan.FromTicks(_TotalElapsedTime), elapsed);

            EngineLoop(time);
        }
        protected void EngineLoop(EngineTime time) {
            Update(time);

            BeforeDraw(time);
            Draw(time);
            AfterDraw(time);

            Present();
        }

        protected abstract void Present();

        protected virtual void Update(EngineTime time) {
            CoreComponents.Update(time);
            Components.Update(time);
        }
        protected virtual void BeforeDraw(EngineTime time) {
            Device.ImmediateContext.OutputMerger.SetDepthStencilState(DepthStencilState);
            Device.ImmediateContext.OutputMerger.SetBlendState(BlendState);
            Device.ImmediateContext.Rasterizer.State = RasterizerState;
            Device.ImmediateContext.OutputMerger.SetTargets(_DepthStencilView, _RenderTargetView);
            Device.ImmediateContext.ClearDepthStencilView(_DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
        }
        protected virtual void Draw(EngineTime time) {
            var world = Matrix.Identity;
            var view = Camera.View;
            var proj = Camera.Projection;

            Draw3D(time, ref world, ref view, ref proj);
        }
        protected virtual void AfterDraw(EngineTime time) {

        }

        private void Draw3D(EngineTime time, ref Matrix world, ref Matrix view, ref Matrix proj) {
            CoreComponents.Draw(time, ref world, ref view, ref proj);
            Components.Draw(time, ref world, ref view, ref proj);
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            Unload();
            if (disposing) {
                if (_BlendState != null)
                    _BlendState.Dispose();
                _BlendState = null;

                if (_DepthStencilView != null)
                    _DepthStencilView.Dispose();
                _DepthStencilView = null;

                if (_DepthStencil != null)
                    _DepthStencil.Dispose();
                _DepthStencil = null;

                if (_RasterizerState != null)
                    _RasterizerState.Dispose();
                _RasterizerState = null;

                if (_RenderTargetView != null)
                    _RenderTargetView.Dispose();
                _RenderTargetView = null;

                if (_RenderTarget != null)
                    _RenderTarget.Dispose();
                _RenderTarget = null;

                if (_StencilState != null)
                    _StencilState = null;
                _StencilState = null;

                if (_Device != null)
                    _Device.Dispose();
                _Device = null;
            }
        }
        #endregion
    }
}
