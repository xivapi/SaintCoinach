using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using SharpDX.Windows;
    using Buffer = SharpDX.Direct3D11.Buffer;
    using Device = SharpDX.Direct3D11.Device;

    public class Engine {

        #region Fields
        private string _Title;
        private RenderForm _Form;
        private Device _Device;
        private SwapChain _SwapChain;

        private InputService _InputService;
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

        private Stopwatch _RunTimer;
        private long _TotalElapsedTime;

        private Content.Cube _Cube;

        private ComponentContainer _CoreComponents;
        private ComponentContainer _Components;

        private ModelFactory _ModelFactory;
        private TextureFactory _TextureFactory;
        private EffectFactory _EffectFactory;
        private MaterialFactory _MaterialFactory;
        #endregion

        #region Properties
        public RenderForm Form { get { return _Form; } }
        public Device Device { get { return _Device; } }
        public InputService InputService { get { return _InputService; } }
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
        #endregion

        #region Constructor
        public Engine(string title) {
            _Title = title;

            _CoreComponents = new ComponentContainer();
            _Components = new ComponentContainer();
        }
        #endregion

        #region Run
        public void Run() {
            using (_Form = new RenderForm(_Title)) {
                CreateDevice();
                Form.ClientSizeChanged += Form_ClientSizeChanged;

                Initialize();

                Load();

                _RunTimer = new Stopwatch();
                _RunTimer.Start();

                RenderLoop.Run(Form, EngineLoop);

                Unload();
            }
        }
        void Form_ClientSizeChanged(object sender, EventArgs e) {
            var newMode = new ModeDescription(
                Form.ClientSize.Width, Form.ClientSize.Height,
                new Rational(60, 1), Format.R8G8B8A8_UNorm);
            Device.ImmediateContext.OutputMerger.ResetTargets();

            _RenderTargetView.Dispose();
            _RenderTarget.Dispose();

            _DepthStencilView.Dispose();
            _DepthStencil.Dispose();

            _SwapChain.ResizeBuffers(1, Form.ClientSize.Width, Form.ClientSize.Height, Format.Unknown, SwapChainFlags.None);

            CreateView();
        }
        private void CreateDevice() {
            var desc = new SwapChainDescription {
                BufferCount = 1,
                Flags = SwapChainFlags.None,
                IsWindowed = true,
                ModeDescription = new ModeDescription(
                    Form.ClientSize.Width, Form.ClientSize.Height,
                    new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = Form.Handle,
                SampleDescription = new SampleDescription(8, 0), //new SampleDescription(8, Device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, 8)),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput | Usage.BackBuffer,
            };

            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out _Device, out _SwapChain);

            var factory = _SwapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(Form.Handle, WindowAssociationFlags.IgnoreAll);

            CreateView();

            var depthDesc = DepthStencilStateDescription.Default();
            _StencilState = new DepthStencilState(Device, depthDesc);
            
            var blendDesc = new BlendStateDescription();
            blendDesc.RenderTarget[0].IsBlendEnabled = true;
            blendDesc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
            blendDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
            blendDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
            blendDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            _BlendState = new BlendState(Device, blendDesc);

            Device.ImmediateContext.OutputMerger.SetDepthStencilState(_StencilState);
            Device.ImmediateContext.OutputMerger.SetBlendState(_BlendState);

            Device.ImmediateContext.Rasterizer.State = _RasterizerState = new RasterizerState(Device, new RasterizerStateDescription {
                CullMode = CullMode.Front,
                FillMode = FillMode.Solid,
                IsMultisampleEnabled = true,
            });
        }
        private void CreateView() {
            _RenderTarget = Texture2D.FromSwapChain<Texture2D>(_SwapChain, 0);
            _RenderTargetView = new RenderTargetView(Device, _RenderTarget);

            var dsTexDesc = new Texture2DDescription {
                Format = Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = _RenderTarget.Description.Width,
                Height = _RenderTarget.Description.Height,
                SampleDescription = _SwapChain.Description.SampleDescription,
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

            
            Device.ImmediateContext.OutputMerger.SetTargets(_DepthStencilView, _RenderTargetView);
            
            Device.ImmediateContext.Rasterizer.SetViewport(new Viewport(0, 0, Form.ClientSize.Width, Form.ClientSize.Height));
        }
        #endregion

        #region Initialize
        protected virtual void Initialize() {
            _InputService = new InputService(Form);
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
        private void Load() {
            CoreComponents.LoadContent();
            Components.LoadContent();

            _Cube = new Content.Cube(this);
            _Cube.LoadContent();

            LoadContent();
        }
        protected virtual void LoadContent() { }

        private void Unload() {
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
        private void EngineLoop() {
            var elapsed = _RunTimer.Elapsed;
            _RunTimer.Restart();
            _TotalElapsedTime += elapsed.Ticks;
            var time = new EngineTime(TimeSpan.FromTicks(_TotalElapsedTime), elapsed);

            Update(time);
            Draw(time);

            _SwapChain.Present(0, PresentFlags.None);
        }

        protected virtual void Update(EngineTime time) {
            CoreComponents.Update(time);
            Components.Update(time);
        }
        protected virtual void Draw(EngineTime time) {
            var world = Matrix.Identity;
            var view = Camera.View;
            var proj = Camera.Projection;


            Device.ImmediateContext.ClearRenderTargetView(_RenderTargetView, Color.CornflowerBlue);
            Device.ImmediateContext.ClearDepthStencilView(_DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);

            Draw3D(time, ref world, ref view, ref proj);
        }

        private void Draw3D(EngineTime time, ref Matrix world, ref Matrix view, ref Matrix proj) {
            CoreComponents.Draw(time, ref world, ref view, ref proj);
            Components.Draw(time, ref world, ref view, ref proj);
        }
        #endregion
    }
}
